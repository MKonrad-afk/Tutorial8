using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ITripsService
{
    Task<List<TripDTO>> GetTrips();
    Task<List<ClientTripDTO>> GetClientTrips(int clientId);

    Task<int> CreateClient(ClientDTO dto);
    Task<bool> RegisterClientToTrip(int clientId, int tripId);
    Task<bool> RemoveClientFromTrip(int clientId, int tripId);
}