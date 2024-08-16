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

namespace HomeServerAdministrator
{
    // Interaction logic for CreateFolderForm window
    public partial class CreateFolderForm : Window
    {
        // Constructor
        public CreateFolderForm()
        {
            InitializeComponent();
        }

        // Create a new folder on submit click (after data validation)
        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            // Ensure entry data is valid
            validateEntries();

            // Create a new folder
            Folder.CreateNewFolder(Name.Text, Email.Text, Password.Text);
        }

        // Entries validated here
        private void validateEntries()
        {
            try
            {
                EntryValidation.ValidateName(Name);
                EntryValidation.ValidateEmail(Email);
                EntryValidation.ValidateEmail(Password);
            }
            catch (Exception ex)
            {
                if (ex is FolderNameException)
                {
                    Name.Text = $"{Name.Text} - {ex.Message}";
                }
                else if (ex is FolderEmailException)
                {
                    Email.Text = $"{Email.Text} - {ex.Message}";
                }
                else
                {
                    Password.Text = $"{Password.Text} - {ex.Message}";
                }
            }
        }
    }
}
