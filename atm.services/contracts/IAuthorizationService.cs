namespace atm.services
{
    public interface IAuthorizationService
    {
        bool IsValiduser(string userName, string filePath);
        bool IsApprover(string userName);
    }
}
