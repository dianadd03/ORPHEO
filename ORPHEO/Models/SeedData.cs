using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;

namespace Orpheo.Models
{
    //pasul 4 : useri si roluri
    public class SeedData
    {
        public static void Initialize(IServiceProvider
serviceProvider)
        {
            using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService
            <DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Roles.Any())
                {
                    return;
                }

                // CREAREA ROLURILOR IN BD
                // daca nu contine roluri, acestea se vor crea
                context.Roles.AddRange(

                new IdentityRole
                {
                    Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },


                new IdentityRole
                {
                    Id = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                    Name = "Artist",
                    NormalizedName = "Artist".ToUpper()
                },


                new IdentityRole
                {
                    Id = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                }


                );

                // o noua instanta pe care o vom utiliza pentru crearea parolelor utilizatorilor
                // parolele sunt de tip hash
                var hasher = new PasswordHasher<ApplicationUser>();

                // CREAREA USERILOR IN BD
                // Se creeaza cate un user pentru fiecare rol
                context.Users.AddRange(
                new ApplicationUser

                {

                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb0",
                    // primary key
                    UserName = "admin@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "ADMIN@TEST.COM",
                    Email = "admin@test.com",
                    NormalizedUserName = "ADMIN@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Admin1!"),
                    Name = "Admin",
                    UserCode = GenerateUserCode()
                },

                new ApplicationUser
                {

                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb1",
                    // primary key
                    UserName = "artist@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "ARTIST@TEST.COM",
                    Email = "artist@test.com",
                    NormalizedUserName = "ARTIST@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Artist1!"),
                    Name = "Artist",
                    UserCode = GenerateUserCode()
                },
                new ApplicationUser

                {

                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb2",
                    // primary key
                    UserName = "user@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "USER@TEST.COM",
                    Email = "user@test.com",
                    NormalizedUserName = "USER@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "User1!"),
                    Name = "User",
                    UserCode = GenerateUserCode()
                },
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                    UserName = "diana@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "DIANA@TEST.COM",
                    Email = "diana@test.com",
                    NormalizedUserName = "DIANA@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Diana1!"),
                    Name = "Diana",
                    UserCode = GenerateUserCode()
                },

                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb4",
                    UserName = "ada@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "ADA@TEST.COM",
                    Email = "ada@test.com",
                    NormalizedUserName = "ADA@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Adaa1!"),
                    Name = "Ada",
                    UserCode = GenerateUserCode()
                },
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb5",
                    UserName = "theweeknd@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "THEWEEKND@TEST.COM",
                    Email = "theweeknd@test.com",
                    NormalizedUserName = "THEWEEKND@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Artist1!"),
                    Name = "The Weeknd",
                    UserCode = GenerateUserCode()
                },

                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb6",
                    UserName = "bethhart@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "BETHHART@TEST.COM",
                    Email = "bethhart@test.com",
                    NormalizedUserName = "BETHHART@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Artist1!"),
                    Name = "Beth Hart",
                    UserCode = GenerateUserCode()
                },

                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb7",
                    UserName = "miley@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "MILEY@TEST.COM",
                    Email = "miley@test.com",
                    NormalizedUserName = "MILEY@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Artist1!"),
                    Name = "Miley Cyrus",
                    UserCode = GenerateUserCode()
                },

                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb8",
                    UserName = "yungblud@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "YUNGBLUD@TEST.COM",
                    Email = "yungblud@test.com",
                    NormalizedUserName = "YUNGBLUD@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Artist1!"),
                    Name = "YungBlud",
                    UserCode = GenerateUserCode()
                },

                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName = "metallica@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "METALLICA@TEST.COM",
                    Email = "metallica@test.com",
                    NormalizedUserName = "METALLICA@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Artist1!"),
                    Name = "Metallica",
                    UserCode = GenerateUserCode()
                }


                );

                // ASOCIEREA USER-ROLE
                context.UserRoles.AddRange(
                new IdentityUserRole<string>
                {

                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",


                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb0"
                },

                new IdentityUserRole<string>

                {

                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",


                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb1"
                },

                new IdentityUserRole<string>

                {

                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",


                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb2"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb3"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb4"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5"
                },

                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6"
                },

                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7"
                },

                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8"
                },

                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }

                );
                context.SaveChanges();
                if (!context.Songs.Any())
                {
                    context.Songs.AddRange(

                        new Song
                        {
                            Title = "Caught Out In The Rain",
                            Url = "/uploads/songs/Beth Hart - Caught Out In The Rain.mp3",
                            DataPublicarii = new DateTime(2012, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6"
                        },
                        new Song
                        {
                            Title = "Fire On The Floor",
                            Url = "/uploads/songs/Beth Hart - Fire On The Floor (Official Lyric Video).mp3",
                            DataPublicarii = new DateTime(2016, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6"
                        },
                        new Song
                        {
                            Title = "Your Heart Is As Black As Night",
                            Url = "/uploads/songs/Beth Hart & Joe Bonamassa - Your Heart Is As Black As Night.mp3",
                            DataPublicarii = new DateTime(2011, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6"
                        },
                        new Song
                        {
                            Title = "Am I The One",
                            Url = "/uploads/songs/Beth Hart Am I The One.mp3",
                            DataPublicarii = new DateTime(1999, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6"
                        },
                        new Song
                        {
                            Title = "I'd Rather Go Blind",
                            Url = "/uploads/songs/RockWiz - Beth Hart - I'd Rather Go Blind.mp3",
                            DataPublicarii = new DateTime(2014, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6"
                        },

                        new Song
                        {
                            Title = "The Best Of Both Worlds",
                            Url = "/uploads/songs/Hannah Montana - The Best Of Both Worlds.mp3",
                            DataPublicarii = new DateTime(2006, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7"
                        },
                        new Song
                        {
                            Title = "Nothing Breaks Like a Heart",
                            Url = "/uploads/songs/Mark Ronson - Nothing Breaks Like a Heart (Official Video) ft. Miley Cyrus.mp3",
                            DataPublicarii = new DateTime(2018, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7"
                        },
                        new Song
                        {
                            Title = "Midnight Sky",
                            Url = "/uploads/songs/Miley Cyrus - Midnight Sky (Official Video).mp3",
                            DataPublicarii = new DateTime(2020, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7"
                        },
                        new Song
                        {
                            Title = "Plastic Hearts",
                            Url = "/uploads/songs/Miley Cyrus - Plastic Hearts (Audio).mp3",
                            DataPublicarii = new DateTime(2020, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7"
                        },
                        new Song
                        {
                            Title = "Why'd You Only Call Me When You're High (Unplugged)",
                            Url = "/uploads/songs/Miley Cyrus Performs Why'd You Only Call Me When You're High - Miley Cyrus Unplugged.mp3",
                            DataPublicarii = new DateTime(2020, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7"
                        },

                        new Song
                        {
                            Title = "Master of Puppets",
                            Url = "/uploads/songs/Master of Puppets (Remastered).mp3",
                            DataPublicarii = new DateTime(1986, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                        },
                        new Song
                        {
                            Title = "Enter Sandman",
                            Url = "/uploads/songs/Metallica_ Enter Sandman (Official Music Video).mp3",
                            DataPublicarii = new DateTime(1991, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                        },
                        new Song
                        {
                            Title = "Nothing Else Matters",
                            Url = "/uploads/songs/Metallica_ Nothing Else Matters (Official Music Video).mp3",
                            DataPublicarii = new DateTime(1991, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                        },
                        new Song
                        {
                            Title = "The Unforgiven",
                            Url = "/uploads/songs/Metallica_ The Unforgiven (Official Music Video).mp3",
                            DataPublicarii = new DateTime(1991, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                        },
                        new Song
                        {
                            Title = "Whiskey in the Jar",
                            Url = "/uploads/songs/Metallica_ Whiskey in the Jar (Official Music Video).mp3",
                            DataPublicarii = new DateTime(1998, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                        },

                        new Song
                        {
                            Title = "Can't Feel My Face",
                            Url = "/uploads/songs/The Weeknd - Can't Feel My Face (Official Video).mp3",
                            DataPublicarii = new DateTime(2015, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5"
                        },
                        new Song
                        {
                            Title = "Die For You",
                            Url = "/uploads/songs/The Weeknd - Die For You.mp3",
                            DataPublicarii = new DateTime(2016, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5"
                        },
                        new Song
                        {
                            Title = "Save Your Tears",
                            Url = "/uploads/songs/The Weeknd - Save Your Tears (Official Music Video).mp3",
                            DataPublicarii = new DateTime(2020, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5"
                        },
                        new Song
                        {
                            Title = "Society",
                            Url = "/uploads/songs/The Weeknd - Society.mp3",
                            DataPublicarii = new DateTime(2011, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5"
                        },
                        new Song
                        {
                            Title = "The Hills",
                            Url = "/uploads/songs/The Weeknd - The Hills.mp3",
                            DataPublicarii = new DateTime(2015, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5"
                        },

                        new Song
                        {
                            Title = "Ghosts (Live)",
                            Url = "/uploads/songs/YUNGBLUD - Ghosts (Live From Hansa Studios).mp3",
                            DataPublicarii = new DateTime(2023, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8"
                        },
                        new Song
                        {
                            Title = "Hello Heaven, Hello",
                            Url = "/uploads/songs/YUNGBLUD - Hello Heaven, Hello.mp3",
                            DataPublicarii = new DateTime(2023, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8"
                        },
                        new Song
                        {
                            Title = "I Was Made For Lovin You",
                            Url = "/uploads/songs/YUNGBLUD - I Was Made For Lovin You (from The Fall Guy).mp3",
                            DataPublicarii = new DateTime(2024, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8"
                        },
                        new Song
                        {
                            Title = "Lowlife",
                            Url = "/uploads/songs/YUNGBLUD - Lowlife (Live From Marigny).mp3",
                            DataPublicarii = new DateTime(2023, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8"
                        },
                        new Song
                        {
                            Title = "Zombie",
                            Url = "/uploads/songs/YUNGBLUD - Zombie (Official Music Video) [Yv97b2oPk3w].mp3",
                            DataPublicarii = new DateTime(2021, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8"
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Playlists.Any())
                {
                    context.Playlists.AddRange(

                        // Diana

                        new Playlist
                        {
                            Name = "Rage & Riffs",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                            IsPublic = false,
                            ImagePath = "/uploads/playlists/1.webp"
                        },

                        new Playlist
                        {
                            Name = "Midnight Feelings",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                            IsPublic = true,
                            ImagePath = "/uploads/playlists/3.jpeg"
                        },

                        new Playlist
                        {
                            Name = "Soul & Blues",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                            IsPublic = false,
                            ImagePath = "/uploads/playlists/default.jpg"
                        },

                        // Ada

                        new Playlist
                        {
                            Name = "Raw Rock Energy",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb4",
                            IsPublic = true,
                            ImagePath = "/uploads/playlists/3.jpeg"
                        },

                        new Playlist
                        {
                            Name = "Obsession",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb4",
                            IsPublic = true,
                            ImagePath = "/uploads/playlists/2.jpg"
                        },

                        new Playlist
                        {
                            Name = "Late Night Mix",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb4",
                            IsPublic = false,
                            ImagePath = "/uploads/playlists/default.jpg"
                        }
                    );
                    context.SaveChanges();
                    if (!context.PlaylistSongs.Any())
                    {
                        context.PlaylistSongs.AddRange(

                            //Diana

                            new PlaylistSong { PlaylistId = 1, SongId = 21 }, 
                            new PlaylistSong { PlaylistId = 1, SongId = 22 }, 
                            new PlaylistSong { PlaylistId = 1, SongId = 25 }, 
                            new PlaylistSong { PlaylistId = 1, SongId = 11 }, 
                            new PlaylistSong { PlaylistId = 1, SongId = 12 }, 

                            new PlaylistSong { PlaylistId = 2, SongId = 8 },  
                            new PlaylistSong { PlaylistId = 2, SongId = 9 },  
                            new PlaylistSong { PlaylistId = 2, SongId = 7 },  
                            new PlaylistSong { PlaylistId = 2, SongId = 17 }, 
                            new PlaylistSong { PlaylistId = 2, SongId = 18 },

                            new PlaylistSong { PlaylistId = 3, SongId = 1 },
                            new PlaylistSong { PlaylistId = 3, SongId = 2 },
                            new PlaylistSong { PlaylistId = 3, SongId = 3 },
                            new PlaylistSong { PlaylistId = 3, SongId = 4 },
                            new PlaylistSong { PlaylistId = 3, SongId = 5 },

                            // Ada

                            new PlaylistSong { PlaylistId = 4, SongId = 23 },
                            new PlaylistSong { PlaylistId = 4, SongId = 24 },
                            new PlaylistSong { PlaylistId = 4, SongId = 21 },
                            new PlaylistSong { PlaylistId = 4, SongId = 14 },
                            new PlaylistSong { PlaylistId = 4, SongId = 15 },

                            new PlaylistSong { PlaylistId = 5, SongId = 16 },
                            new PlaylistSong { PlaylistId = 5, SongId = 17 },
                            new PlaylistSong { PlaylistId = 5, SongId = 18 },
                            new PlaylistSong { PlaylistId = 5, SongId = 19 },
                            new PlaylistSong { PlaylistId = 5, SongId = 20 },

                            new PlaylistSong { PlaylistId = 6, SongId = 8 },
                            new PlaylistSong { PlaylistId = 6, SongId = 21 },
                            new PlaylistSong { PlaylistId = 6, SongId = 1 },
                            new PlaylistSong { PlaylistId = 6, SongId = 16 },
                            new PlaylistSong { PlaylistId = 6, SongId = 12 }
                        );
                        context.SaveChanges();
                    }
                    if (!context.SessionRooms.Any())
                    {
                        context.SessionRooms.Add(
                            new SessionRoom
                            {
                                Name = "Rock Night Session",
                                HostUserId = "8e445865-a24d-4543-a6c6-9443d048cdb4", // Ada
                                PlaylistId = 4 // Raw Rock Energy
                            }
                        );
                        context.SaveChanges();
                    }
                    if (!context.SessionRoomUsers.Any())
                    {
                        context.SessionRoomUsers.AddRange(

                            // Ada (host)
                            new SessionRoomUser
                            {
                                SessionRoomId = 1,
                                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb4"
                            },

                            // Diana
                            new SessionRoomUser
                            {
                                SessionRoomId = 1,
                                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb3"
                            }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Tags.Any())
                    {
                        context.Tags.AddRange(
                            new Tag { Name = "happy" },
                            new Tag { Name = "sad" },
                            new Tag { Name = "angry" },
                            new Tag { Name = "love" },
                            new Tag { Name = "breakup" },
                            new Tag { Name = "melancholic" },
                            new Tag { Name = "energetic" },
                            new Tag { Name = "dark" },
                            new Tag { Name = "chill" },
                            new Tag { Name = "nostalgic" },
                            new Tag { Name = "hope" }
                        );

                        context.SaveChanges();
                    }

                }

            }
        }
        private static string GenerateUserCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}

