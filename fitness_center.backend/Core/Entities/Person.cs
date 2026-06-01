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
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string? ProfilePhotoUrl { get; private set; }

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
            Surname = surname;
            Name = name;

            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new ArgumentException("IdentityUserId обязателен", nameof(identityUserId));
            IdentityUserId = identityUserId;

            ProfilePhotoUrl = profilePhotoUrl ?? default;
        }


        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Имя не может быть пустым", nameof(newName));
            Name = newName;
        }

        public void UpdateSurname(string newsurname)
        {
            if (string.IsNullOrWhiteSpace(newsurname))
                throw new ArgumentException("Имя не может быть пустым", nameof(newsurname));
            Surname = newsurname;
        }

        public void SetProfilePhoto(string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
                throw new ArgumentException("URL фото не может быть пустым", nameof(photoUrl));
            ProfilePhotoUrl = photoUrl;
        }

        public void RemoveProfilePhoto()
        {
            ProfilePhotoUrl = null;
        }
    }
}
