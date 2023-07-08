using seeds.Dal.Model;
using seeds1.MauiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds1.Services;

public interface IFeedEntryService
{
    public User CurrentUser { get; set; }
    public Task<List<FeedEntry>> GetFeedEntriesPaginated(int page, int maxPageSize);
}
