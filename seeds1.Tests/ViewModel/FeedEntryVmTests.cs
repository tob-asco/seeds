using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.ViewModel;

namespace seeds1.Tests.ViewModel;

public class FeedEntryVmTests
{
    private readonly IGlobalService globalService;
    private readonly IUserIdeaInteractionService uiiService;
    private readonly FeedEntryViewModel vm;

    public FeedEntryVmTests()
    {
        globalService = A.Fake<IGlobalService>();
        uiiService = A.Fake<IUserIdeaInteractionService>();
        vm = new(globalService, uiiService);
    }

    [Theory]
    [InlineData("up")]
    [InlineData("down")]
    public async Task FeedEntryVm_ChangeVote_ChangesUpvotes(string updown)
    {
        // Arrange
        vm.FeedEntry = new();
        vm.FeedEntry.Upvoted = true;
        int upvotes = vm.FeedEntry.Upvotes;
        A.CallTo(() => uiiService.PostOrPutUserIdeaInteractionAsync(
            A<UserIdeaInteraction>.Ignored));

        // Act
        await vm.ChangeVote(updown);

        // Assert
        vm.FeedEntry.Upvotes.Should().NotBe(upvotes);
    }
}
