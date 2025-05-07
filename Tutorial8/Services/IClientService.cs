using System.Collections.Generic;
using System.Threading.Tasks;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public interface IClientsService
    {
        Task<int> CreateClient(ClientDTO dto);  // Create a new client and return the ID of the created client
        Task<List<ClientTripDTO>> GetClientTrips(int clientId);  // Get all trips for a specific client
    }
}