using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oxagile.Internal.Api.Dtos;
using Oxagile.Internal.Api.Entities;
using Oxagile.Internal.Api.Repositories;
using Serilog.Context;

namespace Oxagile.Internal.Api.Controllers
{
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public CompaniesController(
            ICompanyRepository companyRepository,
            IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetCompanyDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var companies = await companyRepository.Get();
            return Ok(mapper.Map<IEnumerable<GetCompanyDto>>(companies));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetCompanyDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody]CreateCompanyDto company)
        {
            if (ModelState.IsValid)
            {
                var @new = await companyRepository.Create(mapper.Map<Company>(company));
                return CreatedAtAction("Get", "Company", new { id = @new.Id }, mapper.Map<GetCompanyDto>(@new));
            }

            return BadRequest(ModelState);
        }
    }
}