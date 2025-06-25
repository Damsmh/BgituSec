using AutoMapper;
using BgituSec.Api.Models.RefreshTokens.Request;
using BgituSec.Api.Models.RefreshTokens.Response;
using BgituSec.Api.Models.Users.Request;
using BgituSec.Api.Models.Users.Response;
using BgituSec.Api.Services;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Security.Claims;

namespace BgituSec.Api.Controllers
{
    [Route("api/auth")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IValidator<CreateUserRequest> _createValidator;
        private readonly IValidator<LoginUserRequest> _loginValidator;
        private readonly IRefreshTokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;

        public AuthController(IMediator mediator, IMapper mapper, ITokenService tokenService,
            IValidator<CreateUserRequest> createValidator, IValidator<LoginUserRequest> loginValidator,
            IRefreshTokenRepository tokenRepository, IUserRepository userRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _createValidator = createValidator;
            _loginValidator = loginValidator;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("refresh-token")]
        [SwaggerOperation(
            Description = "Возвращает token и refreshToken при валидном переданном refreshToken'e." +
            "Возвращает 400 ошибку при непереданном/невалидном refreshToken'е."
        )]
        [SwaggerResponse(200, "Возвращает token и refreshToken.", typeof(RefreshTokenResponse))]
        [SwaggerResponse(400, "Невалидный/отсутствующий refreshToken в запросе.")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {

            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { response = "AccessToken и RefreshToken обязательны." });
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
            {
                return BadRequest(new { response = "Недействительный токен доступа." });
            }

            var sub = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(sub, out var userId))
            {
                return BadRequest(new { response = "Невозможно извлечь userId из токена." });
            }

            var refreshToken = await _tokenRepository.GetAsync(userId);

            if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow || !_tokenService.Verify(request.RefreshToken, refreshToken.Token))
            {
                return BadRequest(new { response = "Недействительный или истекший refresh token." });
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _tokenRepository.UpdateAsync(refreshToken);

            var getUserCommand = new GetUserCommand { Id = userId };
            var userDto = await _mediator.Send(getUserCommand);
            if (userDto == null)
            {
                return NotFound(new { response = "Пользователь не найден." });
            }

            var newJwtToken = _tokenService.CreateToken(userDto);
            var tokenDTO = _tokenService.GenerateRefreshToken(userId);
            var response = _mapper.Map<RefreshTokenResponse>(tokenDTO);
            tokenDTO.Token = _tokenService.Hash(tokenDTO.Token);
            var newRefreshToken = _mapper.Map<RefreshToken>(tokenDTO);
            await _tokenRepository.AddAsync(newRefreshToken);

            response.RefreshToken = newJwtToken;

            return Ok(response);
        }

        [HttpPost]
        [Route("sign-up")]
        [SwaggerOperation(
            Description = "Возвращает token, refreshToken и информацию о созданном пользователе при успешной аутентификации."
        )]
        [SwaggerResponse(200, "Возвращает token и refreshToken.", (typeof(CreateUserResponse)))]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(ValidationResult))]
        [SwaggerResponse(401, "Ошибка добавления записи в базу данных.")]
        public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest model)
        {
            ValidationResult result = await _createValidator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            model.Password = _tokenService.Hash(model.Password);
            var command = _mapper.Map<CreateUserCommand>(model);

            var userDto = await _mediator.Send(command);

            if (userDto is null)
                return Unauthorized();

            var token = _tokenService.CreateToken(userDto);

            var tokenDTO = _tokenService.GenerateRefreshToken(userDto.Id);
            var newRefreshToken = tokenDTO.Token;
            var refreshToken = _mapper.Map<RefreshToken>(tokenDTO);
            await _tokenRepository.AddAsync(refreshToken);

            var response = _mapper.Map<CreateUserResponse>(userDto);
            response.RefreshToken = tokenDTO.Token;
            response.Token = token;
            


            return CreatedAtAction(nameof(Create), new { response });
        }

        [HttpPost]
        [Route("sign-in")]
        [SwaggerOperation(
            Description = "Возвращает token и refreshToken при успешной аутентификации."
        )]
        [SwaggerResponse(200, "Возвращает token и refreshToken.", typeof(RefreshTokenResponse))]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(ValidationResult))]
        [SwaggerResponse(401, "Неправильный логин/пароль.")]
        
        public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest model)
        {
            ValidationResult result = await _loginValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var command = _mapper.Map<LoginUserCommand>(model);

            var userDto = await _mediator.Send(command);

            if (userDto == null)
                return Unauthorized();
            var token = _tokenService.CreateToken(userDto);
            var tokenDTO = _tokenService.GenerateRefreshToken(userDto.Id);
            var newRefreshToken = tokenDTO.Token;
            var refreshToken = _mapper.Map<RefreshToken>(tokenDTO);
            await _tokenRepository.AddAsync(refreshToken);
            var response = new RefreshTokenResponse
            {
                RefreshToken = tokenDTO.Token,
                Token = token
            };

            return Ok(response);
        }
    }
}
