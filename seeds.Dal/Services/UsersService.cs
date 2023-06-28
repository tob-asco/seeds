using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Services;

public class UsersService : IUsersService
{
    private readonly HttpClient _httpClient;
    public UsersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        //_httpClient.BaseAddress = new Uri("http://localhost:5282/"); //w/o Dev tunnel
        _httpClient.BaseAddress = new Uri("https://z4bppc68-5282.uks1.devtunnels.ms/");
    }
    public async Task<List<User>> GetUsers()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/users")
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
    public async Task<User?> GetUserByUsername(string username)
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Users/" + username)
                                            .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>()
                .ConfigureAwait(false) ?? throw new Exception();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public async Task<User?> GetUserById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync("api/users/id/" + id.ToString())
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
