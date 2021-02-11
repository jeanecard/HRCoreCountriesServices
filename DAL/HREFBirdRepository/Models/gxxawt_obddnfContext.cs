using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HREFBirdRepository.Models
{
    public partial class gxxawt_obddnfContext : DbContext
    {
        public gxxawt_obddnfContext()
        {
        }

        public gxxawt_obddnfContext(DbContextOptions<gxxawt_obddnfContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Boundaries> Boundaries { get; set; }
        public virtual DbSet<Hrbird> Hrbird { get; set; }
        public virtual DbSet<HrmainPicture> HrmainPicture { get; set; }
        public virtual DbSet<HrmainSound> HrmainSound { get; set; }
        public virtual DbSet<Hrnames> Hrnames { get; set; }
        public virtual DbSet<Hrpicture> Hrpicture { get; set; }
        public virtual DbSet<Hrplace> Hrplace { get; set; }
        public virtual DbSet<Hrsimilarity> Hrsimilarity { get; set; }
        public virtual DbSet<Hrsound> Hrsound { get; set; }
        public virtual DbSet<HrsoundType> HrsoundType { get; set; }
        public virtual DbSet<Hrsource> Hrsource { get; set; }
        public virtual DbSet<HrstatutEspece> HrstatutEspece { get; set; }
        public virtual DbSet<HrtypeAge> HrtypeAge { get; set; }
        public virtual DbSet<Layer> Layer { get; set; }
        public virtual DbSet<Topology> Topology { get; set; }
        public virtual DbSet<VMainRecords> VMainRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("host = db.qgiscloud.com; Username = canard; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("hstore")
                .HasPostgresExtension("postgis")
                .HasPostgresExtension("postgis_topology");

            modelBuilder.Entity<Boundaries>(entity =>
            {
                entity.HasKey(e => e.QcId)
                    .HasName("boundaries_pkey");

                entity.ToTable("boundaries");

                entity.Property(e => e.QcId).HasColumnName("qc_id");

                entity.Property(e => e.Area).HasColumnName("area");

                entity.Property(e => e.Fips)
                    .HasColumnName("fips")
                    .HasColumnType("character varying");

                entity.Property(e => e.Iso2)
                    .HasColumnName("iso2")
                    .HasColumnType("character varying");

                entity.Property(e => e.Iso3)
                    .HasColumnName("iso3")
                    .HasColumnType("character varying");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Pop2005).HasColumnName("pop2005");

                entity.Property(e => e.Region).HasColumnName("region");

                entity.Property(e => e.Subregion).HasColumnName("subregion");

                entity.Property(e => e.Un).HasColumnName("un");
            });

            modelBuilder.Entity<Hrbird>(entity =>
            {
                entity.ToTable("HRBird");

                entity.HasComment("Main HR Table Birds");

                entity.HasIndex(e => e.Source)
                    .HasName("fki_source_fk");

                entity.HasIndex(e => e.Statut)
                    .HasName("fki_statutEspece_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(60);

                entity.Property(e => e.Caption).HasColumnName("caption");

                entity.Property(e => e.EnvergureMax).HasColumnName("envergure_max");

                entity.Property(e => e.EnvergureMin).HasColumnName("envergure_min");

                entity.Property(e => e.LongueurMax).HasColumnName("longueur_max");

                entity.Property(e => e.LongueurMin).HasColumnName("longueur_min");

                entity.Property(e => e.NombrePonteMax).HasColumnName("nombre_ponte_max");

                entity.Property(e => e.NombrePonteMin).HasColumnName("nombre_ponte_min");

                entity.Property(e => e.OeufsParPonteMax).HasColumnName("oeufs_par_ponte_max");

                entity.Property(e => e.OeufsParPonteMin).HasColumnName("oeufs_par_ponte_min");

                entity.Property(e => e.PeriodeRepro).HasColumnName("periode_repro");

                entity.Property(e => e.PoidsMax).HasColumnName("poids_max");

                entity.Property(e => e.PoidsMin).HasColumnName("poids_min");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.Property(e => e.Statut).HasColumnName("statut");

                entity.HasOne(d => d.SourceNavigation)
                    .WithMany(p => p.Hrbird)
                    .HasForeignKey(d => d.Source)
                    .HasConstraintName("source_fk");

                entity.HasOne(d => d.StatutNavigation)
                    .WithMany(p => p.Hrbird)
                    .HasForeignKey(d => d.Statut)
                    .HasConstraintName("statutEspece_fk");
            });

            modelBuilder.Entity<HrmainPicture>(entity =>
            {
                entity.HasKey(e => e.IdBird)
                    .HasName("PK_MainPicture");

                entity.ToTable("HRMainPicture");

                entity.HasIndex(e => e.IdPicture)
                    .HasName("fki_FK_HRMainPicture_pic");

                entity.Property(e => e.IdBird)
                    .HasColumnName("id_bird")
                    .HasMaxLength(60);

                entity.Property(e => e.IdPicture).HasColumnName("id_picture");

                entity.HasOne(d => d.IdBirdNavigation)
                    .WithOne(p => p.HrmainPicture)
                    .HasForeignKey<HrmainPicture>(d => d.IdBird)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HRMAinPicture_bird");

                entity.HasOne(d => d.IdPictureNavigation)
                    .WithMany(p => p.HrmainPicture)
                    .HasForeignKey(d => d.IdPicture)
                    .HasConstraintName("FK_HRMainPicture_pic");
            });

            modelBuilder.Entity<HrmainSound>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HRMainSound");

                entity.HasIndex(e => e.IdBird)
                    .HasName("fki_FK_MAinSound_bird");

                entity.HasIndex(e => e.IdSound)
                    .HasName("fki_FK_MainSound_sound");

                entity.Property(e => e.IdBird)
                    .HasColumnName("id_bird")
                    .HasMaxLength(60);

                entity.Property(e => e.IdSound).HasColumnName("id_sound");

                entity.HasOne(d => d.IdBirdNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdBird)
                    .HasConstraintName("FK_MAinSound_bird");

                entity.HasOne(d => d.IdSoundNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdSound)
                    .HasConstraintName("FK_MainSound_sound");
            });

            modelBuilder.Entity<Hrnames>(entity =>
            {
                entity.HasKey(e => new { e.IdBird, e.Name })
                    .HasName("HRNames_pkey");

                entity.ToTable("HRNames");

                entity.HasIndex(e => e.IdBird)
                    .HasName("fki_id_bird_fk");

                entity.Property(e => e.IdBird)
                    .HasColumnName("id_bird")
                    .HasMaxLength(60);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(60);

                entity.Property(e => e.LangIso639)
                    .HasColumnName("lang_iso639")
                    .HasMaxLength(2);

                entity.HasOne(d => d.IdBirdNavigation)
                    .WithMany(p => p.Hrnames)
                    .HasForeignKey(d => d.IdBird)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_bird_fk");
            });

            modelBuilder.Entity<Hrpicture>(entity =>
            {
                entity.HasKey(e => e.IdPicture)
                    .HasName("PK_HRPicture_id");

                entity.ToTable("HRPicture");

                entity.Property(e => e.IdPicture)
                    .HasColumnName("id_picture")
                    .ValueGeneratedNever();

                entity.Property(e => e.Credit)
                    .HasColumnName("credit")
                    .HasMaxLength(100);

                entity.Property(e => e.IdBird)
                    .HasColumnName("id_bird")
                    .HasMaxLength(60);

                entity.Property(e => e.IdMale).HasColumnName("id_male");

                entity.Property(e => e.IdTypeAge).HasColumnName("id_type_age");

                entity.Property(e => e.Link).HasColumnName("link");

                entity.Property(e => e.Source).HasColumnName("source");
            });

            modelBuilder.Entity<Hrplace>(entity =>
            {
                entity.HasKey(e => new { e.IdBird, e.IdCountry, e.RegionCountry })
                    .HasName("HRPlace_pkey");

                entity.ToTable("HRPlace");

                entity.Property(e => e.IdBird)
                    .HasColumnName("id_bird")
                    .HasMaxLength(60);

                entity.Property(e => e.IdCountry)
                    .HasColumnName("id_country")
                    .HasColumnType("character(3)[]");

                entity.Property(e => e.RegionCountry)
                    .HasColumnName("region_country")
                    .HasMaxLength(100);

                entity.Property(e => e.FrequenceObservation).HasColumnName("frequence_observation");

                entity.Property(e => e.PeriodPresence).HasColumnName("period_presence");
            });

            modelBuilder.Entity<Hrsimilarity>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HRSimilarity");

                entity.HasComment(@"HRBird similarities :
eg. merle / grive musicienne");

                entity.Property(e => e.Caption)
                    .HasColumnName("caption")
                    .HasMaxLength(2000);

                entity.Property(e => e.Id1)
                    .IsRequired()
                    .HasColumnName("id_1")
                    .HasMaxLength(60)
                    .HasComment("First bird id");

                entity.Property(e => e.Id2)
                    .IsRequired()
                    .HasColumnName("id_2")
                    .HasMaxLength(60)
                    .HasComment("bird ID 2");
            });

            modelBuilder.Entity<Hrsound>(entity =>
            {
                entity.HasKey(e => e.IdSound);

                entity.ToTable("HRSound");

                entity.Property(e => e.IdSound)
                    .HasColumnName("id_sound")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdBird)
                    .IsRequired()
                    .HasColumnName("id_bird")
                    .HasMaxLength(60);

                entity.Property(e => e.IdSource).HasColumnName("id_source");

                entity.Property(e => e.IdTypeSound).HasColumnName("id_type_sound");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<HrsoundType>(entity =>
            {
                entity.HasKey(e => e.IdTypeSound)
                    .HasName("HRSoundType_pkey");

                entity.ToTable("HRSoundType");

                entity.Property(e => e.IdTypeSound)
                    .HasColumnName("id_type_sound")
                    .ValueGeneratedNever();

                entity.Property(e => e.Caption)
                    .HasColumnName("caption")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Hrsource>(entity =>
            {
                entity.HasKey(e => e.IdSource)
                    .HasName("HRSource_pkey");

                entity.ToTable("HRSource");

                entity.Property(e => e.IdSource)
                    .HasColumnName("id_source")
                    .ValueGeneratedNever();

                entity.Property(e => e.Caption).HasColumnName("caption");
            });

            modelBuilder.Entity<HrstatutEspece>(entity =>
            {
                entity.HasKey(e => e.IdStatut)
                    .HasName("HRStatutEspece_pkey");

                entity.ToTable("HRStatutEspece");

                entity.Property(e => e.IdStatut)
                    .HasColumnName("id_statut")
                    .ValueGeneratedNever();

                entity.Property(e => e.Caption)
                    .HasColumnName("caption")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<HrtypeAge>(entity =>
            {
                entity.HasKey(e => e.IdType)
                    .HasName("HRTypeAge_pkey");

                entity.ToTable("HRTypeAge");

                entity.Property(e => e.IdType)
                    .HasColumnName("id_type")
                    .ValueGeneratedNever();

                entity.Property(e => e.Caption)
                    .HasColumnName("caption")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Layer>(entity =>
            {
                entity.HasKey(e => new { e.TopologyId, e.LayerId })
                    .HasName("layer_pkey");

                entity.ToTable("layer", "topology");

                entity.HasIndex(e => new { e.SchemaName, e.TableName, e.FeatureColumn })
                    .HasName("layer_schema_name_table_name_feature_column_key")
                    .IsUnique();

                entity.Property(e => e.TopologyId).HasColumnName("topology_id");

                entity.Property(e => e.LayerId).HasColumnName("layer_id");

                entity.Property(e => e.ChildId).HasColumnName("child_id");

                entity.Property(e => e.FeatureColumn)
                    .IsRequired()
                    .HasColumnName("feature_column")
                    .HasColumnType("character varying");

                entity.Property(e => e.FeatureType).HasColumnName("feature_type");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.SchemaName)
                    .IsRequired()
                    .HasColumnName("schema_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnName("table_name")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Topology)
                    .WithMany(p => p.Layer)
                    .HasForeignKey(d => d.TopologyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("layer_topology_id_fkey");
            });

            modelBuilder.Entity<Topology>(entity =>
            {
                entity.ToTable("topology", "topology");

                entity.HasIndex(e => e.Name)
                    .HasName("topology_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Hasz).HasColumnName("hasz");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Precision).HasColumnName("precision");

                entity.Property(e => e.Srid).HasColumnName("srid");
            });

            modelBuilder.Entity<VMainRecords>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("V_MAIN_RECORDS");

                entity.Property(e => e.Language)
                    .HasColumnName("language")
                    .HasMaxLength(2);

                entity.Property(e => e.Mainpicture).HasColumnName("mainpicture");

                entity.Property(e => e.Mainsound)
                    .HasColumnName("mainsound")
                    .HasMaxLength(250);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(60);

                entity.Property(e => e.Scientificname)
                    .HasColumnName("scientificname")
                    .HasMaxLength(60);

                entity.Property(e => e.Sumup).HasColumnName("sumup");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
