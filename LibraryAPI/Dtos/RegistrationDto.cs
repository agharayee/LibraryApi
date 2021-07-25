using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class RegistrationDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ErrorMessage { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
