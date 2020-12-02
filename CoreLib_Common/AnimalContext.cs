﻿using System;
using System.Collections.Generic;
using CoreLib_Common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoreLib_Common
{
    public class AnimalContext: DbContext
    {
        #region Tables

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Beaver> Beavers { get; set; }
        public DbSet<Crow> Crows { get; set; }
        public DbSet<Deer> Deers { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Drawback> Drawbacks { get; set; }
        public DbSet<JobDrawback> JobDrawbacks { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<NormalFood> NormalFood { get; set; }
        public DbSet<VeganFood> VeganFood { get; set; }
        public DbSet<MapToQuery> MapToQuery { get; set; }

        // Property bags
        public DbSet<Dictionary<string, object>> Products => Set<Dictionary<string, object>>("Product");
        public DbSet<Dictionary<string, object>> Categories => Set<Dictionary<string, object>>("Category");

        #endregion

        #region C-ors

        public AnimalContext()
        {
        }

        public AnimalContext(DbContextOptions<AnimalContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: fix connection property
                optionsBuilder.UseSqlServer("Server=unit-1019\\sqlexpress;Database=BeaversLife;Trusted_Connection=True;"+
                                            "MultipleActiveResultSets=True");
                //optionsBuilder.UseSqlServer("Server=localhost;Database=BeaversLife;Trusted_Connection=True;"+
                //                            "MultipleActiveResultSets=True");
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
                optionsBuilder.AddInterceptors(new MySaveChangesInterceptor());
            }
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TPT
            modelBuilder.Entity<Animal>().ToTable("Animals");
            modelBuilder.Entity<Beaver>().ToTable("Beavers");
            modelBuilder.Entity<Crow>().ToTable("Crows");
            modelBuilder.Entity<Deer>().ToTable("Deers");

            // Many-to-many
            modelBuilder.Entity<Animal>()
                        .HasMany(a => a.Clubs)
                        .WithMany(c => c.Animals)
                        .UsingEntity<AnimalClub>(
                            c => c.HasOne(c => c.Club)
                                  .WithMany().HasForeignKey(c => c.ClubId),
                            a => a.HasOne(a => a.Animal)
                                  .WithMany().HasForeignKey(a => a.AnimalId),
                            j =>
                            {
                                j.Property(pt => pt.PublicationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                                j.HasKey(t => new {t.AnimalId, t.ClubId});
                            }
                        )
                ;

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.Property(e => e.TheGrade)
                      .HasColumnType("decimal(3, 2)")
                      .HasAnnotation("Relational:ColumnType", "decimal(3, 2)");
            });

            // Many-to-many old style
            modelBuilder.Entity<JobDrawback>(entity =>
            {
                entity.HasKey(j => new
                {
                    j.JobId, j.DrawbackId
                });

                entity.HasOne(jd => jd.Job)
                      .WithMany(j => j.JobDrawbacks)
                      .HasForeignKey(x => x.JobId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(jd => jd.Drawback)
                      .WithMany(d => d.JobDrawbacks)
                      .HasForeignKey(x => x.DrawbackId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            //Map entity types to queries
            modelBuilder.Entity<MapToQuery>().ToSqlQuery(
                @"    select b.Id, b.Fluffiness, b.Size, ac.ClubId as ClubId from Beavers b
                              inner join AnimalClub ac on b.Id = ac.AnimalId
                              Union all
                              select cr.Id, 2, 1, ac.ClubId as ClubId  from Crows cr
                              inner join AnimalClub ac on cr.Id = ac.AnimalId
                    ");

            // Property bags
            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Category", category =>
            {
                category.IndexerProperty<int>("Id");
                category.IndexerProperty<string>("Name").IsRequired();
                category.IndexerProperty<int?>("FoodId");

                category.HasOne(typeof(Food)).WithOne() /*.HasForeignKey("Category", "FoodId")*/;
            });

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Product", product =>
            {
                product.IndexerProperty<int>("Id");
                product.IndexerProperty<string>("Name").IsRequired();
                product.IndexerProperty<int?>("CategoryId");

                product.HasOne("Category", null).WithMany();
            });

            // Required 1:1 dependents
            modelBuilder.Entity<Drawback>(drawback =>
            {
                drawback.OwnsOne(d => d.Consequence,
                    cons => { cons.Property(c => c.Name).IsRequired(); });
                drawback.Navigation(d => d.Consequence).IsRequired();
            });
        }
    }
}
