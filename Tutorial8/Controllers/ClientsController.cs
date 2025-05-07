using Tutorial8.Services;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientDTO dto)
        {
            if (string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.Pesel))
                return BadRequest("Missing required fields.");
    
            var id = await _clientsService.CreateClient(dto);
            return CreatedAtAction(nameof(GetClientTrips), new { id }, $"Client created with ID {id}");
        }

        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetClientTrips(int id)
        {
            var trips = await _clientsService.GetClientTrips(id);
            if (!trips.Any())
                return NotFound($"No trips found for client ID {id}.");
            return Ok(trips);
        }
    }
 
}