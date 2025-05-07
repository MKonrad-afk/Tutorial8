using System.Collections.Generic;
using System.Threading.Tasks;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public interface IClientsService
    {
        Task<int> CreateClient(ClientDTO dto);
        Task<List<ClientTripDTO>> GetClientTrips(int clientId);
    }
}