using Microsoft.Data.SqlClient;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server" +
                                                " Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    
    public async Task<List<TripDTO>> GetTrips()
    {
        var trips = new List<TripDTO>();
        
        string tripQuery = @"
        SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople
        FROM Trip";

        string countryQuery = @"
        SELECT ct.IdTrip, c.Name
        FROM Country_Trip ct
        JOIN Country c ON ct.IdCountry = c.IdCountry";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            using (SqlCommand cmd = new SqlCommand(tripQuery, conn))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int idOrdinal = reader.GetOrdinal("IdTrip");
                        trips.Add(new TripDTO
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateFrom = reader.GetDateTime(3),
                            DateTo = reader.GetDateTime(4),
                            MaxPeople = reader.GetInt32(5),
                            Countries = new List<CountryDTO>()
                        });
                    }
                }
            }

            using (SqlCommand cmd = new SqlCommand(countryQuery, conn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int tripId = reader.GetInt32(0);
                    string countryName = reader.GetString(1);

                    var trip = trips.FirstOrDefault(t => t.Id == tripId);
                    if (trip != null)
                    {
                        trip.Countries.Add(new CountryDTO { Name = countryName });
                    }
                }
            }
        }

        return trips;
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
