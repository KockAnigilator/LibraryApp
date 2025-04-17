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

namespace Library.Формы
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

        private void RemoveBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string udk = PromptForUDK("Введите УДК книги для удаления:");
                if (!string.IsNullOrEmpty(udk))
                {
                    bool removed = libraryService.Remove(udk);
                    if (removed)
                    {
                        UpdateBooksList();
                        ShowStatus($"Книга с УДК {udk} удалена");
                    }
                    else
                    {
                        MessageBox.Show("Книга с указанным УДК не найдена!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string PromptForUDK(string prompt)
        {
            var dialog = new InputDialog(prompt, this);
            if (dialog.ShowDialog() == true)
            {
                return dialog.Answer;
            }
            return null;
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

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.Owner = this;
            helpWindow.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }

    public class InputDialog : Window
    {
        public string Answer { get; private set; }

        public InputDialog(string prompt, Window owner)
        {
            this.Width = 300;
            this.Height = 150;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Owner = owner;
            this.Title = "Ввод данных";

            var stackPanel = new StackPanel { Margin = new Thickness(10) };

            var promptText = new TextBlock { Text = prompt, Margin = new Thickness(0, 0, 0, 10) };
            var inputBox = new TextBox { Height = 23 };
            var okButton = new Button { Content = "OK", Width = 75, Margin = new Thickness(0, 10, 0, 0) };

            okButton.Click += (sender, e) =>
            {
                Answer = inputBox.Text;
                DialogResult = true;
            };

            stackPanel.Children.Add(promptText);
            stackPanel.Children.Add(inputBox);
            stackPanel.Children.Add(okButton);

            this.Content = stackPanel;
        }
    }
}
