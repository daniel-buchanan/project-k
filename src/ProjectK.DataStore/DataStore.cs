using ProjectK.Model;

namespace ProjectK.DataStore
{
    public static class DataStore
    {
        public static IDataRepository<Animal> Animals => new DataRepository<Animal>();
        public static IDataRepository<Mob> Mobs => new DataRepository<Mob>();
        public static IDataRepository<Observation> Observations => new DataRepository<Observation>();
        public static IDataRepository<AnimalMobHistory> AnimalMobHistories => new DataRepository<AnimalMobHistory>();
    }
}
