﻿using System.ComponentModel.DataAnnotations;

public class ForgotPasswordVM
{
    [Required]
    public string Username { get; set; }
}

public class ResetPasswordVM
{
    [Required]
    public string Username { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}
