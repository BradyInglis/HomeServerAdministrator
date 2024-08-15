using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HomeServerAdministrator.Enums;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace HomeServerAdministrator
{
    public partial class MainWindow : Window
    {

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            InstantiateFolders();
            CurrentPage = PageState.Folders;
        }

        // Private fields
        private PageState _currentPage;
        private List<StackPanel> visibleFolders = new List<StackPanel>();

        // Properties
        public PageState CurrentPage
        {
            get { return _currentPage; }
            set 
            { _currentPage = value;
              ChangePage();
            }
        }

        // When a Folders button is pressed, animate the navbar and shift the content to Folders content
        private void OnFoldersClick(object sender, RoutedEventArgs e)
        {
            // Change the page
            CurrentPage = PageState.Folders;
        }

        // When a Requests button is pressed, animate the navbar and shift the content to Requests content
        private void OnRequestsClick(object sender, RoutedEventArgs e)
        {
            // Change the page
            CurrentPage = PageState.Requests;
        }

        // Change the page content
        private void ChangePage()
        {
            // Change page visibility based on current page, swap nav buttons, move nav bar
            if (CurrentPage == PageState.Folders)
            {

                FoldersPage.Visibility = Visibility.Visible;
                RequestsPage.Visibility = Visibility.Hidden;
                FoldersBtn.IsEnabled = false;
                RequestsBtn.IsEnabled = true;
                Grid.SetColumn(Navbar, 0);
            }
            else
            {
                FoldersPage.Visibility = Visibility.Hidden;
                RequestsPage.Visibility = Visibility.Visible;
                RequestsBtn.IsEnabled = false;
                FoldersBtn.IsEnabled = true;
                Grid.SetColumn(Navbar, 1);
            }
        }

        // Instantiate each folder contained on the server
        private void InstantiateFolders()
        {
            // Grab folders from server, instantiate into memory
            Folder.CreateFolders();

            // Display folders on Folders page
            DisplayFolders();
        }

        // Instantiate and appropriately modify a label for each folder in Folders
        private void DisplayFolders()
        {
            // Create add folder button to be added below last file
            Button addFolderButton = new Button { Content = "Add New Folder", Style = (Style)FindResource("NavButtonStyle") };

            // Iterate through each folder
            foreach (Folder folder in Folder.Folders)
            {
                // Create a grid to contain all folder metadata 
                Grid currentFolderContainer = new Grid()
                {
                    Name = folder.Name,
                    Style = (Style)FindResource("FileHoverIndicator")
                };

                // Add 3 columns and a single row to the grid
                currentFolderContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                currentFolderContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                currentFolderContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                currentFolderContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // Create text blocks corresponding to Folder metadata 
                TextBlock folderName = new TextBlock { Text = folder.Name, Style = (Style)FindResource("FileText") };
                TextBlock folderDateCreated = new TextBlock { Text = folder.DateCreated, Style = (Style)FindResource("FileText") };
                TextBlock folderSizeInGB = new TextBlock { Text = $"{folder.SizeInGB}", Style = (Style)FindResource("FileText") };

                // Create delete button for folder, add click handler  TEMP TEMP TEMP will organize
                Button deleteButton = new Button { Style = (Style)FindResource("DeleteFileButton") };
                deleteButton.Click += (sender, e) =>
                {
                    Folder.DeleteFolderByName(currentFolderContainer.Name);
                    FoldersArea.Children.Remove(currentFolderContainer);
                    MessageBox.Show($"Folders in memory: {Folder.Folders.Count}");
                };

                // Set grid columns for corresponding textblocks and then add to folder container grid
                Grid.SetColumn(folderName, 0);
                currentFolderContainer.Children.Add(folderName);
                Grid.SetColumn(folderDateCreated, 1);
                currentFolderContainer.Children.Add(folderDateCreated);
                Grid.SetColumn(folderSizeInGB, 2);
                currentFolderContainer.Children.Add(folderSizeInGB);

                // Set grid columns for delete button and then add to folder container grid
                Grid.SetColumn(deleteButton, 2);
                currentFolderContainer.Children.Add(deleteButton);

                // Add folder container grid to Folders Area
                FoldersArea.Children.Add(currentFolderContainer);
            }

            // Add add folder button to Folders Area
            FoldersArea.Children.Add(addFolderButton);


        }
    }
}