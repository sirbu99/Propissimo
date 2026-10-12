using Propissimo.Domain.Common;
using Propissimo.Domain.Enums;
using Propissimo.Domain.ValueObjects;

namespace Propissimo.Domain.UnitTests.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Create_WithValidFields_ReturnsAddress()
    {
        // Arrange
        var country = " Romania ";
        var city = "  Bucharest ";
        var street = "Main\tStreet  ";
        var streetNumber = " 12 A ";
        var building = " B1 ";
        int? floor = 2;
        var apartment = " 3B ";
        var postalCode = " 12345 ";

        // Act
        var address = Address.Create(country, city, street, streetNumber, building, floor, apartment, postalCode);

        // Assert
        Assert.Equal("Romania", address.Country);
        Assert.Equal("Bucharest", address.City);
        Assert.Equal("Main Street", address.Street);
        Assert.Equal("12 A", address.StreetNumber);
        Assert.Equal("B1", address.Building);
        Assert.Equal(2, address.Floor);
        Assert.Equal("3B", address.Apartment);
        Assert.Equal("12345", address.PostalCode);
    }

    [Fact]
    public void Create_WithEmptyCountry_ThrowsDomainException()
    {
        // Arrange
        var country = "   ";
        var city = "City";
        var street = "Street";

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Address.Create(country, city, street));
        Assert.Equal(DomainExceptionCode.AddressCountryRequired, ex.Code);
    }

    [Fact]
    public void Create_WithEmptyCity_ThrowsDomainException()
    {
        // Arrange
        var country = "Country";
        var city = "";
        var street = "Street";

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Address.Create(country, city, street));
        Assert.Equal(DomainExceptionCode.AddressCityRequired, ex.Code);
    }

    [Fact]
    public void Create_WithEmptyStreet_ThrowsDomainException()
    {
        // Arrange
        var country = "Country";
        var city = "City";
        var street = " \t  ";

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Address.Create(country, city, street));
        Assert.Equal(DomainExceptionCode.AddressStreetRequired, ex.Code);
    }

    [Fact]
    public void Create_WithEmptyOptionalFields_SetsNullForOptionals()
    {
        // Arrange
        var country = "Country";
        var city = "City";
        var street = "Street";
        var streetNumber = " ";
        var building = "   ";
        int? floor = null;
        var apartment = "";
        var postalCode = " \t ";

        // Act
        var address = Address.Create(country, city, street, streetNumber, building, floor, apartment, postalCode);

        // Assert
        Assert.Null(address.StreetNumber);
        Assert.Null(address.Building);
        Assert.Null(address.Apartment);
        Assert.Null(address.PostalCode);
    }

    [Fact]
    public void Equals_SameFields_AreEqual()
    {
        // Arrange
        var first = Address.Create("Country", "City", "Street");
        var second = Address.Create("Country", "City", "Street");

        // Act & Assert
        Assert.Equal(first, second);
    }

    public static IEnumerable<object[]> InvalidRequiredFieldCases()
    {
        yield return new object[] { nameof(Address.Country), "", DomainExceptionCode.AddressCountryRequired };
        yield return new object[] { nameof(Address.Country), " ", DomainExceptionCode.AddressCountryRequired };

        yield return new object[] { nameof(Address.City), "", DomainExceptionCode.AddressCityRequired };
        yield return new object[] { nameof(Address.City), " ", DomainExceptionCode.AddressCityRequired };

        yield return new object[] { nameof(Address.Street), "", DomainExceptionCode.AddressStreetRequired };
        yield return new object[] { nameof(Address.Street), " ", DomainExceptionCode.AddressStreetRequired };
    }

    [Theory]
    [MemberData(nameof(InvalidRequiredFieldCases))]
    public void Create_WithInvalidRequiredField_ThrowsDomainException(string field, string value, DomainExceptionCode expectedCode)
    {
        // Arrange
        var country = "Country";
        var city = "City";
        var street = "Street";

        if (field == nameof(Address.Country)) country = value;
        else if (field == nameof(Address.City)) city = value;
        else if (field == nameof(Address.Street)) street = value;

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Address.Create(country, city, street, null, null, null, null, null));
        Assert.Equal(expectedCode, ex.Code);
    }

    [Fact]
    public void Equals_AfterNormalization_AreEqual()
    {
        // Arrange
        var first = Address.Create(" Romania ", " Bucharest ", "Main Street");
        var second = Address.Create("Romania", "Bucharest", "Main Street");

        // Act & Assert
        Assert.Equal(first, second);
    }
}
