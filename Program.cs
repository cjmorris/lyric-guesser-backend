
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

string configFile = "config.json";
string json = File.ReadAllText(configFile);
Config config = JsonSerializer.Deserialize<Config>(json)!;

DataController httpClient = new();

SongSelector selector = new();
string randomSongUrl = selector.SelectRandomSong();

var lyrics = await httpClient.GetLyrics(randomSongUrl);

var okResult = lyrics as OkObjectResult;

if (okResult != null && okResult.StatusCode == 200 && okResult.Value != null){
    Parser parser = new();
    Song song = new Song();
    song.Lyrics = [.. parser.ParseLyrics(okResult.Value.ToString())];
    song.Name = parser.ParseSong(okResult.Value.ToString());
    song.Artist = parser.ParseArtist(okResult.Value.ToString());
    Console.WriteLine(song.Artist);
    Console.WriteLine(song.Name);
}

