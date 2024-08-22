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


        // Properties
        public PageState CurrentPage
        {
            get { return _currentPage; }
            set 
            { _currentPage = value;
              ChangePage();
            }
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
            Folder.CreateFolders(this);

            // Display folders on Folders page
            DisplayFolders();
        }


        // Instantiate and appropriately modify a label for each folder in Folders
        private void DisplayFolders()
        {
            // Iterate through each folder, createing a visual container for each
            foreach (Folder folder in Folder.Folders)
            {
                CreateFolderContainer(folder);
            }

            // Create add folder button displayed at bottom of folders area
            CreateAddFolderButton();
        }


        // For creating visual representation of folder
        private void CreateFolderContainer(Folder folder)
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
            
            // Create text blocks corresponding to Folder metadata 
            TextBlock folderName = new TextBlock { Text = folder.Name, Style = (Style)FindResource("FileText") };
            TextBlock folderDateCreated = new TextBlock { Text = folder.DateCreated, Style = (Style)FindResource("FileText") };
            TextBlock folderSizeInGB = new TextBlock { Text = $"{folder.Size}", Style = (Style)FindResource("FileText") };

            // Create delete button for this folder 
            Button deleteButton = new Button { Style = (Style)FindResource("DeleteFileButton") };

            // Set grid columns for corresponding textblocks 
            Grid.SetColumn(folderName, 0);
            Grid.SetColumn(folderDateCreated, 1);
            Grid.SetColumn(folderSizeInGB, 2);

            // Set grid columns for delete button 
            Grid.SetColumn(deleteButton, 2);

            // Subscribe methods to event handlers of necessary elements
            AddFolderEventHandlers(currentFolderContainer, deleteButton);

            // Add all elemetns to container element
            currentFolderContainer.Children.Add(folderName);
            currentFolderContainer.Children.Add(folderDateCreated);
            currentFolderContainer.Children.Add(folderSizeInGB);
            currentFolderContainer.Children.Add(deleteButton);

            // Add folder container grid to Folders Area
            FoldersArea.Children.Add(currentFolderContainer);
        }


        // For appending add folder button to bottom of folders area
        private void CreateAddFolderButton()
        {
            Button addFolderButton = new Button { Content = "Add New Folder", Style = (Style)FindResource("NavButtonStyle") };
            addFolderButton.Click += OnAddFolderButtonClick;
            FoldersArea.Children.Add(addFolderButton);
        }


        // Event handlers for folder container
        private void AddFolderEventHandlers(Grid currentFolderContainer, Button deleteButton)
        {
            // Delete button should only be visible when mouse is hovering over this folder
            currentFolderContainer.MouseEnter += (sender, e) =>
            {
                deleteButton.Visibility = Visibility.Visible;
            };
            currentFolderContainer.MouseLeave += (sender, e) =>
            {
                deleteButton.Visibility = Visibility.Hidden;
            };
            
            // Delete button click creates instance of delete folder window and subscribes to event handler
            deleteButton.Click += (sender, e) =>
            {
                DeleteFolderVerificationWindow deleteFolderWindow = new DeleteFolderVerificationWindow();
                deleteFolderWindow.DeleteButton.Click += (sender, e) =>
                {
                    if (deleteFolderWindow.IsSubmissionSuccessful)
                    {
                        deleteFolderWindow.Close();
                        Folder.DeleteFolderByName(currentFolderContainer.Name);
                        FoldersArea.Children.Remove(currentFolderContainer);
                    }
                };
                deleteFolderWindow.ShowDialog();
            };
        }


        // When add folder button is pressed, a child window is opened with a form containing the new Folders properties to be filled
        private void OnAddFolderButtonClick(object sender, RoutedEventArgs args)
        {
            // Create a new CreateFolderForm window, subscribe to submit button press
            CreateFolderForm newFolderForm = new CreateFolderForm();
            newFolderForm.SubmitFolderButton.Click += RefreshFolders;
            newFolderForm.ShowDialog();
        }


        // When a Folders button is pressed, animate the navbar and shift the content to Folders content
        private void OnFoldersClick(object sender, RoutedEventArgs args)
        {
            // Change the page
            CurrentPage = PageState.Folders;
        }


        // When a Requests button is pressed, animate the navbar and shift the content to Requests content
        private void OnRequestsClick(object sender, RoutedEventArgs args)
        {
            // Change the page
            CurrentPage = PageState.Requests;
        }


        // Clears folders area list completely and refills it
        public void RefreshFolders()
        {
            // Remove all folders from folder area
            FoldersArea.Children.Clear();

            // Add new list of folders
            DisplayFolders();
        }
        private void RefreshFolders(object sender, RoutedEventArgs args)
        {
            RefreshFolders();
        }
    }
}