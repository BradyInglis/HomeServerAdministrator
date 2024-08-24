/*
 *  This class defines a Folder object. It represents the metadata that corresponds to a folder on the server.
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;

namespace HomeServerAdministrator
{
    public class Folder
    {

        // Constants and static fields
        private static List<Folder> _folders = new List<Folder>();
        private static bool _listExists = false;
        private const string DefaultName = "Name could not be found.";
        private const string DefaultDate = "Date could not be found";
        private const float DefaultSize = 0.0f;


        // Constructor
        public Folder(string name, string dateCreated, float size)
        {
            Name = name;
            DateCreated = dateCreated;
            Size = size;
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
            get;
            set;
        }
        public string DateCreated
        {
            get;
            set;
        }
        public float Size
        {
            get;
            set;
        }

        // Static methods - CreateFolder method currently loads example Folders
        public static async void CreateFolders(MainWindow window)
        {
            // Return instantly if folders have already been instantiated
            if (ListExists)
            {
                return;
            }

            // Pull folders from server, store in folders list
            Folders = await Server.GetFolders();

            // When list is complete, UI will update
            ListExists = true;
            window.RefreshFolders();
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
        public static async Task<HttpResponseMessage> DeleteFolderByName(string name, string adminPassword)
        {
            // Delete from memory if it exists
            Folder folderToDelete = FindFolderByName(name);
            if (folderToDelete != null) { Folders.Remove(folderToDelete); }

            // Attempt to delete folder from server and grab response to determine if success
            HttpResponseMessage deletionResponse = await Server.DeleteFolder(name, adminPassword);

            // Return response to UI
            return deletionResponse;
        }
        
        // Send folder to server and keep it in memory. Password/email need not be saved to memory. (Only newly made folders will ever need to be saved to server)
        public static async Task<HttpResponseMessage> CreateNewFolder(string name, string email, string password, string adminPassword)
        {
            // Attempt to send to server for permanent storage
            HttpResponseMessage folderAddResponse = await Server.SaveFolder(name, email, password, adminPassword);

            // Store in memory if server save was success
            if (folderAddResponse.IsSuccessStatusCode)
            {
                string currentDate = DateTime.Now.Date.ToString().Split(" ")[0];
                Folders.Add(new Folder(name, currentDate, DefaultSize));
            }

            // Return response
            return folderAddResponse;
        }
    }
}
