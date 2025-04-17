using Library.Модели;
using Library.Сервисы; 


namespace LibraryTest
{
    [TestFixture]
    public class LibraryServiceTests
    {
        private LibraryService _library;
        private Book _testBook1;
        private Book _testBook2;

        [SetUp]
        public void Setup()
        {
            _library = new LibraryService(null, null, 0);
            _testBook1 = new Book
            {
                UDK = "123",
                Author = "Толстой",
                Title = "Война и мир",
                Year = 1869,
                Count = 5
            };
            _testBook2 = new Book
            {
                UDK = "456",
                Author = "Достоевский",
                Title = "Преступление и наказание",
                Year = 1866,
                Count = 3
            };
        }

        [TearDown]
        public void Cleanup()
        {
            _library.Clear();
        }

        [Test]
        public void AddToFront_AddsBookToEmptyList_HeadAndTailAreSame()
        {
            _library.AddToFront(_testBook1);

            Assert.That(_library.Count, Is.EqualTo(1));
            Assert.That(_library.GetAllBooks()?.Data, Is.EqualTo(_testBook1));
        }

        [Test]
        public void AddToFront_AddsBookToNonEmptyList_HeadUpdated()
        {
            _library.AddToFront(_testBook1);
            _library.AddToFront(_testBook2);

            Assert.That(_library.Count, Is.EqualTo(2));
            Assert.That(_library.GetAllBooks()?.Data, Is.EqualTo(_testBook2));
        }

        [Test]
        public void AddToEnd_AddsBookToEmptyList_HeadAndTailAreSame()
        {
            _library.AddToEnd(_testBook1);

            Assert.That(_library.Count, Is.EqualTo(1));
            Assert.That(_library.GetAllBooks()?.Data, Is.EqualTo(_testBook1));
        }

        [Test]
        public void AddToEnd_AddsBookToNonEmptyList_TailUpdated()
        {
            _library.AddToEnd(_testBook1);
            _library.AddToEnd(_testBook2);

            Assert.That(_library.Count, Is.EqualTo(2));

            var head = _library.GetAllBooks();
            var tail = head?.Next;

            Assert.That(tail?.Data, Is.EqualTo(_testBook2));
        }

        [Test]
        public void FindByAuthorAndYear_BookExists_ReturnsBookNode()
        {
            _library.AddToEnd(_testBook1);
            var found = _library.FindByAuthorAndYear("Толстой", 1869);

            Assert.That(found, Is.Not.Null);
            Assert.That(found.Data, Is.EqualTo(_testBook1));
        }

        [Test]
        public void FindByAuthorAndYear_BookDoesNotExist_ReturnsNull()
        {
            _library.AddToEnd(_testBook1);
            var found = _library.FindByAuthorAndYear("Достоевский", 0);

            Assert.That(found, Is.Null);
        }

        [Test]
        public void AddSorted_AddsInCorrectOrder()
        {
            var bookA = new Book { Author = "Акунин", Title = "Азазель", Year = 2000 };
            var bookB = new Book { Author = "Булгаков", Title = "Мастер и Маргарита", Year = 1967 };

            _library.AddSorted(bookB);
            _library.AddSorted(bookA);

            var head = _library.GetAllBooks();
            Assert.That(head.Data.Author, Is.EqualTo("Акунин"));
            Assert.That(head.Next.Data.Author, Is.EqualTo("Булгаков"));
        }

        [Test]
        public void Remove_ExistingBook_ReturnsTrueAndDecrementsCount()
        {
            _library.AddToEnd(_testBook1);
            bool result = _library.Remove("123");

            Assert.That(result, Is.True);
            Assert.That(_library.Count, Is.EqualTo(0));
        }

        [Test]
        public void Remove_NonExistingBook_ReturnsFalse()
        {
            _library.AddToEnd(_testBook1);
            bool result = _library.Remove("999");

            Assert.That(result, Is.False);
            Assert.That(_library.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetBooksSortedByYear_SortsCorrectly()
        {
            _library.AddToEnd(_testBook1); // 1869
            _library.AddToEnd(_testBook2); // 1866

            var sortedHead = _library.GetBooksSortedByYear();

            Assert.That(sortedHead.Data.Year, Is.EqualTo(1866));
            Assert.That(sortedHead.Next.Data.Year, Is.EqualTo(1869));
        }

        [Test]
        public void SaveAndLoad_File_BooksArePreserved()
        {
            _library.AddToEnd(_testBook1);
            _library.AddToEnd(_testBook2);

            string testFile = Path.GetTempFileName();
            _library.SaveToFile(testFile);

            var newLibrary = new LibraryService(null, null, 0);
            newLibrary.LoadFromFile(testFile);

            Assert.That(newLibrary.Count, Is.EqualTo(2));

            var loadedBook = newLibrary.FindByAuthorAndYear("Толстой", 1869);
            Assert.That(loadedBook, Is.Not.Null);
            Assert.That(loadedBook.Data.Title, Is.EqualTo("Война и мир"));

            File.Delete(testFile);
        }

        [Test]
        public void LoadFromFile_InvalidFile_ThrowsException()
        {
            string invalidFile = "invalid_file.txt";

            var ex = Assert.Throws<Exception>(() => _library.LoadFromFile(invalidFile));
            Assert.That(ex.Message, Does.Contain("Ошибка загрузки файла"));
        }

        [Test]
        public void Clear_RemovesAllBooks()
        {
            _library.AddToEnd(_testBook1);
            _library.AddToEnd(_testBook2);

            _library.Clear();

            Assert.That(_library.Count, Is.EqualTo(0));
            Assert.That(_library.GetAllBooks(), Is.Null);
        }
    }
}