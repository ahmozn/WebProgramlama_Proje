using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace webProgProje.Areas.Identity.Data;

// Add profile data for application users by adding properties to the DbKullanici class
public class DbKullanici : IdentityUser
{
    public string kulaniciad{ get; set; }
}

