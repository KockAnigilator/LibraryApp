using LibraryApp.Модели;
using LibraryApp.Сервисы;
using LibraryApp.Формы;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryApp
{
    /// <summary>
    /// Главная форма приложения - интерфейс библиотеки
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly LibraryService _libraryService;

        //Элементы формы
        private ListBox booksListBox;
        private Button addButton;
        private Button removeButton;
        private Button searchButton;
        private TextBox authorSearchTextBox;
        private TextBox yearSearchTextBox;
        private Label authorSearchLabel;
        private Label yearSearchLabel;
        private Button sortYearButton;
        private Button saveButton;
        private Button loadButton;
        private Label statusLabel;

        /// <summary>
        /// Конструктор главной формы
        /// </summary>
        public MainForm()
        {
            // Настройка основной формы
            this.Text = "Библиотека";
            this.Size = new Size(650, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Инициализация сервиса библиотеки
            _libraryService = new LibraryService();


            InitializeComponent();
            SetupUI();
            UpdateBookList();
        }

        /// <summary>
        /// Обновление списка книг в ListBox
        /// </summary>
        private void UpdateBookList()
        {
            // Очищаем текущий список
            booksListBox.Items.Clear();

            // Получаем все книги из сервиса
            BookNode current = _libraryService.GetAllBooks();

            // Добавляем каждую книгу в ListBox
            while (current != null)
            {
                booksListBox.Items.Add(current.Data);
                current = current.Next;
            }

            // Обновляем статус с количеством книг
            statusLabel.Text = $"Всего книг: {_libraryService.Count}";
        }

        private void SetupUI()
        {
            // ListBox для отображения книг
            booksListBox = new ListBox
            {
                Location = new Point(20, 20),
                Size = new Size(600, 350),
                Font = new Font("Arial", 10),
                HorizontalScrollbar = true
            };
            this.Controls.Add(booksListBox);

            // Кнопка добавления книги
            addButton = new Button
            {
                Text = "Добавить",
                Location = new Point(20, 390),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10)
            };
            addButton.Click += addButton_Click;
            this.Controls.Add(addButton);

            // Кнопка удаления книги
            removeButton = new Button
            {
                Text = "Удалить",
                Location = new Point(130, 390),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10)
            };
            removeButton.Click += removeButton_Click;
            this.Controls.Add(removeButton);

            // Метка для поля поиска автора
            authorSearchLabel = new Label
            {
                Text = "Фамилия:",
                Location = new Point(240, 395),
                Size = new Size(70, 20),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(authorSearchLabel);

            // Поле для ввода автора для поиска
            authorSearchTextBox = new TextBox
            {
                Location = new Point(310, 390),
                Size = new Size(150, 25),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(authorSearchTextBox);

            // Метка для поля поиска года
            yearSearchLabel = new Label
            {
                Text = "Год:",
                Location = new Point(240, 425),
                Size = new Size(36, 20),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(yearSearchLabel);

            // Поле для ввода года для поиска
            yearSearchTextBox = new TextBox
            {
                Location = new Point(310, 420),
                Size = new Size(150, 25),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(yearSearchTextBox);

            // Кнопка поиска
            searchButton = new Button
            {
                Text = "Поиск",
                Location = new Point(470, 390),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10)
            };
            searchButton.Click += searchButton_Click;
            this.Controls.Add(searchButton);

            // Кнопка сортировки по году
            sortYearButton = new Button
            {
                Text = "Сорт. по году",
                Location = new Point(20, 430),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10)
            };
            sortYearButton.Click += sortYearButton_Click;
            this.Controls.Add(sortYearButton);

            // Кнопка сохранения
            saveButton = new Button
            {
                Text = "Сохранить",
                Location = new Point(130, 430),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10)
            };
            saveButton.Click += saveButton_Click;
            this.Controls.Add(saveButton);

            // Кнопка загрузки
            loadButton = new Button
            {
                Text = "Загрузить",
                Location = new Point(20, 470),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10)
            };
            loadButton.Click += loadButton_Click;
            this.Controls.Add(loadButton);

            // Метка статуса
            statusLabel = new Label
            {
                Text = "Всего книг: 0",
                Location = new Point(20, 510),
                Size = new Size(200, 20),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(statusLabel);
        }
        /// <summary>
        /// Обработчик нажатия кнопки "Добавить"
        /// </summary>
        private void addButton_Click(object sender, EventArgs e)
        {
            // Создаем форму для добавления книги
            AddBookForm addForm = new AddBookForm();

            // Если пользователь нажал OK в форме добавления
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                // Получаем новую книгу из формы
                Book newBook = addForm.GetBook();

                // Добавляем книгу в отсортированном порядке
                _libraryService.AddSorted(newBook);

                // Обновляем список книг
                UpdateBookList();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Удалить"
        /// </summary>
        private void removeButton_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли книга в списке
            if (booksListBox.SelectedItem != null)
            {
                // Получаем выбранную книгу
                Book selectedBook = (Book)booksListBox.SelectedItem;

                // Удаляем книгу по УДК
                _libraryService.Remove(selectedBook.UDK);

                // Обновляем список книг
                UpdateBookList();
            }
            else
            {
                // Если книга не выбрана, показываем сообщение
                MessageBox.Show("Выберите книгу для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Поиск"
        /// </summary>
        private void searchButton_Click(object sender, EventArgs e)
        {
            // Получаем фамилию автора из текстового поля
            string author = authorSearchTextBox.Text;

            // Проверяем, что фамилия введена
            if (string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("Введите фамилию автора", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Пробуем получить год из текстового поля (может быть пустым)
            if (!int.TryParse(yearSearchTextBox.Text, out int year))
            {
                year = 0; // Если год не указан, ищем только по автору
            }

            // Ищем книгу по автору и году
            BookNode found = _libraryService.FindByAuthorAndYear(author, year);

            // Если нашли
            if (found != null)
            {
                // Выделяем найденную книгу в списке
                booksListBox.SelectedItem = found.Data;

                // Прокручиваем список до найденной книги
                booksListBox.TopIndex = booksListBox.Items.IndexOf(found.Data);
            }
            else
            {
                // Если не нашли, показываем сообщение
                MessageBox.Show("Книга не найдена", "Результат поиска",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Сортировать по году"
        /// </summary>
        private void sortYearButton_Click(object sender, EventArgs e)
        {
            // Сортируем книги по году
            _libraryService.GetBooksSortedByYear();

            // Обновляем список книг
            UpdateBookList();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Сохранить"
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            // Создаем диалог сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Сохранить данные библиотеки"
            };

            // Если пользователь выбрал файл и нажал OK
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Сохраняем данные в файл
                    _libraryService.SaveToFile(saveFileDialog.FileName);

                    // Показываем сообщение об успехе
                    MessageBox.Show("Данные успешно сохранены", "Сохранение",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // В случае ошибки показываем сообщение
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Загрузить"
        /// </summary>
        private void loadButton_Click(object sender, EventArgs e)
        {
            // Создаем диалог открытия файла
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Загрузить данные библиотеки"
            };

            // Если пользователь выбрал файл и нажал OK
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Загружаем данные из файла
                    _libraryService.LoadFromFile(openFileDialog.FileName);

                    // Обновляем список книг
                    UpdateBookList();

                    // Показываем сообщение об успехе
                    MessageBox.Show("Данные успешно загружены", "Загрузка",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // В случае ошибки показываем сообщение
                    MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



        }
    }
}
