using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Intrerfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ITokenRepository _tokenRepository;

		public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
		{
			_userManager = userManager;
			_tokenRepository = tokenRepository;
		}

		[HttpPost("register")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto request)
		{
			var user = new IdentityUser()
			{
				UserName = request.Email,
				Email = request.Email,
			};

			var identityResult = await _userManager.CreateAsync(user, request.Password);

			if (identityResult.Succeeded)
			{
				var roleResult = await _userManager.AddToRoleAsync(user, "User");

				if (roleResult.Succeeded)
				{
					var response = new RegisterResponseDto()
					{
						Email = request.Email,
						UserName = request.Email,
					};

					return Created("", response);
				}
				else
				{
					foreach (var error in roleResult.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}
				}
			}
			else 
			{
				foreach (var error in identityResult.Errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}
			}

			return ValidationProblem(ModelState);

		}

		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
		{
			var idetityUser = await _userManager.FindByEmailAsync(request.Email);

			if (idetityUser == null)
			{
				ModelState.AddModelError("Email", "Email not found");
				return ValidationProblem(ModelState);
			}

			var passwordResult = await _userManager.CheckPasswordAsync(idetityUser, request.Password);

			if (!passwordResult)
			{
				ModelState.AddModelError("Password", "Password is incorrect");
				return ValidationProblem(ModelState);
			}

			var roles = await _userManager.GetRolesAsync(idetityUser);
			var token = _tokenRepository.GenerateToken(idetityUser, roles.ToList());
			var response = new LoginResponseDto()
			{
				Email = request.Email,
				Roles = roles.ToList(),
				Token = token,
			};

			return Created("", response);
		}
	}
}
