using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(o => {
  o.ConstraintMap.Add("setNum", typeof(Sfko.Lego.Routing.SetNumberRouteConstraint));
});
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
  c.SwaggerDoc("v1", new OpenApiInfo {
    Title = "LEGO Inventory API",
    Version = "v1",
    Description = "A simple demo of a .NET web API",
    Contact = new OpenApiContact {
      Name = "Sixten Otto",
      Url = new ("https://github.com/sixten"),
    },
  });

  var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

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
