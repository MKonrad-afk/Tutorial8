namespace Tutorial8.Models.DTOs;

public class TripDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CountryDTO> Countries { get; set; }
}

public class CountryDTO
{
    public int Id { get; set;  }
    public string Name { get; set; }
}

public class ClientDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Pesel { get; set; }
}