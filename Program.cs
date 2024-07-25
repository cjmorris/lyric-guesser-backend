using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Added CORS
builder.Services.AddCors(options => {
    options.AddPolicy("FrontEnd", policyBuilder => {
        policyBuilder.WithOrigins("http://localhost:5173");
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


app.MapGet("/lyrics", async () =>
{
    DataController httpClient = new();

    SongSelector selector = new();
    string randomSongUrl = selector.SelectRandomSong();

    var lyrics = await httpClient.GetLyrics("https://genius.com/Ac-dc-back-in-black-lyrics");

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