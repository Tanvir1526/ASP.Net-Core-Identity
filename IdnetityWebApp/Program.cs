using IdnetityWebApp.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();



//adding authentication
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", option =>
{
    option.Cookie.Name = "MyCookieAuth";
  /*  option.LoginPath = "/Account/Login";
    option.AccessDeniedPath = "/Account/AccessDenied";*/
 option.ExpireTimeSpan=TimeSpan.FromMinutes(2);
});
builder.Services.AddAuthorization(option=>
{
    option.AddPolicy("HRDerpartment", policy => 
        policy.RequireClaim("Department", "HR"));
   
    option.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Admin"));

    option.AddPolicy("HRManagerOnly", policy =>policy.
            RequireClaim("Department", "HR").
            RequireClaim("HRManager").
            Requirements.Add(new HRManagerProbationRequirement(3)));
});

builder.Services.AddSingleton<IAuthorizationHandler,HRManagerProbationRequirementsHandeler>();

//adding HttpClint
builder.Services.AddHttpClient("OurWebApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:44332/");
});
//adding session

builder.Services.AddSession(option =>
{
    option.Cookie.HttpOnly = true;
    option.IdleTimeout = TimeSpan.FromHours(8);
    option.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.Run();
