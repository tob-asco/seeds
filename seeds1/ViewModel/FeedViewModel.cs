using seeds.Dal.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace seeds1.ViewModel;

//    ...     ( property here   ...   , queryId    ...      )]
[QueryProperty(nameof(CurrentUsername),nameof(User.Username))]
public partial class FeedViewModel : BasisViewModel
{
    public FeedViewModel()
    {
        
    }
}
