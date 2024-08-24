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
            HttpResponseMessage httpMessage = await Client.GetAsync($"{URL}/GetFolders");
            string jsonContent = await httpMessage.Content.ReadAsStringAsync();
            var deserializedFolders = JsonSerializer.Deserialize<List<Folder>>(jsonContent, Options);
            if (deserializedFolders != null) { folders = deserializedFolders; }

            // Return list of folders
            return folders;
        }

        // Sends post request to create new folder
        public static async Task<HttpResponseMessage> SaveFolder(string name, string email, string password, string adminPassword)
        {
            // For storing requested user data
            var user = new
            {
                Name = name,
                Email = email,
                Password = password,
                AdminPassword = adminPassword
            };

            // Try and send user data as json. Inform them of the status of their request
            HttpResponseMessage httpMessage = await Client.PostAsJsonAsync($"{URL}/AddFolder", user);
            return httpMessage;
        }

        // Sends post request to delete folder
        public static async Task<HttpResponseMessage> DeleteFolder(string name, string adminPassword)
        {
            // Try and send the request. Return respoonse to UI for user.
            HttpResponseMessage httpMessage = await Client.PostAsJsonAsync($"{URL}/DeleteFolder", new { Name = name, Password = adminPassword });
            return httpMessage;
        } 
    }
}
