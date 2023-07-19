using FakeItEasy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Tests.Controllers;

public class CategoriesControllerTests : ApiBaseControllerTests
{
    public List<Category> Categories { get; set; } = new();

    public CategoriesControllerTests()
    {
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 10; i++)
        {
            Categories.Add(
            new Category()
            {
                Key = $"Cat{i}",
                Name = $"Category{i}"
            });
        }
        if(!_context.Category.Any()) { _context.Category.AddRange(Categories); }
    }


}
