using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Azure.Cosmos;
using System.Data.SqlClient;

namespace Application.Migrate
{
    class Color {
        public Color(int ColorID, string ColorName, int LastEditedBy, DateTime ValidFrom, DateTime ValidTo)
        {
            this.id = Guid.NewGuid().ToString();
            this.ColorID = ColorID;
            this.ColorName = ColorName;
            this.LastEditedBy = LastEditedBy;
            this.ValidFrom = ValidFrom;
            this.ValidTo = ValidTo;
        }

        public string id {get; set;}
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
    class Program
    {
        private const string sqlDBConnectionString = "";
        private const string cosmosDBConnectionString = "";
        static async Task Main(string[] args)
        {
            List<Color> items = await GetSqlDatabaseItems();
            //await InsertItemsToAzureCosmosDB(items);

            List<Color> colors = await GetColorsAsync();
            foreach (var color in colors) {
                await Console.Out.WriteLineAsync($"color {color.ColorName}");
            }
        }

        static async Task<List<Color>> GetSqlDatabaseItems()
        {
            List<Color> items = new List<Color>();
            using (SqlConnection connection = new SqlConnection(sqlDBConnectionString)) {
                connection.Open();
                using (SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Warehouse].[Colors]",
                    connection
                )) {
                    await using (SqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            Color color = new Color(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetDateTime(3),
                                reader.GetDateTime(4)
                            );

                            items.Add(color);
                        }
                    }

                    return await Task.FromResult(items);
                }
            }
        }

        static async Task InsertItemsToAzureCosmosDB(List<Color> items)
        {
            using CosmosClient client = new CosmosClient(cosmosDBConnectionString);

            Database db = await client.CreateDatabaseIfNotExistsAsync("Migrate");
            Container container = await db.CreateContainerIfNotExistsAsync(
                "Colors",
                partitionKeyPath: "/ColorID",
                throughput: 400
            );

            int count = 0;
            foreach (var item in items)
            {
                Console.Out.WriteLine($"Item {item.ColorName}"); 
                ItemResponse<Color> document = await container.UpsertItemAsync<Color>(item);
                await Console.Out.WriteLineAsync($"Upserted document #{++count:000} [Activity Id: {document.ActivityId}]");
            }

            await Console.Out.WriteLineAsync($"Total Azure Cosmos DB Documents: {count}");
        }

        static async Task<List<Color>> GetColorsAsync()
        {
            using CosmosClient client = new CosmosClient(cosmosDBConnectionString);
            Container container = client.GetDatabase("Migrate").GetContainer("Colors");
            string query = $@"SELECT * FROM Colors";
            var _iterator = container.GetItemQueryIterator<Color>(query);

            List<Color> matches = new List<Color>();
            while (_iterator.HasMoreResults) {
                var next = await _iterator.ReadNextAsync();
                matches.AddRange(next);
            }

            return matches;
        }
    }
}
