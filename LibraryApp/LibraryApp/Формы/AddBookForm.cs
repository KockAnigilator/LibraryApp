using LibraryApp.Модели;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryApp.Формы
{
    public partial class AddBookForm: Form
    {
        // Элементы формы
        private TextBox udkTextBox;
        private TextBox authorTextBox;
        private TextBox titleTextBox;
        private NumericUpDown yearNumeric;
        private NumericUpDown countNumeric;
        private Button okButton;
        private Button cancelButton;

        public AddBookForm()
        {
            // Настройка формы
            this.Text = "Добавить книгу";
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Создание и настройка элементов формы
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Метка и поле для УДК
            Label udkLabel = new Label
            {
                Text = "УДК:",
                Location = new Point(20, 20),
                Size = new Size(80, 20),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(udkLabel);

            udkTextBox = new TextBox
            {
                Location = new Point(120, 20),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(udkTextBox);

            // Метка и поле для автора
            Label authorLabel = new Label
            {
                Text = "Автор:",
                Location = new Point(20, 50),
                Size = new Size(80, 20),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(authorLabel);

            authorTextBox = new TextBox
            {
                Location = new Point(120, 50),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(authorTextBox);

            // Метка и поле для названия
            Label titleLabel = new Label
            {
                Text = "Название:",
                Location = new Point(20, 80),
                Size = new Size(80, 20),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(titleLabel);

            titleTextBox = new TextBox
            {
                Location = new Point(120, 80),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(titleTextBox);

            // Метка и поле для года
            Label yearLabel = new Label
            {
                Text = "Год:",
                Location = new Point(20, 110),
                Size = new Size(80, 20),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(yearLabel);

            yearNumeric = new NumericUpDown
            {
                Location = new Point(120, 110),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10),
                Minimum = 1800,
                Maximum = 2100,
                Value = DateTime.Now.Year
            };
            this.Controls.Add(yearNumeric);

            // Метка и поле для количества
            Label countLabel = new Label
            {
                Text = "Количество:",
                Location = new Point(20, 140),
                Size = new Size(80, 20),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(countLabel);

            countNumeric = new NumericUpDown
            {
                Location = new Point(120, 140),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10),
                Minimum = 1,
                Value = 1
            };
            this.Controls.Add(countNumeric);

            // Кнопка OK
            okButton = new Button
            {
                Text = "OK",
                Location = new Point(120, 180),
                Size = new Size(90, 30),
                Font = new Font("Arial", 10),
                DialogResult = DialogResult.OK
            };
            okButton.Click += okButton_Click;
            this.Controls.Add(okButton);

            // Кнопка Отмена
            cancelButton = new Button
            {
                Text = "Отмена",
                Location = new Point(220, 180),
                Size = new Size(90, 30),
                Font = new Font("Arial", 10),
                DialogResult = DialogResult.Cancel
            };
            cancelButton.Click += cancelButton_Click;
            this.Controls.Add(cancelButton);

            // Назначение кнопок Accept и Cancel для формы
            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        public Book GetBook()
        {
            return new Book
            {
                UDK = udkTextBox.Text,
                Author = authorTextBox.Text,
                Title = titleTextBox.Text,
                Year = (int)yearNumeric.Value,
                Count = (int)countNumeric.Value
            };
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(udkTextBox.Text))
            {
                MessageBox.Show("Введите УДК", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(authorTextBox.Text))
            {
                MessageBox.Show("Введите автора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
