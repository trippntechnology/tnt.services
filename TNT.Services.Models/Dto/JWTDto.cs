namespace TNT.Services.Models.Dto;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class JWTDto
{
  public JWT Token { get; private set; }

  public JWTDto(string token) { Token = new JWT(token); }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
