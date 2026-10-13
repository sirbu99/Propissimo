using Propissimo.Domain.Common;
using Propissimo.Domain.Enums;
using System.Text.RegularExpressions;

namespace Propissimo.Domain.ValueObjects;

public sealed record Address
{
    public string Country { get; }

    public string City { get; }

    public string Street { get; }

    public string? StreetNumber { get; } 

    public string? Building { get; } 

    public int? Floor { get; }

    public string? Apartment { get; } 

    public string? PostalCode { get; } 

    public static Address Create(string country, string city, string street, string? streetNumber = null, string? building = null, int? floor = null, string? apartment = null, string? postalCode = null)
    {
        // Normalize required fields first (trim + collapse internal whitespace)
        var normCountry = Normalize(country);
        if (string.IsNullOrWhiteSpace(normCountry))
            throw new DomainException(DomainExceptionCode.AddressCountryRequired, "Country cannot be null or empty.");

        var normCity = Normalize(city);
        if (string.IsNullOrWhiteSpace(normCity))
            throw new DomainException(DomainExceptionCode.AddressCityRequired, "City cannot be null or empty.");

        var normStreet = Normalize(street);
        if (string.IsNullOrWhiteSpace(normStreet))
            throw new DomainException(DomainExceptionCode.AddressStreetRequired, "Street cannot be null or empty.");

        // Normalize optional fields; allow null when empty after normalization
        var normBuilding = NormalizeOrNull(building);
        var normApartment = NormalizeOrNull(apartment);
        var normPostalCode = NormalizeOrNull(postalCode);
        var normStreetNumber = NormalizeOrNull(streetNumber);

        return new Address(normCountry, normCity, normStreet, normStreetNumber, normBuilding, floor, normApartment, normPostalCode);
    }

    private Address(string country, string city, string street, string? streetNumber = null, string? building = null, int? floor = null, string? apartment = null, string? postalCode = null) 
    {
        Country = country;
        City = city;
        Street = street;
        StreetNumber = streetNumber;
        Building = building;
        Floor = floor;
        Apartment = apartment;
        PostalCode = postalCode;
    }

    private static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        // Collapse any whitespace sequence to a single space and trim ends
        var collapsed = Regex.Replace(value, "\\s+", " ");
        return collapsed.Trim();
    }

    private static string? NormalizeOrNull(string? value)
    {
        var norm = Normalize(value);
        return string.IsNullOrEmpty(norm) ? null : norm;
    }
}