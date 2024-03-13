var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Cookie Authentication
builder.Services.AddAuthentication("MyAuthenticationScheme")
    .AddCookie("MyAuthenticationScheme", options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/SignOut";
        options.AccessDeniedPath = "/AccessDenied";
        options.Cookie.Name = "my_cookie_tokenX";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });
builder.Services.AddHttpContextAccessor();

// Add session
builder.Services.AddDistributedMemoryCache();
// Make sessions work
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//-----------------------------------------------------------
// Me, use sessions
app.UseSession();
//-----------------------------------------------------------
// Use Cookie Authentication
// "Configure HTTP Request pipline"
app.UseAuthentication();
//app.MapRazorPages(); // Made some error/problem, seems to work without it
app.MapControllers();

app.UseAuthorization(); // Is used anyway
//-----------------------------------------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=TodayOverview}/{id?}");

app.Run();
