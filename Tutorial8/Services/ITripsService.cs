﻿
namespace Tutorial8.Services;

public interface ITripsService
{
    Task<List<TripDTO>> GetTrips();


}