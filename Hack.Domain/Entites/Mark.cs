﻿using Microsoft.EntityFrameworkCore;

namespace Hack.Domain.Entities;

[PrimaryKey("Id")]
[Index(nameof(Title), IsUnique = true)]
public class Mark // локация для поиска, собственно
{
    public int Id { get; set; }

    public string EmojifiedTitle { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Type { get; set; } = 0;
    public string HeaderImg { get; set; } = string.Empty;
    
    public int Reward { get; set; } = 0;

    public double Longitude { get; set; } = 0;
    public double Latitude { get; set; } = 0;
}