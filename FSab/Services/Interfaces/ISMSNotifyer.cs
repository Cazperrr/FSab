namespace FSab.Services.Interfaces
{
    public interface ISMSNotifyer
    {
        Task SendSMSAsync(string message);
    }
}
