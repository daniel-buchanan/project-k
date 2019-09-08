using System;
using ProjectK.Core.Extrapolations;

namespace ProjectK.Model
{
    public class AnimalMobHistory : IExtrapolation
    {
        public Guid MobId { get; set; }
        public Guid AnimalId { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime? DateOut { get; set; }

        public Guid Id => Guid.Empty;

        public int Version => 0;
    }
}
