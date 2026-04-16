using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftHub.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"));
// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Add Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();


// ✅ IMPORTANT: Auto migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Apply migrations automatically
        context.Database.Migrate();

        // Optional: create default roles
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                roleManager.CreateAsync(new IdentityRole(role)).Wait();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Migration error: " + ex.Message);
    }
}


// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();





//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using SoftHub.Data;

//var builder = WebApplication.CreateBuilder(args);

//// 🔗 ربط قاعدة البيانات
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlite("Data Source=app.db"));

//// 🔐 Identity + Roles
//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//})
//.AddRoles<IdentityRole>() // 🔥 مهم جداً
//.AddEntityFrameworkStores<ApplicationDbContext>();

//// MVC
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// ⚙️ إعدادات التطبيق
//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication(); // 🔐
//app.UseAuthorization();  // 🔐

//// 🧭 Routing
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapRazorPages();

//// 👑 إنشاء Admin تلقائياً
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

//    string roleName = "Admin";

//    // إنشاء الدور
//    if (!roleManager.RoleExistsAsync(roleName).Result)
//    {
//        roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
//    }

//    // إنشاء الأدمن
//    string email = "admin@soft.com";
//    string password = "Admin123!";

//    var user = userManager.FindByEmailAsync(email).Result;

//    if (user == null)
//    {
//        user = new IdentityUser
//        {
//            UserName = email,
//            Email = email,
//            EmailConfirmed = true
//        };

//        userManager.CreateAsync(user, password).Wait();
//    }

//    // إعطاء صلاحية Admin
//    if (!userManager.IsInRoleAsync(user, roleName).Result)
//    {
//        userManager.AddToRoleAsync(user, roleName).Wait();
//    }
//}
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<ApplicationDbContext>();

//    context.Database.Migrate();
//}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var context = services.GetRequiredService<ApplicationDbContext>();
//    context.Database.Migrate();

//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

//    string[] roles = { "Admin", "User" };

//    foreach (var role in roles)
//    {
//        if (!roleManager.RoleExistsAsync(role).Result)
//        {
//            roleManager.CreateAsync(new IdentityRole(role)).Wait();
//        }
//    }
//}
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    try
//    {
//        var context = services.GetRequiredService<ApplicationDbContext>();

//        Console.WriteLine("Applying migrations...");
//        context.Database.Migrate();
//        Console.WriteLine("Migrations applied!");

//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine("Migration ERROR: " + ex.Message);
//    }
//}
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    context.Database.Migrate();
//}
//app.Run();