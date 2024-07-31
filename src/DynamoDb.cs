using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;

public class DynamoDb : IDynamoDb {

  private readonly IAmazonDynamoDB _dynamoDb;
  private readonly DbSettings _dbSettings;
 
  public DynamoDb(IAmazonDynamoDB dynamoDb, DbSettings dbSettings){
    _dynamoDb = dynamoDb;
    _dbSettings = dbSettings;
  }
  public async Task<bool> Add(Song song){
    var songJson = JsonSerializer.Serialize(song);
    var songDocument = Document.FromJson(songJson);
    var songAttribute = songDocument.ToAttributeMap();

    var putRequest = new PutItemRequest(){
      TableName = _dbSettings.SongsTable,
      Item = songAttribute
    };

    var response = await _dynamoDb.PutItemAsync(putRequest);
    return response.HttpStatusCode == HttpStatusCode.OK;
  }
}