using AutoMapper;
using BgituSec.Api.Models.Users;
using BgituSec.Api.Services;
using BgituSec.Application.Features.Users.Commands;
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

        public UsersController(IMediator mediator, IMapper mapper,  ITokenService tokenService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _tokenService = tokenService;
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

        [HttpPost]
        [Route("sign-up")]
        public async Task<ActionResult<UsersResponse>> Create([FromBody] CreateUserRequest model)
        {
            var command = _mapper.Map<CreateUserCommand>(model);

            var userDto = await _mediator.Send(command);

            if (userDto is null)
                return Unauthorized();
            var token = _tokenService.CreateToken(userDto);
            var response = _mapper.Map<UsersResponse>(userDto);

            return CreatedAtAction(nameof(Create), new { token }, response);
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest model)
        {
            var command = _mapper.Map<LoginUserCommand>(model);

            var userDto = await _mediator.Send(command);

            if (userDto == null)
                return Unauthorized();

            var token = _tokenService.CreateToken(userDto);
            return Ok(new { token });
        }
    }
}
