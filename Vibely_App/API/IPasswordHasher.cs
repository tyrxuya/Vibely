namespace Vibely_App.API
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
    }
}
