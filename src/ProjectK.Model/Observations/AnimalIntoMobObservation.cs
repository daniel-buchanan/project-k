using System;

namespace ProjectK.Model.Observations
{
    public class AnimalIntoMobObservation : AbstractObservation
    {
        public Guid AnimalId { get; set; }
        public Guid MobId { get; set; }
    }
}
