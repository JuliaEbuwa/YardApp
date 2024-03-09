using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using yard.api.Utility;
using yard.application.Services.Interface;
using yard.domain.Models;
using yard.domain.ViewModels;
using yard.infrastructure.Utility;

namespace yard.api.Controllers
{
	public class AuthenticationController : BaseApiController
	{
		private readonly IIdentityUserService _identityUser;

		public AuthenticationController(IIdentityUserService identityUser)
		{
			_identityUser = identityUser;
		}

		[HttpPost("register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Register([FromBody] RegistrationVM model)
		{
			if (model.ConfirmPassword != model.Password)
			{
				return StatusCode(StatusCodes.Status400BadRequest);
			}

			var userExists = await _identityUser.FindUser(model.Email);

			if (userExists != null)
				return StatusCode(StatusCodes.Status409Conflict);
			AppUser user = _identityUser.CreateUserFromModel(model);

			var result = await _identityUser.RegisterUser(model.Password, user);

            if (!result.Succeeded)
			{
				//string errors = _identityUser.GetErrorsFromIdentityResult(result);

				return StatusCode(StatusCodes.Status400BadRequest);
			}

			return Response(result);
		}

		[HttpPost("register-admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationVM model)
		{
			if (model.ConfirmPassword != model.Password)
			{
				return StatusCode(StatusCodes.Status400BadRequest);
			}

			var userExists = await _identityUser.FindUser(model.Email);

			if (userExists != null)
				return StatusCode(StatusCodes.Status409Conflict);
			AppUser user = _identityUser.CreateUserFromModel(model);

			var result = await _identityUser.RegisterUser(model.Password, user);

			if (!result.Succeeded)
			{
				//string errors = _identityUser.GetErrorsFromIdentityResult(result);

				return StatusCode(StatusCodes.Status400BadRequest);
			}

			await _identityUser.AssignRolesToUser(user);

			return Ok();
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginVM loginModel)
		{
			var data = await _identityUser.Login(loginModel);
			if (data == null)
			{
				return Unauthorized();
			}

            return Response(data);
			// var response = await _identityUser.Login(loginModel);

			// return Response(response);
		}

		[HttpPost("ForgotPassword")]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			var result = await _identityUser.ForgotPasswordAsync(email);
			if (result.Succeeded)
				return Ok(result);

			return BadRequest();
		}


        // [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            var data = await _identityUser.ChangePassword(model);
            return Response(data);
        }

		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordVM model)
		{
			if (ModelState.IsValid)
			{
				var result = await _identityUser.ResetPasswordAsync(model);

				if (result.Succeeded)
				{
					return Ok(result);
				}
				return BadRequest(result);
			}
			return BadRequest("Some properties are not valid");
		}

        // [Authorize]
        [HttpPost("GetUser")]
		public async Task<IActionResult> GetUser([FromBody] string email)
		{
			var data = await _identityUser.GetUser(email);
			if (data == null)
			{
				return Unauthorized();
			}

            return Response(data);
			// var response = await _identityUser.Login(loginModel);

			// return Response(response);
		}



		[HttpGet("RegisteredUsers")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetRegisteredUsers()
		{
			return Ok(await _identityUser.GetRegisteredUsers());
		}

		[HttpGet("RegisteredUserById")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetRegisteredUserById(int AppUserId)
		{
			var userExist = await _identityUser.UserExistAsync(AppUserId);
			if (userExist == false)
			{
				return NotFound($"User Id {AppUserId} does not exist");
			}

			return Ok(await _identityUser.GetUserById(AppUserId));
		}
	}
}