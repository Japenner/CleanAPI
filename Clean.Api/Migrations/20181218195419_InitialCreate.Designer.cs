﻿// <auto-generated />
#pragma warning disable 1591
using System;
using Clean.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Clean.Api.Migrations
{
    [DbContext(typeof(CleanContext))]
    [Migration("20181218195419_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Clean.Core.Entities.EntityVersion", b =>
                {
                    b.Property<Guid>("EntityId");

                    b.Property<string>("EntityType");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("ChangedBy")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Changes")
                        .IsRequired();

                    b.HasKey("EntityId", "EntityType", "Timestamp");

                    b.ToTable("Versions");
                });

            modelBuilder.Entity("Clean.Core.Entities.ToDoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<bool>("IsDone")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset>("ModifiedAt");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("ToDoItems");
                });
#pragma warning restore 612, 618, 1591
        }
    }
}
