using Newtonsoft.Json;

namespace Api.Models;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public Address Address { get; set; }

    public string Phone { get; set; }

    public string Website { get; set; }

    public Company Company { get; set; }
}

public class Address
{
    public string Street { get; set; }

    public string Suite { get; set; }

    public string City { get; set; }

    public string ZipCode { get; set; }

    [JsonProperty("geo")]
    public Geo GeographicInformation { get; set; }
}

public class Geo
{
    [JsonProperty("lat")]
    public string Latitude { get; set; }

    [JsonProperty("lng")]
    public string Longitude { get; set; }
}

public class Company
{
    public string Name { get; set; }

    public string CatchPhrase { get; set; }

    public string Bs { get; set; }
}
