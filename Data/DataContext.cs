using System;
using Microsoft.EntityFrameworkCore;
using PasswordManagerApi.Models;

namespace PasswordManagerApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Password> Passwords { get; set; }
    }
}

