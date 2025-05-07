
namespace Tutorial8.Services;

public interface ITripsService
{
    Task<List<TripDTO>> GetTrips();
    Task<bool> RegisterClientToTrip(int clientId, int tripId);
    Task<bool> RemoveClientFromTrip(int clientId, int tripId);
}