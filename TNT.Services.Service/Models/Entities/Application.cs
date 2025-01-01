﻿namespace TNT.Services.Service.Models.Entities;

public class Application
{
  public Guid ID { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Secret { get; set; } = string.Empty;
}
