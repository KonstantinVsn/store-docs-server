using StoreDocApi.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver.GridFS;
using StoreDocApi.Models;
using StoreDocApi.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace StoreDocApi.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly FileContext _context = null;

        public FileRepository(IOptions<Settings> settings)
        {
            _context = new FileContext(settings);
        }

        public async Task<IEnumerable<FileBlock>> GetAllNotes()
        {
            try
            {
                return await _context.Notes.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<FileBlock> GetNote(string id)
        {
            var filter = Builders<FileBlock>.Filter.Eq("Id", id);

            try
            {
                return await _context.Notes
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddNote(FileBlock item)
        {
            try
            {
                await _context.Notes.InsertOneAsync(item);

            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<DeleteResult> RemoveNote(string id)
        {
            try
            {
                return await _context.Notes.DeleteOneAsync(
                     Builders<FileBlock>.Filter.Eq("Id", id));
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<UpdateResult> UpdateNote(string id, string body)
        {
            var filter = Builders<FileBlock>.Filter.Eq(s => s.Id, id);
            var update = Builders<FileBlock>.Update
                            .Set(s => s.FileBody, body)
                            .CurrentDate(s => s.Date);

            try
            {
                return await _context.Notes.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ReplaceOneResult> UpdateNote(string id, FileBlock item)
        {
            try
            {
                return await _context.Notes
                            .ReplaceOneAsync(n => n.Id.Equals(id)
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // Demo function - full document update
        public async Task<ReplaceOneResult> UpdateNoteDocument(string id, string body)
        {
            var item = await GetNote(id) ?? new FileBlock();
            //item.Body = body;
            //item.UpdatedOn = DateTime.Now;

            return await UpdateNote(id, item);
        }

        public async Task<DeleteResult> RemoveAllNotes()
        {
            try
            {
                return await _context.Notes.DeleteManyAsync(new BsonDocument());
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ObjectId> UploadFile(List<IFormFile> files)
        {
            try
            {
                var file = files.FirstOrDefault();
                var stream = file.OpenReadStream();
                var filename = file.FileName;
                int signatureId = 10;

                var url = $"https://test.ukey.net.ua:3020/api/v1/signatures/file/{signatureId}?mode=full";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "w5n77v3GRLGq6RJMybpVeaPv6V7sogG3");
                    using (HttpResponseMessage res = await client.GetAsync(url))
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            Console.WriteLine(data);
                        }
                    }
                }


                return await _context.Bucket.UploadFromStreamAsync(filename, stream);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                return new ObjectId(ex.ToString());
            }
        }

        public async void UploadFile(IFormFileCollection files, string signatureId)
        {
            foreach (var file in files)
            {
                //using (var fileStream = new FileStream(path, FileMode.Create))
                //{
                //    await file.CopyToAsync(fileStream);
                //}
            }
        }




        public async Task<String> GetFileInfo(string id)
        {
            GridFSFileInfo info = null;
            var objectId = new ObjectId(id);
            try
            {
                using (var stream = await _context.Bucket.OpenDownloadStreamAsync(objectId))
                {
                    info = stream.FileInfo;
                }
                return info.Filename;
            }
            catch (Exception)
            {
                return "Not Found";
            }
        }
    }
}
