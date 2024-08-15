/*
 *  This class defines a Folder object. It represents the metadata that corresponds to a folder on the server.
 */

namespace HomeServerAdministrator
{
    public class Folder
    {

        // Constants and static fields
        private static List<Folder> _folders = new List<Folder>();
        private static bool _listExists = false;
        private const string DefaultName = "Name could not be found.";
        private const string DefaultDate = "Date could not be found.";
        private const double DefaultSize = 0.0;

        // Private fields
        private string _name;
        private string _dateCreated;
        private double _sizeInGB;

        // Constructors
        public Folder(string name, string dateCreated, double sizeInGB)
        {
            Name = name;
            DateCreated = dateCreated;
            SizeInGB = sizeInGB;
        }
        // Properties
        public static List<Folder> Folders
        {
            get { return _folders; }
            private set { _folders = value;  }
        }
        public static bool ListExists
        {
            get { return _listExists; }
            private set { _listExists = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = string.IsNullOrEmpty(value) ? DefaultName : value;
            }
        }
        public string DateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = string.IsNullOrEmpty(value) ? DefaultDate : value;
            }
        }
        public double SizeInGB
        {
            get { return _sizeInGB; }
            set
            {
                _sizeInGB = value < 0 ? DefaultSize : value;
            }
        }

        // Static methods - CreateFolder method currently loads example Folders
        public static void CreateFolders()
        {
            // Return instantly if folders have already been instantiated
            if (ListExists)
            {
                return;
            }

            // Otherwise, list exists now
            ListExists = true;

            // Instantiate dummy folders
            string[] dummyNames = { "Brady", "Chris", "Dave", "Shitrack", "Dicksmack", "CRACK"};
            double[] dummySizes = { 2.3, 5.3, 1.9, 2.1, 3.2, 93.3 };
            for (int i = 0; i < dummyNames.Length; i++)
            {
                Folders.Add(new Folder($"{dummyNames[i]}", "2024-08-14", dummySizes[i]));
            }
        }

        // Search through static list of Folder and return the matching Folder if it exists, else null
        public static Folder FindFolderByName(string name)
        {
            // Iterate through the list, if a match is found return
            foreach (Folder folder in Folders)
            {
                if (folder.Name == name)
                {
                    return folder;
                }
            }

            // Return null if no folder was found
            return null;
        }

        // Search through static list of Folder and delete the matching Folder if it exists
        public static void DeleteFolderByName(string name)
        {
            // Search for a matching folder
            Folder folderToDelete = FindFolderByName(name);

            // If the folder exists, delete it
            if (folderToDelete != null)
            {
                Folders.Remove(folderToDelete);
            }
        }
    }
}
