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
    public Task<HttpResponseMessage> PutAsync(string url, HttpContent httpContent);
    public Task<HttpResponseMessage> PostAsync(string url, HttpContent httpContent);
}
