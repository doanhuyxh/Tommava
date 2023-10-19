using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.EpisodesVideoVM
{
    public class EpisodesVideoVM
    {
        public int Id { get; set; }
        [Display(Name = "Video")]
        public int VideoId { get; set; }
        public string? VideoName { get; set; }
        [Display(Name = "Tập")]
        public int EpisodeNumber { get; set; }
        [Display(Name = "Lượt xem")]
        public int ViewCount { get; set; }
        [Display(Name = "Tên tiếng việt")]
        public string TitleEN { get; set; }
        [Display(Name = "Tên tiếng anh")]
        public string TitleVN { get; set; }
        [Display(Name = "Chi tiết tập")]
        public string Description { get; set; }
        public string VideoLink { get; set; }
        public string ImgLink { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "Video tập phim")]
        public IFormFile? VideoFile { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ImgFile { get; set; }

        public static implicit operator EpisodesVideoVM(EpisodesVideo episodesVideoVM)
        {
            return new EpisodesVideoVM {
                Id = episodesVideoVM.Id,
                VideoId= episodesVideoVM.VideoId,
                EpisodeNumber = episodesVideoVM.EpisodeNumber,
                ViewCount = episodesVideoVM.ViewCount,
                TitleEN = episodesVideoVM.TitleEN,
                TitleVN = episodesVideoVM.TitleVN,
                Description = episodesVideoVM.Description,
                VideoLink = episodesVideoVM.VideoLink,
                ImgLink = episodesVideoVM.ImgLink,
                IsDelete = episodesVideoVM.IsDelete,
            };
        }
        public static implicit operator EpisodesVideo(EpisodesVideoVM vm)
        {
            return new EpisodesVideo
            {
                Id = vm.Id,
                VideoId= vm.VideoId,
                EpisodeNumber = vm.EpisodeNumber,
                TitleEN = vm.TitleEN,
                TitleVN = vm.TitleVN,
                ViewCount= vm.ViewCount,
                Description = vm.Description,
                ImgLink = vm.ImgLink,
                VideoLink = vm.VideoLink,
                IsDelete = vm.IsDelete,
            };
        }
    }
}
