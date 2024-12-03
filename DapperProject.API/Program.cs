using DapperProject.API.DataContext;

var builder = WebApplication.CreateBuilder(args);

var connectionString = @"Data Source=.\DB\DapperProjectDB.db";
var context = new DapperDbContext(connectionString);

builder.Services.AddSingleton(context);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

context.InitializeDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
