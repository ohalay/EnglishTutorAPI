using System.Collections.Generic;

namespace EnglishTutor.Common.Dto
{
    public class Word
    {
        public string Name { get; set; }
        public string Defination { get; set; }
        public string AudioFilePath { get; set; }
        public string PhoneticSpelling { get; set; }
        public IDictionary<string, string> Translations { get; set; }
        public IEnumerable<string> Examples { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
