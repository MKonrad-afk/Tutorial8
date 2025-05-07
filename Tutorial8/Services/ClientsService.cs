using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public class ClientsService : IClientsService
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        public async Task<int> CreateClient(ClientDTO dto)
        {
            string sql = @"
                INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                OUTPUT INSERTED.IdClient
                VALUES (@fn, @ln, @em, @tel, @pesel)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@fn", dto.FirstName);
            cmd.Parameters.AddWithValue("@ln", dto.LastName);
            cmd.Parameters.AddWithValue("@em", dto.Email);
            cmd.Parameters.AddWithValue("@tel", dto.PhoneNumber);
            cmd.Parameters.AddWithValue("@pesel", dto.Pesel);

            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync();
        }

        public async Task<List<ClientTripDTO>> GetClientTrips(int clientId)
        {
            var trips = new List<ClientTripDTO>();

            string sql = @"
                SELECT t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, ct.RegisteredAt, ct.PaymentDate
                FROM Client_Trip ct
                JOIN Trip t ON ct.IdTrip = t.IdTrip
                WHERE ct.IdClient = @id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", clientId);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                trips.Add(new ClientTripDTO
                {
                    TripName = reader.GetString(0),
                    Description = reader.GetString(1),
                    DateFrom = reader.GetDateTime(2),
                    DateTo = reader.GetDateTime(3),
                    MaxPeople = reader.GetInt32(4),
                    RegisteredAt = reader.GetInt32(5),
                    PaymentDate = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                });
            }

            return trips;
        }
    }
}
