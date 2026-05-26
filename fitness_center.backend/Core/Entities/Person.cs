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
        private ContactInfo _contact;
        private string? _profilePhotoUrl;

        public string Name => _name;
        public ContactInfo Contact => _contact;
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
        protected Person(string name, ContactInfo contact, string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым", nameof(name));

            _name = name;
            _contact = contact ?? throw new ArgumentNullException(nameof(contact));

            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new ArgumentException("IdentityUserId обязателен", nameof(identityUserId));
            IdentityUserId = identityUserId;
        }


        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Имя не может быть пустым", nameof(newName));
            _name = newName;
        }

        public void UpdateContact(ContactInfo newContact)
        {
            _contact = newContact ?? throw new ArgumentNullException(nameof(newContact));
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
