using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using StoreDocApi.Interfaces;
using StoreDocApi.Models;

namespace StoreDocApi.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json", "multipart/form-data")]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileRepository _fileRepo;

        public FilesController(IFileRepository repo)
        {
            _fileRepo = repo;
        }

        [NoCache]
        [HttpGet]
        public Task<IEnumerable<FileBlock>> Get()
        {
            return GetNoteInternal();
        }

        private async Task<IEnumerable<FileBlock>> GetNoteInternal()
        {
            return await _fileRepo.GetAllNotes();
        }

        // GET api/notes/5
        [HttpGet("{id}")]
        public Task<FileBlock> Get(string id)
        {
            return GetNoteByIdInternal(id);
        }

        private async Task<FileBlock> GetNoteByIdInternal(string id)
        {
            return await _fileRepo.GetNote(id) ?? new FileBlock();
        }

        // POST api/notes
        [HttpPost]
        public void Post([FromBody]string value)
        {
           // _fileRepo.AddNote(new FileBlock() { Body = value, CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now });
        }

        // PUT api/notes/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {
            _fileRepo.UpdateNoteDocument(id, value);
        }

        // DELETE api/notes/23243423
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _fileRepo.RemoveNote(id);
        }

        // POST api/notes/uploadFile
        [HttpPost("uploadFile")]
        public async Task<ObjectId> UploadFile(List<IFormFile> files)
        {
            return await _fileRepo.UploadFile(files);
        }
        // GET api/notes/getFileInfo/d1we24ras41wr
        [HttpGet("getFileInfo/{id}")]
        public Task<String> GetFileInfo(string id)
        {
            return _fileRepo.GetFileInfo(id);
        }

        [HttpPost]
        [Route("upload")]
        public async Task PostOrUpdateAsync(string signatureId)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                await _fileRepo.UploadFile(files, signatureId);
            }
        }


    }
    public class FileInputModel
    {
        public IFormFile File { get; set; }
        public string Param { get; set; }
    }
}
