using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oxagile.Internal.Api.Dtos;
using Oxagile.Internal.Api.Entities;
using Oxagile.Internal.Api.Repositories;

namespace Oxagile.Internal.Api.Controllers
{
    [Route("api/companies/{id:int}")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public CompanyController(
            ICompanyRepository companyRepository,
            IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (ModelState.IsValid)
            {
                var company = await companyRepository.Get(id);
                return Ok(mapper.Map<GetCompanyDto>(company));    
            }

            return BadRequest(ModelState);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(int id)
        {
            if (ModelState.IsValid)
            {
                var company = await companyRepository.Get(id);
                return Ok(mapper.Map<IEnumerable<GetCompanyUserDto>>(company.Users));    
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody]EditCompanyDto company)
        {
            if (ModelState.IsValid)
            {
                var existing = await companyRepository.Get(id);
                if (existing == null)
                {
                    return NotFound();
                }

                existing.Name = company.Name;

                var updated = await companyRepository.Update(existing);
                return Ok(mapper.Map<GetCompanyDto>(updated));
            }
            
            return BadRequest(ModelState);
        }

        [HttpDelete]
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

            return BadRequest(ModelState);
        }
    }
}