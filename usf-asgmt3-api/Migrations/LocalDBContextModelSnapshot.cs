﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using usf_asgmt3_api.Integration.LocalDataRepo;

namespace usf_asgmt3_api.Migrations
{
    [DbContext(typeof(LocalDBContext))]
    partial class LocalDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("usf_asgmt3_api.model.Company", b =>
                {
                    b.Property<string>("ticker")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("cik");

                    b.Property<string>("industry_category");

                    b.Property<string>("lei");

                    b.Property<long?>("market_cap");

                    b.Property<string>("name");

                    b.HasKey("ticker");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("usf_asgmt3_api.model.Price", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("adj_close");

                    b.Property<float>("adj_high");

                    b.Property<float>("adj_low");

                    b.Property<float>("adj_open");

                    b.Property<int>("adj_volume");

                    b.Property<float>("close");

                    b.Property<string>("date");

                    b.Property<string>("frequency");

                    b.Property<float>("high");

                    b.Property<bool>("intraperiod");

                    b.Property<float>("low");

                    b.Property<float>("open");

                    b.Property<string>("ticker");

                    b.Property<int>("volume");

                    b.HasKey("id");

                    b.ToTable("Prices");
                });
#pragma warning restore 612, 618
        }
    }
}
