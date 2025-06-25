using AutoMapper;
using BgituSec.Api.Models.Users.Request;
using BgituSec.Api.Models.Users.Response;
using BgituSec.Api.Validators;
using BgituSec.Application.Features.Users.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Security.Claims;

namespace BgituSec.Api.Controllers
{
    [Route("api/profile")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly UpdateUserRequestValidator _updateValidator;
        public ProfileController(IMediator mediator, IMapper mapper, UpdateUserRequestValidator updateValidator)
        {
            _mediator = mediator;
            _mapper = mapper;
            _updateValidator = updateValidator;
        }
        [Authorize]
        [HttpGet]
        [Route("")]
        [SwaggerOperation(
            Description = "Возвращает информацию о пользователе."
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

        [Authorize]
        [HttpPut]
        [Route("")]
        [SwaggerOperation(
            Description = "Обновляет информацию о пользователе. Получает Id пользователя из переданного jwt."
        )]
        [SwaggerResponse(200, "Обновление выполнено успешно.")]
        [SwaggerResponse(400, "Ошибки валидации.")]
        [SwaggerResponse(404, "Пользователь не найден.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        public async Task<ActionResult> Update([FromBody] UpdateUserRequest request)
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
               ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(sub, out int userId);
            var validateRequest = _mapper.Map<UpdateUserByIdRequest>(request);
            validateRequest.Id = userId;
            ValidationResult result = await _updateValidator.ValidateAsync(validateRequest);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<UpdateUserCommand>(request);
            command.Id = userId;
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(userId);
            }
        }
    }
}
