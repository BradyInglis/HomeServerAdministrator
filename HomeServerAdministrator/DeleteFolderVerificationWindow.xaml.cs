using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http;

namespace HomeServerAdministrator
{
    // Interaction logic for DeledteFolderVerificationWindow
    public partial class DeleteFolderVerificationWindow : Window
    {
        public DeleteFolderVerificationWindow(string folderToDelete)
        {
            FolderToDelete = folderToDelete;
            InitializeComponent();
        }

        // Properties
        public string FolderToDelete
        {
            get;
            set;
        }


        // Make sure password was correct
        public async Task<bool> DeleteClicked(object sender, RoutedEventArgs args)
        {
            // Attempt to delete the folder. Read message as string.
            HttpResponseMessage deletionResponse = await Folder.DeleteFolderByName(FolderToDelete, Password.Text);
            string serializedDeletionResponse = await deletionResponse.Content.ReadAsStringAsync();
            
            // Display success message and close if success
            if (deletionResponse.IsSuccessStatusCode) { MessageBox.Show(serializedDeletionResponse); return true; }

            // Otherwise show error, return false
            ErrorText.Visibility = Visibility.Visible;
            return false;
        }
    }
}
