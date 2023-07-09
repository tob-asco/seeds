﻿using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Services;

public interface IUsersService
{
    public Task<List<User>> GetUsersAsync();
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<User?> GetUserByIdAsync(int id);
}
