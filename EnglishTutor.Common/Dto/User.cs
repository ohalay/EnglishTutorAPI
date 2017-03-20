namespace EnglishTutor.Common.Dto
{
    public class User
    {
        public string Email { get; set; }

        public Settings Settings { get; set; }

        public string Statistics => string.Empty;

    }
}