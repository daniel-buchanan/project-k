using System;
using System.Collections.Generic;

namespace ProjectK.Model.Observations
{
    public class ArrivalObservation : AbstractObservation
    {
        public ArrivalObservation()
        {
            Animals = new List<Animal>();
        }

        public string From { get; set; }
        public Guid MobId { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
