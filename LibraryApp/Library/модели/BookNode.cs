﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Модели
{
    /// <summary>
    /// Класс, представляющий узел двусвязного списка
    /// Каждый узел содержит данные о книге и ссылки на соседние узлы
    /// </summary>
    public class BookNode
    {
        /// <summary>
        /// Конструктор узла
        /// </summary>
        /// <param name="book">Книга, которая будет храниться в этом узле</param>
        public BookNode(Book book)
        {
            Data = book;    // Сохраняем переданную книгу
            Next = null;    // Пока не знаем следующий узел
            Previous = null; // Пока не знаем предыдущий узел
        }

        public Book Data { get; set; } // Данные о книге, хранящиеся в этом узле
        public BookNode Next { get; set; } // Ссылка на следующий узел в списке
        public BookNode Previous { get; set; } // Ссылка на предыдущий узел в списке

    }
}
