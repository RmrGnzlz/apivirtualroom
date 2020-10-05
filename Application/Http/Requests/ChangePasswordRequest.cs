using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class ChangePasswordRequest : BaseRequest
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
    }
    public class RecoverPasswordRequest : BaseRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
    public class RecoveryCodeRequest : BaseRequest
    {
        public string Username { get; set; }
        public string RecoveryCode { get; set; }
    }
}