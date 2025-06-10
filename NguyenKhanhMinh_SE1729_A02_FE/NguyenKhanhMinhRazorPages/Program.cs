using Microsoft.EntityFrameworkCore;
using NguyenKhanhMinhRazorPages;
using NguyenKhanhMinhRazorPages.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ISystemAccountService, SystemAccountService>();
builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddHttpClient<ITagService, TagService>();
builder.Services.AddHttpClient<INewsArticleService, NewsArticleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();//

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/", async (context) => context.Response.Redirect("/Login"));

app.MapHub<SignalrServer>("/signalrServer");

app.Run();
