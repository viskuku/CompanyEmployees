using AutoMapper;
using CompanyEmployees.ActionFilters;
using CompanyEmployees.Utility;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILoggerManager _loggerManager;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IDataShaper<EmployeeDto> _dataShaper;
        private readonly EmployeeLinks _employeeLinks;

        public EmployeesController(ILoggerManager loggerManager, IRepositoryManager repositoryManager, IMapper mapper,
            IDataShaper<EmployeeDto> dataShaper, EmployeeLinks employeeLinks)
        {
            _loggerManager = loggerManager;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _dataShaper = dataShaper;
            _employeeLinks = employeeLinks;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
        //{
        //    var company = await _repositoryManager.Company.GetCompany(companyId, trackChanges: false);

        //    if (company == null)
        //    {
        //        _loggerManager.LogInfo($"Company with id: {companyId} doesn't exist");
        //        return NotFound();
        //    }
        //    else
        //    {
        //        var employeesFromDb = await _repositoryManager.Employee.GetEmployees(companyId, trackChanges: false);

        //        var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        //        return Ok(employeeDto);
        //    }
        //}

        [HttpGet]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeFilter))]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameter)
        {
            var company = await _repositoryManager.Company.GetCompany(companyId, trackChanges: false);

            if (company == null)
            {
                _loggerManager.LogInfo($"Company with id: {companyId} doesn't exist");
                return NotFound();
            }
            else
            {
                if (!employeeParameter.ValidAgeRange)
                    return BadRequest("Max age can't be less than min age.");

                var employeesFromDb = await _repositoryManager.Employee.GetEmployees(companyId, employeeParameter, trackChanges: false);

                Response.Headers.Add("x-pagination", JsonSerializer.Serialize(employeesFromDb.MetaData));

                var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

                // return Ok(employeeDto);

                // return Ok(_dataShaper.ShapeData(employeeDto, employeeParameter.Fields));

                var links = _employeeLinks.TryGenerateLinks(employeeDto, employeeParameter.Fields, companyId, HttpContext);

                return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
            }
        }

        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployee(Guid companyId, Guid id)
        {
            var company = await _repositoryManager.Company.GetCompany(companyId, trackChanges: false);

            if (company == null)
            {
                _loggerManager.LogInfo($"Company with id: {companyId} doesn't exist");

                return NotFound();
            }

            var employee = await _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: false);

            if (employee == null)
            {
                _loggerManager.LogInfo($"Employee with id: {companyId} doesn't exist");

                return NotFound();
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return Ok(employeeDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            //if (employee == null)
            //{
            //    _loggerManager.LogError("EmployeeForCreationDto object sent from client is null.");
            //    return BadRequest("EmployeeForCreationDto is null");
            //}

            //if (!ModelState.IsValid)
            //{
            //    _loggerManager.LogError("Invalid Model State for the EmployeeForCreationDto object");

            //    return UnprocessableEntity(ModelState);
            //}

            var company = await _repositoryManager.Company.GetCompany(companyId, trackChanges: false);

            if (company == null)
            {
                _loggerManager.LogError($"Given Company ID {companyId} is invalid");

                return NotFound();
            }

            var employeeEntity = _mapper.Map<Employee>(employee);

            _repositoryManager.Employee.CreateEmployeeForCompany(company.Id, employeeEntity);
            await _repositoryManager.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId = company.Id, empId = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            //var company = await _repositoryManager.Company.GetCompany(companyId, trackChanges: false);

            //if (company == null)
            //{
            //    _loggerManager.LogError($"Given Company Id {companyId} is invalid");
            //    return NotFound();
            //}

            //var employee = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: false);

            //if (employee == null)
            //{
            //    _loggerManager.LogError($"Given Employee Id {id} is invalid");
            //    return NotFound();
            //}

            var employee = HttpContext.Items["employee"] as Employee;

            _repositoryManager.Employee.DeleteEmployee(employee);
            await _repositoryManager.SaveAsync();

            return NoContent();

        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            //var company = await _repositoryManager.Company.GetCompany(companyId, trackChanges: false);

            //if (company == null)
            //{
            //    _loggerManager.LogInfo($"Given company id: {id} is not valid");
            //    return NotFound();
            //}

            //var employeeEntity = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: true);

            //if (employee == null)
            //{
            //    _loggerManager.LogInfo($"EmployeeForUpdateDto object is not valid");
            //    return NotFound();
            //}

            //if (!ModelState.IsValid)
            //{
            //    _loggerManager.LogError("Invalid Model state for the EmployeeForUpdateDto object");

            //    return UnprocessableEntity(ModelState);
            //}

            var employeeEntity = HttpContext.Items["employee"] as Employee;

            _mapper.Map(employee, employeeEntity);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }


        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _loggerManager.LogError("patchDoc object is null");
                return BadRequest("patchDoc object is null");
            }

            //var companyEntity = await _repositoryManager.Company.GetCompany(companyId, trackChanges: false);

            //if (companyEntity == null)
            //{
            //    _loggerManager.LogError($"Given Company Id: {companyId} doesn't exist");

            //    return NotFound();
            //}

            //var employeeEntity = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: true);

            //if (employeeEntity == null)
            //{
            //    _loggerManager.LogError($"Given employee Id: {id} doesn't exist");

            //    return NotFound();
            //}

            var employeeEntity = HttpContext.Items["employee"] as Employee;

            var employeePatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            patchDoc.ApplyTo(employeePatch, ModelState);

            TryValidateModel(employeePatch); //This will trigger a validation and every error will make ModelState invalid.

            if (!ModelState.IsValid)
            {
                _loggerManager.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _loggerManager.LogInfo($"Employee patch before update: {employeePatch}");

            _mapper.Map(employeePatch, employeeEntity);

            await _repositoryManager.SaveAsync();

            return NoContent();
        }

    }
}
