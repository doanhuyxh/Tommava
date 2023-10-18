using Tommava.Models.videoVM;
using Tommava.Models.CategoryVM;

namespace Tommava.Models.CombinedViewModel
{
    public class CombinedViewModel
    {
        public List<VideoVM> VideoData { get; set; }
        public List<CategoryVM.CategoryVM> CategoryData { get; set; }
        public VideoVM VideoVMOj { get; set; }
    }
}
