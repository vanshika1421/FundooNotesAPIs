using BusinessLayer.Interface.Users;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.User.RequestModel;
using ModelLayer.User.ResponseModel;
using Middleware.Services.JWT;

namespace Fundoo.Controllers
{

    [ApiController] //mera frontend se data is class m ayega// annotation btati h ye controler class h
    [Route("api/[controller]")] //Automatically consider address as WeatherForecast (anything before controller word)
    public class FundooController : ControllerBase
    {
        private IUserBl _userBl;
        private readonly JwtService _jwtService;
        public FundooController(IUserBl userBl, JwtService jwtService)
        {
            _userBl = userBl;
            _jwtService = jwtService;
        }
        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok("You are authorized!");
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<ResponseModel<RegistrationResponseModel>> RegisterUser(RegistrationModel registrationModel)
        {
            var registrationResult = _userBl.RegisterUserBL(registrationModel);

            if (registrationResult == null)
            {
                return BadRequest(new ResponseModel<RegistrationResponseModel>
                {
                    success = false,
                    Message = "Registration failed"
                });
            }

            var responseData = new RegistrationResponseModel
            {
                UserId = 0, // Will be populated from database upon successful registration
                FirstName = registrationResult.FirstName ?? "",
                LastName = registrationResult.LastName ?? "",
                Email = registrationResult.email,
                UserName = registrationResult.UserName ?? "",
                Message = "Registration successful"
            };

            return Ok(new ResponseModel<RegistrationResponseModel>
            {
                success = true,
                Message = "User registered successfully",
                Data = responseData
            });
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel<LoginResponseModel>>> Login(LoginModel loginModel)
        {
            var (loginResult, userId, userName) = await _userBl.GetLoginUserWithIdAsync(loginModel.email, loginModel.password);

            if (loginResult == null)
            {
                return Unauthorized(new ResponseModel<LoginResponseModel>
                {
                    success = false,
                    Message = "Invalid email or password"
                });
            }

            string token = _jwtService.GenerateToken(loginResult.email, userId.ToString());

            var responseData = new LoginResponseModel
            {
                UserId = userId,
                Email = loginResult.email,
                UserName = userName,
                Token = token,
                Message = "Login successful"
            };

            return Ok(new ResponseModel<LoginResponseModel>
            {
                success = true,
                Message = "Login successful",
                Data = responseData
            });
        }
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ResponseModel<ForgotPasswordResponseModel>>> ForgotPassword(ForgotPasswordModel model)
        {
            var result = await _userBl.ForgotPasswordAsync(model);

            if (!result)
            {
                return NotFound(new ResponseModel<ForgotPasswordResponseModel>
                {
                    success = false,
                    Message = "Email not found"
                });
            }

            var responseData = new ForgotPasswordResponseModel
            {
                Message = "Password reset email sent successfully",
                SentAt = DateTime.UtcNow
            };

            return Ok(new ResponseModel<ForgotPasswordResponseModel>
            {
                success = true,
                Message = "Password reset email sent successfully",
                Data = responseData
            });
        }
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ResponseModel<ResetPasswordResponseModel>>> ResetPassword(ResetPasswordModel model)
        {
            var result = await _userBl.ResetPasswordAsync(
                model.Token,
                model.NewPassword);

            if (!result)
            {
                return BadRequest(new ResponseModel<ResetPasswordResponseModel>
                {
                    success = false,
                    Message = "Invalid or expired reset token"
                });
            }

            var responseData = new ResetPasswordResponseModel
            {
                Message = "Password reset successfully",
                ResetAt = DateTime.UtcNow
            };

            return Ok(new ResponseModel<ResetPasswordResponseModel>
            {
                success = true,
                Message = "Password reset successfully",
                Data = responseData
            });
        }
        [HttpGet("test-logging")]
        public IActionResult TestLogging( [FromServices] ILogger<FundooController> logger)
        {
            

            try
            {
                throw new InvalidOperationException(
                    "This is a deliberate test exception.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Test exception occurred.");

                return StatusCode(
                    500,
                    "Test exception logged successfully.");
            }
        }
    }
}
