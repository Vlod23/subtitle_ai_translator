namespace SubtitlesTranslator.Application.Authorization
{
    public static class RoleNames
    {
        public const string Admin = "Admin";
        public const string Support = "Support";
    }

    public static class PermissionNames
    {
        public const string ViewAdminArea = "CanViewAdminArea";
        public const string ManageRoles = "CanManageRoles";
        public const string GenerateInvoices = "CanGenerateInvoices";

        public static readonly string[] All =
        [
            ViewAdminArea,
            ManageRoles,
            GenerateInvoices,
        ];
    }

    public static class AuthorizationConstants
    {
        public const string PermissionClaimType = "permission";
    }
}
