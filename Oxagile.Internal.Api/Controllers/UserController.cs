using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oxagile.Internal.Api.Dtos;
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
        public async Task<IActionResult> Get()
        {
            var users = await userRepository.Get();
            return Ok(mapper.Map<IEnumerable<GetUserDto>>(users));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (ModelState.IsValid)
            {
                var user = await userRepository.Get(id);
                return Ok(mapper.Map<GetUserDto>(user));
            }

            return new StatusCodeResult(422);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody]EditUserDto user)
        {
            if (ModelState.IsValid)
            {
                var existing = await userRepository.Get(id);
                if (existing == null)
                {
                    return NotFound();
                }

                if (existing.CompanyId != user.CompanyId)
                {
                    var company = await companyRepository.Get(user.CompanyId);
                    if (company == null)
                    {
                        return BadRequest();
                    }
                }

                existing.Name = user.Name;
                existing.Surname = user.Surname;
                existing.BirthDate = user.BirthDate;
                existing.CompanyId = user.CompanyId;
                existing.Email = user.Email;

                var updated = await userRepository.Update(existing);
                return Ok(mapper.Map<GetUserDto>(updated));
            }
            
            return new StatusCodeResult(422);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUserDto user)
        {
            if (ModelState.IsValid)
            {
                var company = await companyRepository.Get(user.CompanyId);
                if (company == null)
                {
                    return BadRequest(ModelState);
                }

                var userToCreate = mapper.Map<User>(user);
                var @new = await userRepository.Create(userToCreate);
                @new.Company = company;
                return CreatedAtAction("Get", new { id = @new.Id }, mapper.Map<GetUserDto>(@new));
            }

            return new StatusCodeResult(422);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var existing = await userRepository.Get(id);
                if (existing == null)
                {
                    return NotFound();
                }

                var result = await userRepository.Delete(id);
                return Ok();
            }

            return new StatusCodeResult(422);
        }
    }
}