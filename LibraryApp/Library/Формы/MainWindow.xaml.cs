using Library.Модели;
using Library.Сервисы;
using Library.Формы;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LibraryService libraryService;

        public MainWindow()
        {
            InitializeComponent();
            libraryService = new LibraryService(null, null, 0);
            UpdateBooksList();
        }

        private void UpdateBooksList()
        {
            booksGrid.ItemsSource = null;
            var books = new List<Book>();
            var current = libraryService.GetAllBooks();
            while (current != null)
            {
                books.Add(current.Data);
                current = current.Next;
            }
            booksGrid.ItemsSource = books;
            booksCount.Text = libraryService.Count.ToString();
        }

        private Book GetBookFromForm()
        {
            if (string.IsNullOrWhiteSpace(txtUDK.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text) ||
                string.IsNullOrWhiteSpace(txtCount.Text))
            {
                throw new Exception("Все поля должны быть заполнены!");
            }

            if (!int.TryParse(txtYear.Text, out int year) || year <= 0)
            {
                throw new Exception("Год издания должен быть положительным числом!");
            }

            if (!int.TryParse(txtCount.Text, out int count) || count <= 0)
            {
                throw new Exception("Количество экземпляров должно быть положительным числом!");
            }

            return new Book
            {
                UDK = txtUDK.Text,
                Author = txtAuthor.Text,
                Title = txtTitle.Text,
                Year = year,
                Count = count
            };
        }

        private void ClearForm()
        {
            txtUDK.Text = "";
            txtAuthor.Text = "";
            txtTitle.Text = "";
            txtYear.Text = "";
            txtCount.Text = "";
        }

        private void ShowStatus(string message)
        {
            statusText.Text = message;
        }

        private void AddToFront_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book book = GetBookFromForm();
                libraryService.AddToFront(book);
                UpdateBooksList();
                ClearForm();
                ShowStatus("Книга добавлена в начало списка");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddToEnd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book book = GetBookFromForm();
                libraryService.AddToEnd(book);
                UpdateBooksList();
                ClearForm();
                ShowStatus("Книга добавлена в конец списка");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddSorted_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book book = GetBookFromForm();
                libraryService.AddSorted(book);
                UpdateBooksList();
                ClearForm();
                ShowStatus("Книга добавлена в алфавитном порядке");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddAfterBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblSearchUDK.Visibility = Visibility.Visible;
                txtSearchUDK.Visibility = Visibility.Visible;

                var result = MessageBox.Show("Введите УДК книги, после которой нужно добавить новую книгу, затем нажмите кнопку еще раз",
                    "Добавление после книги", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    Book newBook = GetBookFromForm();
                    string udk = txtSearchUDK.Text;

                    // Находим книгу, после которой нужно добавить
                    var current = libraryService.GetAllBooks();
                    while (current != null && current.Data.UDK != udk)
                    {
                        current = current.Next;
                    }

                    if (current == null)
                    {
                        throw new Exception("Книга с указанным УДК не найдена!");
                    }

                    // Создаем новый узел
                    BookNode newNode = new BookNode(newBook);

                    // Устанавливаем связи
                    newNode.Next = current.Next;
                    newNode.Previous = current;

                    if (current.Next != null)
                    {
                        current.Next.Previous = newNode;
                    }
                    else
                    {
                        // Если добавляем после последнего элемента, обновляем хвост
                        libraryService.GetType().GetField("_tail", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            .SetValue(libraryService, newNode);
                    }

                    current.Next = newNode;

                    // Обновляем счетчик
                    libraryService.GetType().GetProperty("Count").SetValue(libraryService, (int)libraryService.GetType().GetProperty("Count").GetValue(libraryService) + 1);

                    UpdateBooksList();
                    ClearForm();
                    lblSearchUDK.Visibility = Visibility.Collapsed;
                    txtSearchUDK.Visibility = Visibility.Collapsed;
                    txtSearchUDK.Text = "";
                    ShowStatus($"Книга добавлена после книги с УДК {udk}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBeforeBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblSearchUDK.Visibility = Visibility.Visible;
                txtSearchUDK.Visibility = Visibility.Visible;

                var result = MessageBox.Show("Введите УДК книги, перед которой нужно добавить новую книгу, затем нажмите кнопку еще раз",
                    "Добавление перед книгой", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    Book newBook = GetBookFromForm();
                    string udk = txtSearchUDK.Text;

                    // Находим книгу, перед которой нужно добавить
                    var current = libraryService.GetAllBooks();
                    while (current != null && current.Data.UDK != udk)
                    {
                        current = current.Next;
                    }

                    if (current == null)
                    {
                        throw new Exception("Книга с указанным УДК не найдена!");
                    }

                    // Создаем новый узел
                    BookNode newNode = new BookNode(newBook);

                    // Устанавливаем связи
                    newNode.Previous = current.Previous;
                    newNode.Next = current;

                    if (current.Previous != null)
                    {
                        current.Previous.Next = newNode;
                    }
                    else
                    {
                        // Если добавляем перед первым элементом, обновляем голову
                        libraryService.GetType().GetField("_head", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            .SetValue(libraryService, newNode);
                    }

                    current.Previous = newNode;

                    // Обновляем счетчик
                    libraryService.GetType().GetProperty("Count").SetValue(libraryService, (int)libraryService.GetType().GetProperty("Count").GetValue(libraryService) + 1);

                    UpdateBooksList();
                    ClearForm();
                    lblSearchUDK.Visibility = Visibility.Collapsed;
                    txtSearchUDK.Visibility = Visibility.Collapsed;
                    txtSearchUDK.Text = "";
                    ShowStatus($"Книга добавлена перед книги с УДК {udk}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblSearchUDK.Visibility = Visibility.Visible;
                txtSearchUDK.Visibility = Visibility.Visible;

                var result = MessageBox.Show("Введите УДК книги для удаления, затем нажмите кнопку еще раз",
                    "Удаление книги", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    string udk = txtSearchUDK.Text;
                    bool removed = libraryService.Remove(udk);

                    if (removed)
                    {
                        UpdateBooksList();
                        lblSearchUDK.Visibility = Visibility.Collapsed;
                        txtSearchUDK.Visibility = Visibility.Collapsed;
                        txtSearchUDK.Text = "";
                        ShowStatus($"Книга с УДК {udk} удалена");
                    }
                    else
                    {
                        throw new Exception("Книга с указанным УДК не найдена!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    libraryService.SaveToFile(saveFileDialog.FileName);
                    ShowStatus($"Данные сохранены в файл: {saveFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    libraryService.LoadFromFile(openFileDialog.FileName);
                    UpdateBooksList();
                    ShowStatus($"Данные загружены из файла: {openFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SortByYear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                libraryService.GetBooksSortedByYear();
                UpdateBooksList();
                ShowStatus("Книги отсортированы по году издания");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchWindow searchWindow = new SearchWindow(libraryService);
                searchWindow.Owner = this;
                searchWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowAllBooks_Click(object sender, RoutedEventArgs e)
        {
            UpdateBooksList();
            ShowStatus("Отображены все книги");
        }
    }
}
