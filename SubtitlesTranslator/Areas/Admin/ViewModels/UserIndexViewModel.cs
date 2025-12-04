namespace SubtitlesTranslator.Areas.Admin.ViewModels
{
    public class UserIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public List<UserListItemViewModel> Users { get; set; } = new();
    }
}
