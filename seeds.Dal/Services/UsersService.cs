using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Services;

public class UsersService : IUsersService
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    public UsersService(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    public async Task<List<User>> GetUsersAsync()
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync("api/users")
                                            .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<User>>()
                .ConfigureAwait(false) ?? throw new NullReferenceException();

            //var response = await _httpClient.GetFromJsonAsync<List<Person>>("api/People");
            //return response ?? throw new NullReferenceException(); //returns response unless it's null
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return await Task.FromException<List<User>>(ex).ConfigureAwait(false);
        }
    }
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync("api/Users/" + username)
                                            .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>()
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // All types of exceptions will land here, e.g.
            // timeout, no such user, server overload, ...
            // not sure if this is expected behaviour. (TODO)
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync("api/users/id/" + id.ToString())
                                            .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>()
                .ConfigureAwait(false) ?? throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
