using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

public class DynamoDb {

   public static async Task<bool> PutSong(AmazonDynamoDBClient client, Song song, string tableName){
        var songJson = JsonSerializer.Serialize(song);
        var songDocument = Document.FromJson(songJson);
        var songAttribute = songDocument.ToAttributeMap();
        
        var putRequest = new PutItemRequest(){
            TableName = tableName,
            Item = songAttribute
        };

        var response = await client.PutItemAsync(putRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }
}