using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProjectK.Core.Buses;
using ProjectK.Core.Commands;
using ProjectK.Core.Events;
using ProjectK.DataStore;
using ProjectK.ES.Aggregates;
using ProjectK.ES.Commands;
using ProjectK.ES.Events;
using ProjectK.ES.Extrapolations;
using ProjectK.Model;
using ProjectK.Model.Observations;

namespace ProjectK
{
    class Program
    {
        private static IDataRepository<Animal> Animals = new DataRepository<Animal>();
        private static IDataRepository<Mob> Mobs = new DataRepository<Mob>();
        private static IDataRepository<Observation> Observations = new DataRepository<Observation>();
        private static IDataRepository<AnimalMobHistory> MobHistories = new DataRepository<AnimalMobHistory>();
        private static List<Guid> AnimalIds = new List<Guid>();
        
        static void Main(string[] args)
        {
            var commandBus = new CommandBus();
            var stream = new EventStream();
            RegisterEventListeners(stream);

            var watch = new Stopwatch();
            var watch2 = new Stopwatch();

            watch.Start();
            foreach (var c in Commands)
            {
                watch2.Start();
                Console.WriteLine("Command: {0}", c.ToString());
                var proc = CommandProcessorFactory.GetForCommand(stream, c);

                if (proc != null)
                {
                    var valid = proc.Validate(c);
                    if (!valid.IsValid)
                    {
                        Console.WriteLine("Command {0} is not valid", c);
                        continue;
                    }

                    proc.Process(c);
                }
                watch2.Stop();
                Console.WriteLine("Processed in: {0}ms", watch2.ElapsedMilliseconds);
            }
            watch.Stop();

            Console.WriteLine("Processing Commands Took: {0}ms", watch.ElapsedMilliseconds);

            
            watch2.Reset();
            watch2.Start();
            var player = new AggregatePlayer<Animal>(new AnimalMutator(), stream);

            long avg = 0;

            foreach (var id in AnimalIds)
            {
                watch.Reset();
                watch.Start();
                var animal = player.PlayToNow(id);
                var fetchedAnimal = Animals.FirstOrDefault(a => a.Id == id);


                var history = MobHistories.Where(a => a.AnimalId == id);
                watch.Stop();
                avg += watch.ElapsedMilliseconds;
            }

            watch2.Stop();

            avg = avg / AnimalIds.Count;

            Console.WriteLine("Total Time: {0}ms", watch2.ElapsedMilliseconds);
            Console.WriteLine("Avg Time: {0}ms", avg);

            Console.ReadLine();
        }

        private static IEnumerable<ICommand> Commands
        {
            get
            {
                for(var i = 0; i < 10; i++)
                    AnimalIds.Add(Guid.NewGuid());

                var mobId = Guid.NewGuid();
                var mobId2 = Guid.NewGuid();
                var mobId3 = Guid.NewGuid();

                yield return new MobCreatedCommand()
                {
                    Mob = new Mob() { Id = mobId, Name = "Bob"}
                };

                yield return new MobCreatedCommand()
                {
                    Mob = new Mob() { Id = mobId2, Name = "James" }
                };

                yield return new MobCreatedCommand()
                {
                    Mob = new Mob() { Id = mobId3, Name = "Sam" }
                };

                var c = new AnimalArrivedCommand()
                {
                    Observation = new ArrivalObservation()
                    {
                        From = "12/123/1234",
                        Observed = DateTime.Now.AddDays(-1),
                        Occurred = DateTime.Now.AddDays(-1),
                        Recorded = DateTime.Now,
                        MobId = mobId
                    }
                };

                var x = 0;
                foreach (var id in AnimalIds)
                {
                    c.Observation.Animals.Add(new Animal()
                    {
                        Id = id,
                        Eid = "0982123456" + x.ToString("D6"),
                        OfficialId = "ID::{0}" + x,
                        Sex = Sex.Unknown
                    });
                    x += 1;
                }

                yield return c;

                for (x = 0; x < 100000; x++)
                {
                    yield return new AnimalUpdatedCommand()
                    {
                        Animal = new Animal()
                        {
                            Id = AnimalIds[x],
                            OfficialId = "James::" + x
                        }
                    };
                }


                for (x = 175000; x < 250000; x++)
                {
                    yield return new AnimalUpdatedCommand()
                    {
                        Animal = new Animal()
                        {
                            Id = AnimalIds[x],
                            BirthDate = new DateTime(2012, 1, 1)
                        }
                    };
                }

                for (x = 410000; x < 625000; x++)
                {
                    yield return new AnimalIntoMobCommand()
                    {
                        Observation = new AnimalIntoMobObservation()
                        {
                            MobId = mobId2,
                            AnimalId = AnimalIds[x],
                            Observed = DateTime.Now,
                            Occurred = DateTime.Now,
                            Recorded = DateTime.Now
                        }
                    };
                }
            }
        }

        private static void RegisterEventListeners(IEventStream stream)
        {
            var observationMutator = new ObservationMutator();
            var observationStore = new AggregateStore<Observation>(Observations);
            var arrived = new AnimalArrivedEventProcessor(observationMutator, observationStore);
            stream.RegisterListener(arrived);

            var animalMutator = new AnimalMutator();
            var animalStore = new AggregateStore<Animal>(Animals);
            var created = new AnimalCreatedEventProcessor(animalMutator, animalStore);
            stream.RegisterListener(created);

            var mobMutator = new MobMutator();
            var mobStore = new AggregateStore<Mob>(Mobs);
            var mobCreated = new MobCreatedEventProcessor(mobMutator, mobStore);
            stream.RegisterListener(mobCreated);

            var animalHistory = new AnimalIntoMobExtrapolator(stream, MobHistories);
            var intoMob = new AnimalIntoMobEventProcessor(animalHistory);
            stream.RegisterListener(intoMob);

            var animalUpdated = new AnimalUpdatedEventProcessor(animalMutator, animalStore);
            stream.RegisterListener(animalUpdated);
        }
    }
}
