namespace VerificationSystem.DB
{
    public class ApplicationSettings
    {
        [System.ComponentModel.DataAnnotations.Key]
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}