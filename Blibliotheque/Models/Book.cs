namespace Blibliotheque.Models
{
    using Amazon.DynamoDBv2.DataModel;

    [DynamoDBTable("bibliotheque")]
    public class Book
    {
        [DynamoDBHashKey("id")]
        public int Id { get; set; }

        [DynamoDBProperty("title")]
        public string Title { get; set; } = string.Empty;


        [DynamoDBRangeKey("author")]
        public string Author { get; set; } = string.Empty;

        [DynamoDBProperty("yearpublished")]
        public int YearPublished { get; set; }
    }
}
