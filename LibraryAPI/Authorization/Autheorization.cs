using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Authorization
{
    public class Autheorization
    {
        public enum Roles
        {
            Administrator,
            User
        }
        public const Roles default_role = Roles.User;
    }
}

