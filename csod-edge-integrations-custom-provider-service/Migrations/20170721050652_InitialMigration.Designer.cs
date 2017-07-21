using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using csod_edge_integrations_custom_provider_service.Models;

namespace csodedgeintegrationscustomproviderservice.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20170721050652_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("csod_edge_integrations_custom_provider_service.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("VendorUrl");

                    b.Property<string>("VendorUserIdForUser");

                    b.HasKey("Id");

                    b.ToTable("Settings");

                    b.HasAnnotation("Sqlite:TableName", "settings");
                });

            modelBuilder.Entity("csod_edge_integrations_custom_provider_service.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.Property<int>("SettingsId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("SettingsId");

                    b.ToTable("Users");

                    b.HasAnnotation("Sqlite:TableName", "users");
                });

            modelBuilder.Entity("csod_edge_integrations_custom_provider_service.Models.User", b =>
                {
                    b.HasOne("csod_edge_integrations_custom_provider_service.Models.Settings", "Settings")
                        .WithMany()
                        .HasForeignKey("SettingsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
