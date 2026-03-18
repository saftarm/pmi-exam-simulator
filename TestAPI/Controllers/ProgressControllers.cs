
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {

        private readonly IProgressService  _progressService;

        public ProgressController(ApplicationDbContext context, IProgressService progressService)
        {
            _progressService = progressService;

        }

        






     


    }
}
