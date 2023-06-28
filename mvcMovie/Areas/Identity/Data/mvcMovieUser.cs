using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace mvcMovie.Areas.Identity.Data;

// Add profile data for application users by adding properties to the mvcMovieUser class
public class mvcMovieUser : IdentityUser
{
    public string lastName { get; set; }
    public string firstName { get; set; }
}

