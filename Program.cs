using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



string configFile = "config.json";
string json = File.ReadAllText(configFile);
Config config = JsonSerializer.Deserialize<Config>(json)!;

// Added CORS
builder.Services.AddCors(options => {
    options.AddPolicy("FrontEnd", policyBuilder => {
        policyBuilder.WithOrigins(config.website_url);
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/getRandomSong", async () =>
{
    DataController httpClient = new();

    SongSelector selector = new();
    string randomSongUrl = selector.SelectRandomSong();

    var lyrics = await httpClient.GetLyrics(randomSongUrl);

    var okResult = lyrics as OkObjectResult;

    if (okResult != null && okResult.StatusCode == 200 && okResult.Value != null){
        Parser parser = new();
        var result = parser.ParseLyrics(okResult.Value.ToString());
        var songName = parser.ParseSong(okResult.Value.ToString());
        var artist = parser.ParseArtist(okResult.Value.ToString());
        Song song = new Song(songName,artist,result);
        return song;
    }
    

    return null;
})
.WithName("Lyrics")
.WithOpenApi();

//Enabling CORS
app.UseCors("FrontEnd");

app.Run();