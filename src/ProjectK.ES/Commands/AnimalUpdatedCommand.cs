using Newtonsoft.Json;
using ProjectK.Core.Commands;
using ProjectK.Model;

namespace ProjectK.ES.Commands
{
    public class AnimalUpdatedCommand : Command
    {
        public override void Initialise(string json)
        {
            base.Initialise(json);
            Animal = JsonConvert.DeserializeObject<Animal>(json);
        }

        public Animal Animal { get; set; }
    }
}
