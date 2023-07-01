using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Wrappers;

public interface IHttpClientWrapper
{
    public Uri BaseAddress { get; set; }
    public Task<HttpResponseMessage> GetAsync(string url);
}
