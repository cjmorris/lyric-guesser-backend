using Amazon;
using Amazon.DynamoDBv2;
using Amazon.S3;
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

//Setup S3
AmazonS3Config  s3ClientConfig = new AmazonS3Config();
s3ClientConfig.RegionEndpoint = RegionEndpoint.APSoutheast2;
AmazonS3Client s3Client = new AmazonS3Client(s3ClientConfig); 

List<Song> existingSongs = await DynamoDb.GetAllSongs(client,"lyricguesser-songs");
List<string> existingURLs = [];
foreach (var song in existingSongs){
    if (song.Url != null){
        existingURLs.Add(song.Url);
    }
} 

SongSelector selector = new();
List<string> allSongs = selector.GetAllSongs();

var songsToAdd = allSongs.Except(existingURLs);

var index = existingURLs.Count;
if (songsToAdd.Count() != 0)
{
    var s3SongsString = "";
    foreach (var song in existingSongs)
    {
        s3SongsString += song.Artist + "\n" + song.Name + "\n";
    }
    foreach (var song in songsToAdd)
    {
        var task = AddSong(song, index);
        index++;
        task.Wait();
        s3SongsString += task.Result.Artist + "\n" + task.Result.Name + "\n";
        Console.WriteLine($"Added {song}");
    }
    var s3Task = AddSongsToS3(s3SongsString);
    s3Task.Wait();
}
else
{
    Console.WriteLine("No new songs to add");
}



async Task<Song> AddSong(string url, int index)
{
    var lyrics = await httpClient.GetLyrics(url);

    var okResult = lyrics as OkObjectResult;

    Parser parser = new();
    Song song = new();

    if (okResult != null && okResult.StatusCode == 200 && okResult.Value != null)
    {
        song.Lyrics = [.. parser.ParseLyrics(okResult.Value.ToString())];
        song.Name = parser.ParseSong(okResult.Value.ToString());
        song.Artist = parser.ParseArtist(okResult.Value.ToString());
        song.Url = url;
        song.id = index;
        var result = await DynamoDb.PutSong(client, song, "lyricguesser-songs");
    }
    return song;
}

async Task AddSongsToS3(string songs){
    var result = await S3.UploadStringToS3(s3Client,"lyricguesser-allsongs","all_songs.txt",songs);
}