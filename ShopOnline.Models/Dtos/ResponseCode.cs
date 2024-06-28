using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
    public class ResponseCode
    {
        public const string LoginNotFound = "Login does not exist.";
        public const string LoginDeactivated = "Login is deactivated.";
        public const string PasswordIncorrect = "Password incorrect.";
        public const string UserNotFound = "User does not exist.";
        public const string UserDeactivated = "User is deactivated.";
        public const string SamePassword = "New password is same as existing.";
        public const string SameEmail = "Email already exits.";
        public const string LoginSuccess = "Successfully logged in.";

    }
}
