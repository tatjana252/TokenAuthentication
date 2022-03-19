using Microsoft.AspNetCore.Identity;
using System;

namespace Domain
{
    public class Person : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
