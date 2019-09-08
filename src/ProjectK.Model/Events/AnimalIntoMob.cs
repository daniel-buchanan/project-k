using System;

namespace ProjectK.Model.Events
{
    public class AnimalIntoMob
    {
        public Guid AnimalId { get; set; }
        public Guid MobId { get; set; }
    }
}
