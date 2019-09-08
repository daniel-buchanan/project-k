using System;
using System.Linq.Expressions;

namespace ProjectK.Model
{
    public class Mob : Aggregate<Mob>
    {
        public string Name { get; set; }
        public int? HeadCount { get; set; }
    }
}
