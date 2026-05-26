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

        public string Specialization { get; private set; }  // Йога, Кроссфит, Пилатес
        public string? Description { get; private set; }    // Биография, достижения
        public int ExperienceYears { get; private set; }    // Опыт в годах

        private Trainer() { }

        public Trainer(
            string name,
            ContactInfo contact,
            string identityUserId,
            string specialization,
            string? description = null,
            int experienceYears = 0)
            : base(name, contact, identityUserId)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                throw new ArgumentException("Специализация не может быть пустой", nameof(specialization));

            Specialization = specialization;
            Description = description ?? string.Empty;
            ExperienceYears = experienceYears;
        }


        public void UpdateSpecialization(string newSpecialization)
        {
            if (string.IsNullOrWhiteSpace(newSpecialization))
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
