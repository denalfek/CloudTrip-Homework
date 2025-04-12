﻿using System.ComponentModel.DataAnnotations;

namespace CloudTrip.Homework.Common.Dto;

public class FlightModel
{
    public record SearchCriteria(
        //string Origin,
        //string Destination,
        [Required] string Airline,
        [Required] DateTime DepartureDate,
        int? Passengers = null,
        decimal? MaxPrice = null,
        int? MaxStops = null);

    public record SortCriteria(
        SortField SortField, bool Accending = true);

    public record AvailableFlight(
        string ProviderName,
        string FlightCode,
        string Airline,
        DateTime DepartureDate,
        DateTime ArrivalTime,
        decimal Price,
        int Stops);
}
