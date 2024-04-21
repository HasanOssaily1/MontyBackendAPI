namespace MontyBackendAPI.Dto
{
    public class JWT
    {
        public string? accessToken { get; set;}
        public string? refreshToken { get; set;}

        public JWT(string accessToken, string refreshToken)
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }
}
}
