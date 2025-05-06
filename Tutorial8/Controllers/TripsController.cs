using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _tripsService.GetTrips();
            return Ok(trips);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            // if( await DoesTripExist(id)){
            //  return NotFound();
            // }
            // var trip = ... GetTrip(id);
            return Ok();
        }

        [HttpGet("/api/clients/{id}/trips")]
        public async Task<IActionResult> GetClientTrips(int id)
        {
            var trips = await _tripsService.GetClientTrips(id);
            if (!trips.Any())
                return NotFound($"No trips found for client ID {id}.");
            return Ok(trips);
        }

        [HttpPost("/api/clients")]
        public async Task<IActionResult> CreateClient([FromBody] ClientDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Pesel))
                return BadRequest("Missing required fields.");
    
            var id = await _tripsService.CreateClient(dto);
            return CreatedAtAction(nameof(GetClientTrips), new { id }, $"Client created with ID {id}");
        }
        [HttpPut("/api/clients/{id}/trips/{tripId}")]
        public async Task<IActionResult> RegisterClientToTrip(int id, int tripId)
        {
            var success = await _tripsService.RegisterClientToTrip(id, tripId);
            if (!success)
                return Conflict("Client already registered or invalid IDs.");
    
            return Ok("Client registered to trip.");
        }
        [HttpDelete("/api/clients/{id}/trips/{tripId}")]
        public async Task<IActionResult> RemoveClientFromTrip(int id, int tripId)
        {
            var result = await _tripsService.RemoveClientFromTrip(id, tripId);
            if (!result)
                return NotFound("No registration found for this client and trip.");
            return Ok("Client unregistered from trip.");
        }

    }
}