using LibraryApp.Модели;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Сервисы
{
    /// <summary>
    /// Класс, реализующий логику работы библиотеки с использованием двухсвязного списка
    /// </summary>
    public class LibraryService
    {
        private BookNode _head; // Голова списка - первый элемент
        private BookNode _tail; // Хвост списка - последний элемент

        public int Count { get; private set; } // Количество книг в библиотеке

        /// <summary>
        /// Конструктор сервиса библиотеки
        /// Инициализирует пустой список
        /// </summary>
        public LibraryService(BookNode head, BookNode tail, int count)
        {
            _head = null;   // Список пока пуст
            _tail = null;   // Нет ни головы, ни хвоста
            Count = 0;      // Количество элементов = 0
        }

        /// <summary>
        /// Добавление книги в начало списка
        /// </summary>
        /// <param name="book">Книга для добавления</param>
        public void AddToFront(Book book)
        {
            BookNode newNode = new BookNode(book);

            // Если список пустой
            if (_head == null)
            {
                // Новый узел становится и головой и хвостом
                _head = _tail = newNode;
            }
            else
            {
                // Новый узел ссылается на текущую голову
                newNode.Next = _head;

                // Текущая голова ссылается на новый узел
                _head.Previous = newNode;

                // Новый узел становится новой головой
                _head = newNode;
            }
            // Увеличиваем счетчик элементов
            Count++;
        }
        /// <summary>
        /// Добавление книги в конец списка
        /// </summary>
        /// <param name="book">Книга для добавления</param>
        public void AddToEnd(Book book)
        {
            // Создаем новый узел для книги
            BookNode newNode = new BookNode(book);

            // Если список пустой
            if (_head == null)
            {
                // Новый узел становится и головой и хвостом
                _head = _tail = newNode;
            }
            else
            {
                // Текущий хвост ссылается на новый узел
                _tail.Next = newNode;

                // Новый узел ссылается на текущий хвост
                newNode.Previous = _tail;

                // Новый узел становится новым хвостом
                _tail = newNode;
            }

            // Увеличиваем счетчик элементов
            Count++;
        }

        /// <summary>
        /// Очистка списка книг
        /// </summary>
        public void Clear()
        {
            // Обнуляем ссылки
            _head = null; 
            _tail = null;

            // Обнуляем счетчик
            Count = 0;
        }

    }
}
