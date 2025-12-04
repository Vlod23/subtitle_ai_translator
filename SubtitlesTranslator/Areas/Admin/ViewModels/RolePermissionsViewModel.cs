namespace SubtitlesTranslator.Areas.Admin.ViewModels
{
    public class RolePermissionsViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<PermissionSelectionViewModel> Permissions { get; set; } = new();
    }
}
