using System;
using System.Collections.Generic;

namespace ProjectK.Model.Events
{
    public class AnimalArrived
    {
        public string From { get; set; }
        public Guid AnimalId { get; set; }
    }
}
