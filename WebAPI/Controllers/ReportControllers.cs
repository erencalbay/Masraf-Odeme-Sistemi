using AutoMapper;
using Base.Response;
using Data.DbContextCon;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schema;
using System.Data.Entity;
using System.Threading;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportControllers : ControllerBase
    {
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public ReportControllers(IMapper mapper, VdDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ApiResponse<List<DemandResponse>>> Index()
        {
            var list = await dbContext.Set<Demand>()
                .Where(x => x.isActive == true)
                .ToListAsync();
            var mappedList = mapper.Map<List<Demand>, List<DemandResponse>>(list);
            return new ApiResponse<List<DemandResponse>>(mappedList);
        }
    }
}
