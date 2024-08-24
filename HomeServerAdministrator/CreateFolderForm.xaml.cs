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
using System.Windows;
using System.Net.Http;

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
        public async Task<bool> OnSubmitClick(object sender, RoutedEventArgs e)
        {
            // Show error and return if entries invalid 
            if (!ValidateEntries()) { return false; }

            // Attempt to send data with admin pass
            HttpResponseMessage response = await Folder.CreateNewFolder(Name.Text, Email.Text, Password.Text, AdminPassword.Text);

            // Show errormessage if admin pass incorrect or server already exists
            if (!response.IsSuccessStatusCode) { AdminPasswordError.Visibility = Visibility.Visible; return false; }

            // Otherwise tell user folder was saved and return true
            MessageBox.Show("Folder successfully added to server");
            return true;
        }

        // Entries validated here
        private bool ValidateEntries()
        {
            bool entriesValid = true;
            try
            {
                EntryValidation.ValidateName(Name.Text);
            }
            catch (FolderNameException ex)
            {
                NameError.Text = ex.Message;
                NameError.Visibility = Visibility.Visible;
                entriesValid = false;
            }
            try
            {
                EntryValidation.ValidateEmail(Email.Text);                    
            }
            catch (FolderEmailException ex)
            {
                EmailError.Text = ex.Message;
                EmailError.Visibility = Visibility.Visible;
                entriesValid = false;
            }
            try
            {
                EntryValidation.ValidatePassword(Password.Text);
            }
            catch (FolderPasswordException ex)
            {
                PasswordError.Text = ex.Message;
                PasswordError.Visibility = Visibility.Visible;
                entriesValid = false;
            }
            return entriesValid;
        }
    }
}
