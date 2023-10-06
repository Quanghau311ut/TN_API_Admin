using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class WebContext : DbContext
    {
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Introduce> Introduce { get; set; }
        public DbSet<Categories> Categories { get; set; }

        public DbSet<NewActicle> NewActicle { get; set; }

        public DbSet<MoreImage> MoreImage { get; set; }
        public DbSet<Hotline> Hotline { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Acount> Acount { get; set; }
        public object MoreImages { get; internal set; }

        public WebContext(DbContextOptions<WebContext> options) : base(options) { }

        public WebContext() : base() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


   

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("MENU", "dbo");
            });

            modelBuilder.Entity<Introduce>(entity =>
            {
                entity.ToTable("INTRODUCE", "dbo");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("CATEGORIES", "dbo");
            });

            modelBuilder.Entity<NewActicle>(entity =>
            {
                entity.ToTable("NEWS_ARTICLE", "dbo");
                entity.HasOne(x => x.Categories)
                .WithMany(x => x.NewActicles)
                .HasForeignKey(x => x.Categories_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NEWS_ARTI__CATEG__07C12930");
            });

            modelBuilder.Entity<MoreImage>(entity =>
            {
                entity.ToTable("MORE_IMAGE", "dbo");
                entity.HasOne(x => x.NewActicle)
                .WithMany(x => x.MoreImages)
                .HasForeignKey(x => x.New_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MORE_IMAG__NEW_I__0C85DE4D");
            });

            modelBuilder.Entity<Hotline>(entity =>
            {
                entity.ToTable("HOTLINE", "dbo");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("BRAND", "dbo");
            });

            modelBuilder.Entity<Slider>(entity =>
            {
                entity.ToTable("SLIDER", "dbo");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("COMMENT", "dbo");
            });

            modelBuilder.Entity<Acount>(entity =>
            {
                entity.ToTable("ACOUNT", "dbo");
            });
        }
    }
}
