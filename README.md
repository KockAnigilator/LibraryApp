# LibraryApp

LibraryApp - это приложение для управления библиотечным фондом с использованием двусвязного списка для хранения данных о книгах.

Library/
│
├── Properties/           # Свойства проекта
├── Модели/               # Классы моделей данных
│   ├── Book.cs           # Класс книги (данные)
│   └── BookNode.cs       # Класс узла двусвязного списка
│
├── Сервисы/              # Бизнес-логика приложения
│   └── LibraryService.cs # Основной сервис работы с библиотекой
│
├── Формы/                # Пользовательский интерфейс
│   ├── MainWindow.xaml   # Главное окно приложения
│   └── SearchWindow.xaml # Окно поиска книг
│
├── App.config            # Конфигурация приложения
└── App.xaml              # Точка входа приложения


📋 Описание
Приложение разработано для учета книг в библиотеке с возможностью выполнения всех основных операций: добавления, удаления, поиска и сортировки книг. Реализован удобный графический интерфейс и работа с файлами для сохранения/загрузки данных.

✨ Основные возможности
📚 Добавление книг в начало/конец списка
🔍 Добавление книг в отсортированном порядке по автору
➕ Добавление книг до/после указанной книги
🗑️ Удаление книг по УДК
💾 Сохранение данных в текстовый файл
📂 Загрузка данных из текстового файла
📅 Сортировка книг по году издания
🔎 Поиск книг по автору и году издания
📋 Просмотр всего списка книг

🚀 Установка и запуск
### Системные требования
- .NET Framework 4.7.2 или выше
- Windows 7/10/11

### Запуск приложения
1. Клонируйте репозиторий:
```bash
git clone https://github.com/ваш-логин/LibrarySystem.git
