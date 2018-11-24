using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using StoreDocApi.Models;

namespace StoreDocApi.Interfaces
{
    public interface IFileRepository
    {
        Task<IEnumerable<FileBlock>> GetAllNotes();
        Task<FileBlock> GetNote(string id);
        Task AddNote(FileBlock item);
        Task<DeleteResult> RemoveNote(string id);

        Task<UpdateResult> UpdateNote(string id, string body);

        // demo interface - full document update
        Task<ReplaceOneResult> UpdateNoteDocument(string id, string body);

        // should be used with high cautious, only in relation with demo setup
        Task<DeleteResult> RemoveAllNotes();

        Task<ObjectId> UploadFile(List<IFormFile> files);
        void UploadFile(IFormFileCollection files, string signatureId);
        Task<String> GetFileInfo(string id);
    }
}
