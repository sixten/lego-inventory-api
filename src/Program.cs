using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Sfko.Lego.DbModel.LegoContext>(options => options
  .UseSqlite(builder.Configuration.GetConnectionString("LegoDatabase"))
  .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if( app.Environment.IsDevelopment() ) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// no auth for this demo
//app.UseAuthorization();

app.MapControllers();

app.Run();
