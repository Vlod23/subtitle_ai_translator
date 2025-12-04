using System.ComponentModel.DataAnnotations;

namespace SubtitlesTranslator.Areas.Admin.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public List<RoleSelectionViewModel> Roles { get; set; } = new();

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [MinLength(8, ErrorMessage = "Password should be at least 8 characters long.")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
