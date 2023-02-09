using Microsoft.Extensions.Options;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var conStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
conStrBuilder.Password = builder.Configuration["DbPassword"];
var connection = conStrBuilder.ConnectionString;
// Add services to the container.

// builder.Services.Configure<PersonController>(options => options._connectionString = connection);

builder.Services.AddOptions();
builder.Services.Configure<MyOptions>(options => options.ConnectionString = connection);
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "person",
	pattern: "{controller=Person}/{action=Index}");

app.Run();

public class MyOptions
{
	public string? ConnectionString { get; set; }

}
