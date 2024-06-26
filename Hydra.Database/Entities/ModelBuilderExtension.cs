﻿using Microsoft.EntityFrameworkCore;

namespace Hydra.Database.Entities
{
    public static class ModelBuilderExtension
    {
        public static DateTime Date = new DateTime(2024, 04, 24);

        public static void SeedData(this ModelBuilder modelBuilder)
        {
            SeedUserMasterData(modelBuilder);
            SeedRoleMasterData(modelBuilder);
            SeedUserRoleMasterData(modelBuilder);
            SeedAccessLevelMasterData(modelBuilder);
            SeedBadgeTypeMasterData(modelBuilder);
        }

        public static void SeedUserMasterData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "",
                UserName = "Admin",
                Email = "hydra@yopmail.com",
                Password = "3AhCUZedQxVLajDQSZhRirNTvEyK/luGud/X7oAXJX0=",
                AccessLevelId = 3,
                IsActive = true,
                IsApproved = true,
                CreatedDate = Date,
                UpdatedDate = Date
            });
        }

        public static void SeedRoleMasterData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "UniversityAdmin" },
            new Role { Id = 3, Name = "Staff" },
            new Role { Id = 4, Name = "Learner" });
        }

        public static void SeedUserRoleMasterData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
            new UserRole { Id = 1, UserId = 1, RoleId = 1 });
        }

        public static void SeedAccessLevelMasterData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLevel>().HasData(
            new AccessLevel { Id = 1, Name = "View only" },
            new AccessLevel { Id = 2, Name = "View and edit" },
            new AccessLevel { Id = 3, Name = "View, edit and delete" });
        }

        public static void SeedBadgeTypeMasterData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BadgeType>().HasData(
            new BadgeType { Id = 1, Name = "Badge" },
            new BadgeType { Id = 2, Name = "Certificate" },
            new BadgeType { Id = 3, Name = "License" },
            new BadgeType { Id = 4, Name = "Miscellaneous" });
        }
    }
}
