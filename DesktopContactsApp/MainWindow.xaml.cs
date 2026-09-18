using DesktopContactsApp.Classes;
using SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Contact> contacts;

        public MainWindow()
        {
            InitializeComponent();

            contacts = new List<Contact>();

            ReadDatabase();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create contact window
            NewContactWindow newContactWindow = new NewContactWindow();
            // newContactWindow.Show();
            newContactWindow.ShowDialog();  // Cannot back to main window when open

            // Get data
            ReadDatabase();
        }

        private void ReadDatabase()
        {
            // Query
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Contact>();
                contacts = connection.Table<Contact>().OrderBy(c => c.Name).ToList();
            }

            // Update UI
            if (contacts != null)
            {
                /*foreach (Contact c in contacts)
                {
                    contactsListView.Items.Add(new ListViewItem()
                    {
                        Content = c
                    });
                }*/
                contactsListView.ItemsSource = contacts;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get TextBox
            TextBox searchTextBox = sender as TextBox;

            // Query
            List<Contact> filteredList = contacts.Where(c => c.Name.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();

            /*var filteredList2 = (from c in contacts
                                 where c.Name.ToLower().Contains(searchTextBox.Text.ToLower())
                                 orderby c.Email
                                 select c).ToList();*/

            // Update UI
            contactsListView.ItemsSource = filteredList;
        }

        private void contactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact selectedContact = (Contact) contactsListView.SelectedItem;
            if (selectedContact != null)
            {
                ContactDetailsWindow contactDetailsWindow = new ContactDetailsWindow(selectedContact);
                contactDetailsWindow.ShowDialog();

                // Get data
                ReadDatabase();
            }
        }
    }
}