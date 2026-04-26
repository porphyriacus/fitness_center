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
    /// в Application подставить фото профиля по умолчанию
    /// </summary>
    public class Person
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IContactInfo? Contact { get; set; }
        public string? ProfilePhotoUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> уникальный идентификатор </param>
        /// <param name="name">не должно быть пустым. может быть уникальным</param>
        /// <param name="contact">контакты для входа в приложение</param>
        /// <param name="profilePhotoUrl">возможность поставить аватарку. если не выбрана то будет скучная по умолчанию</param>
        /// <exception cref="Exception"></exception>
        public Person(Guid id, string? name, IContactInfo? contact, string? profilePhotoUrl = null)
        {
            Id = id;

            if (string.IsNullOrEmpty(name))
                throw new Exception("Клиент должен иметь имя");
            Name = name;

            if (contact is null)
                throw new Exception("Клиент должен иметь контакты");
            Contact = contact;

            ProfilePhotoUrl = profilePhotoUrl;
        }
    }
}
