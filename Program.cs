using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using productExpiry_system.entities;
using productExpiry_system.Interface;
using productExpiry_system.Interface.Repository;
using productExpiry_system.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//adding dbcontext
builder.Services.AddDbContext<DBproduct>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsConnection")));
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<DBproduct>()
               .AddDefaultTokenProviders();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
