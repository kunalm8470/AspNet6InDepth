using System.Security.Claims;

List<Claim> aadharClaims = new()
{
    new Claim("name", "John Doe"),
    new Claim("aadharNumber", "1234 5678 9012")
};

ClaimsIdentity aadharIdentity = new(aadharClaims);

List<Claim> panCardClaims = new()
{
    new Claim("name", "John Doe"),
    new Claim("panNumber", "AWER45670A")
};

ClaimsIdentity panCardIdentity = new ClaimsIdentity(panCardClaims);

List<ClaimsIdentity> claimsIdentities = new()
{
    aadharIdentity,
    panCardIdentity
};

ClaimsPrincipal claimsPrincipal = new(claimsIdentities);

Console.ReadLine();