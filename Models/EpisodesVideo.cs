using System.ComponentModel.DataAnnotations;

namespace Tommava.Models
{
    public class EpisodesVideo
    {
        [Key]
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int EpisodeNumber { get; set; }
        public int ViewCount { get; set; }
        public string TitleEN { get; set; }
        public string TitleVN { get; set; }
        public string Description { get; set; }
        public string VideoLink { get; set; }
        public string ImgLink { get; set; }
        public bool IsDelete { get; set; }
    }
}
