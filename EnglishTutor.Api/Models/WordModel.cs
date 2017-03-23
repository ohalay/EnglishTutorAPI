using System.Collections.Generic;

namespace EnglishTutor.Api.Models
{
    public class WordModel
    {
        public string Name { get; set; }
        public string Defination { get; set; }
        public string AudioFilePath { get; set; }
        public string PhoneticSpelling { get; set; }
        public string Translation { get; set; }
        public IEnumerable<string> Examples { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
