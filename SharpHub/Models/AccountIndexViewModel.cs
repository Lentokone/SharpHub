namespace SharpHub.Models
{
    public class AccountIndexViewModel
    {
        public UserLoginViewModel Login { get; set; } = new UserLoginViewModel();
        public UserRegisterViewModel Register { get; set; } = new UserRegisterViewModel();

    }
}
