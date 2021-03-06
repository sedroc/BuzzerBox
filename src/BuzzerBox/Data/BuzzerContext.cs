﻿using BuzzerEntities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Data
{
    public class BuzzerContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<RegistrationToken> RegistrationTokens {get;set;}
        public DbSet<SessionToken> SessionTokens { get; set; }

        public BuzzerContext(DbContextOptions<BuzzerContext> options) : base(options)
        {
            //
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // the room entity is on one end of the depedency chain
            modelBuilder.Entity<Room>().ToTable("Rooms");
            // the user entity is on one end of the dependency chain
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().Property(u => u.Name).IsRequired();

            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Question>().HasOne(q => q.Room).WithMany(r => r.Questions).HasForeignKey(q => q.RoomId);

            modelBuilder.Entity<Response>().ToTable("Responses");
            modelBuilder.Entity<Response>().HasOne(r => r.Question).WithMany(q => q.Responses).HasForeignKey(r => r.QuestionId);

            modelBuilder.Entity<Vote>().ToTable("Votes");
            modelBuilder.Entity<Vote>().HasOne(v => v.Response).WithMany(r => r.Votes).HasForeignKey(v => v.ResponseId);
            modelBuilder.Entity<Vote>().HasOne(v => v.User).WithMany(u => u.Votes).HasForeignKey(v => v.UserId);

            // RegistrationTokens dont need a reference to the user whom they are used by.
            modelBuilder.Entity<RegistrationToken>().ToTable("RegistrationTokens");
            modelBuilder.Entity<RegistrationToken>().HasAlternateKey(s => s.Token).HasName("AlternateKey_Token");

            modelBuilder.Entity<SessionToken>().ToTable("SessionTokens");
            modelBuilder.Entity<SessionToken>().HasOne(t => t.User).WithMany(u => u.SessionToken).HasForeignKey(t => t.UserId);
            modelBuilder.Entity<SessionToken>().HasAlternateKey(s => s.Token).HasName("AlternateKey_Token");
        }
    }
}
