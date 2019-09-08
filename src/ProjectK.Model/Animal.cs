using System;

namespace ProjectK.Model
{
    public class Animal : Aggregate<Animal>
    {
        public string Eid { get; set; }
        public string OfficialId { get; set; }
        public DateTime? BirthDate { get; set; }
        public Sex Sex { get; set; }
        public Guid MobId { get; set; }
    }
}
