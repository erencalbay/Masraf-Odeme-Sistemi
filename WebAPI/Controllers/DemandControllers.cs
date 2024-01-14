using Data.DbContextCon;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandControllers : ControllerBase
    {
        private readonly VdDbContext _dbContext;

        public DemandControllers(VdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Demand>> Get()
        {
            return await _dbContext.Set<Demand>()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Demand> Get(int id)
        {
            var customer = await _dbContext.Set<Demand>()
                .Where(x => x.DemandNumber == id).FirstOrDefaultAsync();

            return customer;
        }

        [HttpPost]
        public async Task Post([FromBody] Demand demand)
        {
            await _dbContext.Set<Demand>().AddAsync(demand);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Demand demand)
        {
            var fromdb = await _dbContext.Set<Demand>().Where(x => x.DemandNumber == id).FirstOrDefaultAsync();
            fromdb.Description = demand.Description;
            fromdb.RejectionResponse = demand.RejectionResponse;

            await _dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var fromdb = await _dbContext.Set<Demand>().Where(x => x.DemandNumber == id).FirstOrDefaultAsync();
            fromdb.isActive = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
