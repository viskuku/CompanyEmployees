using AutoMapper;
using CompanyEmployees.ActionFilters;
using CompanyEmployees.ModelBinders;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/companies")]
    //[ResponseCache(CacheProfileName = "120SecondDuration")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILoggerManager _loggerManager;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public CompaniesController(ILoggerManager loggerManager, IRepositoryManager repositoryManager, IMapper mapper)
        {
            _loggerManager = loggerManager;
            _repository = repositoryManager;
            _mapper = mapper;
        }

        [HttpOptions]
        public IActionResult GetCompaniesOption()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");

            return Ok();

        }

        /// <summary>
        /// Get Companies
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetCompanies"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCompanies()
        {
            //throw new Exception(); testing Global Exception
            var companies = await _repository.Company.GetAllCompanies(trackChanges: false);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companiesDto);
        }

        //[Route("CompanyById")]
        //[HttpGet("{id}")]
        [HttpGet("{id}", Name = "CompanyById")]
        //[ResponseCache(Duration = 60)]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _repository.Company.GetCompany(id, trackChanges: false);

            if (company == null)
            {
                _loggerManager.LogInfo($"Company with id: {id} doesn't exist in the database.");

                return NotFound();
            }
            else
            {
                var companiesDto = _mapper.Map<CompanyDto>(company);

                return Ok(companiesDto);
            }
        }


        [HttpGet("collection/({ids})", Name = "GetCompanyList")]
        public async Task<IActionResult> GetCompanyList([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _loggerManager.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var companyEntities = await _repository.Company.GetCompanyByIds(ids, trackChanges: false);

            if (ids.Count() != companyEntities.Count())
            {
                _loggerManager.LogError("some ids are not valid in a collection");
                return NotFound();
            }

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return Ok(companiesToReturn);
        }

        [HttpPost(Name = "CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            //if (company == null)
            //{
            //    _loggerManager.LogError("CompanyForCreationDto object sent from client is null.");

            //    return BadRequest("CompanyForCreationDto object is null");
            //}

            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection == null)
            {
                _loggerManager.LogError("Company collection parameter is null");
                return BadRequest("Company collection parameter is null");
            }

            var companyEnities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach (var company in companyEnities)
            {
                _repository.Company.CreateCompany(company);
            }

            await _repository.SaveAsync();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEnities);

            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("GetCompanyList", new { ids = ids }, companyCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            //var company = await _repository.Company.GetCompany(id, trackChanges: false);

            //if (company == null)
            //{
            //    _loggerManager.LogError($"Given company id {id} doesn't exist");
            //    return NotFound();
            //}

            var company = HttpContext.Items["company"] as Company;

            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            //if (company == null)
            //{
            //    _loggerManager.LogError("CompanyForUpdateDto object is null");

            //    return BadRequest("CompanyForUpdateDto object is null");
            //}

            //var companyEntity = await _repository.Company.GetCompany(id, trackChanges: true);

            //if (companyEntity == null)
            //{
            //    _loggerManager.LogError($"Company with id: {id} does't exist in the database");
            //    return NotFound();
            //}

            var companyEntity = HttpContext.Items["company"] as Company;

            _mapper.Map(company, companyEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
