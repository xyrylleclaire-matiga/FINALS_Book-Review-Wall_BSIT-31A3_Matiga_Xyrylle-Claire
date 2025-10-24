using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Library_Management.Data;
using Library_Management.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Connection string (check your appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ??
    throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// ✅ Register DbContext for EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// ✅ Register Identity (user accounts)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// ✅ Add MVC and Razor support
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ✅ Register your custom services (THIS FIXES YOUR ERROR)
builder.Services.AddScoped<BookService>(); // BookService DI registration
builder.Services.AddScoped<ILibraryService, LibraryService>(); // LibraryService DI registration

var app = builder.Build();

// ✅ Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Auth middlewares must come here
app.UseAuthentication();
app.UseAuthorization();

// ✅ Map routes and Razor Pages
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
