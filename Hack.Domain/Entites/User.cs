using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Hack.Domain.Entities;

public class User : IdentityUser
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;

    public byte? ProfilePicture { get; set; } // сюда по-хорошему нужно blob хранилище прикрутить, поэтому потом сделаем

    public int Points { get; set; } = 0;
    public List<int>? MarksDiscovered { get; set; } // сюда пихаем массив айдишников мест для аналитики

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}