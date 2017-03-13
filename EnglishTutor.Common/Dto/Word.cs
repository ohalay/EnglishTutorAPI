namespace EnglishTutor.Common.Dto
{
    public class Word
    {
        public string Name { get; set; }
        public string Defination { get; set; }
        public string AudioFilePath { get; set; }
        public string PhoneticSpelling { get; set; }
        public string Translation { get; set; }
        public string[] Examples { get; set; }
    }
}
