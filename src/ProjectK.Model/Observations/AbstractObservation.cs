using System;

namespace ProjectK.Model.Observations
{
    public abstract class AbstractObservation
    {
        public DateTime Observed { get; set; }
        public DateTime Recorded { get; set; }
        public DateTime Occurred { get; set; }
    }
}
