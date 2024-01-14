using Data.DbContextCon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeControllers : ControllerBase
    {
        private readonly VdDbContext _dbContext;

        public EmployeeControllers(VdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Employee>> Get()
        {
            return await _dbContext.Set<Employee>()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Employee> Get(int id)
        {
            var customer = await _dbContext.Set<Employee>()
                .Include(x => x.Demands)
                .Include(x => x.Infos)
                .Where(x => x.EmployeeNumber == id).FirstOrDefaultAsync();

            return customer;
        }

        [HttpPost]
        public async Task Post([FromBody] Employee employee)
        {
            await _dbContext.Set<Employee>().AddAsync(employee);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Employee employee)
        {
            var fromdb = await _dbContext.Set<Employee>().Where(x => x.EmployeeNumber == id).FirstOrDefaultAsync();
            fromdb.FirstName = employee.FirstName;
            fromdb.LastName = employee.LastName;

            await _dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var fromdb = await _dbContext.Set<Employee>().Where(x => x.EmployeeNumber == id).FirstOrDefaultAsync();
            fromdb.isActive = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
