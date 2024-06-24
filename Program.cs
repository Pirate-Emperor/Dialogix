using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.FileProviders;
using Dialogix.AppCode;
using Dialogix.Controllers;
using Dialogix.Hubs;
using Dialogix.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<SessionAdmin>();
builder.Services.AddScoped<RedirectIfLoggedInAttribute>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IChatHub, ChatHub>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var firebaseConfig = builder.Configuration.GetSection("Firebase").Get<FirebaseConfig>();
var serviceAccountPath = Path.Combine(Directory.GetCurrentDirectory(),
    firebaseConfig.ServiceAccountFile.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar));


if (FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromFile(serviceAccountPath)
    });
}

var app = builder.Build();

// Common Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Login}/{id?}");
});

app.Run();
