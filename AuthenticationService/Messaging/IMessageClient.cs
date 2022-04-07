namespace AuthenticationService.Messaging{
    public interface IMessageClient{
        void SendEmail(string content);
    }
}