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
        
        
        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> RegisterClientToTrip(int id, int tripId)
        {
            var success = await _clientsService.RegisterClientToTrip(id, tripId);
            if (!success)
                return Conflict("Client already registered or invalid IDs.");

            return Ok("Client registered to trip.");
        }
        
        
        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> RemoveClientFromTrip(int id, int tripId)
        {
            var result = await _clientsService.RemoveClientFromTrip(id, tripId);
            if (!result)
                return NotFound("No registration found for this client and trip.");
            return Ok("Client unregistered from trip.");
        }
    }
 
}