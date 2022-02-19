using Microsoft.EntityFrameworkCore;
using Shaper.API.AuthenticationOptions;
using Shaper.API.RequestHandlers;
using Shaper.API.RequestHandlers.IRequestHandlers;
using Shaper.DataAccess.Context;
using Shaper.DataAccess.Repo;
using Shaper.DataAccess.Repo.IRepo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRequestHandler, RequestHandler>();
builder.Services.AddControllers();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Custom Athentication Configuration. 
builder.AddShaperAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
