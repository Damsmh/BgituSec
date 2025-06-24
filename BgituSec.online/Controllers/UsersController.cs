using AutoMapper;
using BgituSec.Api.Models.Users;
using BgituSec.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;


namespace BgituSec.Api.Controllers
{
    [Route("api/users")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [Route("profile")]
        [SwaggerOperation(
            Description = "Возвращает список всех пользователей."
        )]
        [SwaggerResponse(200, "Возвращает информацию о пользователе.", typeof(UserResponse))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        public async Task<ActionResult<UserResponse>> Profile()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
               ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(sub, out int userId);
            var command = new GetUserCommand { Id = userId };
            var userDto = await _mediator.Send(command);
            if (userDto == null)
                return NotFound(userId);
            var response = _mapper.Map<UserResponse>(userDto);
            return Ok(response);
        }

        [Authorize(Roles="ROLE_ADMIN")]
        [HttpGet]
        [Route("get-all")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Возвращает список всех пользователей."
        )]
        [SwaggerResponse(200, "Возвращает список всех пользователей.", typeof(List<UserResponse>))]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        public async Task<ActionResult<List<UserResponse>>> GetAll()
        {
            var command = new GetAllUsersCommand();
            var usersDto = await _mediator.Send(command);
            var response = _mapper.Map<List<UserResponse>>(usersDto);
            return Ok(response);
        }
    }
}
