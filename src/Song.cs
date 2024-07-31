using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("lyricguesser-songs")]
public class Song{
  [DynamoDBHashKey("id")]
  public int? Id { get; set;}
  [DynamoDBProperty("name")]
  public string? Name { get; set;}
    [DynamoDBProperty("artist")]
  public string? Artist { get; set;}
    [DynamoDBProperty("lyrics")]
  public string[]? Lyrics { get; set;}

}