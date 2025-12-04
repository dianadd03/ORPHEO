using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orpheo.Models;

namespace Orpheo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SongTag> SongTags { get; set; }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }

        public DbSet<Comm> Comms { get; set; }

        public DbSet<SessionRoom> SessionRooms { get; set; }
        public DbSet<SessionRoomUser> SessionRoomUsers { get; set; }

        public DbSet<RoleRequest> RoleRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // M–M Song–Tag
            builder.Entity<SongTag>()
                .HasKey(st => new { st.SongId, st.TagId });

            builder.Entity<SongTag>()
                .HasOne(st => st.Song)
                .WithMany(s => s.SongTags)
                .HasForeignKey(st => st.SongId);

            builder.Entity<SongTag>()
                .HasOne(st => st.Tag)
                .WithMany(t => t.SongTags)
                .HasForeignKey(st => st.TagId);

            // M–M Playlist–Song
            builder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            builder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId)
                .OnDelete(DeleteBehavior.Restrict);


            // M–M User–SessionRoom
            builder.Entity<SessionRoomUser>()
                .HasKey(sru => new { sru.SessionRoomId, sru.UserId });

            builder.Entity<SessionRoomUser>()
                .HasOne(sru => sru.SessionRoom)
                .WithMany(sr => sr.SessionRoomUsers)
                .HasForeignKey(sru => sru.SessionRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SessionRoomUser>()
                .HasOne(sru => sru.User)
                .WithMany(u => u.SessionRoomUsers)
                .HasForeignKey(sru => sru.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1–M User–Song 
            builder.Entity<Song>()
                .HasOne(s => s.User)
                .WithMany(u => u.Songs)
                .HasForeignKey(s => s.UserId);

            // 1–M User–Playlist
            builder.Entity<Playlist>()
                .HasOne(p => p.User)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId);

            // 1–M User–Comm
            builder.Entity<Comm>()
                  .HasOne(c => c.User)
                  .WithMany(u => u.Comms)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict);


            // 1–M Song–Comm
            builder.Entity<Comm>()
                .HasOne(c => c.Song)
                .WithMany(s => s.Comms)
                .HasForeignKey(c => c.SongId)
                .OnDelete(DeleteBehavior.Restrict);


            // 1–M Playlist–SessionRoom
            builder.Entity<SessionRoom>()
                .HasOne(sr => sr.Playlist)
                .WithMany(p => p.SessionRooms)
                .HasForeignKey(sr => sr.PlaylistId);

            // 1–M HostUser – SessionRoom 
            builder.Entity<SessionRoom>()
                .HasOne(sr => sr.HostUser)
                .WithMany() 
                .HasForeignKey(sr => sr.HostUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
