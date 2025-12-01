using System.ComponentModel.DataAnnotations;

namespace SubtitlesTranslator.Models
{
    public class UserDetailViewModel
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
    }
}
