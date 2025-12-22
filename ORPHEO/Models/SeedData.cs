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
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6",
                            Lyrics = "I'm under your spell\r\nAin't nobody's business\r\nI'm already there\r\nIt ain't nobody's business\r\nYeyeeee\r\nEvery time he walks out the door\r\nI wonder if he's ever coming back\r\nI can't help but love the taste of danger baby\r\nAnd the how and the when and the roughness baby\r\nI got caught out in the rain\r\nIf I die I don't care, I'm in love\r\nI'm in love, I'm in love with this man\r\nI got caught out, caught out\r\nCaught out in the rain\r\nI heard him crying in his sleep last night\r\nNo man wants to be told he been crying\r\nWhen he wakes up I tell him it's gonna be alright\r\nBut I know that he knows that I'm just lying\r\nI heard he shot a man down in the street\r\nAnd it torn his soul apart\r\nLast night when he was making love to me\r\nThere was a, another woman in his heart\r\nI got caught out in the rain\r\nIf I die I don't care, I'm in love\r\nI'm in love, I'm in love with this man\r\nI got caught out, caught out, caught out in the rain\r\nIn the rain\r\nOh oh oh, oh oh oh, oh oh oh, oh oh oh\r\nOh oh oh, oh, oh, oh, oh, oh, oh, oh\r\nI got caught out in the rain\r\nIf I die I don't care, I don't care\r\nI'm in love, I'm in love\r\nI got caught out, caught out, caught out in the rain\r\nIn the rain\r\nHis name\r\nHis pain\r\nAin't nobody's business\r\nAin't nobody, nobody, nobody business\r\nI won't kiss and tell\r\nI got a spell"
                        },
                        new Song
                        {
                            Title = "Fire On The Floor",
                            Url = "/uploads/songs/Beth Hart - Fire On The Floor (Official Lyric Video).mp3",
                            DataPublicarii = new DateTime(2016, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6",
                            Lyrics = "Love is a fever\r\nAnd is burning me alive\r\nIt can't be tamed or satisfied\r\nThere is no mercy\r\nFor the fallen or for the weak\r\nLove is a nasty word to speak\r\nI don't wanna love him anymore\r\nHe's nothing like the man I loved before\r\nBut the pain gets real comfortable\r\nWhen it's all you got\r\nAshes and smoke they can't compete\r\nNot even hell can take the heat\r\nAnd I be sliding off of my seat\r\nFor his flame\r\nHis love is like fire on the floor\r\nIt's got me running for the door\r\nBut I'll be crawling back for more\r\nOf his fire on the floor\r\nIt don't matter what you say\r\nYou can't survive it, there ain't no way\r\nSo tonight, I'm gonna stay\r\nAnd play with his fire on the floor\r\nWanna play with his fire\r\nOn the floor child, child\r\nThis kinda love\r\nDon't need no bed or satin sheets\r\nNothing soft, nothing soft or sweet to drink\r\nLove is a lesson, you were born to never learn\r\nAnd your soul will beg to burn\r\nI don't wanna love him anymore\r\nHe's nothing like the man I loved before\r\nAnd there's a sign above the door\r\nSaying no way out\r\nAshes and smoke they can't compete\r\nNot even hell can take the heat\r\nI be sliding off of my seat\r\nFor his flame\r\nHis love is like fire on the floor\r\nIt's got me running for the door\r\nBut I keep crawling back for more\r\nOf his fire on the floor\r\nIt don't matter what you say\r\nYou can't survive it, there ain't no way\r\nSo tonight, I'm gonna stay\r\nAnd play with his fire on the floor\r\nI'm gonna stay and play with his fire\r\nOn the floor\r\nI'm gonna play with his fire\r\nOn the floor"
                        },
                        new Song
                        {
                            Title = "Your Heart Is As Black As Night",
                            Url = "/uploads/songs/Beth Hart & Joe Bonamassa - Your Heart Is As Black As Night.mp3",
                            DataPublicarii = new DateTime(2011, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6",
                            Lyrics = "our eyes may be whole but the story I'm told is\r\nYour heart is as black as night\r\nYour lips may be sweet such that I can't compete\r\nBut your heart is as black as night\r\n\r\nI don't know why you came along\r\nAt such a perfect time\r\nBut if I let you hang around\r\nI'm bound to lose my mind\r\n\r\nCause your hands may be strong\r\nBut the feeling's all wrong\r\nYour heart is as black as night\r\n\r\nI don't know why you came along\r\nAt such a perfect time\r\nBut if I let you hang around\r\nI'm bound to lose my mind\r\n\r\nCause your hands may be strong\r\nBut the feeling's all wrong\r\nYour heart is as black as night\r\n\r\nYour eyes may be whole but the story I'm told is\r\nYour heart is as black as night\r\nYour lips may be sweet such that I can't compete\r\nBut your heart is as black as night\r\nSee Beth Hart Live\r\nGet tickets as low as $89\r\nYou might also like\r\nWind of Change\r\nScorpions\r\nSpace Oddity\r\nDavid Bowie\r\nIce + Alabaster\r\n9mice & Kai Angel\r\nI don't know why you came along\r\nAt such a perfect time\r\nBut if I let you hang around\r\nI'm bound to lose my mind\r\nCause your hands may be strong\r\nBut the feeling's all wrong\r\nYour heart is as black, your heart is as black\r\nOh, your heart is as black as night, as night, as night"
                        },
                        new Song
                        {
                            Title = "Am I The One",
                            Url = "/uploads/songs/Beth Hart Am I The One.mp3",
                            DataPublicarii = new DateTime(1999, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6",
                            Lyrics = "I sing these words, time and time again\r\nTo express my life, of being your lover, and your friend\r\nAnd as the clouds, cry cry cry cry, high above, she'd their tears\r\nI'll embrace you, with love, from all your fear\r\nAm I the one, am i the one that you love\r\nAm I the one, hheeyy,\r\nAm I the one that you think of\r\nAm, am I, am I the one.\r\nOh yeh yeh yeh\r\nA passion in your cares, floats from your fingertips\r\nAnd i pray for the day, that i hear those precious words pass through your lips\r\nWishinh upon the start, from up above.uhh\r\nThat soon you'll love at me baby, and say I'm the one that you love\r\nAm I the one, yeh,\r\nAm I the one that you love\r\nAm I the one, hey hey\r\nAm I the one that you think of\r\nDon't you make me feel crazy, if i break down and cry\r\nJust tell me that you love me baby\r\nEven if it is a lie\r\nAm, am I\r\nAm, am I\r\nAm, am I\r\nAm I the one."
                        },
                        new Song
                        {
                            Title = "I'd Rather Go Blind",
                            Url = "/uploads/songs/RockWiz - Beth Hart - I'd Rather Go Blind.mp3",
                            DataPublicarii = new DateTime(2014, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb6",
                            Lyrics = "[Verse 1]\r\nSomething told me that it was over, baby, yeah\r\nWhen I saw you\r\nWhen I saw you and that girl\r\nAnd y'all was talking\r\n\r\n[Verse 2]\r\nSomething deep down\r\nSomething deep down in my soul said\r\n\"Go on, go on and cry, girl\"\r\nWhen I saw you, when I saw you with that same person\r\nAnd y'all was walking around\r\n\r\n[Chorus]\r\nAnd I'd rather\r\nI'd rather be a blind girl, baby, yeah, yeah\r\nThan to see you walk away, walk away from me, baby\r\nDon't leave me, I don't wanna see you go\r\n\r\n[Verse 3]\r\n'Cause you see, I love you so much\r\nAnd I don't wanna watch you leave me\r\nDon't wanna watch you leave me, baby\r\nAnd another thing is, one more thing is\r\nI just don't, I just don't wanna be free\r\nScared to be by myself\r\nSee Beth Hart Live\r\nGet tickets as low as $89\r\nYou might also like\r\nComfortably Numb\r\nPink Floyd\r\nI’ll Take Care of You\r\nBeth Hart & Joe Bonamassa\r\nBohemian Rhapsody\r\nQueen\r\n[Verse 4]\r\nI was just, I was just sitting here thinking\r\nAbout your sweet kiss and your, your warm embrace\r\nHmm, your warm embrace\r\nHmm, yo, yo, warm, warm embrace\r\n\r\n[Bridge]\r\nWhen I look down in the glass that I held to my lips\r\nAnd I saw the reflection of the tears rolling down my face\r\nThat's when I knew I love you and I couldn't do without you\r\nAnd I'd rather be a blind girl\r\nBaby, baby, baby, baby, baby\r\nBaby, baby, babe, no, babe, oh, oh\r\nOh babe\r\n\r\n[Interlude]\r\nAll day sitting up\r\nSitting up thinking about you, mmhm\r\n\r\n[Guitar Solo]\r\n\r\n[Break]\r\nMyself, I don't wanna see you leave\r\nPlease don't go\r\nOh, babe, no, babe, oh, babe\r\nI'd rather be a blind girl\r\n[Verse 1]\r\nSomething told me that it was over, baby\r\nWhen I saw you\r\nWhen I saw you and that girl\r\nAnd y'all was talking\r\n\r\n[Verse 2]\r\nSomething deep down in my soul said\r\n\"Go on, go on and cry, girl\"\r\nWhen I saw you, when I saw you and that girl\r\nAnd y'all was walking by\r\n\r\n[Chorus]\r\nAnd I'd rather go blind, I'd rather go blind\r\nI'd rather go blind, babe\r\nThan to see you walk away, walk away\r\nDon't walk away 'cause I'd rather go blind"
                        },

                        new Song
                        {
                            Title = "The Best Of Both Worlds",
                            Url = "/uploads/songs/Hannah Montana - The Best Of Both Worlds.mp3",
                            DataPublicarii = new DateTime(2006, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7",
                            Lyrics = "Oh, yeah\r\nCome on\r\nYou get the limo out front (oh-whoa)\r\nHottest styles, every shoe, every color\r\nYeah, when you're famous it can be kinda fun\r\nIt's really you, but no one ever discovers\r\nIn some ways, you're just like all your friends\r\nBut on stage, you're a star\r\nYou get the best of both worlds\r\nChill it out, take it slow\r\nThen you rock out the show\r\nYou get the best of both worlds\r\nMix it all together\r\nAnd you know that it's the best of both worlds\r\nThe best of both worlds, yeah\r\nYou go to movie premiers (is that Orlando Bloom?)\r\nHear your songs on the radio (oh-whoa)\r\nLivin' two lives is a little weird, yeah\r\nBut school's cool 'cause nobody knows\r\nYeah, you get to be a small town girl\r\nBut big time when you play your guitar\r\nYou get the best of both worlds\r\nChill it out, take it slow\r\nThen you rock out the show\r\nYou get the best of both worlds\r\nMix it all together\r\nAnd you know that it's the best of both\r\nYou know the best of both worlds\r\nPictures and autographs\r\nYou get your face in all the magazines\r\nThe best part is that\r\nYou get to be whoever you wanna be\r\nYeah, the the best of both\r\n(Best, best) You got the best of both\r\n(Best, best) Come on, the best of both\r\nWho would've thought that a girl like me\r\nWould double as a super star?\r\nYou get the best of both worlds\r\nChill it out, take it slow\r\nThen you rock out the show\r\nYou get the best of both worlds\r\nMix it all together\r\nAnd you know that it's the best\r\nYou get the best of both worlds\r\nWithout the shades and the hair\r\nYou can go anywhere\r\nYou get the best of both girls\r\nMix it all together, oh yeah\r\nIt's so much better\r\n'Cause you know you got\r\nThe best of both worlds"
                        },
                        new Song
                        {
                            Title = "Nothing Breaks Like a Heart",
                            Url = "/uploads/songs/Mark Ronson - Nothing Breaks Like a Heart (Official Video) ft. Miley Cyrus.mp3",
                            DataPublicarii = new DateTime(2018, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7",
                            Lyrics = "This world can't hurt you\r\nCuts you deep and leaves a scar\r\nThings fall apart, nothing breaks like a heart\r\nThis song is for all my friends in Ukraine\r\nI heard you on the phone last night\r\nWe live and die by pretty lies, you know it\r\nWe both know it\r\nThis silver bullet cigarettes\r\nThis burning house, there's nothing left\r\nAnd it's smoking, we both know it\r\nWe got all night to fall in love\r\nJust like that we'd fall apart\r\nWe're broken, we're broken\r\nOoh, and nothing, nothing, nothing gon' save us now\r\nThere's broken silence\r\nBy thunder crashing in the dark (crash in the dark)\r\nThis broken record\r\nSpin endless circles in the bar (spin 'round in the bar)\r\nThis world can't hurt you\r\nIt cuts you deep and leaves a scar\r\nThings fall apart, but nothing breaks like a heart\r\nNothing breaks like a heart\r\nWe'll leave each other cold as ice and high and dry\r\nThe desert wind is blowing\r\nYou tryin' to get me wet? (Is blowing)\r\nRemember what you said to me\r\nDrunk in love in Tennessee\r\nI hold it, we both know it\r\nOoh, and nothing, nothing, nothing gon' save us now\r\nOoh, yeah, nothing, nothing, nothing gon' save us now\r\nThere's broken silence\r\nBy thunder crashing in the dark (crash in the dark)\r\nThis broken record\r\nSpin endless circles in the bar (spin 'round in the bar)\r\nThis world can't hurt you\r\nIt cuts you deep and leaves a scar\r\nThings fall a apart, but nothing breaks like a heart\r\nNothing breaks like a heart\r\nNothing breaks like a heart\r\nNothing breaks like a heart\r\nOoh, yeah, nothing, nothing, nothing gon' save us now\r\nOoh, yeah, nothing, nothing, nothing gon' save us now\r\nThere's broken silence\r\nBy thunder crashing in the dark (crash in the dark)\r\nThis broken record\r\nSpin endless circles in the bar (spin 'round in the bar)\r\nThis world can't hurt you\r\nIt cuts you deep and leaves a scar\r\nThings fall apart, but nothing breaks like a heart\r\nNothing breaks like a heart\r\nNothing breaks like a heart\r\nNothing breaks like a heart"
                        },
                        new Song
                        {
                            Title = "Midnight Sky",
                            Url = "/uploads/songs/Miley Cyrus - Midnight Sky (Official Video).mp3",
                            DataPublicarii = new DateTime(2020, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7",
                            Lyrics = "La-la, la-la, la\r\nYeah, it's been a long night and the mirror's tellin' me to go home (home)\r\nBut it's been a long time since I felt this good on my own\r\nUh, lotta years went by with my hands tied up in your ropes\r\nForever and ever, no more\r\nThe midnight sky is the road I'm takin'\r\nHead high up in the clouds\r\nOh\r\nI was born to run, I don't belong to anyone, oh no\r\nI don't need to be loved by you (by you)\r\nFire in my lungs, can't bite the devil on my tongue, oh no\r\nI don't need to be loved by you\r\nSee my lips on her mouth, everybody's talking now, baby\r\nOoh, you know it's true, yeah\r\nThat I was born to run, I don't belong to anyone, oh no\r\nI don't need to be loved by you (loved by you)\r\nLa-la, la-la, la\r\nShe got her hair pulled back 'cause the sweat's drippin' off of her face (her face)\r\nSaid it ain't so bad if I wanna make a couple mistakes\r\nYou should know right now that I never stay put in one place\r\nForever and ever, no more (no more)\r\nThe midnight sky is the road I'm takin'\r\nHead high up in the clouds\r\nOh\r\nI was born to run, I don't belong to anyone, oh no\r\nI don't need to be loved by you (by you)\r\nFire in my lungs, can't bite the devil on my tongue, oh no\r\nI don't need to be loved by you\r\nSee my lips on her mouth, everybody's talking now, baby\r\nOoh, you know it's true, yeah\r\nThat I was born to run, I don't belong to anyone, oh no\r\nI don't need to be loved by you (by you)\r\nOh\r\nI don't hide blurry eyes like you\r\nLike you\r\nI was born to run, I don't belong to anyone, oh no\r\nI don't need to be loved by you (by you)\r\nFire in my lungs, can't bite the devil on my tongue, you know\r\nI don't need to be loved by you\r\nSee his hands on my waist, thought you'd never be replaced, baby\r\nOoh, you know it's true, yeah\r\nThat I was born to run, I don't belong to anyone, oh no\r\nI don't need to be loved by you, yeah\r\nLa-la, la-la, la\r\nLa-la\r\nYou know it's true\r\nYou know it's true\r\n(Loved by you)"
                        },
                        new Song
                        {
                            Title = "Plastic Hearts",
                            Url = "/uploads/songs/Miley Cyrus - Plastic Hearts (Audio).mp3",
                            DataPublicarii = new DateTime(2020, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7",
                            Lyrics = "Hello\r\nThe sunny place for shady people\r\nA crowded room where nobody goes\r\nYou can be whoever you wanna be here\r\nAnd oh\r\nI've been livin' at the Château\r\nShouldn't drive, but I should really go home\r\nI don't even know 'em, but they won't leave here\r\nFrightened by my own reflection\r\nDesperate for a new connection\r\nPull you in, but don't you get too close\r\nLove you now, but not tomorrow\r\nWrong to steal, but not to borrow\r\nPull you in, but don't you get too close\r\nI've been California dreamin'\r\nPlastic hearts are bleedin'\r\nKeep me up all night (keep me up)\r\nKeep me up all night (all night)\r\nLost in black hole conversation\r\nSunrise suffocation\r\nKeep me up all night (keep me up)\r\nKeep me up all night\r\nI just wanna feel (feel)\r\nI just wanna feel somethin' (feel somethin' now)\r\nBut I keep feeling nothin' all night long\r\nAll night long\r\nAll night long\r\nAll night long\r\nHello (hello)\r\nI'll tell you all the people I know (I know)\r\nSell you somethin' that you already own (you own)\r\nI can be whoever you want me to be\r\nLove me now, but not tomorrow\r\nFill me up, but leave me hollow\r\nPull me in, but don't you get too close, oh\r\nI've been California dreamin'\r\nPlastic hearts are bleedin'\r\nKeep me up all night (keep me up)\r\nKeep me up all night (all night)\r\nLost in black hole conversations\r\nSunrise suffocation\r\nKeep me up all night (keep me up)\r\nKeep me up all night\r\nI just wanna feel (feel)\r\nI just wanna feel somethin' (feel somethin' now)\r\nBut I keep feelin' nothin' all night long\r\nAll night long\r\nAll night long\r\nAll night long\r\nAll night long\r\nI've been California dreamin' (dreamin')\r\nPlastic hearts are bleedin' (are bleedin')\r\nKeep me up all night (keep me up)\r\nKeep me up all night (all night)\r\nLost in black hole conversations\r\nSunrise suffocation\r\nKeep me up all night, oh yeah (keep me up)\r\nKeep me up all night\r\nI just wanna feel (feel)\r\nI just wanna feel somethin' (feel somethin' now)\r\nBut I keep feelin' nothin' all night long"
                        },
                        new Song
                        {
                            Title = "Why'd You Only Call Me When You're High (Unplugged)",
                            Url = "/uploads/songs/Miley Cyrus Performs Why'd You Only Call Me When You're High - Miley Cyrus Unplugged.mp3",
                            DataPublicarii = new DateTime(2020, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb7",
                            Lyrics = "[Verse 1]\r\nThe mirror's image tells me it's home time\r\nBut I'm not finished, 'cause you're not by my side\r\nAnd as I arrived I thought I saw you leavin'\r\nCarryin' your shoes\r\nDecided that once again I was just dreamin'\r\nOf bumpin' into you\r\n\r\n[Chorus]\r\nNow, it's three in the mornin'\r\nAnd I'm tryna' change your mind\r\nLeft you multiple missed calls\r\nAnd to my message, you reply\r\n\"Why'd you only call me when you're high?\"\r\n\"Hi, why'd you only call me when you're high?\"\r\n\r\n[Verse 2]\r\nSomewhere darker, talkin' the same shite\r\nI need a partner, (High) well are you out tonight?\r\nIt's harder and harder to get you to listen\r\nMore I get through the gears\r\nIncapable of makin' alright decisions\r\nAnd having bad ideas\r\n\r\n[Chorus]\r\nNow, it's three in the mornin'\r\nAnd I'm tryna' change your mind\r\nLeft you multiple missed calls\r\nAnd to my message, you reply (Message, you reply)\r\n\"Why'd you only call me when you're high?\"\r\n(Why'd you only call me when you're)\r\n\"Hi, why'd you only call me when you're high?\"\r\nSee upcoming rock shows\r\nGet tickets for your favorite artists\r\nYou might also like\r\nDo I Wanna Know?\r\nArctic Monkeys\r\nShe Knows\r\nJ. Cole\r\nDarling, I\r\nTyler, The Creator\r\n[Bridge]\r\nAnd I can't see you here, wonder where I might\r\nIt sort of feels like I'm runnin' out of time\r\nI haven't found all I was hopin' to find\r\nYou said you got to be up in the mornin'\r\nGonna have an early night\r\nAnd you're startin' to bore me, baby\r\n\"Why'd you only call me when you're high?\"\r\n\r\n[Outro]\r\n\"Why'd you only ever phone me when you're high?\"\r\n\"Why'd you only ever phone me when you're high?\"\r\n\"Why'd you only ever phone me when you're high?\"\r\n\"Why'd you only ever phone me when you're (high)?\""
                        },

                        new Song
                        {
                            Title = "Master of Puppets",
                            Url = "/uploads/songs/Master of Puppets (Remastered).mp3",
                            DataPublicarii = new DateTime(1986, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                            Lyrics = "End of passion play, crumbling away\r\nI'm your source of self-destruction\r\nVeins that pump with fear, sucking darkest clear\r\nLeading on your death's construction\r\nTaste me, you will see\r\nMore is all you need\r\nDedicated to\r\nHow I'm killing you\r\nCome crawling faster (faster)\r\nObey your master (master)\r\nYour life burns faster (faster)\r\nObey your master, master\r\nMaster of puppets, I'm pulling your strings\r\nTwisting your mind and smashing your dreams\r\nBlinded by me, you can't see a thing\r\nJust call my name 'cause I'll hear you scream\r\nMaster, master\r\nJust call my name 'cause I'll hear you scream\r\nMaster, master\r\nNeedlework the way, never you betray\r\nLife of death becoming clearer\r\nPain monopoly, ritual misery\r\nChop your breakfast on a mirror\r\nTaste me, you will see\r\nMore is all you need\r\nDedicated to\r\nHow I'm killing you\r\nCome crawling faster (faster)\r\nObey your master (master)\r\nYour life burns faster (faster)\r\nObey your master, master\r\nMaster of puppets, I'm pulling your strings\r\nTwisting your mind and smashing your dreams\r\nBlinded by me, you can't see a thing\r\nJust call my name 'cause I'll hear you scream\r\nMaster, master\r\nJust call my name 'cause I'll hear you scream\r\nMaster, master\r\n(Master, master, master, master)\r\nMaster, master, where's the dreams that I've been after?\r\nMaster, master, you promised only lies\r\nLaughter, laughter, all I hear or see is laughter\r\nLaughter, laughter, laughing at my cries\r\nFix me!\r\nHell is worth all that, natural habitat\r\nJust a rhyme without a reason\r\nNever-ending maze, drift on numbered days\r\nNow your life is out of season\r\nI will occupy\r\nI will help you die\r\nI will run through you\r\nNow I rule you too\r\nCome crawling faster (faster)\r\nObey your master (master)\r\nYour life burns faster (faster)\r\nObey your master, master\r\nMaster of puppets, I'm pulling your strings\r\nTwisting your mind and smashing your dreams\r\nBlinded by me, you can't see a thing\r\nJust call my name 'cause I'll hear you scream\r\nMaster, master\r\nJust call my name 'cause I'll hear you scream\r\nMaster, master"
                        },
                        new Song
                        {
                            Title = "Enter Sandman",
                            Url = "/uploads/songs/Metallica_ Enter Sandman (Official Music Video).mp3",
                            DataPublicarii = new DateTime(1991, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                            Lyrics = "Say your prayers, little one\r\nDon't forget, my son\r\nTo include everyone\r\nI tuck you in, warm within\r\nKeep you free from sin\r\n'Til the sandman, he comes\r\nSleep with one eye open\r\nGripping your pillow tight\r\nExit light\r\nEnter night\r\nTake my hand\r\nWe're off to never-never land\r\nSomething's wrong, shut the light\r\nHeavy thoughts tonight\r\nAnd they aren't of Snow White\r\nDreams of war, dreams of liars\r\nDreams of dragon's fire\r\nAnd of things that will bite, yeah\r\nSleep with one eye open\r\nGripping your pillow tight\r\nExit light\r\nEnter night\r\nTake my hand\r\nWe're off to never-never land, yeah\r\nNow I lay me down to sleep (now I lay me down to sleep)\r\nPray the Lord my soul to keep (pray the Lord my soul to keep)\r\nIf I die before I wake (if I die before I wake)\r\nPray the Lord my soul to take (pray the Lord my soul to take)\r\nHush, little baby, don't say a word\r\nAnd never mind that noise you heard\r\nIt's just the beasts under your bed\r\nIn your closet, in your head\r\nExit light\r\nEnter night\r\nGrain of sand\r\nExit light\r\nEnter night\r\nTake my hand\r\nWe're off to never-never land, yeah\r\nBoo!\r\nYeah-yeah\r\nYo, whoa\r\nWe're off to never-never land\r\nTake my hand\r\nWe're off to never-never land\r\nTake my hand\r\nWe're off to never-never land\r\nWe're off to never-never land\r\nWe're off to never-never land\r\nWe're off to never-never land"
                        },
                        new Song
                        {
                            Title = "Nothing Else Matters",
                            Url = "/uploads/songs/Metallica_ Nothing Else Matters (Official Music Video).mp3",
                            DataPublicarii = new DateTime(1991, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                            Lyrics = "So close, no matter how far\r\nCouldn't be much more from the heart\r\nForever trusting who we are\r\nAnd nothing else matters\r\nNever opened myself this way\r\nLife is ours, we live it our way\r\nAll these words, I don't just say\r\nAnd nothing else matters\r\nTrust I seek and I find in you\r\nEvery day for us something new\r\nOpen mind for a different view\r\nAnd nothing else matters\r\nNever cared for what they do\r\nNever cared for what they know\r\nBut I know\r\nSo close, no matter how far\r\nIt couldn't be much more from the heart\r\nForever trusting who we are\r\nAnd nothing else matters\r\nNever cared for what they do\r\nNever cared for what they know\r\nBut I know\r\nI never opened myself this way\r\nLife is ours, we live it our way\r\nAll these words, I don't just say\r\nAnd nothing else matters\r\nTrust I seek and I find in you\r\nEvery day for us something new\r\nOpen mind for a different view\r\nAnd nothing else matters\r\nNever cared for what they say\r\nNever cared for games they play\r\nNever cared for what they do\r\nNever cared for what they know\r\nAnd I know, yeah, yeah\r\nSo close, no matter how far\r\nCouldn't be much more from the heart\r\nForever trusting who we are\r\nNo, nothing else matters"
                        },
                        new Song
                        {
                            Title = "The Unforgiven",
                            Url = "/uploads/songs/Metallica_ The Unforgiven (Official Music Video).mp3",
                            DataPublicarii = new DateTime(1991, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                            Lyrics = "New blood joins this Earth\r\nAnd quickly he's subdued\r\nThrough constant pained disgrace\r\nThe young boy learns their rules\r\nWith time, the child draws in\r\nThis whipping boy done wrong\r\nDeprived of all his thoughts\r\nThe young man struggles on and on, he's known\r\nOoh, a vow unto his own\r\nThat never from this day\r\nHis will they'll take away\r\nWhat I've felt, what I've known\r\nNever shined through in what I've shown\r\nNever be, never see\r\nWon't see what might have been\r\nWhat I've felt, what I've known\r\nNever shined through in what I've shown\r\nNever free, never me\r\nSo I dub thee unforgiven\r\nThey dedicate their lives\r\nTo running all of his\r\nHe tries to please them all\r\nThis bitter man he is\r\nThroughout his life, the same\r\nHe's battled constantly\r\nThis fight he cannot win\r\nA tired man they see no longer cares\r\nThe old man then prepares\r\nTo die regretfully\r\nThat old man here is me\r\nWhat I've felt, what I've known\r\nNever shined through in what I've shown\r\nNever be, never see\r\nWon't see what might have been\r\nWhat I've felt, what I've known\r\nNever shined through in what I've shown\r\nNever free, never me\r\nSo I dub thee unforgiven\r\nWhat I've felt, what I've known\r\nNever shined through in what I've shown\r\nNever be, never see\r\nWon't see what might have been\r\nWhat I've felt, what I've known\r\nNever shined through in what I've shown\r\nNever free, never me\r\nSo I dub thee unforgiven\r\nOh-ooh-oh\r\nNever free, never me\r\nSo I dub thee unforgiven\r\nYou labeled me, I'll label you\r\nSo I dub thee unforgiven\r\nNever free, never me\r\nSo I dub thee unforgiven\r\nYou labeled me, I'll label you\r\nSo I dub thee unforgiven\r\nNever free, never me\r\nSo I dub thee unforgiven"
                        },
                        new Song
                        {
                            Title = "Whiskey in the Jar",
                            Url = "/uploads/songs/Metallica_ Whiskey in the Jar (Official Music Video).mp3",
                            DataPublicarii = new DateTime(1998, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                            Lyrics = "As I was goin' over\r\nThe Cork and Kerry Mountains\r\nI saw Captain Farrell\r\nAnd his money, he was countin'\r\nI first produced my pistol\r\nI then produced my rapier\r\nI said, \"Stand and deliver oh, or the devil he may take ya\"\r\nYeah\r\nI took all of his money\r\nAnd it was a pretty penny\r\nI took all of his money\r\nYeah, and I brought it home to Molly\r\nShe swore that she loved me\r\nNo, never would she leave me\r\nBut the devil take that woman\r\nYeah, for you know she tricked me easy\r\nMusha rain dum a doo, dum a da\r\nWhack for my daddy, oh\r\nWhack for my daddy, oh\r\nThere's whiskey in the jar, oh\r\nBeing drunk and weary\r\nI went to Molly's chamber\r\nTakin' Molly with me\r\nBut I never knew the danger\r\nFor about six or maybe seven\r\nYeah, in walked Captain Farrell\r\nI jumped up, fired my pistols\r\nAnd I shot him with both barrels\r\nYeah, musha rain dum a doo, dum a da, ha, yeah\r\nWhack for my daddy, oh\r\nWhack for my daddy, oh\r\nThere's whiskey in the jar, oh\r\nYeah, whiskey, yo, whiskey\r\nOh, yeah, yeah, oh, go\r\nOh, oh, yeah\r\nNow some men like a fishin'\r\nAnd some men like the fowlin'\r\nAnd some men like to hear\r\nTo hear the cannonball roarin'\r\nMe, I like sleepin'\r\n'Specially in my Molly's chamber\r\nBut here I am in prison\r\nHere I am with a ball and chain, yeah\r\nMusha rain dum a doo, dum a da, heh, heh\r\nWhack for my daddy, oh\r\nWhack for my daddy, oh\r\nThere's whiskey in the jar, oh, yeah\r\nWhiskey in the jar, oh, yeah\r\nMusha rain dum a doo, dum a da\r\nMusha rain dum a doo, dum a da, hey\r\nMusha rain dum a doo, dum a da\r\nMusha rain dum a doo, dum a da, yeah"
                        },

                        new Song
                        {
                            Title = "Can't Feel My Face",
                            Url = "/uploads/songs/The Weeknd - Can't Feel My Face (Official Video).mp3",
                            DataPublicarii = new DateTime(2015, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5", 
                            Lyrics = "[Verse 1]\r\nAnd I know she'll be the death of me\r\nAt least we'll both be numb\r\nAnd she'll always get the best of me\r\nThe worst is yet to come\r\nBut at least we'll both be beautiful\r\nAnd stay forever young\r\nThis I know, uh\r\nThis I know\r\n\r\n[Pre-Chorus]\r\nShe told me, \"Don't worry\r\nAbout it\"\r\nShe told me, \"Don't worry\r\nNo more\"\r\nWe both know we can't go\r\nWithout it\r\nShe told me, \"You'll never\r\nBe alo-oh-oh-ooh\"\r\n\r\n[Chorus]\r\nI can't feel my face when I'm with you\r\nBut I love it\r\nBut I love it, oh\r\nI can't feel my face when I'm with you\r\nBut I love it\r\nBut I love it, oh\r\nSee The Weeknd Live\r\nGet tickets as low as $39\r\nYou might also like\r\nEnjoy The Show\r\nThe Weeknd & Future\r\nGiven Up On Me\r\nThe Weeknd\r\nTOPIA TWINS\r\nTravis Scott\r\n[Verse 2]\r\nAnd I know she'll be the death of me\r\nAt least we'll both be numb\r\nAnd she'll always get the best of me\r\nThe worst is yet to come\r\nAll the misery was necessary\r\nWhen we're deep in love\r\nThis I know (Girl)\r\nGirl, I know, uh\r\n\r\n[Pre-Chorus]\r\nShe told me, \"Don't worry\r\nAbout it\"\r\nShe told me, \"Don't worry\r\nNo more\"\r\nWe both know we can't go\r\nWithout it\r\nShe told me, \"You'll never\r\nBe alo-oh-oh-ooh\"\r\n\r\n[Chorus]\r\nI can't feel my face when I'm with you\r\nBut I love it\r\nBut I love it, oh\r\nI can't feel my face when I'm with\r\nYou (I can't feel my face)\r\nBut I love it (But I love it)\r\nBut I love it (I can't feel my face), oh\r\nI can't feel my face when I'm with you (When I'm with you)\r\nBut I love it (But I love it, yeah)\r\nBut I love it (But I love it), oh\r\n(I can't feel my face) I can't feel my face when I'm with (When I'm with you) you\r\nBut I love it (But I love it)\r\nBut I love it (Girl, I love it), oh\r\n[Bridge]\r\nOh\r\nOh-oh\r\nOoh-ooh\r\n\r\n[Pre-Chorus]\r\nShe told me, \"Don't worry\r\nAbout it\"\r\nShe told me, \"Don't worry\r\nNo more\"\r\nWe both know we can't go (Can't go)\r\nWithout it\r\nShe told me, \"You'll never\r\nBe alo-oh-oh—\r\nOoh\"\r\n\r\n[Chorus]\r\nI can't feel my face when I'm with you (I can't feel my face, girl)\r\nBut I love it (But I love it, yeah)\r\nBut I love it (Oh, I love it, yeah) oh\r\nI can't feel my face when I'm with\r\nYou (Said, I can't feel my face)\r\nBut I love it (But I love it)\r\nBut I love it (Girl, I love it), oh\r\n(I can't feel my face) I can't feel my face when I'm with\r\n(When I'm with you) you\r\nBut I love it (Know I love it, girl)\r\nBut I love it (Don't you think I do?), oh\r\n(I can't feel my face) I can't feel my face when I'm with\r\n(When I'm with you) you (I can't feel my face, girl)\r\nBut I love it (When I'm with you, baby)\r\nBut I love it (Said, when I'm with you, baby), oh\r\nI can't feel my fa—"
                        },
                        new Song
                        {
                            Title = "Die For You",
                            Url = "/uploads/songs/The Weeknd - Die For You.mp3",
                            DataPublicarii = new DateTime(2016, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5",
                            Lyrics = "I'm findin' ways to articulate the feeling I'm goin' through\r\nI just can't say I don't love you (yeah)\r\n'Cause I love you, yeah\r\nIt's hard for me to communicate the thoughts that I hold\r\nBut tonight, I'm gon' let you know\r\nLet me tell the truth\r\nBaby, let me tell the truth, yeah\r\nYou know what I'm thinkin', see it in your eyes\r\nYou hate that you want me, hate it when you cry\r\nYou're scared to be lonely, especially in the night\r\nI'm scared that I'll miss you, happens every time\r\nI don't want this feelin', I can't afford love\r\nI try to find a reason to pull us apart\r\nIt ain't workin' 'cause you're perfect\r\nAnd I know that you're worth it\r\nI can't walk away, (oh)\r\nEven though we're going through it\r\nAnd it makes you feel alone\r\nJust know that I would die for you\r\nBaby, I would die for you, yeah\r\nThe distance and the time between us\r\nIt'll never change my mind\r\n'Cause baby, I would die for you\r\nBaby, I would die for you, yeah\r\nI'm finding ways to manipulate the feelin' you're going through\r\nBut baby-girl, I'm not blaming you\r\nJust don't blame me too, yeah\r\n'Cause I can't take this pain forever\r\nAnd you won't find no one that's better\r\n'Cause I'm right for you, babe\r\nI think I'm right for you, babe\r\nYou know what I'm thinking, see it in your eyes\r\nYou hate that you want me, hate it when you cry\r\nIt ain't workin' 'cause you're perfect\r\nAnd I know that you're worth it\r\nI can't walk away\r\nEven though we're going through it\r\nAnd it makes you feel alone\r\nJust know that I would die for you\r\nBaby, I would die for you, yeah\r\nThe distance and the time between us\r\nIt'll never change my mind\r\n'Cause baby, I would die for you\r\nBaby, I would die for you, yeah\r\nI would die for you, I would lie for you\r\nKeep it real with you, I would kill for you, my baby\r\nI'm just sayin', yeah\r\nI would die for you, I would lie for you\r\nKeep it real with you, I would kill for you, my baby\r\nNa, na, na, na, na, na, na, na\r\nEven though we're going through it\r\nAnd it makes you feel alone\r\nJust know that I would die for you\r\nBaby, I would die for you, yeah\r\nThe distance and the time between us\r\nIt'll never change my mind\r\n'Cause baby, I would die for you\r\nBaby, I would die for you, yeah"
                        },
                        new Song
                        {
                            Title = "Save Your Tears",
                            Url = "/uploads/songs/The Weeknd - Save Your Tears (Official Music Video).mp3",
                            DataPublicarii = new DateTime(2020, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5",
                            Lyrics = "na-na, yeah\r\nI saw you dancing in a crowded room, uh\r\nYou look so happy when I'm not with you\r\nBut then you saw me, caught you by surprise\r\nA single teardrop falling from your eye\r\nI don't know why I run away\r\nI'll make you cry when I run away\r\nYou could've asked me why I broke your heart\r\nYou could've told me that you fell apart\r\nBut you walked past me like I wasn't there\r\nAnd just pretended like you didn't care\r\nI don't know why I run away\r\nI'll make you cry when I run away\r\nTake me back 'cause I wanna stay\r\nSave your tears for another\r\nSave your tears for another day\r\nSave your tears for another day\r\nSo\r\nI made you think that I would always stay\r\nI said some things that I should never say\r\nYeah, I broke your heart like someone did to mine\r\nAnd now you won't love me for a second time\r\nI don't know why I run away\r\nOh, girl\r\nSaid, I'll make you cry when I run away\r\nGirl, take me back 'cause I wanna stay\r\nSave your tears for another\r\nI realize that I'm much too late\r\nAnd you deserve someone better\r\nSave your tears for another day (oh, yeah)\r\nSave your tears for another day (yeah)\r\nI don't know why I run away\r\nI'll make you cry when I run away\r\nSave your tears for another day\r\nOoh, girl\r\nI said, save your tears for another day (ah)\r\nSave your tears for another day (ah)\r\nSave your tears for another day (ah)"
                        },
                        new Song
                        {
                            Title = "Society",
                            Url = "/uploads/songs/The Weeknd - Society.mp3",
                            DataPublicarii = new DateTime(2011, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5",
                            Lyrics = "[Intro]\r\nOh-oh\r\nYeah\r\n\r\n[Verse 1]\r\nThey say they loved me\r\nWhen they never loved before\r\nThey wanna tie me\r\nHang me from the highest pole, you know\r\nIn a society\r\nWhere they only want your soul, your soul\r\n\r\n[Pre-Chorus]\r\nOn this hill I been dyin', so I'll never stop fighting\r\n'Cause nobody gonna love me more\r\n\r\n[Chorus]\r\nI don't know what they told you\r\nYou're a victim to the lies\r\nI spent my whole life to show you\r\nThat my heart beats every time\r\nI wanna show you how it feels\r\nI wanna show you how it feels\r\n\r\n[Verse 2]\r\nThey triеd to kill me\r\nBy a thousand papercuts (By a thousand papercuts)\r\nI need you bеside me (Oh-oh)\r\nYou're the only one I trust, I trust\r\nSee The Weeknd Live\r\nGet tickets as low as $39\r\nYou might also like\r\nCity Walls\r\nTwenty One Pilots\r\nDOG HOUSE\r\nDrake, Julia Wolf & Yeat\r\nRunaway\r\nThe Weeknd\r\n[Pre-Chorus]\r\nOn this hill I been dyin', and I'll never stop fighting\r\n'Cause nobody gonna love you more\r\n\r\n[Chorus]\r\nI don't know what they told you (Told you)\r\nYou're a victim to the lies\r\nI spent my whole life to show you (Show you)\r\nThat my heart beats every time\r\nI wanna show you how it feels, ooh\r\nI wanna show you how it feels\r\n\r\n[Outro]\r\nShow you how it feels\r\nShow you how it feels, woah-oh, yeah\r\nShow you how it feels\r\nHow it feels"
                        },
                        new Song
                        {
                            Title = "The Hills",
                            Url = "/uploads/songs/The Weeknd - The Hills.mp3",
                            DataPublicarii = new DateTime(2015, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5",
                            Lyrics = "Yeah\r\nYeah\r\nYeah\r\nYour man on the road, he doin' promo\r\nYou said, \"Keep our business on the low-low\"\r\nI'm just tryna get you out the friend zone\r\n'Cause you look even better than the photos\r\nI can't find your house, send me the info\r\nDrivin' through the gated residential\r\nFound out I was comin', sent your friends home\r\nKeep on tryna hide it but your friends know\r\nI only call you when it's half past five\r\nThe only time that I'd be by your side\r\nI only love it when you touch me, not feel me\r\nWhen I'm f- up, that's the real me\r\nWhen I'm f- up, that's the real me, yeah\r\nI only call you when it's half past five\r\nThe only time I'd ever call you mine\r\nI only love it when you touch me, not feel me\r\nWhen I'm f- up, that's the real me\r\nWhen I'm f- up, that's the real me, babe\r\nI'ma let you know and keep it simple\r\nTryna keep it up don't seem so simple\r\nI just f- two b- 'fore I saw you\r\nAnd you gon' have to do it at my tempo\r\nAlways tryna send me off to rehab\r\nD- started feelin' like it's decaf\r\nI'm just tryna live life for the moment\r\nAnd all these motherf- want a relapse\r\nI only call you when it's half past five\r\nThe only time that I'd be by your side\r\nI only love it when you touch me, not feel me\r\nWhen I'm f- up, that's the real me\r\nWhen I'm f- up, that's the real me, yeah\r\nI only call you when it's half past five\r\nThe only time I'd ever call you mine\r\nI only love it when you touch me, not feel me\r\nWhen I'm f- up, that's the real me\r\nWhen I'm f- up, that's the real me, babe\r\nHills have eyes, the hills have eyes\r\nWho are you to judge? Who are you to judge?\r\nHide your lies, girl, hide your lies\r\nOnly you to trust, only you\r\nI only call you when it's half past five\r\nThe only time that I'd be by your side\r\nI only love it when you touch me, not feel me\r\nWhen I'm f- up, that's the real me\r\nWhen I'm f- up, that's the real me, yeah\r\nI only call you when it's half past five\r\nThe only time I'd ever call you mine\r\nI only love it when you touch me, not feel me\r\nWhen I'm f- up, that's the real me\r\nWhen I'm f- up, that's the real me, babe"
                        },

                        new Song
                        {
                            Title = "Ghosts (Live)",
                            Url = "/uploads/songs/YUNGBLUD - Ghosts (Live From Hansa Studios).mp3",
                            DataPublicarii = new DateTime(2023, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8",
                            Lyrics = "Ooh-ooh\r\nOoh-ooh\r\nOoh-ooh\r\nOoh-ooh\r\nI see the distant smoke across the waterway\r\nI'd swim a thousand miles just to hear you say\r\nThe words that never come when you need them most\r\nTo dry your eyes out\r\nIs it your mother's tongue? Or your father's ghosts\r\nRemember sticks and stones and how they broke your bones\r\nPain is how you learn, boy, you learned a lot\r\nWhen they hit you in the face, and you begged them all to stop\r\nUntil you rose up again\r\nAnd you tried your best to surrender\r\nBowed down your head\r\nThen you looked up to the sky and said\r\n\"My God, what a beautiful scene\r\nNow I know what you mean\r\nWanna stay here forever\r\nGod, what a beautiful scene\r\nNow I know what you mean\r\nYou're my gateway to Heaven\"\r\nFrom underneath the dust, you see the curtain call\r\nCan't bear to watch another generation stall\r\nTo the hands of greed that will take it all\r\nAnd cut your heart out\r\nSee the face in the shadow\r\nJust a thread in the seams\r\nWe'll be ghosts by tomorrow\r\nJust to a day in a dream\r\nBut you'll rise up again, and you'll fight against the surrender\r\nMy friends, don't wait\r\nJust look up to the sky and say\r\n\"My God, what a beautiful scene\r\nNow I know what you mean\r\nWanna stay here forever\r\nGod, what a beautiful scene\r\nNow I know what you mean\r\nYou're my gateway to Heaven\"\r\n\"God, what a beautiful scene\r\nNow I know what you mean\r\nWanna stay here forever\r\nGod, what a beautiful scene\r\nNow I know what you mean\r\nYou're my gateway to Heaven\"\r\nGateway to Heaven\r\nYou're my gateway to Heaven\r\nGateway to Heaven\r\nAlright"
                        },
                        new Song
                        {
                            Title = "Hello Heaven, Hello",
                            Url = "/uploads/songs/YUNGBLUD - Hello Heaven, Hello.mp3",
                            DataPublicarii = new DateTime(2023, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8",
                            Lyrics = "Hello, are you out there?\r\nAre you trying? Are you patient?\r\nAre you blind?\r\nAre you with me? Against me?\r\nDon't know me at all\r\nHello, hello, hello, hello, hello\r\nHello, hello, hello\r\nHello, hello, hello, hello\r\nHello, hello, hello\r\nHello, hello, hello, hello, hello\r\nHello, hello, hello\r\nHello, hello, hello, hello\r\nHello, hello, hello\r\nHello, are you in there?\r\nDo you still remember, or have you forgotten where you're from?\r\nAre you still scared of dying?\r\nScared of them finding out that you don't know who you are?\r\nAnd I don't know what's in my head, but I know what's in my chest\r\nI don't know if I can make it, I don't know if I can change it\r\nBut I know it's how I feel, even if it isn't real\r\nI wanna feel alive, tell me, do you wanna feel alive?\r\nOh, I wanna feel alive\r\nHello, hello, hello, hello, hello\r\nHello, hello, hello (oh, I wanna feel alive)\r\nHello, hello, hello, hello\r\nHello, hello, hello\r\nHello, hello, hello, hello, hello\r\nHello, hello, hello\r\nHello, hello, hello, hello\r\nHello, hello, hello (tell me, do you wanna feel alive?)\r\nHello, hello, hello, hello, hello (tell me, do you wanna feel alive?)\r\nHello, hello, hello (tell me, do you wanna feel alive?)\r\nHello, hello, hello, hello (tell me, do you wanna feel alive?)\r\nHello, hello, hello (tell me, do you wanna feel alive?)\r\nHello, hello, hello, hello, hello\r\nHello, hello, hello (oh-oh)\r\nHello, hello, hello, hello (oh-oh)\r\nHello, hello, hello (oh-oh)\r\nSince I was a little boy, I devised a windmill getaway\r\nThey kicked me in the mud, and they told me, \"That's the price you pay\"\r\nSo tell me, are you gonna die with the lies that they forced inside your head?\r\nOr are you gonna live by the thorns in what you said?\r\nLittle freak, gonna walk, they don't talk\r\n'Til you've packed up and gone away\r\n\"Little boy, stupid boy, what you after each and every day?\"\r\nFor it's the fool who's the last to jump off the edge\r\none step into heaven\r\nBut first, you'll go to hell and back\r\n(One step) one step into heaven\r\nAre you gonna be the fool who's the last to jump off the edge?\r\nDon't give a damn about what they said\r\nSince I was a little boy, I always had a tear upon my face\r\nThey'd hit me in the mouth, and they told me, \"It's time to act your age\"\r\nSo tell me, are gonna die in the pain that they all inflict on you?\r\nOr are you gonna swim through the storm of what you have to do?\r\nLittle freak, gonna walk, they don't talk\r\n'Til you've packed up and gone away\r\nLittle boy, stupid boy, what you after each and every day?\r\nFor it's the fool who's last to jump off the edge\r\none step into heaven\r\nBut first, you'll go to hell and back\r\n(One step) one step into heaven\r\nAre you gonna be the fool who's the last to jump off the edge?\r\nDon't give a damn about what they said\r\nLittle freak, gonna walk, they don't talk\r\n'Til you've packed up and gone away\r\nLittle boy, stupid boy, what you after each and every day?\r\nFor it's the fool who's last to jump off the edge\r\none step into heaven\r\nBut first, you'll go to hell and back\r\n(One step) one step into heaven\r\nAre you gonna be the fool who's the last to jump off the edge?\r\nDon't give a damn about what they said\r\nThere's a chance I won't see you tomorrow\r\nSo I will spend today saying, \"Hello\"\r\nAnd all the hopes and dreams I may have borrowed\r\nJust know, my friend, I leave them all to you\r\nHello, hello\r\nHello\r\nHello, hello\r\nHello\r\nThat was powerful to hear that synth at the end\r\nLook, hey\r\nThe lion looking down at you and I\r\nWe're on the back of all the mankind\r\nAll the war, the pain, and thus\r\nYourselves, don't forget yourselves\r\nHey, hey, hey\r\nI'm missing out on all the love\r\nI'm so oblivious to love\r\nBut, oh\r\nYou reminded me\r\nI tried to write this song\r\nIt's as good as gold\r\nThat was really fucking good\r\nNo synths at the end\r\nI miss you, and I wish you'd hear this\r\nA reverb after the chorus\r\nOh"
                        },
                        new Song
                        {
                            Title = "I Was Made For Lovin You",
                            Url = "/uploads/songs/YUNGBLUD - I Was Made For Lovin You (from The Fall Guy).mp3",
                            DataPublicarii = new DateTime(2024, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8",
                            Lyrics = "Tonight\r\nI wanna give it all to you\r\nIn the darkness\r\nThere's so much I wanna do\r\nAnd tonight\r\nI wanna lay it at your feet\r\n'Cause, girl, I was made for you\r\nAnd, girl, you were made for me\r\n\r\n[Chorus]\r\nI was made for lovin' you, baby\r\nYou were made for lovin' me\r\nAnd I can't get enough of you, baby\r\nCan you get enough of me?\r\nSee upcoming rock shows\r\nGet tickets for your favorite artists\r\nYou might also like\r\nDarling, I\r\nTyler, The Creator\r\nSt. Chroma\r\nTyler, The Creator\r\nL’AMOUR DE MA VIE\r\nBillie Eilish\r\n[Verse 2]\r\nTonight\r\nI wanna see it in your eyes\r\nFeel the magic\r\nThere's something that drives me wild\r\nAnd tonight\r\nWe're gonna make it all come true\r\n'Cause, girl, you were made for me\r\nAnd, girl, I was made for you\r\n\r\n[Chorus]\r\nI was made for lovin' you, baby\r\nYou were made for lovin' me\r\nAnd I can't get enough of you, baby\r\nCan you get enough of me?\r\nI was made for lovin' you, baby\r\nYou were made for lovin' me\r\nAnd I can give it all to you, baby\r\nCan you give it all to me?\r\n\r\n[Bridge]\r\nOh, woah, can't get enough\r\nOh, woah, I can't get enough\r\nOh, woah, I can't get enough\r\n\r\n[Instrumental Break]\r\n[Guitar Solo]\r\nHa\r\n\r\n[Refrain]\r\nDo, do, do, do, do, do, do, do, do\r\nDo, do, do, do, do, do, do\r\nDo, do, do, do, do, do, do, do, do\r\nDo, do, do, do, do, do, do\r\n\r\n[Chorus]\r\nI was made for lovin' you, baby\r\nYou were made for lovin' me\r\nAnd I can't get enough of you, baby\r\nCan you get enough of me?\r\n\r\n[Post-Chorus]\r\nOh, I was made\r\nYou were made\r\nI can't get enough\r\nNo, I can't get enough\r\n\r\n[Chorus]\r\nI was made for lovin' you, baby\r\nYou were made for lovin' me\r\nAnd I can't get enough of you, baby\r\nCan you get enough of me?\r\nI was made for lovin' you, baby\r\nYou were made for lovin' me\r\nAnd I can't get enough\r\n\r\n\r\n\r\n"
                        },
                        new Song
                        {
                            Title = "Lowlife",
                            Url = "/uploads/songs/YUNGBLUD - Lowlife (Live From Marigny).mp3",
                            DataPublicarii = new DateTime(2023, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8",
                            Lyrics = "I'm not gonna go out today\r\nI gonna sit right here and wish the world away\r\n'Cause I'm a lowlife, lowlife\r\nI don't care if the people stare\r\nI'm gonna stay right here in my underwear\r\n'Cause I'm a lowlife, lowlife\r\nYou know I get embarrassed most of the days\r\nWhen I'm walking 'round Camden with a smirk on my face\r\nPut my hand in my pocket to find some change\r\nBuy a bottle of gin to get the memories erased\r\n'Cause you know what, life is extremely divisive\r\nIt's a fraudulent razor for you to trim your sides with\r\nYoung, dumb, living off mum\r\nThat's a line from a friend I thought I should bum\r\nNow all the words are my own, but I don't want you to judge\r\nI thought inspiration was all about fun\r\nLife's been eating me up, it's poisoned my cup\r\nAnd if I leave the house, I'll get hit by a truck\r\nI'm not gonna go out today\r\nI gonna sit right here and wish the world away\r\n'Cause I'm a lowlife, lowlife\r\nI don't care if the people stare\r\nI'm gonna stay right here in my underwear\r\n'Cause I'm a lowlife, lowlife\r\nIn the world of normal people, I'm a monkey\r\nIt's painted on their faces that they think I'm a junkie\r\nIn the way that I dress\r\nThe people I bed\r\nThe way I express\r\nThe things in my head\r\nI don't go to the bar\r\nI sit in the dark\r\nI smoke a cigar\r\nI play the guitar\r\nI stay in my house\r\nI put on my sounds\r\nThe neighbors tell me to turn it down, so\r\nI'm not gonna go out today\r\nI gonna sit right here and wish the world away\r\n'Cause I'm a lowlife, lowlife\r\nI don't care if the people stare\r\nI'm gonna stay right here in my underwear\r\n'Cause I'm a lowlife, lowlife\r\nLa-la-la-la-la (la-la-la-la-la)\r\nLa-la-la-la-la (la-la-la-la-la)\r\nLa-la-la-la-la\r\nLowlife\r\nLa-la-la-la-la (la-la-la-la-la)\r\nLa-la-la-la-la (la-la-la-la-la)\r\nLa-la-la-la-la\r\nLowlife\r\nPeople come, people go\r\nPeople high, people low\r\nPeople stay, people change\r\nPeople lie, people don't\r\nI don't fulfill my potential\r\n'Cause everyone around me is so self-referential\r\nHypocritical, maybe, a petulant baby\r\n(Wah wah wah, shut up)\r\nShut up, you're driving me crazy\r\nCall me a lowlife, that shit don't faze me\r\nI'm alright on my own, I don't need you to save me\r\nI'm not gonna go out today\r\nI gonna sit right here and wish the world away\r\n'Cause I'm a lowlife, lowlife\r\nI don't care if the people stare\r\nI'm gonna stay right here in my underwear\r\n'Cause I'm a lowlife, lowlife\r\nLa-la-la-la-la (la-la-la-la-la)\r\nLa-la-la-la-la (la-la-la-la-la)\r\nLa-la-la-la-la\r\nLowlife\r\nLa-la-la-la-la (la-la-la-la-la)\r\nLa-la-la-la-la (la-la-la-la-la)\r\nLa-la-la-la-la\r\nLowlife\r\nHello, mum\r\nNah, I'm not comin' over for Sunday dinner\r\nI think I've had too much to drink\r\nI love you\r\nLowlife\r\nYUNGBLUD (YUNG, YUNG)"
                        },
                        new Song
                        {
                            Title = "Zombie",
                            Url = "/uploads/songs/YUNGBLUD - Zombie (Official Music Video) [Yv97b2oPk3w].mp3",
                            DataPublicarii = new DateTime(2021, 1, 1),
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb8",
                            Lyrics = "If I was to talk about the words\r\nThey would hurt, they would hurt\r\nSo if you were to ask about the pain\r\nI would lie, I would lie\r\nTo fix my mind, I need time\r\nBut it's running out, it's running out\r\nOh, I know that I can't live without you\r\nBut this world will keep turning if you do\r\nWould you even want me looking like a zombie?\r\nWould you even want me, want me, want me?\r\nWe could catch a spaceship to the moon\r\nBut we'd crash, it wouldn't last\r\nBecause the world is just a figment of the fools\r\nA blank stare, they don't care\r\nSo say your prayers, you're almost there\r\nBut it's running out, it's running out\r\nOh, I know that I can't live without you\r\nBut this world will keep turning if you do\r\nWould you even want me looking like a zombie?\r\nWould you even want me, want me, want me?\r\nWould you even want me looking like a zombie?\r\nWould you even want me, want me, want me?\r\nOh, I don't know what I'll turn out to be\r\nBut you'll love every moment, believe me\r\nWould you even want me looking like a zombie?\r\nWould you even want me, want me, want me?"
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

