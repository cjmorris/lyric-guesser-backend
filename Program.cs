using Amazon;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
    Song song = new();
    song.Lyrics = [.. parser.ParseLyrics(okResult.Value.ToString())];
    song.Name = parser.ParseSong(okResult.Value.ToString());
    song.Artist = parser.ParseArtist(okResult.Value.ToString());
    song.id = "bdjhasbyukhabsjaskhj";
    // AmazonDynamoDBClient client = new(RegionEndpoint.APSoutheast2);
    // DbSettings settings = new();
    // DynamoDb db = new(client,settings);
    // var success = await db.Add(song);

    AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
    // This client will access the US East 1 region.
    clientConfig.RegionEndpoint = RegionEndpoint.APSoutheast2;
    AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig); 

    var result =  await Test.PutSong(client,song,"lyricguesser-songs");
    Console.WriteLine(result);
}


