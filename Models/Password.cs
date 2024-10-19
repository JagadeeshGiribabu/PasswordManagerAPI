using System;
using System.ComponentModel.DataAnnotations;

namespace PasswordManagerApi.Models
{
	public class Password
	{
            public int Id { get; set; }

            [Required]
            public string? Category { get; set; }

            [Required]
            public string? App { get; set; }

            [Required]
            public string? UserName { get; set; }

            [Required]
            public string? EncryptedPassword { get; set; }
	}
}