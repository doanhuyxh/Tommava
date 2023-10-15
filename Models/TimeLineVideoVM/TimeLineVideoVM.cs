using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.TimeLineVideoVM
{
    public class TimeLineVideoVM
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        [Display(Name = "Thời điểm")]
        public string TimePoint { get; set; }
        [Display(Name = "Nội dung tiếng Việt")]
        public string Vietnamese { get; set; }
        [Display(Name = "Nội dung tiếng Anh")]
        public string English { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator TimeLineVideoVM(TimeLineVideo timeLineVideo)
        {
            return new TimeLineVideoVM
            {
                Id = timeLineVideo.Id,
                VideoId = timeLineVideo.VideoId,
                TimePoint = timeLineVideo.TimePoint,
                Vietnamese = timeLineVideo.Vietnamese,
                English = timeLineVideo.English,
                CreatedDate = timeLineVideo.CreatedDate,
                IsDeleted = timeLineVideo.IsDeleted,
            };
        }
        public static implicit operator TimeLineVideo(TimeLineVideoVM vm)
        {
            return new TimeLineVideo
            {
                Id = vm.Id,
                VideoId = vm.VideoId,
                TimePoint = vm.TimePoint,
                Vietnamese = vm.Vietnamese,
                English = vm.English,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
