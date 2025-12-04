using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using Stripe;
using Stripe.Checkout;
using SubtitlesTranslator.Application.Authorization;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.Services;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Infrastructure.Identity;
using SubtitlesTranslator.Infrastructure.Repositories;
using SubtitlesTranslator.Infrastructure.Services;
using SubtitlesTranslator.Infrastructure.Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration 
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets<Program>(optional: true) // take sensitive details from secret keys instead of appsettings
    .AddEnvironmentVariables();

QuestPDF.Settings.License = LicenseType.Community;
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.Configure<IdentityOptions>(options => {
    options.User.RequireUniqueEmail = true; // Povinný unikátny email
});
builder.Services.AddDefaultIdentity<ApplicationUser>(
    options => {
        options.SignIn.RequireConfirmedAccount = true;
        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PermissionNames.ViewAdminArea, policy =>
        policy.RequireClaim(AuthorizationConstants.PermissionClaimType, PermissionNames.ViewAdminArea));
    options.AddPolicy(PermissionNames.ManageRoles, policy =>
        policy.RequireClaim(AuthorizationConstants.PermissionClaimType, PermissionNames.ManageRoles));
    options.AddPolicy(PermissionNames.GenerateInvoices, policy =>
        policy.RequireClaim(AuthorizationConstants.PermissionClaimType, PermissionNames.GenerateInvoices));
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddControllersWithViews()
        .AddRazorRuntimeCompilation(); // Moznost vidiet zmeny vo view hned cez refresh
}
else
{
    builder.Services.AddControllersWithViews();
}

builder.Services.AddHttpClient<IChatGptService, ChatGptService>(client => {
    client.Timeout = TimeSpan.FromMinutes(20); // ⏰ timeout
});

// Hangfire for background jobs
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// Redirect identity sender to custom mail service
builder.Services.AddTransient<IEmailSender, SesMailKitEmailService>();

// Interfaces
builder.Services.AddScoped<IEmailService, SesMailKitEmailService>();
builder.Services.AddScoped<IChatGptService, ChatGptService>();
builder.Services.AddScoped<ISubtitleRepository, EFSubtitleRepository>();
builder.Services.AddScoped<IUserSubtitleService, EFUserSubtitleService>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<ICreditEstimator, CreditEstimator>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<ISubtitleLikeRepository, EFSubtitleLikeRepository>();
builder.Services.AddScoped<IStripeClientAdapter, StripeClientAdapter>();
builder.Services.AddScoped<IInvoicingProfileService, InvoicingProfileService>();
builder.Services.AddScoped<IInvoicingProfileRepository, EFInvoicingProfileRepository>();
builder.Services.AddScoped<IPaymentTransactionRepository, EFPaymentTransactionRepository>();
builder.Services.AddScoped<IInvoiceCounterRepository, EFInvoiceCounterRepository>();
builder.Services.AddScoped<IQuestPDFGenerator, QuestPDFGenerator>();
builder.Services.AddScoped<ILanguageService, LanguageService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddSingleton<CustomerService>();
builder.Services.AddSingleton<SessionService>();

// Use cases
builder.Services.AddScoped<CreateDummyRecordUseCase>();
builder.Services.AddScoped<BackgroundTranslateUseCase>();
builder.Services.AddScoped<ToggleSubtitleVisibilityUseCase>();
builder.Services.AddScoped<ToggleSubtitleLikeUseCase>();
builder.Services.AddScoped<UpdateSubtitleUseCase>();
builder.Services.AddScoped<DeleteSubtitleUseCase>();
builder.Services.AddScoped<DownloadSubtitleUseCase>();
builder.Services.AddScoped<GetPublicSubtitlesUseCase>();
builder.Services.AddScoped<DeleteAllForeignLikesTiedToUserUseCase>();
builder.Services.AddScoped<UpdateUserCreditsUseCase>();
builder.Services.AddScoped<GetUserStripeTransactionsUseCase>(); // not used. transaction data are now saved in db
builder.Services.AddScoped<SaveStripeCustomerIdUseCase>();
builder.Services.AddScoped<GetUserDashboardDataUseCase>();
builder.Services.AddScoped<CreateCheckoutSessionUseCase>();
builder.Services.AddScoped<UpdateUsernameUseCase>();
builder.Services.AddScoped<DeleteInvoicingProfileUseCase>();
builder.Services.AddScoped<CreatePaymentTransactionUseCase>();
builder.Services.AddScoped<GenerateNextInvoiceNumberUseCase>();
builder.Services.AddScoped<GetUserTransactionsUseCase>();
builder.Services.AddScoped<GeneratePDFInvoiceUseCase>();
builder.Services.AddScoped<SendMailWithInvoiceUseCase>();

// Google auth
builder.Services.AddAuthentication()
    .AddGoogle(options => {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });


//Stripe
builder.Services.AddScoped<SessionService>();

var app = builder.Build();

await IdentityDataSeeder.SeedAsync(app.Services, app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
