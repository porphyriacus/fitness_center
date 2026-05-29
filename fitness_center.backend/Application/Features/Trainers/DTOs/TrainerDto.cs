using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.DTOs
{
    public class TrainerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Specialization { get; set; }  // Йога, Кроссфит, Пилатес
        public string? Description { get; set; }    // Биография, достижения
        public int ExperienceYears { get; set; }    // Опыт в годах
    }
}
