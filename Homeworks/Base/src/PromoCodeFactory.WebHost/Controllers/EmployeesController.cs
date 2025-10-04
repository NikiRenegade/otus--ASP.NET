using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Mappers;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync(EmployeeRequest request)
        {
            var newEmployee = request.ToEmployee();
            newEmployee.Id = Guid.NewGuid();

            var createdEmployee = await _employeeRepository.CreateAsync(newEmployee);
            if (!createdEmployee.Id.Equals(newEmployee.Id))
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = createdEmployee.Id }, createdEmployee.ToEmployeeResponse());
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployeeAsync(Guid id, EmployeeRequest request)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }
            var updatedEmployee = request.ToEmployee();
            updatedEmployee.Id = id;
            var resultEmployee = await _employeeRepository.UpdateAsync(updatedEmployee);
            return resultEmployee.ToEmployeeResponse();
        }

        /// <summary>
        /// Удалить сотрудника по Id
        /// </summary>
        /// <param name="id"></param>   
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> DeleteEmployeeAsync(Guid id)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }
            var result = await _employeeRepository.DeleteAsync(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}