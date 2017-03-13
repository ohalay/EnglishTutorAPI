using System;

namespace EnglishTutor.Common
{
    public class AppSettings
    {
        public OxforDictionary OxforDictionary { get; set; }

        public Firebase Firebase { get; set; }
    }

    public class Firebase
    {
        public Uri BaseUrl { get; set; }
    }

    public class OxforDictionary
    {
        public Uri BaseUrl { get; set; }
        public string AppId { get; set; }
        public string AppKey { get; set; }
    }
}
