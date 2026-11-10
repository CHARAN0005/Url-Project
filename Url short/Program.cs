using Url;
using UrlTable;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<UrlTableService>();

builder.Services.AddScoped<IUrl,UrlService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{

}

using (var scope = app.Services.CreateScope())
{
    var dbService = scope.ServiceProvider.GetRequiredService<UrlTableService>();

    await dbService.CreateUrlTableAsync();
}



app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.Run();
