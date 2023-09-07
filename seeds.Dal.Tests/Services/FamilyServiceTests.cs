using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class FamilyServiceTests
{
    private readonly IDalBaseService baseService;
    private readonly FamilyService service;
    public FamilyServiceTests()
    {
        baseService = A.Fake<IDalBaseService>();
        service = new(baseService);
    }

    [Fact]
    public async Task FamilyService_GetFamiliesAsync_ReturnsItselfs()
    {
        // Arrange
        string key = "ABC", name = "ABeCe";
        List<FamilyFromDb> fams = new()
        {
           new() { CategoryKey = key, Name = "abceee" },
            new() { CategoryKey = "BLA", Name = name },
        };
        A.CallTo(() => baseService.GetDalModelAsync<List<FamilyFromDb>>(A<string>.Ignored))
            .Returns(fams);

        // Act
        var result = await service.GetFamiliesAsync();

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(2);
        result?[0]?.CategoryKey.Should().Be(key);
        result?[1]?.Name.Should().Be(name);
    }
    [Fact]
    public async Task FamilyService_GetFamiliesAsync_IfBaseReturnsNullThrows()
    {
        // Arrange
        A.CallTo(() => baseService.GetDalModelAsync<List<FamilyFromDb>>(A<string>.Ignored))
            .Returns<List<FamilyFromDb>?>(null);

        // Act
        Func<Task> act = async () => await service.GetFamiliesAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
