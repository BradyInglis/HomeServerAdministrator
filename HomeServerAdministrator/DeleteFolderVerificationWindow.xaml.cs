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
    // Interaction logic for DeledteFolderVerificationWindow
    public partial class DeleteFolderVerificationWindow : Window
    {
        public DeleteFolderVerificationWindow()
        {
            InitializeComponent();
        }


        // Properties
        public bool IsSubmissionSuccessful
        {
            get; set;
        }


        // Make sure password was correct
        private void OnDeleteButtonClick(object sender, RoutedEventArgs args)
        {
            if (!EntryValidation.IsAdminPasswordValid(Password.Text))
            {
                ErrorText.Visibility = Visibility.Visible;
                return;
            }
            IsSubmissionSuccessful = true;
        }
    }
}
