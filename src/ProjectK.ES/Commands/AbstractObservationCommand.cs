using Newtonsoft.Json;
using ProjectK.Core.Commands;
using ProjectK.Model.Observations;

namespace ProjectK.ES.Commands
{
    public abstract class AbstractObservationCommand<T> : Command where T: AbstractObservation
    {
        public T Observation { get; set; }

        public override void Initialise(string json)
        {
            base.Initialise(json);
            Observation = JsonConvert.DeserializeObject<T>(Json);
        }
    }
}
