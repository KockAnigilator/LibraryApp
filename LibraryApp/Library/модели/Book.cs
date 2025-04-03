using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Library.Модели
{
    /// <summary>
    /// Класс, представляющий книгу в библиотеке
    /// Содержит все основные свойства книги
    /// </summary>
    public class Book
    {
        
        public string UDK { get; set; } // Уникальный идентификатор книги (Универсальная десятичная классификация)
        public string Author { get; set; } // Фамилия и инициалы автора книги
        public string Title { get; set; } // Название книги
        public int Year { get; set; }  // Год издания книги
        public int Count { get; set; } // Количество экземпляров книги в библиотеке

        /// <summary>
        /// Переопределение метода ToString() для удобного отображения книги
        /// </summary>
        /// <returns>Строковое представление книги</returns>
        public override string ToString()
        {
            return $"{Title} by {Author} ({Year}), UDK: {UDK}, Available: {Count}";
        }
    }


}
