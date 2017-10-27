using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Oxagile.Internal.Api.Dtos;
using Oxagile.Internal.Api.Dtos.Validation;
using Oxagile.Internal.Api.Entities;
using Oxagile.Internal.Api.Repositories;

namespace Oxagile.Internal.Api.Controllers
{
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserController(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetUserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var users = await userRepository.Get();
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
                var user = await userRepository.Get(id);
                if (user == null)
                {
                    return NotFound(new { respose = "error", message = $"user id = {id} does not exist"});
                }

                return Ok(mapper.Map<GetUserDto>(user));
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
                var existing = await userRepository.Get(id);
                if (existing == null)
                {
                    return NotFound(new { respose = "error", message = $"user id = {id} does not exist"});
                }

                existing.Name = user.Name;
                existing.Surname = user.Surname;
                existing.BirthDate = user.BirthDate;
                existing.CompanyId = user.CompanyId;
                existing.Email = user.Email;

                var updated = await userRepository.Update(existing);
                return Ok(mapper.Map<GetUserDto>(updated));
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
                var company = await companyRepository.Get(user.CompanyId);
                var userToCreate = mapper.Map<User>(user);
                var @new = await userRepository.Create(userToCreate);
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
            var existing = await userRepository.Get(id);
            if (existing == null)
            {
                return NotFound(new { respose = "error", message = $"user id = {id} does not exist"});
            }

            var result = await userRepository.Delete(id);
            return Ok();
        }
    }
}