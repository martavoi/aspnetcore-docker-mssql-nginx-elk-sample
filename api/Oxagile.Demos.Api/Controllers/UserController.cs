using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Oxagile.Demos.Api.Dtos;
using Oxagile.Demos.Api.Dtos.Validation;
using Oxagile.Demos.Data.Entities;
using Oxagile.Demos.Api.Infrastructure.Extensions;
using Oxagile.Demos.Data.Repositories;
using Oxagile.Demos.Api.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Oxagile.Demos.Data;

namespace Oxagile.Demos.Api.Controllers
{
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IImageProcessor imageProcessor;
        private readonly IBlobStorage blobStorage;
        private readonly IMapper mapper;
        private readonly Settings settings;

        public UserController(
            IUnitOfWork uow,
            IImageProcessor imageProcessor,
            IBlobStorage blobStorage,
            IOptions<Settings> options,
            IMapper mapper)
        {
            this.uow = uow;
            this.imageProcessor = imageProcessor;
            this.blobStorage = blobStorage;
            this.mapper = mapper;
            this.settings = options.Value;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetUserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var users = await uow.User.Get();
            return Ok(mapper.Map<IEnumerable<GetUserDto>>(users));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(GetUserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            if (ModelState.IsValid)
            {
                var user = await uow.User.Get(id);
                if (user == null)
                {
                    return NotFound(new { respose = "error", message = $"user id = {id} does not exist"});
                }

                var userPic = await uow.Media.GetUserPic(id);
                var userDto = mapper.Map<GetUserDto>(user);
                userDto.PicUrl = userPic == null ? null : Url.ActionUserPic(userPic.BlobPath);

                return Ok(userDto);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(GetUserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Edit(int id, [FromBody]EditUserDto user)
        {
            if (ModelState.IsValid)
            {
                var existing = await uow.User.Get(id);
                if (existing == null)
                {
                    return NotFound(new { respose = "error", message = $"user id = {id} does not exist"});
                }

                existing.Name = user.Name;
                existing.Surname = user.Surname;
                existing.BirthDate = user.BirthDate;
                existing.CompanyId = user.CompanyId;
                existing.Email = user.Email;

                var updated = uow.User.Update(existing);
                await uow.CommitAsync();
                
                var userPic = await uow.Media.GetUserPic(id);
                var userDto = mapper.Map<GetUserDto>(updated);
                userDto.PicUrl = userPic == null ? null : Url.ActionUserPic(userPic.BlobPath);

                return Ok(userDto);
            }
            
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetUserDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody]CreateUserDto user)
        {
            if (ModelState.IsValid)
            {
                var company = await uow.Company.Get(user.CompanyId);
                var userToCreate = mapper.Map<User>(user);
                var @new = await uow.User.Create(userToCreate);
                await uow.CommitAsync();
                @new.Company = company;
                return CreatedAtAction("Get", new { id = @new.Id }, mapper.Map<GetUserDto>(@new));
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await uow.User.Get(id);
            if (user == null)
            {
                return NotFound(new { respose = "error", message = $"user id = {id} does not exist"});
            }

            uow.User.Delete(user);
            await uow.CommitAsync();
            return Ok();
        }

        [HttpPost("{id:int}/pic")]
        [SwaggerOperation(operationId: "postuserpic")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Pic(int id, IFormFile file)
        {
            var user = await uow.User.Get(id);
            if (user == null)
            {
                return NotFound(new { respose = "error", message = $"user id = {id} does not exist"});
            }

            using (var memStream = new MemoryStream())
            {
                await file.CopyToAsync(memStream);
                var image = memStream.ToArray();

                var mimeType = imageProcessor.GetImageMimeType(image);
                if (!imageProcessor.SupportedMimeTypes.Contains(mimeType))
                {
                    return BadRequest(new { Result = "error", Message = "unsupported format. the only supported format is JPEG" });
                }

                var blobName = await blobStorage.SaveAsync(image);
                
                var media = await uow.Media.Add(new UserMedia 
                {
                    BlobPath = blobName,
                    Extension = imageProcessor.GetImageExtension(image),
                    Uploaded = DateTime.UtcNow,
                    Rel = MediaRelationType.UserPic,
                    UserId = id
                });
                await uow.CommitAsync();

                return Ok(new { PicUrl = Url.ActionUserPic(blobName)});
            }
        }
    }
}