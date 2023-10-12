using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.GenreVM
{
    public class GenreVM
    {
        public int Id { get; set; }
        [Display(Name = "Tên thể loại")]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator GenreVM(Genre trademark)
        {
            return new GenreVM
            {
                Id = trademark.Id,
                Name = trademark.Name,
                CreatedDate = trademark.CreatedDate,
                IsDeleted = trademark.IsDeleted,
            };
        }
        public static implicit operator Genre(GenreVM vm)
        {
            return new Genre
            {
                Id = vm.Id,
                Name = vm.Name,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
