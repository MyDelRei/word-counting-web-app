using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WordCounting.Models
{
    public class UploadModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile UploadFile { get; set; }
    }

   
}
