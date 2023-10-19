using Tommava.Models.videoVM;
using Tommava.Models.CategoryVM;
using Tommava.Models.Page;

namespace Tommava.Models.CombinedViewModel
{
    public class CombinedViewModel
    {
        public List<VideoVM> VideoData { get; set; }

        public List<EpisodesVideoVM.EpisodesVideoVM> EpisodesVideoVMData { get; set; }

        public EpisodesVideoVM.EpisodesVideoVM EpisodesVideoOBJ { get; set; }

        public List<CategoryVM.CategoryVM> CategoryData { get; set; }

        public VideoVM VideoVMOj { get; set; }

        public PagedResults<VideoVM> VideoPage { get; set; }
    }
}
