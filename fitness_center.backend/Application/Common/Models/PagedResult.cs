using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{

    /// <summary>
    /// Результат запроса с пагинацией
    /// </summary>
    /// <typeparam name="T">Тип элементов в списке</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// элементы на текущей странице
        /// </summary>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// номер текущей страницы (начиная с 1)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// размер страницы (количество элементов на странице)
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// общее количество элементов во всех страницах
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// общее количество страниц
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// есть ли предыдущая страница
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// есть ли следующая страница
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// пустой результат
        /// </summary>
        public static PagedResult<T> Empty => new()
        {
            Items = new List<T>(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0
        };
    }

}
