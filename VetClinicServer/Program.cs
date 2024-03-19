using Microsoft.EntityFrameworkCore;
using VetClinicServer.Data;
using VetClinicServer.Repositories;
using VetClinicServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<DrugRepository>();
builder.Services.AddTransient<DrugService>();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
