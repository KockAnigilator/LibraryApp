using LibraryApp.Модели;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// Поиск книги по автору и году издания
        /// </summary>
        /// <param name="author">Фамилия автора</param>
        /// <param name="year">Год издания (0 - если год не важен)</param>
        /// <returns>Найденный узел или null</returns>
        public BookNode FindByAuthorAndYear(string author, int year)
        {
            // Начинаем с головы списка
            BookNode current = _head;

            while (current != null)
            {
                // Проверяем совпадение автора (без учета регистра) и года (если год указан)
                if (current.Data.Author.Equals(author, StringComparison.OrdinalIgnoreCase)
                    && (year == 0 || current.Data.Year == year))
                {
                    return current; // Нашли нужную книгу
                }
                current = current.Next; // Переходим к следующей книге
            }

            // Не нашли
            return null;
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
        /// Сортировка книг по году издания (пузырьковая сортировка)
        /// </summary>
        /// <returns>Голова отсортированного списка или null, если список пуст</returns>
        public BookNode GetBooksSortedByYear()
        {
            // Если список пуст, возвращаем null
            if (_head == null) return null;

            bool swapped;
            do
            {
                swapped = false;
                BookNode current = _head;

                // Проходим по списку
                while (current.Next != null)
                {
                    // Если текущий год больше следующего, меняем книги местами
                    if (current.Data.Year > current.Next.Data.Year)
                    {
                        // Меняем данные местами
                        Book temp = current.Data;
                        current.Data = current.Next.Data;
                        current.Next.Data = temp;
                        swapped = true; // Была перестановка
                    }
                    current = current.Next;
                }
            } while (swapped); // Повторяем, пока есть перестановки

            return _head;
        }

        /// <summary>
        /// Сохранение списка книг в файл
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                BookNode current = _head;

                // Проходим по всем книгам
                while (current != null)
                {
                    // Записываем данные книги через разделитель
                    writer.WriteLine($"{current.Data.UDK}|{current.Data.Author}|{current.Data.Title}|{current.Data.Year}|{current.Data.Count}");
                    current = current.Next;
                }
            }
        }

        /// <summary>
        /// Загрузка списка книг из файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void LoadFromFile(string filePath)
        {
            // Очищаем текущий список
            Clear();

            try
            {
                // Используем StreamReader для чтения из файла
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;

                    // Читаем файл построчно
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Разбиваем строку по разделителю |
                        string[] parts = line.Split('|');

                        // Должно быть 5 частей: УДК, автор, название, год, количество
                        if (parts.Length == 5)
                        {
                            // Создаем новую книгу
                            Book book = new Book
                            {
                                UDK = parts[0],
                                Author = parts[1],
                                Title = parts[2],
                                Year = int.Parse(parts[3]),
                                Count = int.Parse(parts[4])
                            };

                            // Добавляем книгу в конец списка
                            AddToEnd(book);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки выбрасываем исключение с понятным сообщением
                throw new Exception("Ошибка загрузки файла: " + ex.Message);
            }
        }


        /// <summary>
        /// Получение всех книг в списке
        /// </summary>
        /// <returns>Голова списка или null, если список пуст</returns>
        public BookNode GetAllBooks()
        {
            return _head;
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
