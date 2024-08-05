using Amazon;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

string configFile = "config.json";
string json = File.ReadAllText(configFile);
Config config = JsonSerializer.Deserialize<Config>(json)!;

DataController httpClient = new();

//Setup DynamoDB
AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
clientConfig.RegionEndpoint = RegionEndpoint.APSoutheast2;
AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig); 

SongSelector selector = new();
string[] allSongs = selector.GetAllSongs();

foreach (var song in allSongs){
    Console.WriteLine(song);
    var task = AddSong(song);
    task.Wait();
}

async Task AddSong(string url){
    var lyrics = await httpClient.GetLyrics(url);

    var okResult = lyrics as OkObjectResult;
    
    if (okResult != null && okResult.StatusCode == 200 && okResult.Value != null){
        Parser parser = new();
        Song song = new();
        song.Lyrics = [.. parser.ParseLyrics(okResult.Value.ToString())];
        song.Name = parser.ParseSong(okResult.Value.ToString());
        song.Artist = parser.ParseArtist(okResult.Value.ToString());
        song.Url = url;
        song.id = Guid.NewGuid().ToString();
        var result = await DynamoDb.PutSong(client,song,"lyricguesser-songs");
    }
}
