using Newtonsoft.Json;
using ProjectK.Core.Commands;
using ProjectK.Model;

namespace ProjectK.ES.Commands
{
    public class MobCreatedCommand : Command
    {
        public override void Initialise(string json)
        {
            base.Initialise(json);
            Mob = JsonConvert.DeserializeObject<Mob>(json);
        }

        public Mob Mob { get; set; }
    }
}
