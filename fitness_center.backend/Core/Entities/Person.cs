using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    /// <summary>
    /// представляет пользователя
    /// !!! в Application подставить фото профиля по умолчанию
    /// </summary>
    // Core/Entities/Person.cs
    public abstract class Person : Entity
    {
        private string _name;
        private string _surname;
        private string? _profilePhotoUrl;

        public string Name => _name;
        public string Surname => _surname;
        public string? ProfilePhotoUrl => _profilePhotoUrl;

        /// <summary>
        /// связь с Identity (ASP.NET Core Identity)
        /// </summary>
        public string IdentityUserId { get; private set; }

        // приватный конструктор (EF Core)
        protected Person() { }

        /// <summary>
        /// создание нового человека
        /// </summary>
        protected Person(string name, string surname, string identityUserId, string? profilePhotoUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым", nameof(name));
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Фамилия не может быть пуста", nameof(surname));
            _surname = surname;
            _name = name;

            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new ArgumentException("IdentityUserId обязателен", nameof(identityUserId));
            IdentityUserId = identityUserId;

            _profilePhotoUrl = profilePhotoUrl ?? default;
        }


        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Имя не может быть пустым", nameof(newName));
            _name = newName;
        }

        public void UpdateSurname(string newsurname)
        {
            if (string.IsNullOrWhiteSpace(newsurname))
                throw new ArgumentException("Имя не может быть пустым", nameof(newsurname));
            _surname = newsurname;
        }

        public void SetProfilePhoto(string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
                throw new ArgumentException("URL фото не может быть пустым", nameof(photoUrl));
            _profilePhotoUrl = photoUrl;
        }

        public void RemoveProfilePhoto()
        {
            _profilePhotoUrl = null;
        }
    }
}
