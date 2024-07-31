using Microsoft.AspNetCore.Mvc;


public interface IDynamoDb
{
    Task<bool> Add(Song song);
} 