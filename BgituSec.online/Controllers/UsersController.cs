using AutoMapper;
using BgituSec.Api.Models.RefreshTokens;
using BgituSec.Api.Models.Users;
using BgituSec.Api.Services;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace BgituSec.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IValidator<CreateUserCommand> _createValidator;
        private readonly IValidator<LoginUserCommand> _loginValidator;
        private readonly IValidator<RefreshTokenRequest> _refreshTokenValidator;
        private readonly IRefreshTokenRepository _tokenRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, IMapper mapper,  ITokenService tokenService, 
            IValidator<CreateUserCommand> createValidator, IValidator<LoginUserCommand> loginValidator, 
            ILogger<UsersController> logger, IRefreshTokenRepository tokenRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _tokenService = tokenService;
            _createValidator = createValidator;
            _loginValidator = loginValidator;
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        
        [Authorize]
        [HttpGet]
        [Route("profile")]
        public async Task<ActionResult<UsersResponse>> Profile()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
               ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(sub, out var userId);
            var command = new GetUserCommand { Id = userId };
            var userDto = await _mediator.Send(command);
            if (userDto == null)
                return NotFound(userId);
            var response = _mapper.Map<UsersResponse>(userDto);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult<RefreshTokensResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {

            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("AccessToken и RefreshToken обязательны.");
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
            {
                return BadRequest("Недействительный токен доступа.");
            }

            var sub = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(sub, out var userId))
            {
                return BadRequest("Невозможно извлечь userId из токена.");
            }

            var refreshToken = await _tokenRepository.GetAsync(userId);

            if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow || !_tokenService.Verify(request.RefreshToken, refreshToken.Token))
            {
                return BadRequest("Недействительный или истекший refresh token.");
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _tokenRepository.UpdateAsync(refreshToken);

            var getUserCommand = new GetUserCommand { Id = userId };
            var userDto = await _mediator.Send(getUserCommand);
            if (userDto == null)
            {
                return NotFound("Пользователь не найден.");
            }

            var newJwtToken = _tokenService.CreateToken(userDto);
            var tokenDTO = _tokenService.GenerateRefreshToken(userId);
            var response = _mapper.Map<RefreshTokensResponse>(tokenDTO);
            tokenDTO.Token = _tokenService.Hash(tokenDTO.Token);
            var newRefreshToken = _mapper.Map<RefreshToken>(tokenDTO);
            await _tokenRepository.AddAsync(newRefreshToken);

            response.JwtToken = newJwtToken;

            return Ok(response);
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<ActionResult<UsersResponse>> Create([FromBody] CreateUserRequest model)
        {
            var command = _mapper.Map<CreateUserCommand>(model);
            ValidationResult result = await _createValidator.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var userDto = await _mediator.Send(command);

            if (userDto is null)
                return Unauthorized();
            var token = _tokenService.CreateToken(userDto);

            var tokenDTO = _tokenService.GenerateRefreshToken(userDto.Id);
            var newRefreshToken = tokenDTO.Token;
            tokenDTO.Token = _tokenService.Hash(tokenDTO.Token);
            var refreshToken = _mapper.Map<RefreshToken>(tokenDTO);
            await _tokenRepository.AddAsync(refreshToken);

            var response = _mapper.Map<UsersResponse>(userDto);

            return CreatedAtAction(nameof(Create), new { token, refreshToken = newRefreshToken }, response);
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest model)
        {
            var command = _mapper.Map<LoginUserCommand>(model);
            ValidationResult result = await _loginValidator.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var userDto = await _mediator.Send(command);

            if (userDto == null)
                return Unauthorized();

            var token = _tokenService.CreateToken(userDto);

            var tokenDTO = _tokenService.GenerateRefreshToken(userDto.Id);
            var newRefreshToken = tokenDTO.Token;
            tokenDTO.Token = _tokenService.Hash(tokenDTO.Token);
            var refreshToken = _mapper.Map<RefreshToken>(tokenDTO);
            await _tokenRepository.AddAsync(refreshToken);

            return Ok(new { token, refreshToken = newRefreshToken });
        }
    }
}
