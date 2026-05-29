using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Trainer : Person
    {
        // один тренер -> много тренировок
        public ICollection<Workout> Workouts { get; private set; } = new List<Workout>();

        public int SpecializationId { get; private set; }
        public Specialization Specialization { get; private set; }  // Йога, Кроссфит, Пилатес
        public string? Description { get; private set; }    // Биография, достижения
        public int ExperienceYears { get; private set; }    // Опыт в годах

        private Trainer() { }

        public Trainer(
            string name,
            string surname,
            string identityUserId,
            string? profilePhotoUrl,
            Specialization specialization,
            string? description = null,
            int experienceYears = 0)
            : base(name, surname, identityUserId, profilePhotoUrl)
        {
            if (specialization == null)
                throw new ArgumentException("Специализация не может быть пустой", nameof(specialization));

            Specialization = specialization;
            Description = description ?? string.Empty;
            ExperienceYears = experienceYears;
        }


        public void UpdateSpecialization(Specialization newSpecialization)
        {
            if (newSpecialization == null)
                throw new ArgumentException("Специализация не может быть пустой", nameof(newSpecialization));
            Specialization = newSpecialization;
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription ?? string.Empty;
        }

        public void UpdateExperience(int years)
        {
            if (years < 0)
                throw new ArgumentException("Опыт не может быть отрицательным", nameof(years));
            ExperienceYears = years;
        }

        public bool HasWorkouts()
        {
            return Workouts.Any();
        }
    }
}
