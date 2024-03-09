using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Policy;
using Newtonsoft.Json.Linq;
using yard.api.Utility;
using yard.application.Services.Interface;
using yard.domain.Context;
using yard.domain.Models;
using yard.domain.ViewModels;
using yard.infrastructure.Utility;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace yard.application.Services.Implementation
{
	public class IdentityUserService : IIdentityUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole<int>> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly ApplicationContext _context;
		private readonly IEmailService _emailService;
		private readonly IMapper _mapper;

        public IdentityUserService(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager,
            IConfiguration configuration, ApplicationContext context, IMapper mapper, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
			_emailService = emailService;
        }

		public async Task<ApiResponse> GetUser(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);

			if (user != null)
			{
				user.Address = await _context.Addresses.Where(a => a.Id == user.AddressId).FirstOrDefaultAsync();

				return ApiResponse.Success(user);
			}
			return ApiResponse.Fail("User does not exist");
		}

        public async Task<AppUserVM> FindUser(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);

            var userToReturn = _mapper.Map<AppUserVM>(user);

			return userToReturn;
		}

		public async Task<bool> CheckRole(string roleName)
		{
			return await _roleManager.RoleExistsAsync(roleName);
		}

		public async Task<ApiResponse> RegisterUser(string password, AppUser user)
		{
			await _userManager.CreateAsync(user, password);

            var userRoles = await _userManager.GetRolesAsync(user);

            var claim = ClaimsImplentation.GetClaims(userRoles, user.UserName);
            //JWT
            string token = JwtImplementation.GetJWTToken(_configuration, claim);

            var userData = FindUser(user.Email);

            var data = new
            {
                user = userData.Result,
                token
            };

            return ApiResponse.Success(data);

        }

		public async Task<IdentityResult> CreateRole(IdentityRole<int> role)
		{
			return await _roleManager.CreateAsync(role);

		}

		public async Task<IdentityResult> AddToRole(AppUser user, string roleName)
		{
			return await _userManager.AddToRoleAsync(user, "Admin");
		}

		public AppUser CreateUserFromModel(RegistrationVM model)
		{
			return new AppUser()
			{
				Email = model.Email,
				UserName = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				ProfilePictureUrl = model.ProfilePictureUrl,
				Address = new Address
				{
					City = model.Address.City,
					State = model.Address.State,
					Street = model.Address.Street,
					PostalCode = model.Address.PostalCode,
					Country = model.Address.Country
				},
			};
		}

		public string GetErrorsFromIdentityResult(IdentityResult result)
		{
			string errors = string.Empty;

			foreach (var error in result.Errors)
			{
				errors += error.Description;
				errors += "@";
			}

			errors = errors.Replace("@", System.Environment.NewLine);

			return errors;
		}

		public async Task AssignRolesToUser(AppUser user)
		{
			foreach (var roleName in new[] { UserRoles.Admin, UserRoles.User })
			{
				if (!await CheckRole(roleName.ToString()))
				{
					await CreateRole(new IdentityRole<int>(roleName));
				}
			}

			if (await CheckRole(UserRoles.Admin))
			{
				await AddToRole(user, UserRoles.Admin);
			}
		}

		public async Task<bool> CheckPassword(AppUser user, string password)
		{
			var isValid = await _userManager.CheckPasswordAsync(user, password);

			return isValid;
		}

		public async Task<IList<string>> GetRoles(AppUser user)
		{
			return await _userManager.GetRolesAsync(user);
		}

		public async Task<ApiResponse> RegisterToken(AppUser user, RegistrationVM request)
		{
			var userExist = FindUser(user.Email);
			if (userExist != null && await _userManager.CheckPasswordAsync(user, request.Password))
			{
				var userRoles = await _userManager.GetRolesAsync(user);

				var claim = ClaimsImplentation.GetClaims(userRoles, user.UserName);
				//JWT
				string token = JwtImplementation.GetJWTToken(_configuration, claim);

				var userData = FindUser(user.Email);

				//return token;

				var data = new
				{
					user = userData.Result,
					token
				};

				return ApiResponse.Success(data);
			}
			return ApiResponse.Fail("Registration failed");
		}

		public async Task<ApiResponse> ChangePassword(ChangePasswordVM model)
		{
            var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return ApiResponse.Fail("User not Found");
			}
			var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			if(result != null)
			{
				return ApiResponse.Success(result);
			}
			return ApiResponse.Fail("Bad Request", 400);
        }

		public async Task<ApiResponse> Login(LoginVM request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);

			if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
			{
				var userRoles = await _userManager.GetRolesAsync(user);

				var claim = ClaimsImplentation.GetClaims(userRoles, user.UserName);
				//JWT
				string token = JwtImplementation.GetJWTToken(_configuration, claim);

				var userData = FindUser(request.Email);

				//return token;

				var data = new
				{
					user = userData.Result,
					token
				};

                return ApiResponse.Success(data);

            }

			return ApiResponse.Fail("Invalid Login Details", 401);
		}



		public async Task<List<AppUser>> GetRegisteredUsers()
		{
			return await _userManager.Users.ToListAsync();
		}

		public async Task<AppUser> GetUserById(int Id)
		{
			var userIdAsString = Id.ToString();
			var user = await _userManager.FindByIdAsync(userIdAsString);
			return user;
		}

		public async Task<bool> UserExistAsync(int AppUserId)
		{
			return await _context.Users.AnyAsync(a => a.Id == AppUserId);
		}

		public async Task<ApiResponse> ConfirmEmail(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return ApiResponse.Fail("User does not exist");
			}
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
			var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
			string url = $"{_configuration["AppUrl"]}/api/authentication/confirmemail?userid={user.Id}&token={validEmailToken}";

            var message = new Message(new string[] { user.Email! }, "Confirmation email link",
				$"<h1>Hi, Welcome to Yard Application</h1>" + $"<p>Please confirm your email by <a href='{url}'> Click here </a></p>");
            await _emailService.SendEmailAsync(message);
			return ApiResponse.Success("Confirmation email sent sucessfully");
        }

		public async Task<ApiResponse> ForgotPasswordAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return ApiResponse.Fail("User does not exist");
			}
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var encodedToken = Encoding.UTF8.GetBytes(token);
			var validToken = WebEncoders.Base64UrlEncode(encodedToken);

			string url = $"{_configuration["AppUrl"]}/resetpassword?email={email}&token={validToken}";
			var message = new Message(new string[] { user.Email! }, "Reset your Password",
				"<h1> Please follow the instructions to reset your password</h1>" +
				$"<p>To reset your password <a href='{url}'>Click here</a></p>");
			await _emailService.SendEmailAsync(message);
			return ApiResponse.Success("Reset Url password has been set successfully");
		}

		public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordVM model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return ApiResponse.Fail("User does not exist", 404);
			}
			if(model.NewPassword != model.ConfirmPassword)
			{
				return ApiResponse.Fail("Password doesn't match it confirmation");
			}
			var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
			if(!result.Succeeded)
			{
                return ApiResponse.Fail("Could not reset password");
            }
			return ApiResponse.Success("Password has been changed ");
		}
	}
}