using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Services;

public interface ICategoryUserPreferenceService
{
    public Task<CategoryUserPreference> GetCategoryUserPreferenceAsync(
        string categoryKey,
        string username
        );
    // returns a bool of success
    public Task<bool> PutCategoryUserPreferenceAsync(
        string categoryKey,
        string username,
        int newPreference
        );
}
