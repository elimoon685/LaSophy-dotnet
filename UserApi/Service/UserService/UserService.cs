using System.Text.Json;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using MassTransit;
using MassTransit.Middleware;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using SharedContract.Event;
using UserApi.DTO;
using UserApi.Exceptions.CustomExceptions;
using UserApi.Models;
using UserApi.Repository.UserRepository;
using UserApi.Sender;
using UserApi.Utils;

namespace UserApi.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly AzureServiceBusSender _azureServiceBusSender;
        public UserService(IUserRepository userRepository,   IConfiguration configuration, IMapper mapper, AzureServiceBusSender azureServiceBusSender)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _azureServiceBusSender= azureServiceBusSender;
            
        }

        public async Task<string?> AuthenticateUserAsync(LoginRequestDto model)
        {
           var user=await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null) 
            {
                throw new NotFoundException("User not exist");
            }

            bool passwordCorrect= await _userRepository.CheckPasswordAsync(user, model.Password);

            if (!passwordCorrect) 
            {
                throw new PasswordNotCorrect("Incorrect password");
            }

            var role=await _userRepository.GetUserRolesAsync(user);
            if (role != model.Role)
            {
                throw new RoleSelectedWrong("Please choose the right role");
            }
            var key = _configuration.GetSection("Jwt:SigningKey").Get<string>();
            var audience = _configuration.GetSection("Jwt:Audience").Get<string>();
            var issuer = _configuration.GetSection("Jwt:Issuer").Get<string>();
            var token = JwtUtils.GenerateJwtToken(user, key, issuer, audience, role);
            return token;
        }


        public async Task<bool> EmailExistsAsync(string email)
        {
           return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<UserInfoResponseDto> GetUserInfoByIdAsync(string id)
        {
            var userInfo=await _userRepository.GetUserInfoByIdAsync(id);

            var result=_mapper.Map<UserInfoResponseDto>(userInfo);
            return result;  
        }

        public async Task<IdentityResult> RegisterAdminAsync(RegisterRequestDto model)
        {
            if (await EmailExistsAsync(model.Email))
            {
                // return IdentityResult.Failed(new IdentityError { Description = "Email already in use." });
                throw new EmailAlreadyInUseException("Email already in use");

            }
            if (model.Password != model.ConfirmPassword)
            {
                throw new PasswordNotMatch("Password not match");
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow,

            };
            var result = await _userRepository.CreateUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userRepository.AddUserToRoleAsync(user, "Admin");
            }
            //
            //create user event
            var createUserEvent = new CreateUserEvent
            {
                UserID = Guid.Parse(user.Id),
                UserName=user.UserName,
                UserEmail=user.Email,
                CreatedAt = user.CreatedAt,
                Role="Admin"
            };
            //Service Bus 
            /*
            var jsonBody = JsonSerializer.Serialize(createUserEvent);

            // Create the message
            var message = new ServiceBusMessage(jsonBody)
            {
                ContentType = "application/json",
                Subject = "Send register email" // Optional metadata
            };

            // Send the message (assumes _serviceBusSender is injected ServiceBusSender)
            await _serviceBusSender.SendMessageAsync(message);
            */
            var queueName = _configuration["AzureServiceBus:QueueName:UserCreated"];
            await _azureServiceBusSender.SendMessageAsync<CreateUserEvent>(queueName, createUserEvent);
            return result;
            //publish

           // await _publishEndpoint.Publish(createUserEvent);

           // return result;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterRequestDto model)
        {
            if (await EmailExistsAsync(model.Email)) 
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email already in use." });

            }
            if (model.Password != model.ConfirmPassword)
            {
                throw new PasswordNotMatch("Password not match");
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow,

            };
            var result=await _userRepository.CreateUserAsync(user, model.Password);
            if (result.Succeeded) 
            {
                await _userRepository.AddUserToRoleAsync(user, "User");
            }

            //Create the user event
            return result;
           
        }

        

        public async Task<bool> SendEmail(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User does not exist");
            }

            var token = await _userRepository.GenerateToken(user);

            var createResetPasswordEvent = new PasswordResetTokenGeneratedEvent
            {
                Email = email,
                UserName = user.UserName,
                Token = token
            };
            var queueName = _configuration["AzureServiceBus:QueueName:EmailVerify"];
            await _azureServiceBusSender.SendMessageAsync<PasswordResetTokenGeneratedEvent>(queueName, createResetPasswordEvent);

            return true;
        }

        public async Task<IdentityResult> UpdatePasswordAsync(ResetPasswordRequestDto model)
        {
            var user = await _userRepository.FindByEmailAsync(model.Email);
            if (user != null)
            {
                throw new NotFoundException("User does not exist");
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                throw new PasswordNotMatch("Password not match");
            }
            var result=await _userRepository.ResetPasswordAsync(user, model.Token, model.NewPassword);

            return result;

        }

        public async Task<UserInfoResponseDto> UpdateUserInfoByIdAsync(string userId, UpdateUserInfoDto model)
        {
            var user=await _userRepository.GetUserInfoByIdAsync(userId);

            user.UserName = model.UserName?? user.UserName;
            user.Bio=model.Bio;
            await _userRepository.UpdateAsync(user);
            var result = _mapper.Map<UserInfoResponseDto>(user);
            return result;

        }
    }
}
