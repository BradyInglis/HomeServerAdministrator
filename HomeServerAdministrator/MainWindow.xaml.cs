using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HomeServerAdministrator
{
    public partial class MainWindow : Window
    {

        // Constants/Enums
        public enum Page
        {
            Folders,
            Requests
        }

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            CurrentPage = Page.Folders;
            InstantiateFolders();
        }

        // Private fields
        private Page _currentPage;
        private int _foldersVisible;

        // Properties
        public Page CurrentPage
        {
            get { return _currentPage; }
            set 
            { _currentPage = value;
              ChangePage();
            }
        }
        public int FoldersVisible
        {
            get { return _foldersVisible; }
            set { _foldersVisible = value; }
        }

        // When a Folders button is pressed, animate the navbar and shift the content to Folders content
        private void OnFoldersClick(object sender, RoutedEventArgs e)
        {
            // Disable Folders button, enable Requests button
            FoldersBtn.IsEnabled = false;
            RequestsBtn.IsEnabled = true;

            // Change the page
            CurrentPage = Page.Folders;
        }

        // When a Requests button is pressed, animate the navbar and shift the content to Requests content
        private void OnRequestsClick(object sender, RoutedEventArgs e)
        {
            // Disable Requests button, enable Folders button
            RequestsBtn.IsEnabled = false;
            FoldersBtn.IsEnabled = true;
            

            // Change the page
            CurrentPage = Page.Requests;
        }

        // Change the page content
        private void ChangePage()
        {
            // Change page visibility based on current page, move nav bar
            if (CurrentPage == Page.Folders)
            {
                FoldersPage.Visibility = Visibility.Visible;
                RequestsPage.Visibility = Visibility.Hidden;
                Grid.SetColumn(Navbar, 0);
            }
            else
            {
                FoldersPage.Visibility = Visibility.Hidden;
                RequestsPage.Visibility = Visibility.Visible;
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
            // Reset folders count
            FoldersVisible = 0;
            int rowCount = 0;

            // Iterate through each folder
            foreach (Folder folder in Folder.Folders)
            {
                // Count each new folder
                FoldersVisible += 1;

                // Get number of rows the FoldersArea grid contains. This is needed to append each new folder Label to the grid.
                rowCount = FoldersArea.RowDefinitions.Count;

                // Instantiate labels with their respective content
                List<Label> labels = CreateLabels(folder);

                // Instantiate hover indicator for folder
                Canvas hoverIndicator = new Canvas { Style = (Style)FindResource("FileHoverIndicator") };

                // Instantiate delete button for folder
                Button deleteButton = new Button { Style = (Style)FindResource("DeleteFileButton") };

                // Set each element up, add grid definitions, event handlers, etc
                SetLabelProperties(labels, rowCount);
                SetInteractiveElementProperties(hoverIndicator, deleteButton, rowCount);

                // Add new row definition to the folders area grid
                RowDefinition row = new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) };
                FoldersArea.RowDefinitions.Add(row);

                // Add all elements to FoldersArea grid
                FoldersArea.Children.Add(labels.ElementAt(0));
                FoldersArea.Children.Add(labels.ElementAt(1));
                FoldersArea.Children.Add(labels.ElementAt(2));
                FoldersArea.Children.Add(hoverIndicator);
                FoldersArea.Children.Add(deleteButton);
            }

        }

        // Instantiate labels
        private List<Label> CreateLabels(Folder folder)
        {
            // Create each label based of their respective folder Property
            Label newFolderName = new Label()
            {
                Content = folder.Name,
                Foreground = Brushes.White
            };
            Label newFolderDateCreated = new Label()
            {
                Content = folder.DateCreated,
                Foreground = Brushes.White
            };
            Label newFolderSizeInGB = new Label()
            {
                Content = $"{folder.SizeInGB} GB",
                Foreground = Brushes.White,
            };

            // Return a list of the labels
            return new List<Label> { newFolderName, newFolderDateCreated, newFolderSizeInGB };
        }

        // Set labels up; add grid definitions
        private void SetLabelProperties(List<Label> labels, int rowCount)
        {
            // Iterate through each label, add respective definitions
            for (int i = 0; i < labels.Count; i++)
            {
                // Set columns for each label
                Grid.SetColumn(labels.ElementAt(i), i*2);

                // Set rows for each label
                Grid.SetRow(labels.ElementAt(i), rowCount);
            }
        }

        // Set hoverIndicator and deleteButton up
        private void SetInteractiveElementProperties(UIElement hoverIndicator, UIElement deleteButton, int rowCount)
        {
            // Set rows for each interactive element
            Grid.SetRow(hoverIndicator, rowCount);
            Grid.SetRow(deleteButton, rowCount);

            // Add event handlers so delete button is only visible when hoverindicator or itself is hovered over
            hoverIndicator.MouseEnter += (sender, e) =>
            {
                deleteButton.Visibility = Visibility.Visible;
            };
            hoverIndicator.MouseLeave += (sender, e) =>
            {
                deleteButton.Visibility = Visibility.Hidden;
            };
            deleteButton.MouseEnter += (sender, e) =>
            {
                deleteButton.Visibility = Visibility.Visible;
            };

            // Add event handler to deleteButton click 
            ((Button)deleteButton).Click += (sender, e) =>
            {
               
                // First, remove the Folder that was clicked from the static list
                int buttonRow = Grid.GetRow(deleteButton);
                for (int folderIndex = 0; folderIndex < Folder.Folders.Count; folderIndex++)
                {
                    // Find the matching folder, remove it
                    foreach (UIElement element in FoldersArea.Children)
                    {
                        if (Grid.GetRow(element) == buttonRow && element is Label && (string)((Label)element).Content == $"{Folder.Folders.ElementAt(folderIndex).Name}")
                        {
                            Folder.Folders.Remove(Folder.Folders.ElementAt(folderIndex));
                            break;
                        }
                    }
                }

                // Refresh all folders
                RefreshFolders();

            };
        }

        // Deletes all Folder visual elements and re instantiates the Folder list
        private void RefreshFolders()
        {
            // Child elements to be deleted stored here
            List<UIElement> childrenToDelete = new List<UIElement>();

            // Collect rows to delete - iterate through rows
            int firstRow = FoldersArea.RowDefinitions.Count - FoldersVisible - 1;
            int totalRows = FoldersArea.RowDefinitions.Count - 1;
            for (int currentRow = totalRows; currentRow > firstRow; currentRow--)
            {
                // Iterate through each child element. If the row matches, add child to be deleted list 
                foreach (UIElement child in FoldersArea.Children)
                {
                    // Check if the row matches. Delete all children before row is deleted
                    if (Grid.GetRow(child) == currentRow)
                    {
                        childrenToDelete.Add(child);
                    }
                }
                // Delete the row now
                FoldersArea.RowDefinitions.RemoveAt(currentRow);

                // Delete all children
                for (int childToDeleteIndex = 0; childToDeleteIndex < childrenToDelete.Count; childToDeleteIndex++)
                {
                    FoldersArea.Children.Remove(childrenToDelete.ElementAt(childToDeleteIndex));
                }

                // Clear list
                childrenToDelete.Clear();
            }

            // Finally, load the Folder list back in 
            DisplayFolders();
        }

    }
}