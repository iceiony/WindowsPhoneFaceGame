using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace FaceGame
{
    public class AppSettings
    {
        private IsolatedStorageSettings _settings = IsolatedStorageSettings.ApplicationSettings;
        
        private const string DefaultRootApiUrl = "http://192.168.0.2:3000/";

        public string RootApiUrl
        {
            get
            {
                if (_settings.Contains("RootApiUrl"))
                    return (string) _settings["RootApiUrl"];
                
                return DefaultRootApiUrl;
            }
        }
    }
}