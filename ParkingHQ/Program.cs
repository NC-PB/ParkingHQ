using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess;
using ParkingHQ.DataAccess.Repository;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ParkingUtility>();
builder.Services.AddScoped<TenantUtility>();
builder.Services.AddScoped<TransactionUtility>();
builder.Services.AddScoped<EntryExitUtility>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
