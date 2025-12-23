namespace Kawerk.Infastructure.ResponseClasses
{
    public class ResponseToken
    {
        public int Status { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public required string msg { get; set; }
    }
}
