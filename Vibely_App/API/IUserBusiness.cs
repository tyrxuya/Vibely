﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibely_App.Data.Models;

namespace Vibely_App.API
{
    public interface IUserBusiness : IBusiness<User>
    {
        User? FindByCredentials(string username, string password);
        bool IsUsernameTaken(string username);
    }
}
