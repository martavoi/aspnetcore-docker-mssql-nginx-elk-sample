using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oxagile.Internal.Api.Dtos;
using Oxagile.Internal.Api.Entities;
using Oxagile.Internal.Api.Repositories;

namespace Oxagile.Internal.Api.Controllers
{
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public CompanyController(
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
            var companies = await companyRepository.Get();
            return Ok(mapper.Map<IEnumerable<GetCompanyDto>>(companies));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (ModelState.IsValid)
            {
                var company = await companyRepository.Get(id);
                return Ok(mapper.Map<GetCompanyDto>(company));    
            }

            return new StatusCodeResult(422);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody]EditCompanyDto company)
        {
            if (ModelState.IsValid)
            {
                var existing = await companyRepository.Get(id);
                if (existing == null)
                {
                    return NotFound();
                }

                existing = mapper.Map<Company>(company);
                existing.Id = id;

                var updated = await companyRepository.Update(existing);
                return Ok(mapper.Map<GetCompanyDto>(updated));
            }
            
            return new StatusCodeResult(422);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCompanyDto company)
        {
            if (ModelState.IsValid)
            {
                var @new = await companyRepository.Create(mapper.Map<Company>(company));
                return CreatedAtAction("Get", new { id = @new.Id }, mapper.Map<GetCompanyDto>(@new));
            }

            return new StatusCodeResult(422);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var existing = await companyRepository.Get(id);
                if (existing == null)
                {
                    return NotFound();
                }

                var result = await companyRepository.Delete(id);
                return Ok();
            }

            return new StatusCodeResult(422);
        }
    }
}