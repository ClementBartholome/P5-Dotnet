﻿// <auto-generated />
using System;
using Express_Voitures.Data;
using Express_Voitures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Express_Voitures.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240405122002_AddVoitureVenteBooleenVendu")]
    partial class AddVoitureVenteBooleenVendu
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Express_Voitures.Models.Annonce", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoFilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VoitureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VoitureId");

                    b.ToTable("Annonce");
                });

            modelBuilder.Entity("Express_Voitures.Models.Finition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ModeleId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ModeleId");

                    b.ToTable("Finition");
                });

            modelBuilder.Entity("Express_Voitures.Models.Marque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Marque");
                });

            modelBuilder.Entity("Express_Voitures.Models.Modele", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MarqueId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MarqueId");

                    b.ToTable("Modele");
                });

            modelBuilder.Entity("Express_Voitures.Models.Voiture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Annee")
                        .HasColumnType("int");

                    b.Property<string>("CodeVin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("CoutReparations")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DateAchat")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Disponible")
                        .HasColumnType("bit");

                    b.Property<int>("FinitionId")
                        .HasColumnType("int");

                    b.Property<int>("MarqueId")
                        .HasColumnType("int");

                    b.Property<int>("ModeleId")
                        .HasColumnType("int");

                    b.Property<decimal>("PrixAchat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Reparations")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FinitionId");

                    b.HasIndex("MarqueId");

                    b.HasIndex("ModeleId");

                    b.ToTable("Voiture");
                });

            modelBuilder.Entity("Express_Voitures.Models.VoitureVente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateDisponibilite")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateVente")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PrixVente")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Vendu")
                        .HasColumnType("bit");

                    b.Property<int>("VoitureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VoitureId");

                    b.ToTable("VoitureVente");
                });

            modelBuilder.Entity("Express_Voitures.Models.Annonce", b =>
                {
                    b.HasOne("Express_Voitures.Models.Voiture", "Voiture")
                        .WithMany()
                        .HasForeignKey("VoitureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Voiture");
                });

            modelBuilder.Entity("Express_Voitures.Models.Finition", b =>
                {
                    b.HasOne("Express_Voitures.Models.Modele", null)
                        .WithMany("Finitions")
                        .HasForeignKey("ModeleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Express_Voitures.Models.Modele", b =>
                {
                    b.HasOne("Express_Voitures.Models.Marque", "Marque")
                        .WithMany("Modeles")
                        .HasForeignKey("MarqueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Marque");
                });

            modelBuilder.Entity("Express_Voitures.Models.Voiture", b =>
                {
                    b.HasOne("Express_Voitures.Models.Finition", "Finition")
                        .WithMany()
                        .HasForeignKey("FinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Express_Voitures.Models.Marque", "Marque")
                        .WithMany("Voitures")
                        .HasForeignKey("MarqueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Express_Voitures.Models.Modele", "Modele")
                        .WithMany()
                        .HasForeignKey("ModeleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Finition");

                    b.Navigation("Marque");

                    b.Navigation("Modele");
                });

            modelBuilder.Entity("Express_Voitures.Models.VoitureVente", b =>
                {
                    b.HasOne("Express_Voitures.Models.Voiture", "Voiture")
                        .WithMany()
                        .HasForeignKey("VoitureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Voiture");
                });

            modelBuilder.Entity("Express_Voitures.Models.Marque", b =>
                {
                    b.Navigation("Modeles");

                    b.Navigation("Voitures");
                });

            modelBuilder.Entity("Express_Voitures.Models.Modele", b =>
                {
                    b.Navigation("Finitions");
                });
#pragma warning restore 612, 618
        }
    }
}
