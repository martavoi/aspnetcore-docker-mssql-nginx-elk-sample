using System.Collections.Generic;
using System.Net;
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
        [ProducesResponseType(typeof(GetCompanyDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var company = await companyRepository.Get(id);
            if (company == null)
            {
                return NotFound(new { respose = "error", message = $"company id = {id} does not exist"});
            }

            return Ok(mapper.Map<GetCompanyDto>(company));
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(IEnumerable<GetCompanyUserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUsers(int id)
        {
            var company = await companyRepository.Get(id);
            if (company == null)
            {
                return NotFound(new { respose = "error", message = $"company id = {id} does not exist"});
            }

            return Ok(mapper.Map<IEnumerable<GetCompanyUserDto>>(company.Users));
        }

        [HttpPut]
        [ProducesResponseType(typeof(GetCompanyDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Edit(int id, [FromBody]EditCompanyDto company)
        {
            if (ModelState.IsValid)
            {
                var existing = await companyRepository.Get(id);
                if (existing == null)
                {
                    return NotFound(new { respose = "error", message = $"company id = {id} does not exist"});
                }

                existing.Name = company.Name;

                var updated = await companyRepository.Update(existing);
                return Ok(mapper.Map<GetCompanyDto>(updated));
            }
            
            return BadRequest(ModelState);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await companyRepository.Get(id);
            if (existing == null)
            {
                return NotFound(new { respose = "error", message = $"company id = {id} does not exist"});
            }

            var result = await companyRepository.Delete(id);
            return Ok();
        }
    }
}