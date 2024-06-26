﻿namespace Versus.Core.Services.Session;

public class RegisterViewModel
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RepeatPassword { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool AcceptedTos { get; set; }
}
