using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Interfaces;

public interface ICategoryService
{
    public Task<Category?> GetCategoryByKeyAsync(string categoryKey);
}
