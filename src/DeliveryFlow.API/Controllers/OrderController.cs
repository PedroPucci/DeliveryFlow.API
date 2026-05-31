using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Contracts.Dto.OrderDto;
using DeliveryFlow.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFlow.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public OrderController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateOrderRequestDto createOrderRequestDto)
        {
            var result = await _uow.OrderService.Add(createOrderRequestDto);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateOrderRequestDto updateOrderRequestDto)
        {
            var result = await _uow.OrderService.Update(id, updateOrderRequestDto);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var result = await _uow.OrderService.Delete(id);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<OrderEntity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var result = await _uow.OrderService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _uow.OrderService.GetById(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id}/delivery")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterDelivery([FromRoute] string id, [FromBody] RegisterDeliveryRequestDto registerDeliveryRequestDto)
        {
            var result = await _uow.OrderService.RegisterDelivery(id, registerDeliveryRequestDto);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }
    }
}