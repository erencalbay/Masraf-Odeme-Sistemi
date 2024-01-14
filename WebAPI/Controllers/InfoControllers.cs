using Data.DbContextCon;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoControllers : ControllerBase
    {
        private readonly VdDbContext _dbContext;

        public InfoControllers(VdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Info>> Get()
        {
            return await _dbContext.Set<Info>()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Info> Get(int id)
        {
            var customer = await _dbContext.Set<Info>()
                .Where(x => x.InfoNumber == id).FirstOrDefaultAsync();

            return customer;
        }

        [HttpPost]
        public async Task Post([FromBody] Info info)
        {
            await _dbContext.Set<Info>().AddAsync(info);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Info info)
        {
            var fromdb = await _dbContext.Set<Info>().Where(x => x.InfoNumber == id).FirstOrDefaultAsync();
            fromdb.IBAN = info.IBAN;
            fromdb.Information = info.Information;

            await _dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var fromdb = await _dbContext.Set<Info>().Where(x => x.InfoNumber == id).FirstOrDefaultAsync();
            fromdb.isActive = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
