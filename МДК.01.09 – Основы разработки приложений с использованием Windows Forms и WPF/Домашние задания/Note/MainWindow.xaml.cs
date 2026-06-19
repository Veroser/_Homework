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

namespace NotesManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Note> notes = new List<Note>();
        public Note currentNote = null;
        public MainWindow()
        {
            InitializeComponent();

            notes.Add(new Note("First", "firstText", DateTime.Now));

            //ListBoxNotes.Items.Add("example); добавить 1 элемент
            ListBoxNotes.ItemsSource = null;
            ListBoxNotes.ItemsSource = notes;
        }

        private void RefreshNotesList()
        {
            ListBoxNotes.ItemsSource = null;
            ListBoxNotes.ItemsSource = notes;
        }

        private void ClearInputFields()
        {
            tBxTitle.Text = string.Empty;
            tBxContent.Text = string.Empty;
            tBxDate.Text = string.Empty;
            tBxStatus.Text = string.Empty;
            currentNote = null;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentNote == null)
            {
                if (string.IsNullOrEmpty(tBxTitle.Text))
                {
                    MessageBox.Show("Введите заголовок", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Note newNote = new Note(tBxTitle.Text, tBxStatus.Text, DateTime.Now);

                notes.Add(newNote);
                RefreshNotesList();

                tBxStatus.Text = "Заметка добавлена";

                ClearInputFields();
            }
            else
            {
                Note newNote = new Note("новая заметка", string.Empty, DateTime.Now);

                notes.Add(newNote);
                RefreshNotesList();
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentNote == null)
            {
                MessageBox.Show("Выберите заметку для сохранения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(tBxTitle.Text))
            {
                MessageBox.Show("Введите заголовок", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentNote.Title = tBxTitle.Text;
            currentNote.Content = tBxContent.Text;

            RefreshNotesList();

            ListBoxNotes.SelectedItem = currentNote;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentNote == null)
            {
                MessageBox.Show("Выберите заметку для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            MessageBoxResult result = MessageBox.Show("Удалить выбранную заметку?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                notes.Remove(currentNote);
                RefreshNotesList();
                ClearInputFields();
            }
        }

        private void ListBoxSelect(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxNotes.SelectedItem is Note selectedNote)
            {
                currentNote = (Note)ListBoxNotes.SelectedItem;

                tBxContent.Text = selectedNote.Content;
                tBxTitle.Text = selectedNote.Title;
                tBxDate.Text = selectedNote.Date.ToString("dd.MM.yyyy HH:mm");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tBxContent.Text = "";
            tBxTitle.Text = "";
        }
    }
}
