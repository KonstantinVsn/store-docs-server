using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using StoreDocApi.Models;

namespace StoreDocApi.DbContext
{
    public class FileContext
    {
        private readonly IMongoDatabase _database = null;
        private readonly GridFSBucket _bucket = null;

        public FileContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.Database);

                var gridFSBucketOptions = new GridFSBucketOptions()
                {
                    BucketName = "images",
                    ChunkSizeBytes = 1048576, // 1МБ
                };

                _bucket = new GridFSBucket(_database, gridFSBucketOptions);

            }
        }

        public IMongoCollection<FileBlock> Notes
        {
            get
            {
                return _database.GetCollection<FileBlock>("FileBlock");
            }
        }

        public GridFSBucket Bucket
        {
            get
            {
                return _bucket;
            }
        }
    }
}
