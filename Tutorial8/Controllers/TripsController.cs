using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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



        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> RemoveClientFromTrip(int id, int tripId)
        {
            var result = await _tripsService.RemoveClientFromTrip(id, tripId);
            if (!result)
                return NotFound("No registration found for this client and trip.");
            return Ok("Client unregistered from trip.");
        }
    }
}