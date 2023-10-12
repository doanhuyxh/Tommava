using System.ComponentModel.DataAnnotations;

namespace Tommava.Models
{
    public class TimeLineVideo
    {
        [Key]
        public int Id { get; set; }
        public int VideoId { get; set; } 
        public string TimePoint { get; set; }
        public string Vietnamese { get; set; }
        public string English { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
