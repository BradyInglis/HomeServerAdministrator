using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Net.Http.Json;


namespace HomeServerAdministrator
{
    // Server is essentially a static container, providing methods for interfacing with the server.
    internal class Server
    {
        // Local server URL and options for json serializer
        private static readonly string URL = "https://localhost:7093/AlterFolders";
        private static JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };


        // Http client instantiated only once
        private static readonly HttpClient Client = new HttpClient();

        // Sends get request for folders to server
        public static async Task<List<Folder>> GetFolders()
        {
            // Folder list to return
            List<Folder> folders = new List<Folder>();

            // Try and Get the folders json, deserialize back Folder
            try
            {
                HttpResponseMessage httpMessage = await Client.GetAsync($"{URL}/GetFolders");
                string jsonContent = await httpMessage.Content.ReadAsStringAsync();
                var deserializedFolders = JsonSerializer.Deserialize<List<Folder>>(jsonContent, Options);
                if (deserializedFolders != null) { folders = deserializedFolders; }
            }
            
            // If there was an error, inform the user via error message box
            catch (HttpRequestException e)
            { 
                MessageBox.Show($"There was an error on the server. Folders could not be found\n{e.Message}");
;           }

            // Return list of folders
            return folders;
        }

        // Sends post request to create new folder
        public static async void SaveFolder(string name, string email, string password)
        {
            // For storing requested user data
            var user = new
            {
                Name = name,
                Email = email,
                Password = password,
            };

            // Try and send user data as json. Inform them of the status of their request
            try
            {
                HttpResponseMessage httpMessage = await Client.PostAsJsonAsync($"{URL}/AddFolder", user);
                string jsonContent = await httpMessage.Content.ReadAsStringAsync();  
                MessageBox.Show($"{jsonContent}");
            }

            // If there was an error, inform the user
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Could not save folder to server. When you exit, the folder will be gone.\n{e.Message}");
            }
        }

        // Sends post request to delete folder
        public static async void DeleteFolder(Folder folder)
        {
            // Try and send the request
            try
            {
                HttpResponseMessage httpMessage = await Client.PostAsJsonAsync($"{URL}/DeleteFolder", folder);
                string jsonContent = await httpMessage.Content.ReadAsStringAsync();
                MessageBox.Show($"{jsonContent}");
            }

            // If there was an error, inform the user
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Could not delete folder to server. Changes will not be saved.\n{e.Message}");
            }
        } 




    }
}
