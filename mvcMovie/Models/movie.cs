using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcMovie.Models
{

    public enum Rating
    {
        PG, G, A0, R13, R16, R18
    }

    public class movie
    {
        public int movieId { get; set; }
        [Display(Name ="Title")]
        public string title { get; set; }
        [Display(Name = "Genre")]
        public string genre { get; set; }
        [Display(Name = "Ratings")] 
        public Rating? rating { get; set; }
        [Display(Name = "Date Released")]
        [DataType(DataType.Date)]
        public DateTime dateReleased { get; set; }
        
        public string fileName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")] 
        public IFormFile imageFile { get; set; }

    }
}
