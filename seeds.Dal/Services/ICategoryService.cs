using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Services;

public interface ICategoryService
{
    public Task<Category> GetCategoryByKey(string categoryKey);
}
