using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public int? IconId { get; set; }
        public int Years { get; set; } = 4;
        public string Description { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public List<Section> Sections { get; set; } = [];
    }
}