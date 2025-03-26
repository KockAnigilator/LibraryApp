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
        /// Добавление книги в отсортированном порядке по фамилии автора
        /// </summary>
        /// <param name="book">Книга для добавления</param>
        public void AddSorted(Book book)
        {
            // Если список пуст или новая книга должна быть первой
            if (_head == null || string.Compare(book.Author, _head.Data.Author) < 0)
            {
                AddToFront(book);  // Добавляем в начало
                return;
            }

            // Начинаем с головы списка
            BookNode current = _head;

            // Ищем место для вставки - пока не конец списка и автор текущей книги <= автору новой книги
            while (current.Next != null && string.Compare(book.Author, current.Next.Data.Author) >= 0)
            {
                current = current.Next; // Переходим к следующему узлу
            }

            // Создаем новый узел
            BookNode newNode = new BookNode(book);

            // Устанавливаем связи нового узла
            newNode.Next = current.Next;
            newNode.Previous = current;

            // Если вставляем не в конец
            if (current.Next != null)
            {
                // Следующий узел ссылается на новый
                current.Next.Previous = newNode;
            }
            else
            {
                // Если вставляем в конец, обновляем хвост
                _tail = newNode;
            }

            // Текущий узел ссылается на новый
            current.Next = newNode;

            // Увеличиваем счетчик элементов
            Count++;
        }

        /// <summary>
        /// Удаление книги по УДК
        /// </summary>
        /// <param name="udk">УДК книги для удаления</param>
        /// <returns>True, если удаление прошло успешно, иначе False</returns>
        public bool Remove(string udk)
        {
            // Если список пуст, ничего не удаляем
            if (_head == null) return false;

            // Начинаем с головы списка
            BookNode current = _head;

            // Ищем узел с нужным УДК
            while (current != null && current.Data.UDK != udk)
            {
                current = current.Next;
            }

            // Если не нашли, возвращаем false
            if (current == null) return false;

            // Если удаляемый узел не первый
            if (current.Previous != null)
            {
                // Предыдущий узел ссылается на следующий после удаляемого
                current.Previous.Next = current.Next;
            }
            else
            {
                // Если удаляем первый узел, обновляем голову
                _head = current.Next;
            }

            // Если удаляемый узел не последний
            if (current.Next != null)
            {
                // Следующий узел ссылается на предыдущий перед удаляемым
                current.Next.Previous = current.Previous;
            }
            else
            {
                // Если удаляем последний узел, обновляем хвост
                _tail = current.Previous;
            }

            // Уменьшаем счетчик элементов
            Count--;

            // Удаление прошло успешно
            return true;
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
