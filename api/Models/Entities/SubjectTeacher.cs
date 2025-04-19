using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Entities
{
    public class SubjectTeacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public required string TeacherId { get; set; }
        public User? Teacher { get; set; }
        public List<ClassSchedule> ClassSchedules { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}