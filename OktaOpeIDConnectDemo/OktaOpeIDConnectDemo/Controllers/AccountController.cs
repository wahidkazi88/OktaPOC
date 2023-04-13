using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OktaOpeIDConnectDemo.Services;

namespace OktaOpeIDConnectDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public AccountController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("GetToken")]
        public async Task<IActionResult> SignInAndGetToken(string username, string password)
        {
            var oktaToken = await _tokenService.GetToken(username, password);
            if (oktaToken != null)
                return Ok(oktaToken);

            return null;
        }

        [HttpGet("GetStudent")]
        public async Task<IActionResult> GetStudentDetails()
        {
            List<string> student = new List<string>();
            student.Add("Student1");
            student.Add("Student2");

            return Ok(student);
        }
    }
}
