using System;
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

        public bool HasLoginInformation
        {
            get { return _settings.Contains("LoginEmail") && _settings.Contains("LoginPassword"); }
        }


        public string LoginEmail
        {
            get { return (string)_settings["LoginEmail"]; }
            set
            {
                if (value == null)
                    _settings.Remove("LoginEmail");
                else
                    _settings["LoginEmail"] = value;
            }
        }

        public string LoginPassword
        {
            get { return (string)_settings["LoginPassword"]; }
            set
            {
                if (value == null)
                    _settings.Remove("LoginPassword");
                else
                    _settings["LoginPassword"] = value;
            }
        }
    }
}