using Library.Модели;
using Library.Сервисы;
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
using System.Windows.Shapes;

namespace Library.Формы
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private LibraryService libraryService;

        public SearchWindow(LibraryService service)
        {
            InitializeComponent();
            libraryService = service;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string author = txtAuthor.Text;
                if (string.IsNullOrWhiteSpace(author))
                {
                    throw new Exception("Введите фамилию автора для поиска");
                }

                if (!int.TryParse(txtYear.Text, out int year))
                {
                    throw new Exception("Год должен быть числом");
                }

                var foundNode = libraryService.FindByAuthorAndYear(author, year);
                var results = new List<Book>();

                // Находим все книги, соответствующие критериям
                var current = foundNode;
                while (current != null)
                {
                    if (current.Data.Author.Equals(author, StringComparison.OrdinalIgnoreCase) &&
                        (year == 0 || current.Data.Year == year))
                    {
                        results.Add(current.Data);
                    }
                    current = current.Next;
                }

                resultsGrid.ItemsSource = results;

                if (results.Count == 0)
                {
                    MessageBox.Show("Книги не найдены", "Результаты поиска", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка поиска", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
