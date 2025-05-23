﻿using System.Collections.Generic;
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
        
        public async Task<bool> RegisterClientToTrip(int clientId, int tripId)
        {
            string checkSql = "SELECT COUNT(*) FROM Client_Trip WHERE IdClient=@cid AND IdTrip=@tid";
            string insertSql = "INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt) VALUES (@cid, @tid, @reg)";

            int todayInt = int.Parse(DateTime.Today.ToString("yyyyMMdd"));

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (var check = new SqlCommand(checkSql, conn))
                {
                    check.Parameters.AddWithValue("@cid", clientId);
                    check.Parameters.AddWithValue("@tid", tripId);

                    int count = (int)await check.ExecuteScalarAsync();
                    if (count > 0)
                        return false;
                }

                using (var insert = new SqlCommand(insertSql, conn))
                {
                    insert.Parameters.AddWithValue("@cid", clientId);
                    insert.Parameters.AddWithValue("@tid", tripId);
                    insert.Parameters.AddWithValue("@reg", todayInt);
                    await insert.ExecuteNonQueryAsync();
                }
            }

            return true;
        }
        public async Task<bool> RemoveClientFromTrip(int clientId, int tripId)
        {
            string sql = "DELETE FROM Client_Trip WHERE IdClient=@cid AND IdTrip=@tid";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@cid", clientId);
                cmd.Parameters.AddWithValue("@tid", tripId);
                await conn.OpenAsync();
                int rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
        }
        
    }
}
