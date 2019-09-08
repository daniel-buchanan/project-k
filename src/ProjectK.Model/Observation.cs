using System;
using System.Linq.Expressions;

namespace ProjectK.Model
{
    public class Observation : Aggregate<Observation>
    {
        public DateTime Observed { get; set; }
        public DateTime Recorded { get; set; }
        public DateTime Occurred { get; set; }
        public string Kind { get; set; }
        public string Data { get; set; }
    }
}
