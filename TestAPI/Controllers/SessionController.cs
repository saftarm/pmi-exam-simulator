
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Services.Interfaces;
using TestAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using TestAPI.Services.Implementation;

namespace TestAPI.Controllers {

[Route("api/[controller]")]
[ApiController]
    public class SessionController : ControllerBase
    {
        

        private readonly IExamService _examService; 

        public SessionController (IExamService examService) {

            _examService = examService;
        }

        // [HttpGet]

        // public async Task<ActionResult<Session>> GetExam{ 


        // }





}
}