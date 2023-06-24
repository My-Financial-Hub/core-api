namespace FinancialHub.Auth.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class SignupController : ControllerBase
    {
        private readonly ISignupService signupService;

        public SignupController(ISignupService signupService)
        {
            this.signupService = signupService;
        }

        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] SignupModel signup)
        {
            var result = await signupService.CreateAccountAsync(signup);

            if(result.HasError)
            {
                return BadRequest(
                    error: new ValidationErrorResponse(result.Error.Message)
                );
            }

            return Ok(result);
        }
    }
}
