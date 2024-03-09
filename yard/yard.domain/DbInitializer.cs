using Azure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers.Text;
using System.Reflection.PortableExecutable;
using System;
using System.Text.Json;
using System.Threading.Channels;
using yard.domain.Context;
using yard.domain.enums;
using yard.domain.Models;
using yard.domain.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;


namespace yard.domain
{
    public static class DbInitializer
    {
        public static async Task SeedData(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            await Initialize(
                serviceScope.ServiceProvider.GetService<UserManager<AppUser>>(),
                serviceScope.ServiceProvider.GetService<ApplicationContext>(),
                serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>());
        }

        private static async Task Initialize(UserManager<AppUser>? userManager, ApplicationContext? context,
            RoleManager<IdentityRole>? roleManager)
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            if (context.Hotels.Any() && context.Users.Any())
            {
                return;
            }

            var address = new Address()
            {
                City = "Lagos",
                Country = "Nigeria",
                PostalCode = "00000",
                State = "La",
                Street = "Don't ask street"
            };

            await context.Addresses.AddAsync(address);

            var controlUser = new AppUser
            {
                FirstName = "John",
                LastName = "Sample",
                UserName = "Doe",
                Email = "testmail@gmail.com",
                PhoneNumber = "08162292349",
                PhoneNumberConfirmed = true,
                Gender = Gender.Male,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmailConfirmed = true,
                ProfilePictureUrl = "www.avatar.com",
                Address = address,
                AddressId = address.Id
            };
            await userManager.CreateAsync(controlUser, "Password@123");

            var rooms = new RoomType[]
            {
                new RoomType
                {
                    Name = "Regular",
                    Description = "thoughtfully designed spaces, essential amenities and a warm ambiance",
                    Price = 5000M,
                    Discount = 1000M,
                    Thumbnail = "www.fakethumbnail.com"
                },
                new RoomType
                {
                    Name = "Executive",
                    Description = "Tastefully appointed decor, modern amenities and personalized services",
                    Price = 150000M,
                    Discount = 10000M,
                    Thumbnail = "www.fakethumbnail.com"
                },
                new RoomType
                {
                    Name = "Presidential Suite",
                    Description = "Sophisticated decor, panoramic views and exclusive amenities",
                    Price = 900000M,
                    Discount = 50000M,
                    Thumbnail = "www.fakethumbnail.com"
                }
            };


            var hotel = new Hotel[]
            {
                new Hotel
                {
                    Name = "Decagon Lounge",
                    Description = "The Best hotel in Benin with a serene and congenial " +
                                  "atmosphere with a picturesque that's second to none",
                    Email = "decagonlounge@gmail.com",
                    Phone = "+2348137246538",
                    Address = new Address
                    {
                        City = "Benin", State = "Edo", Country = "Nigeria", PostalCode = "11111", Street = "Lagos road"
                    },
                    RoomTypes = rooms
                }
            };

            context.Hotels.AddRange(hotel);
            await context.SaveChangesAsync();
        }

        public static async Task SeedHotels(ApplicationContext? context)
        {
            var rooms = new RoomType[]
            {
                new RoomType
                {
                    Name = "Regular",
                    Description = "thoughtfully designed spaces, essential amenities and a warm ambiance",
                    Price = 5000M,
                    Discount = 1000M,
                    Thumbnail = "www.fakethumbnail.com"
                },
                new RoomType
                {
                    Name = "Executive",
                    Description = "Tastefully appointed decor, modern amenities and personalized services",
                    Price = 150000M,
                    Discount = 10000M,
                    Thumbnail = "www.fakethumbnail.com"
                },
                new RoomType
                {
                    Name = "Presidential Suite",
                    Description = "Sophisticated decor, panoramic views and exclusive amenities",
                    Price = 900000M,
                    Discount = 50000M,
                    Thumbnail = "www.fakethumbnail.com"
                }
            };


            var hotelsArray = new Hotel[]
            {
                //EDO Hotels
                new Hotel
                {
                    Name = "Protea Hotel Benin",

                    Description = "Protea Hotel Benin is a luxurious 3-Star hotel located at No 4 Central Road," +
                    " off Sapele Road, Benin City Edo state. Protea Hotel Benin stands in the heart of Benin City" +
                    " and it's only a 10-minute drive from the Benin Airport and 5 minutes drive from the Benin Golf Course." +
                    "\n\n\nProtea Hotel Benin has a total of 87 tastefully furnished rooms which come in the categories of Standard," +
                    " Standard Twin and Suites. Each room features state of the art facilities such as: A king-sized bed," +
                    " air conditioning, flat screen television set with access to multi-channel satellite, electronic safe," +
                    " sofa, reading table and chair, coffee/tea maker, high-speed wireless internet access, telephone," +
                    " workstation and hair dryers.\n\n\nIt also provides guests with excellent hotel facilities which include:" +
                    " Adequate parking arrangement, Event Hall, Gym with state of the art fitness facilities, Swimming pool," +
                    " Restaurant, Bar/Lounge, guaranteed security, free onsite parking, 3 meeting rooms, Casino, " +
                    "24 hours power supply and round-the-clock room service.\n\n\nGuests at Protea Hotel Benin " +
                    "also get treated to extra services and amenities like: Car hire, ATM, Business centre services, " +
                    "Currency exchange, Beauty services including Spa and Massage, Airport shuttle at an extra cost, " +
                    "Laundry services, Picnic area, coffee/tea in room, daily housekeeping services, concierge services," +
                    " newspaper in lobby and a special diet menu on request.\n\n\nTerms and Conditions\nCheck in: 2:00pm" +
                    "\nCheck out: 11:00am\nPets: Not allowed\nSmoking: Only in non-prohibited areas." +
                    "\n\nProtea Hotel Select Emotan is a luxury hotel in Benin, Edo.\n\n\nPlaces Of Interest Near Protea Hotel Benin" +
                    "\nBenin Museum\nOba's Palace\nKada Plaza\nOgba Zoo and Nature Park\nBenin City Airport\nBenin Golf Course",

                    Email = "reservations@phselectemotam.com",
                    Phone = "+2348109958643",
                    Address = new Address
                    {
                        City = "Benin", State = "Edo", Country = "Nigeria", PostalCode = "10235",
                        Street = "4 Central Road, Off Sapele Road"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168738/yard/Benin%20Hotels/Protea%20Hotel%20Select%20Emotan/protea-hotel-select-emotan-77974-28_pi60nq.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168729/yard/Benin%20Hotels/Protea%20Hotel%20Select%20Emotan/protea-hotel-select-emotan-77974-7_vog7hc.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168730/yard/Benin%20Hotels/Protea%20Hotel%20Select%20Emotan/protea-hotel-select-emotan-77974-21_vqjby9.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168741/yard/Benin%20Hotels/Protea%20Hotel%20Select%20Emotan/protea-hotel-select-emotan-77974-31_zzrcln.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168731/yard/Benin%20Hotels/Protea%20Hotel%20Select%20Emotan/protea-hotel-select-emotan-77974-26_etelbd.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168734/yard/Benin%20Hotels/Protea%20Hotel%20Select%20Emotan/protea-hotel-select-emotan-77974-27_agt3pm.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Double",

                            Description = "A comfortable and classic hotel room featuring a double bed, " +
                            "ideal for couples or solo travelers seeking a standard yet cozy accommodation",

                            Price = 95000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168806/yard/Benin%20Hotels/Protea%20Hotel%20Select%20Emotan/RoomTypes/standard_double_zqquvd.jpg"
                        },
                        new RoomType
                        {
                            Name = "Suite",

                            Description = "Indulge in luxury and spaciousness with our suite, offering a refined retreat complete" +
                            " with separate living and sleeping areas, perfect for those seeking an elevated stay experience.",

                            Price = 180000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168804/yard/Benin%20Hotels/Protea%20Hotel%20Select%20Emotan/RoomTypes/Suite_a3jo8x.jpg"
                        }
                    },
                    IsDelisted = false,
                    Popular = true
                },


                new Hotel
                {
                    Name = "Prestige Hotel and Suites",

                    Description = "Prestige Hotel and Suites is a luxurious hospitality outfit strategically planted in " +
                    "the highbrow GRA of Benin City, Edo. Its beautiful exterior reflects the comfort and charm awaiting" +
                    " guests in the rooms. It is situated 5 minutes away from the Benin Airport at 1 Ihama Road, GRA, Oka," +
                    " Benin City. Prestige Hotel is an excellent lodging facility for business and leisure travellers." +
                    "\n\n\nPrestige Hotel has 65 guest rooms and suites designed and equipped to exude warmth. " +
                    "They are categorised as Presidential Suite, Business Suite, Royal Suite and Executive rooms." +
                    " The suites have a private balcony while all rooms feature air-conditioners, LCD flat screen TV sets," +
                    " telephones, king size beds, refrigerators and en-suite bathrooms.\n\n\nMeals and drinks are available" +
                    " at the gourmet restaurant and exotic bar respectively, while the gym is available for workout sessions." +
                    " The swimming pool offers a relaxation spot and the business centre is available for use." +
                    " Parking is free on-site and round-the-clock power supply is guaranteed.\n\n\nPrestige Hotel also" +
                    " renders laundry, car hire, room service and concierge services on request.\n\nPrestige Hotel " +
                    "And Suites is a top-class hotel in Benin, Edo.\n\n\nTerms and Conditions about Prestige Hotel " +
                    "And Suites\nCheck-In: from 2pm (ID Required)\nCheck-Out: By 12pm\nPayment: Cash\nChildren: " +
                    "up to age 12 allowed to stay for free\n\n\nInteresting Places to visit near Prestige Hotel" +
                    "\nNational Museum, Benin (2.6km)\nOgba Zoo and Nature Park (9.5km)\nOba's Palace (2.6km)\nBenin" +
                    " Golf Course (1.5km)",

                    Email = "info@prestigehotel.com.ng",
                    Phone = "+2348151974044",

                    Address = new Address
                    {
                        City = "Benin", State = "Edo", Country = "Nigeria", PostalCode = "11111",
                        Street = "No. 1 Ihama Road by Airport Road junction, G.R.A."
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168655/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/5_qu5x6s.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168656/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/1_av7kbb.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168654/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/3_enaial.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168654/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/10_kxmj0z.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168653/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/16_aj6put.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Executive",

                            Description = "The Standard Executive room offers a perfect blend of comfort and functionality" +
                            " for the discerning traveler. Featuring contemporary decor and modern amenities, this room type" +
                            " provides a cozy retreat with essential conveniences, making it an ideal choice for both business" +
                            " and leisure travelers seeking a comfortable stay.",

                            Price = 27500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168684/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/RoomTypes/Presidential_Suite_x3iqwo.jpg"
                        },
                        new RoomType
                        {
                            Name = "Business Suite",

                            Description = "The Business Suite is designed to cater to the needs of the modern business traveler." +
                            " With a spacious layout and dedicated workspace, this suite combines luxury with functionality." +
                            " Guests can enjoy a seamless blend of comfort and productivity, making it an excellent choice " +
                            "for those who need a sophisticated environment to work and relax.",

                            Price = 38500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168682/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/RoomTypes/Royal_Suite_xz3r4n.jpg"
                        },
                        new RoomType
                        {
                            Name = "Royal Suite",

                            Description = "Indulge in opulence with the Royal Suite, a lavish accommodation that exudes elegance" +
                            " and style. This expansive suite is adorned with luxurious furnishings, fine fabrics," +
                            " and exquisite details. Guests can bask in the regal ambiance while enjoying top-notch amenities " +
                            "and personalized services, creating a truly memorable and majestic experience.",

                            Price = 44000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168681/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/RoomTypes/Executive_Room_ebmg3m.jpg"
                        },
                           new RoomType
                        {
                            Name = "Presidential Suite",

                            Description = "The Presidential Suite represents the epitome of luxury and sophistication." +
                            " This grand accommodation offers an unparalleled level of comfort and privacy, with a spacious" +
                            " living area, multiple bedrooms, and exclusive features. Guests staying in the Presidential Suite" +
                            " can expect unparalleled service, bespoke amenities, and an extraordinary stay designed for those " +
                            "who appreciate the finer things in life.",

                            Price = 71500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168681/yard/Benin%20Hotels/Prestige%20Hotels%20and%20Suites/RoomTypes/Bussiness_suite_kg5xrh.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Randekhi Gold Hotel Benin",

                    Description = "Randekhi Royal Hotel presents a true definition of luxury, comfort and style." +
                    " It is situated 5 minutes' drive from the Benin Airport at 5, Udemuyi Street, Avbriaria," +
                    " Benin City, Edo State. Its rooms and amenities are top-notch and well-suited to the needs of all" +
                    " types of guests. Randekhi Hotel presents a perfect stay for families on vacation.\n\n\nEach of" +
                    " the beautiful rooms in Randekhi Hotel Benin is exquisitely designed and air-conditioned; " +
                    "they have sofas, work desks, wardrobes, tea facilities, LCD TV sets, telephone sytems, " +
                    "refrigerator and en-suite bathrooms. Guests can select rooms from any of the categories " +
                    "available such as: Gold Premium, Gold Standard, Gold Chic, Gold Creme, Gold Grand, " +
                    "The View, Mini Suite, Posh Suite and Deluxe Suite. The rooms offer amazing views.\n\n\nThe " +
                    "restaurant serves both local and continental dishes while the exotic bar ensures guests obtain " +
                    "their choice of drinks. The hotel also boasts a fully equipped gym, spa, a standard outdoor " +
                    "swimming pool and tennis court where guests can have fun playing against each other and keeping " +
                    "their body in shape in the process. Services offered on request include safety deposit, " +
                    "laundry, room service and car hire services. \n\n\nTerms and Conditions about Randekhi Royal " +
                    "Hotel (Gold Wing)\nCheck-In: From 9am (ID Required\nCheck-Out: By 12pm\nPayment: Cash, Credit" +
                    " card and Corporate cheque\nChildren: up to age 18 can stay for freeInteresting Places to visit " +
                    "around Randekhi Royal Hotel\nOgba Zoo and Nature Park (9.4km)\nBenin Museum (3.6km)\nOba's " +
                    "Palace (3 6km)\nBenin Moat (6.5km).",

                    Email = "info@randekhihotels.com",
                    Phone = "08092127611",
                    Address = new Address
                    {
                        City = "Benin", State = "Edo", Country = "Nigeria", PostalCode = "10231", Street = "5 Udemuyi Street, Avbriaria"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168857/yard/Benin%20Hotels/Randekhi%20Gold/randekhi-royal-hotel-_gold-wing_-1006608-7_shnupv.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168852/yard/Benin%20Hotels/Randekhi%20Gold/randekhi-royal-hotel-_gold-wing_-1006608-8_sbu0kk.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168847/yard/Benin%20Hotels/Randekhi%20Gold/randekhi-royal-hotel-_gold-wing_-1006608-9_v3qvda.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168842/yard/Benin%20Hotels/Randekhi%20Gold/randekhi-royal-hotel-_gold-wing_-1006608-11_lpq1w1.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168841/yard/Benin%20Hotels/Randekhi%20Gold/randekhi-royal-hotel-_gold-wing_-1006608-6_b5o826.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Gold Premium",

                            Description = "Elevate your stay with the Gold Premium room, offering enhanced features and added luxury. " +
                            "Guests can enjoy a touch of sophistication in a refined setting, making this room a step above the " +
                            "standard option for those looking for a bit more indulgence during their stay.",

                            Price = 27600M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168914/yard/Benin%20Hotels/Randekhi%20Gold/RoomTypes/Gold_Premium_r19swo.jpg"
                        },
                        new RoomType
                        {
                            Name = "Gold Standard",

                            Description = "Embrace a comfortable stay with the Gold Standard room. This entry-level option provides" +
                            " a solid foundation for a pleasant experience, featuring essential amenities and a cozy atmosphere for " +
                            "travelers seeking a straightforward and welcoming accommodation.",

                            Price = 32920M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168909/yard/Benin%20Hotels/Randekhi%20Gold/RoomTypes/Gold_Standard_v9tx0v.jpg"
                        },
                        new RoomType
                        {
                            Name = "Gold Chic",

                            Description = "Step into contemporary elegance with the Gold Chic room. This stylish accommodation " +
                            "combines modern design elements with comfort, creating a chic and trendy atmosphere. Ideal for guests " +
                            "who appreciate aesthetics and a fashionable ambiance.",

                            Price = 36400M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168903/yard/Benin%20Hotels/Randekhi%20Gold/RoomTypes/Gold_Chic_dvfbpj.jpg"
                        },
                        new RoomType
                        {
                            Name = "Gold Creme",

                            Description = "Experience a luxurious retreat in the Gold Creme room, where plush furnishings and creamy " +
                            "tones create a soothing and upscale environment. This room type is designed for those seeking a refined " +
                            "escape with an emphasis on comfort and style.",

                            Price = 40000M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168920/yard/Benin%20Hotels/Randekhi%20Gold/RoomTypes/Gold_Creme_r7jzzt.jpg"
                        },
                        new RoomType
                        {
                            Name = "Gold Grand",

                            Description = "Indulge in opulence with the Gold Grand room, a lavish accommodation that exudes elegance" +
                            " and grandeur. Guests can immerse themselves in a truly upscale experience, surrounded by luxurious " +
                            "amenities and sophisticated design.",

                            Price = 44400M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168901/yard/Benin%20Hotels/Randekhi%20Gold/RoomTypes/Gold_Grand_bwspgu.jpg"
                        },
                        new RoomType
                        {
                            Name = "Mini Suite",

                            Description = "Step into a world of added space and comfort with the Mini Suite. This intermediate option" +
                            " provides a more generous living area and additional amenities, offering a taste of luxury without " +
                            "the full extravagance of a larger suite.",

                            Price = 88800M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168926/yard/Benin%20Hotels/Randekhi%20Gold/RoomTypes/Mini_suite_fbftlv.jpg"
                        },
                         new RoomType
                        {
                            Name = "Posh Suite",

                            Description = "Embrace the pinnacle of luxury with the Posh Suite. This top-tier accommodation offers a" +
                            " spacious and opulent living space, complemented by exclusive amenities and personalized services. Ideal " +
                            "for those seeking the ultimate indulgence, the Posh Suite promises an unforgettable and exquisite stay.",

                            Price = 88800M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168932/yard/Benin%20Hotels/Randekhi%20Gold/RoomTypes/Posh_suite_kduupk.jpg"
                        }
                    },

                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Eterno Hotels Benin",

                    Description = "Eterno Hotel is located at 103B, Aiguobasimwin Crescent, off Ikpopan Road, GRA Benin-City Edo State." +
                    " Its location, 2.4km away from Benin Airport, makes it a preferred choice for business travellers and tourists. " +
                    "It offers optimum tourism experience due to its proximity to several places of interest in the ancient city " +
                    "of Benin.\n\n\nGuests have several room categories to select from such as the Classic Deluxe, Exquisite, " +
                    "Executive Suite, Premier Suite and Presidential Suite. Irrespective of the room category, maximum comfort " +
                    "is guaranteed with features such as feather-soft king sized beds, air conditioners, wardrobes, refrigerators," +
                    " flat screen TV sets and en-suite bathrooms. Presidential Suite rooms offer additional value. All guests" +
                    " are treated to a complimentary breakfast.\n\n\nSeveral modern facilities are made available to guests at" +
                    " Eterno Hotel and these include a swimming pool, adequate parking lot, on-site restaurant and bar, " +
                    "an elevator system to ease access, Wi-Fi internet connection and an event hall. Security is optimised" +
                    " for an electric fence and CCTV cameras.\n\n\nIt offers additional services on requests, such as: luggage" +
                    " storage, laundry/dry cleaning services and taxi pickup.\n\nEterno Hotels is a luxury hotel in Benin City, Edo." +
                    "\n\n\nTerms and Conditions\nCheck In: From 3pm\nCheck Out: By 11.30am\nPayments: Cash and card payment options" +
                    "\n\n\nInteresting Places To Visit Near Eterno Hotels\nBenin City National Museum (2.7km)\nOba of " +
                    "Benin’s Palace (2.7km)\nOba Zoo and Nature Park (9.5km)\nBenin Moat (7.1km)\nHoly Aruosa Cathedral (3.3km).",

                    Email = "eternohotels1@gmail.com",
                    Phone = "+23452294403",
                    Address = new Address
                    {
                        City = "Benin", State = "Edo", Country = "Nigeria", PostalCode = "10231",
                        Street = "103B, Aiguobasimwin Crescent,off Ikpopan Road, GRA."
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154684/yard/Benin%20Hotels/Eterno%20Hotels/Eterno_Hotels_Limited_Benin_nfom8i.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154688/yard/Benin%20Hotels/Eterno%20Hotels/Eterno_Images4_ewd18m.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154691/yard/Benin%20Hotels/Eterno%20Hotels/Eterno_Images5_aa8m4e.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154686/yard/Benin%20Hotels/Eterno%20Hotels/Eterno_Images3_rjp2bx.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154685/yard/Benin%20Hotels/Eterno%20Hotels/Eterno_Images2_stxlvt.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Classic Deluxe",

                            Description = "This is Classic deluxe, It is the first category of our rooms and it comes with the same " +
                            "trappings and comfort you desire in hospitality industry.",

                            Price = 53750M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154775/yard/Benin%20Hotels/Eterno%20Hotels/Eterno%20Roomtypes/Classic_tw0l0a.jpg"
                        },
                        new RoomType
                        {
                            Name = "Exquisite Lodge",

                            Description = "This is Exquisite Lodge. This is a comfortable lodge comes with a (6'x6') king size bed" +
                            " with headrest. This lodge is exquisitely and lavishly furnished for your delight. This lodge is a" +
                            " “home away from home.",

                            Price = 66250M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154778/yard/Benin%20Hotels/Eterno%20Hotels/Eterno%20Roomtypes/Exquisite_c8xmtb.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Suite",

                            Description = "This room type has all it takes to make you feel at home. It provides exceptional guest" +
                            " experience. Here you have hospitality redefined. It is indeed an executive suite with every touch to" +
                            " give you that executive comfort in a serene environment.",

                            Price = 93750M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154776/yard/Benin%20Hotels/Eterno%20Hotels/Eterno%20Roomtypes/Executive_spvpxp.jpg"
                        },
                             new RoomType
                        {
                            Name = "Premier Suites",

                            Description = "This Room type we describe as Premier, It is actually on the lead, a suite with a difference," +
                            " next to the presidential suite and comes with a (6'x 6') king size bed and spacious outer room comfortably" +
                            " furnished for a lovely stay that will exceed your expectation.",

                            Price = 107500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154782/yard/Benin%20Hotels/Eterno%20Hotels/Eterno%20Roomtypes/Premier_Suite_amhuil.jpg"
                        },
                        new RoomType
                        {
                            Name = "Presidential Suite",

                            Description = "This is Presidential suite in every sense of it, lavishly and tastefully furnished and it " +
                            "is spacious to give you all the comfort you desired. It is a two bed room APARTMENT suite with Jacuzzi, " +
                            "a fully equipped dinning, kitchenette, Bar, two Toilet etc. The Parlor is big with all the necessary " +
                            "trappings. This is our jewel, come and experience the luxury.",

                            Price = 207500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705154786/yard/Benin%20Hotels/Eterno%20Hotels/Eterno%20Roomtypes/Presidential_Suite_zsfhop.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "OVIC Hotel Benin",

                    Description = "Ovic Hotel Limited is your ultimate destination for exceptional hospitality and remarkable stays. " +
                    "We prioritize excellence in every aspect of your visit. Experience unmatched comfort and luxury in our meticulously" +
                    " crafted rooms and suites, boasting awe-inspiring views and modern amenities.\n\n\nIndulge your taste buds at " +
                    "our renowned restaurants, serving a delightful blend of local and international cuisines. Our world-class facilities" +
                    " and flexible event spaces make us the ideal choice for both business and leisure travelers.  Book your stay today" +
                    " and discover the best hotel experience in Benin City, Nigeria.",

                    Email = "admin@ovichotel.com",
                    Phone = "+2348096410000",
                    Address = new Address
                    {
                        City = "Benin", State = "Edo", Country = "Nigeria", PostalCode = "10121", Street = "65 Ihama Road"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168001/yard/Benin%20Hotels/OVIC%20Hotel/OVIC_Hotel_Benin_fuozxd.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705167997/yard/Benin%20Hotels/OVIC%20Hotel/SRM-00342-scaled_sd4quc.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705167994/yard/Benin%20Hotels/OVIC%20Hotel/SRM-0008-scaled_dh1utz.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705167993/yard/Benin%20Hotels/OVIC%20Hotel/SRM-9983-scaled_isaatl.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Studio Room",

                            Description = "Experience the epitome of luxury in Ovic Hotel's Studio Room, where modern amenities," +
                            " advanced air conditioning, and a widescreen TV enhance your stay. Indulge in complimentary breakfast " +
                            "and immerse yourself in a world of comfort and elegance at the heart of Ovic Hotel.",

                            Price = 26000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168074/yard/Benin%20Hotels/OVIC%20Hotel/RoomTypes/Studio_Room_soogw2.jpg"
                        },
                        new RoomType
                        {
                            Name = "Standard Room",

                            Description = "Indulge in unmatched comfort at Ovic Hotel's Standard Room, featuring modern amenities like " +
                            "advanced air conditioning and a widescreen TV. Delight in a complimentary breakfast, tailored to your tastes," +
                            " and book your stay for a luxurious experience in the heart of Ovic Hotel.",

                            Price = 320000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168068/yard/Benin%20Hotels/OVIC%20Hotel/RoomTypes/Standard_Room_hngwfc.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Room",

                            Description = "This room type has all it takes to make you feel at home. It provides exceptional " +
                            "guest experience. Here you have hospitality redefined. It is indeed an executive suite with every " +
                            "touch to give you that executive comfort in a serene environment.",

                            Price = 38000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168062/yard/Benin%20Hotels/OVIC%20Hotel/RoomTypes/Deluxe_pxxk39.jpg"
                        },
                           new RoomType
                        {
                            Name = "Executive Room",

                            Description = "Discover unparalleled comfort and luxury in Ovic Hotel's Executive Room, featuring modern" +
                            " amenities and advanced air conditioning. Enjoy a delightful surprise with a complimentary breakfast, " +
                            "making your stay a seamless blend of luxury, convenience, and top-notch service.",

                            Price = 46000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168057/yard/Benin%20Hotels/OVIC%20Hotel/RoomTypes/Executive_evkgu7.jpg"
                        },
                         new RoomType
                        {
                            Name = "1 Bedroom Suite",

                            Description = "Experience ultimate comfort and luxury in Ovic Hotel's 1 Bedroom Suite, offering a spacious" +
                            " retreat with modern amenities and advanced air conditioning. Indulge in entertainment on the widescreen TV" +
                            " and begin your day with a complimentary breakfast, making your stay a blend of luxury, convenience, and" +
                            " top-notch service.",

                            Price = 55000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168051/yard/Benin%20Hotels/OVIC%20Hotel/RoomTypes/1_bedroom_Suite_lyixly.jpg"
                        },
                        new RoomType
                        {
                            Name = "2 Bedroom Suite",

                            Description = "Discover ultimate comfort and luxury in Ovic Hotel's 2 Bedroom Suite, featuring a spacious" +
                            " retreat with modern amenities and advanced air conditioning. Elevate your stay with widescreen " +
                            "TV entertainment and a delightful surprise of complimentary breakfast, creating a luxurious experience" +
                            " that blends convenience and top-notch service.",

                            Price = 850000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168046/yard/Benin%20Hotels/OVIC%20Hotel/RoomTypes/2_bedroom_suite_frbune.jpg"
                        },
                            new RoomType
                        {
                            Name = "Presidential Suite",

                            Description = "Experience the pinnacle of comfort and luxury in Ovic Hotel's Presidential Suite, featuring" +
                            " a spacious retreat with modern amenities and advanced air conditioning. Indulge in widescreen TV " +
                            "entertainment and awaken to a delightful surprise of complimentary breakfast, making your stay an " +
                            "unparalleled blend of luxury, convenience, and top-notch service.",

                            Price = 120000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705168044/yard/Benin%20Hotels/OVIC%20Hotel/RoomTypes/Presidential_suite_fuslv0.jpg"
                        }
                    },
                    IsDelisted = false
                },

                 //ABUJA Hotels
                new Hotel
                {
                    Name = "Transcorp Hilton",

                    Description = "Transcorp Hilton Abuja is a 5-Star, state of the art, hotel located at 1 Aguiyi Ironsi Street, " +
                    "Maitama, Abuja. It lies within the capital’s commercial district and is easily accessible by road from the" +
                    " Nnamdi Azikiwe International Airport. Hilton Abuja is strategically located near local businesses, embassies " +
                    "and government buildings. It is also a 7-minute drive from the world-class IBB International Golf and Country " +
                    "Club.\n\n\nTranscorp Hilton Abuja has 670 luxuriously decorated rooms and offers complimentary breakfast, all-day" +
                    " snacks and refreshment, cocktails, local/international newspapers and magazines to all guests. These rooms are " +
                    "categorised into: Twin Guest Room, King Guest Room, King Deluxe, King Room, Twin Guest Room, Business Suite, " +
                    "Ambassadorial Suite and Presidential Suite. Guests also enjoy 24 hours room service and outside catering services." +
                    "\n\n\nTranscorp Hilton Abuja offers a wide range of leisure facilities including an outdoor swimming pool, " +
                    "a children's wading pool, an onsite casino, a fitness centre, a sports hall for squash and volleyball, a barber " +
                    "shop, a hair salon, a tennis court, and a shopping arcade. Free WiFi is also available throughout the hotel. " +
                    "The hotel has 7 restaurants and bars with a wide range of international and local cuisine coupled with a selection" +
                    " of the best wines and cocktails.\n\n\nTranscorp Hilton offers a choice of 24 multi-purpose meeting rooms - all" +
                    " with air conditioning and WiFi internet access. It also has 1,200 capacity Congress Centre. Business centre " +
                    "services and ATM are also available on the premises. Taxi, bus and car rental services are available at the " +
                    "hotel.\n\n\nOther services on offer include safety deposit boxes, laundry/dry cleaning, car hire, 24 hours " +
                    "room service, event facilities, concierge, storage room, 24 hours front desk, and shops. Transcorp Hilton is " +
                    "professionally guarded with high-end security equipment and personnel.\n\n\nInteresting Places to Visit near " +
                    "Transcorp Hilton Abuja\nIBB International Golf and Country Club.\nMillennium Park\nAso Rock\nZuma rock\nAbuja" +
                    " Arts & Crafts Village\nGrand\nTowers Abuja Mall\nKryxtal Lounge\nThought Pyramid Art Centre\nJabi Lake " +
                    "Mall\nFarin Ruwa Falls\nSignature Nigeria.\n\nTranscorp Hilton Abuja is a luxury hotel in Maitama, Abuja." +
                    "\n\nTerms and Conditions\nCheck in: From 3:00pm (ID Required)\nCheck out: By 12:00pm\nPayment: Cash and " +
                    "card payments are accepted\nChildren: Children under age 13 stay free (rolling bed available).",

                    Email = "abuja.hilton.com",
                    Phone = "+234 9 461 3003",
                    Address = new Address
                    {
                        City = "Abuja", State = "Abuja-FCT", Country = "Nigeria", PostalCode = "10235",
                        Street = "1 Aguiyi Ironsi Street Maitama, Abuja, Maitama, Abuja"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171393/yard/Abuja%20Hotels/Transcorp%20Hilton/Transcorp3_csstpo.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171406/yard/Abuja%20Hotels/Transcorp%20Hilton/Transcorp5_pjzjcs.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171399/yard/Abuja%20Hotels/Transcorp%20Hilton/Transcorp4_at6em3.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171390/yard/Abuja%20Hotels/Transcorp%20Hilton/Transcorp2_htcnah.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171547/yard/Abuja%20Hotels/Transcorp%20Hilton/Transcorp8_q6lthq.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Twin Room",

                            Description = "2 double beds(1.31m-1.5m wide)\nExtra beds and cribs are unavailable for this room " +
                            "type\nNon-smoking,Free Wi-Fi | Wired Internet (additional charge),Private Bathroom,Air Conditioning," +
                            "Kitchenware,Telephone.",

                            Price = 206000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171717/yard/Abuja%20Hotels/Transcorp%20Hilton/RoomTypes/Twin_Room_sdvuoc.webp"
                        },
                        new RoomType
                        {
                            Name = "King Room",

                            Description = "1 king bed(1.81m-3m wide)\nExtra beds can be added for this room type, but cribs " +
                            "cannot be provided\nAdult NGN 7,000 (approx. US$7.31) per person per night,Non-smoking,Free Wi-Fi | " +
                            "Wired Internet (additional charge),Private Bathroom,Air Conditioning.",

                            Price = 222000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171718/yard/Abuja%20Hotels/Transcorp%20Hilton/RoomTypes/King_Room_with_pool_j6rnrf.webp"
                        },
                        new RoomType
                        {
                            Name = "Deluxe King Room",

                            Description = "1 king bed(1.81m-3m wide)\nExtra beds can be added for this room type, but cribs cannot" +
                            " be provided/nAdult NGN 7,000 (approx. US$7.31) per person per night\nCity View,Non-smoking,Free Wi-Fi |" +
                            " Wired Internet (additional charge),Bathtub, Private Bathroom, Air Conditioning, Sofa, Minibar, Telephone.",

                            Price = 227000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171718/yard/Abuja%20Hotels/Transcorp%20Hilton/RoomTypes/Deluxe_King_Room_qkxwbx.webp"
                        },
                        new RoomType
                        {
                            Name = "Business Suite",

                            Description = "1 king bed(1.81m-3m wide)\nExtra beds can be added for this room type, but cribs cannot " +
                            "be provided\nAdult NGN 7,000 (approx. US$7.31) per person per night\nCity View,Non-smoking,Free Wi-Fi |" +
                            " Wired Internet (additional charge),Air Conditioning,Kitchenware,Telephone.",

                            Price = 340000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171718/yard/Abuja%20Hotels/Transcorp%20Hilton/RoomTypes/Business_Suite_sye7w8.webp"
                        }
                    },
                    IsDelisted = false
                },


                new Hotel
                {
                    Name = "The Destination by Gidanka",

                    Description = "The Destination by Gidanka has free bikes, outdoor swimming pool, a fitness centre and garden in" +
                    " Abuja. Each accommodation at the 4-star hotel has city views, and guests can enjoy access to a terrace and to a " +
                    "bar. The accommodation features entertainment staff and room service.\n\n\nAt the hotel rooms are fitted with " +
                    "air conditioning, a seating area, a flat-screen TV with satellite channels, a safety deposit box and a private" +
                    " bathroom with a shower, free toiletries and slippers. Rooms are equipped with a kettle, while certain rooms " +
                    "come with a kitchen with a dishwasher and a minibar. At The Destination by Gidanka every room includes bed linen " +
                    "and towels.\n\n\nA buffet breakfast is available each morning at the accommodation. At The Destination by " +
                    "Gidanka you will find a restaurant serving African, American and Chinese cuisine. Vegetarian, halal and " +
                    "gluten-free options can also be requested.\n\nThe hotel offers a children's playground.\n\n\nSpeaking Arabic," +
                    " English and French at the reception, staff are ready to help around the clock.\n\n\nMagic Land Abuja is 6.8 km" +
                    " from The Destination by Gidanka, while IBB Golf Club is 11 km from the property. The nearest airport is " +
                    "Nnamdi Azikiwe International, 28 km from the accommodation, and the property offers a paid airport shuttle " +
                    "service.\n\nCouples particularly like the location — they rated it 9.0 for a two-person trip." +
                    " Golf Course (1.5km)",

                    Email = "info@thedestinationbygardanka.com ",
                    Phone = "+23410331551",

                    Address = new Address
                    {
                        City = "Abuja", State = "Abuja", Country = "Nigeria", PostalCode = "11111",
                        Street = "20 Ndjamena Crescent Off Aminu Kano Road, 904101 Abuja, Nigeria"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170958/yard/Abuja%20Hotels/The%20Destination%20Hotel/The_Destination_by_Gidanka_Abuja_acpbfl.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170943/yard/Abuja%20Hotels/The%20Destination%20Hotel/The_Destination_Image6_zucxkw.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171079/yard/Abuja%20Hotels/The%20Destination%20Hotel/The_Destination_Image4_mtrgmq.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170944/yard/Abuja%20Hotels/The%20Destination%20Hotel/The_Destination_Image7_nfypsc.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170963/yard/Abuja%20Hotels/The%20Destination%20Hotel/The_Destination_Image1_qaosqz.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170948/yard/Abuja%20Hotels/The%20Destination%20Hotel/The_Destination_Image8_kfjvbf.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170968/yard/Abuja%20Hotels/The%20Destination%20Hotel/The_Destination_Image2_pzalbm.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Junior Suite",

                            Description = "Private suite 18 m², Kitchenette, Private bathroomv, Garden view, Pool view, Landmark view," +
                            " City view, Air conditioning, Flat-screen TV, Soundproofing",

                            Price = 72718M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171116/yard/Abuja%20Hotels/The%20Destination%20Hotel/RoomTypes/Junior_Suite_daijpf.jpg"
                        },
                        new RoomType
                        {
                            Name = "One-Bedroom Suite",

                            Description = "Private suite 170 m², Kitchen, Private bathroom, Balcony, Garden view, Pool view, Landmark" +
                            " view, City view, Inner courtyard view, Air conditioning, Patio, Dishwasher, Flat-screen TV, Soundproofing," +
                            " Coffee machine, Minibar",

                            Price = 159980M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171117/yard/Abuja%20Hotels/The%20Destination%20Hotel/RoomTypes/One_Bedroom_Suite_wwkf1z.jpg"
                        },
                        new RoomType
                        {
                            Name = "Two-Bedroom Suite",

                            Description = "Private suite 145 m², Kitchen, Private bathroom, Balcony, Garden view, Pool view, Landmark" +
                            " view, City view, Air conditioning, Patio, Dishwasher, Flat-screen TV, Soundproofing, Coffee machine.",

                            Price = 191491M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171128/yard/Abuja%20Hotels/The%20Destination%20Hotel/RoomTypes/Two-Bedroom_Suite_wnkhsg.jpg"
                        },
                           new RoomType
                        {
                            Name = "Presidential Suite",

                            Description = "Private suite 375 m², Kitchen, Private bathroom, Balcony, Garden view, Pool view, Landmark" +
                            " view, City view, Inner courtyard view, Air conditioning, Patio,Dishwasher, Flat-screen TV, Soundproofing," +
                            " Coffee machine, Minibar..",

                            Price = 776467M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705171122/yard/Abuja%20Hotels/The%20Destination%20Hotel/RoomTypes/Presidential_Suite_wntsmk.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Nordic Hotel",

                    Description = "Nordic Hotel is located in proximity to the Cameroon Embassy in Abuja and features a exquisite " +
                    "restaurant and bar. It is located at Plot 1322, Shehu Yaradua, Boulevard, BluCabana, Mabushi, Abuja, Nigeria." +
                    "\n\n\nRooms at the hotel are categorised into Standard Single, Standard Double Room, Junior Suite and the Nordic " +
                    "Suite. Each room features air-conditioning units, flat-screen TV with satellite reception and an en-suite bathroom." +
                    " Some of these rooms have a separate spacious sitting area.\n\n\nNordic Hotel boasts Free Wi-Fi, a spa area, a " +
                    "well-equipped fitness centre/gym for workout sessions, a standard outdoor swimming pool where guest can enjoy a " +
                    "relaxing swim, an on-site restaurant which serves a variety of local and continental meals and a well-stocked bar" +
                    " where guest can order for different kinds of alcoholic and non-alcoholic drinks while relaxing at the lounge. " +
                    "Adequate parking is available and the hotel premises is guarded by security personnel.\n\n\nAdditional services" +
                    " such as airport shuttle, laundry/dry cleaning, car hire, a 24-hour room service, ironing and concierge are made" +
                    " available by the hotel on request. A meeting/banquet facility is available for both business and social meetings." +
                    "\n\nNordic Hotel is a luxury hotel in Mabushi , Abuja.\n\n\nInteresting Places to Visit near Nordic Hotel\nJabi " +
                    "Lake (1 km)\nZuma Rock (20 km)\nBluCabana (0.1 km)\nNext Supermarket (1 km)\nShoprite (1.2 km)\nGame (1 km)\nMama " +
                    "Africa (4.1 km)\nThe nearest airport is the Nnamdi Azikwe International Airport which is just 20 km away\n\n\nTerms" +
                    " and Conditions\nClock-In: 2:00 PM(ID Required)\nClock - Out: 12:00 PM\nCancellation: Cancellation Policies vary " +
                    "with room type.\nChildren: Children are not allowed to stay for free\nPets: Pets are not allowed\nPayment: Cash, " +
                    "verve and MasterCard are accepted here.",

                    Email = "info@nordichotelabuja.com",
                    Phone = "08099944480",
                    Address = new Address
                    {
                        City = "Abuja", State = "Abuja", Country = "Nigeria", PostalCode = "10231",
                        Street = "Plot 1322, Shehu Yaradua, Boulevard, BluCabana, Mabushi, Abuja, Nigeria."
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170787/yard/Abuja%20Hotels/Nordic%20Hotel/Nordic_Hotel_Abuja_epnmdj.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170784/yard/Abuja%20Hotels/Nordic%20Hotel/Nordic_2_xoinh6.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170794/yard/Abuja%20Hotels/Nordic%20Hotel/Nordic4_vrtcqt.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170791/yard/Abuja%20Hotels/Nordic%20Hotel/Nordic3_p5vd2o.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170781/yard/Abuja%20Hotels/Nordic%20Hotel/Nordic_1_r6kjeq.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170780/yard/Abuja%20Hotels/Nordic%20Hotel/Nordic9_s5jxsy.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170808/yard/Abuja%20Hotels/Nordic%20Hotel/Nordic10_d1cv0y.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Single Room",

                            Description = " Room size 20 m², Comfy beds, 8.2 – Based on 55 reviews, This air-conditioned single room" +
                            " includes a flat-screen TV with cable channels, a private bathroom as well as a balcony. The unit offers" +
                            " 1 bed.",

                            Price = 102850M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170859/yard/Abuja%20Hotels/Nordic%20Hotel/RoomTypes/Standard_Single_uqkdta.jpg"
                        },
                        new RoomType
                        {
                            Name = "Standard Double Room",

                            Description = "Room size 30 m², Comfy beds, 8.2 – Based on 55 reviews, The double room offers a safe " +
                            "deposit box, a tiled floor, as well as a private bathroom boasting a shower and a hairdryer. The spacious" +
                            " air-conditioned double room features a flat-screen TV with cable channels, a minibar, a tea and coffee " +
                            "maker and a wardrobe. The unit has 1 bed.",

                            Price = 113190M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170855/yard/Abuja%20Hotels/Nordic%20Hotel/RoomTypes/Standard_Double_Room_tfrkjr.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Room",

                            Description = "40 m² Balcony View, Air conditioning, Private bathroom, Flat-screen TV, Minibar, Free WiFi," +
                            " Room size 40 m², Comfy beds, 8.2 – Based on 55 reviews, A seating area with a flat-screen TV, a desk, " +
                            "a balcony and a private bathroom are available in this spacious double room. The unit has 1 bed.",

                            Price = 132825M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170853/yard/Abuja%20Hotels/Nordic%20Hotel/RoomTypes/Deluxe_Suite_ls5jcb.jpg"
                        },
                        new RoomType
                        {
                            Name = "Nordic Suite",

                            Description = "Private suite 55 m², Balcony View, Air conditioning, Private bathroom, Flat-screen TV, " +
                            "Minibar, Free WiFi,Comfy beds, 8.2 – Based on 55 reviews, This spacious suite includes 1 living room, " +
                            "1 separate bedroom and 1 bathroom with a shower and bathrobes. Boasting a balcony, this suite also offers " +
                            "air conditioning, a minibar and a flat-screen TV with cable channels. The unit has 1 bed.",

                            Price = 175560M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170854/yard/Abuja%20Hotels/Nordic%20Hotel/RoomTypes/Nordic_Suite_tddzeg.jpg"
                        },

                    },

                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Musada Hotel",

                    Description = "Musada Luxury Hotels and Suites is located at No. 3 Nola Close, off Bangui Street, Wuse 2, Abuja." +
                    " Situated in the heart of the Abuja Urban Centre, it is easily accessible and close to public and private " +
                    "institutions.\n\n\nRooms at the Musada Luxury Hotels and Suites come with a fan/air conditioner, private bathroom" +
                    " with stand-in shower facilities, flat screen television set with multi-channel cable TV subscription, comfortable" +
                    " bed, work table and chair plus a wardrobe.\n\n\nAn array of hotel amenities is provided to guests at the Musada " +
                    "Luxury Hotels and Suites. These include: an outdoor pool, bar/lounge, round-the-clock electricity supply, " +
                    "sufficient car parking space, and great security facilitated by capable security personnel.\n\n\nOther services " +
                    "which guests can take advantage of at the Musada Luxury Suites are laundry services, housekeeping, and 24 hours" +
                    " front desk and room service.\n\nMusada Luxury Hotels and Suites is a budget hotel in Wuse 2, Abuja.\n\n\nTerms" +
                    " and Conditions about Musada Luxury Hotels and Suites\nCheck in- from 12:00pm\nCheck out- by 12:00pm\nPayment:" +
                    " CashKids: free lodgings for children yet to attain the age of 15\nSmoking: not allowed in rooms.\n\n\nPlaces Of" +
                    " Interest Near Musada Luxury Hotels and Suites\nBloomsbury Mall\nThe Carribean Lounge\nO'Neals\nNational Pension" +
                    " Commission\nFederal Ministry of Mines and Steel development\nGrills in and out\nSofii lounge.",

                    Email = "musadaluxuryhotelsandsuit.alohomoro.com",
                    Phone = "+234 808 268 3177",
                    Address = new Address
                        {
                             City = "Abuja", State = "Abuja", Country = "Nigeria", PostalCode = "10231",
                             Street = "No. 3 Nola Close, off Bangui Street, Wuse 2, Wuse 2, Abuja"
                        },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170647/yard/Abuja%20Hotels/Musada%20Hotel/Musada_Luxury_Hotel_and_Suites_Abuja_xvscdv.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170654/yard/Abuja%20Hotels/Musada%20Hotel/Musada_Image5_qk6lmw.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170653/yard/Abuja%20Hotels/Musada%20Hotel/Musada_Image4_i8nfe3.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170651/yard/Abuja%20Hotels/Musada%20Hotel/Musada_image3_wiveru.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170658/yard/Abuja%20Hotels/Musada%20Hotel/Musada_Image10_phgpfq.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Deluxe Double Room",

                            Description = "Room size 25 m², 1 large double bed, Comfy beds, 8.9 – Based on 32 reviews,This " +
                            "air-conditioned double room is comprised of a flat-screen TV with streaming services, a private bathroom" +
                            " as well as a balcony with a quiet street view. The unit has 1 bed",

                            Price = 76000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170723/yard/Abuja%20Hotels/Musada%20Hotel/RoomTypes/Deluxe_Double_Room_y7f2oc.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Suite",

                            Description = "Room size 37 m², Bedroom 1: 1 large double bed, Living room: 3 sofa beds, Comfy beds, " +
                            "8.9 – Based on 32 reviews,This suite is comprised of 1 living room, 1 separate bedroom and 1 bathroom with" +
                            " a bath and free toiletries. This suite features air conditioning, a tea and coffee maker, flat-screen TV" +
                            " with streaming services, as well as chocolate for guests. The unit offers 4 beds.",

                            Price = 114000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170724/yard/Abuja%20Hotels/Musada%20Hotel/RoomTypes/Deluxe_Suite_egfers.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "BON Hotel",

                    Description = "An imposing architectural masterpiece with a perfect touch of class, the BON Hotel is elegantly" +
                    " nestled close to Abuja's city centre. This 4-Star luxury hotel offers a superior hospitality experience with " +
                    "comfortable lodging. It is located at 3 Negro Crescent, Maitama, Abuja about 3km from the British Embassy.\n\n\nThe" +
                    " 28 well-appointed luxury rooms available in the hotel are categorised into Classic, Executive and Loft Suites." +
                    " Each room features a lounge and king-size beds. Loft Suites have a downstairs lounge and a guest bathroom. Also" +
                    " available are: a hair dryer, TV with local and international channels, electronic safe, work tables, sofas, " +
                    "wardrobes and tea/coffee facilities. All guests are treated to sumptuous complimentary breakfast.\n\n\nA gourmet" +
                    " restaurant serves mouth-watering buffet breakfast, and delicious lunch and dinner complimented with an exotic " +
                    "bar stocked with all kinds of assorted drinks. A coffee bar is also available for light dining and socialising." +
                    " An outdoor swimming pool is available for a refreshing dip as well as gym facilities. BON Hotel Abuja boasts a" +
                    " business centre and a 20-seater Regent Hills Boardroom equipped with a projector and WiFi internet access, " +
                    "perfect for meetings and presentations.\n\n\nServices such as laundry, airport shuttle and car hire services " +
                    "are also available on request.\n\n\nTerms and Conditions\nCheck-In: from 12 pm (ID Required)\nCheck-Out: by 12 pm" +
                    "\nPayment: Cash and Credit card\nChildren: allowed to stay for free\nCancellation: varies according to room " +
                    "categories\n\nPlease note that non-refundable rates can only be booked via card prepayments and reservations " +
                    "made on these rates cannot be refunded.\n\n\nInteresting Places To Visit Near BON Hotel\nJabi Lake (8.3km)" +
                    "\nMillennium Park (8.5km)\nSilverbird Cinemas (6.1km)\nShoprite (6.9km)",

                    Email = "gm.bonhotelabuja@outlook.com",
                    Phone = "07086300440",
                    Address = new Address
                    {
                        City = "Abuja", State = "Abuja", Country = "Nigeria", PostalCode = "10121",
                        Street = "3 Negro Crescent, Maitama Abuja,Nigeria."
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170337/yard/Abuja%20Hotels/Bon%20Hotel/Bon5_si1hvo.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170361/yard/Abuja%20Hotels/Bon%20Hotel/Bon8_ktw9jy.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170345/yard/Abuja%20Hotels/Bon%20Hotel/Bon6_aryvo9.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170332/yard/Abuja%20Hotels/Bon%20Hotel/Bon4_ggpmaq.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170353/yard/Abuja%20Hotels/Bon%20Hotel/Bon7_swsxho.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Classic Room",

                            Description = "Featuring free toiletries, this double room includes a private bathroom with a walk-in " +
                            "shower, a bath and a bidet. The air-conditioned double room features a flat-screen TV, soundproof walls," +
                            " a tea and coffee maker, a seating area as well as pool views. The unit has 1 bed.",

                            Price = 86250M,
                            Discount = 13.75M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170526/yard/Abuja%20Hotels/Bon%20Hotel/RoomTypes/Suite_x1q75u.jpg"
                        },
                        new RoomType
                        {
                            Name = "Execeutive Suite",

                            Description = "The spacious suite features air conditioning, soundproof walls, as well as a private " +
                            "bathroom boasting a bath and a shower. This suite has a tea and coffee maker, flat-screen TV, pool views," +
                            " as well as wine/champagne for guests. The unit has 2 beds.",

                            Price = 103500M,
                            Discount = 13.75M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170526/yard/Abuja%20Hotels/Bon%20Hotel/RoomTypes/Suite_x1q75u.jpg"
                        },
                        new RoomType
                        {
                            Name = "Loft room",

                            Description = "The spacious suite offers air conditioning, soundproof walls, as well as a private " +
                            "bathroom boasting a walk-in shower and a bath. This suite has a tea and coffee maker, flat-screen TV," +
                            " pool views, as well as wine/champagne for guests. The unit has 2 beds.",

                            Price = 115000M,
                            Discount = 17.86M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705170526/yard/Abuja%20Hotels/Bon%20Hotel/RoomTypes/Suite_x1q75u.jpg"
                        }
                    },
                    IsDelisted = false
                },

                 //LAGOS Hotels
                new Hotel
                {
                    Name = "Hotel Continental",

                    Description = "The Lagos Continental Hotel is a 5-Star hotel lying in the popular high-brow area of Lagos," +
                    " at Plot 52, Kofo Abayomi Street, Victoria Island, Lagos, Nigeria. It is the tallest hospitality building" +
                    " in Nigeria, and with its terraces, it offers splendid views of the Lagos city, most importantly the Lagos" +
                    " Port and Lagos skyline. It is a 30-minute drive away from Ikoyi Club; with its proximity to Silverbird " +
                    "Cinemas and Embassies also, it has the right location for guests to enjoy their stay.\n\n\nThe 23-storey" +
                    " houses 358 exquisite rooms and suites categorised into: King Superior, The Lagos Continental Club, Club" +
                    " King, One Bedroom Ambassador Suite, and One Bedroom Deluxe Suite. Rooms in The Lagos Continental Hotel " +
                    "are luxurious and exquisitely furnished with king-size beds, fine tapestries from around the world, " +
                    "air -conditioners, ornate lampshade, DVD-player, a refrigerator, a flat-screen TV with satellite reception," +
                    " a work desk and chair, a wardrobe and a balcony that offers a breath-taking view of the surrounding. The" +
                    " rooms also come with a bathtub, a private bathroom with a hairdryer and a bathrobe.\n\n\nGuests can enjoy" +
                    " the free Wi-Fi access and the swimming sessions at The Lagos Continental Hotel outdoor swimming pool, and" +
                    " workout session at the ultramodern fitness centre. It also houses a spa that offers a variety of services" +
                    " ranging from facials, hairstyling, and general body grooming. The Lagos Continental Hotel provides families" +
                    " with free cribs/infant beds, free rollaway/extra beds and a connecting/adjoining rooms which ensure that " +
                    "family members are not separated. It provides a 24-hour front desk service and has 4 restaurants that serve" +
                    " local and continental meals. Breakfast is available but at a surcharge. Milano Restaurant offers a fine" +
                    " Italian experience. The Ekaabo restaurant serves a buffet of Nigerian, Italian, Chinese and Indian " +
                    "specialities. A variety of alcoholic and non-alcoholic drinks are available at the Ariya Terrace and " +
                    "Milano Bar. Guests can also enjoy the 24-hour room service.\n\n\nThe hotel has storage rooms, electronic" +
                    " room keys, in-room safe, safety deposit boxes and trained security personnel guard to ensure that all " +
                    "guests and luggage are safe. With 8 meeting rooms for conferences, meetings, AGMs and a business centre," +
                    " The Lagos Continental Hotel caters for the corporate and commercial needs of guests. In addition to all" +
                    " the facilities above, The Lagos Continental Hotel offers: dry cleaning/laundry services, concierge services," +
                    " free newspapers in the lobby, multilingual staff, wedding services, and a porter. Airport shuttle and car" +
                    " hire services are also provided on request. On-site near parking is available within the hotel premises." +
                    "\n\nThe Lagos Continental Hotel (Formerly Intercontinental Lagos Hotel) is a luxury hotel in Victoria " +
                    "Island, Lagos.\n\n\nTerms and Conditions.\nCheck in: From 2:00 pm with ID card\nCheck out: By 12:00pmPayment:" +
                    " Cash or card\nPets are not allowed\nChildren below age 13 years can stay free.\nCancellation: Cancellation" +
                    " 2 days before arrival is free, but 100 % will be charged if guests cancel less than 2 days before arrival." +
                    "\n\n\nInteresting Places to Visit near The Lagos Continental Hotel:\nSilverbird Galleria\nBar Beach\nLekki" +
                    " Conservation Centre\nMUSON centre\nLagos City Mall\nNigerian National Museum\nNike Art Gallery\nNational " +
                    "Art Theatre",

                    Email = "Reservation@hotelcontinental.com",
                    Phone = "+234 1 236 6666",
                    Address = new Address
                    {
                        City = "Lagos", State = "Lagos", Country = "Nigeria", PostalCode = "10235",
                        Street = "Plot 52A Kofo Abayomi Street, Victoria Island"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332550/yard/Lagos%20Hotels/Hotel%20Continental/Hotel_Continental_Lagos_ffzjcw.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332536/yard/Lagos%20Hotels/Hotel%20Continental/Hotel_Continental_Image4_xreivr.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332536/yard/Lagos%20Hotels/Hotel%20Continental/Hotel_Continental_Image4_xreivr.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332532/yard/Lagos%20Hotels/Hotel%20Continental/Hotel_Continental_Image1_wtcjyj.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332545/yard/Lagos%20Hotels/Hotel%20Continental/Hotel_Continental_Image9_smnm5o.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Superior Room",

                            Description = "Guests will have a special experience as this double room features a pool with a " +
                            "view. Offering free toiletries and bathrobes, this double room includes a private bathroom with " +
                            "a walk-in shower, a bath and a hairdryer.\nThe spacious air-conditioned double room offers a " +
                            "flat-screen TV with cable channels, a private entrance, a mini-bar, a seating area as well as " +
                            "sea views. The unit offers 1 bed.",

                            Price = 281800M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332585/yard/Lagos%20Hotels/Hotel%20Continental/RoomTypes/Superior_Room_glaphs.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Room",

                            Description = "Guests will have a special experience as the suite features a pool with a view." +
                            " The spacious air-conditioned suite provides a flat-screen TV with cable channels, a private " +
                            "entrance, a mini-bar, a tea and coffee maker as well as sea views. The unit offers 1 bed.",

                            Price = 307750M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332583/yard/Lagos%20Hotels/Hotel%20Continental/RoomTypes/Delixe_Room_fmownm.jpg"
                        },
                        new RoomType
                        {
                            Name = "Club Room",

                            Description = "This double room's special feature is the pool with a view. Providing free toiletries" +
                            " and bathrobes, this double room includes a private bathroom with a walk-in shower, a bath and a" +
                            " hairdryer. The spacious air-conditioned double room provides a flat-screen TV with cable channels," +
                            " a private entrance, a mini-bar, a tea and coffee maker as well as sea views. The unit offers 1 bed.",

                            Price = 336600M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332582/yard/Lagos%20Hotels/Hotel%20Continental/RoomTypes/Club_Room_jaqfy3.jpg"
                        },
                        new RoomType
                        {
                            Name = "Ambassador Suite",

                            Description = "The pool with a view is a top feature of this suite. The spacious air-conditioned" +
                            " suite offers a flat-screen TV with cable channels, a private entrance, a mini-bar, a tea and " +
                            "coffee maker as well as sea views. The unit has 1 bed.",

                            Price = 337700M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705332580/yard/Lagos%20Hotels/Hotel%20Continental/RoomTypes/Ambassador_Suite_kvrmqz.jpg"
                        }
                    },
                    IsDelisted = false,
                    Popular = true
                },


                new Hotel
                {
                    Name = "Radisson Lagos",

                    Description = "Radisson (Formerly Protea Hotel Ikeja) is an elegant 4-Star hotel located in the calm and" +
                    " secure residential area of 42/44 Isaac John Street, GRA, Ikeja. It is the place to be for excellent " +
                    "customer service and a wonderful shopping experience. It is arranged over three floors, making it a " +
                    "visible structure around its surroundings.\n\n\nRadisson (Formerly Protea Hotel Ikeja) features 92 rooms" +
                    " that are exquisitely fitted with LCD TVs, high-speed internet connection, air conditioners, comfortable" +
                    " beds and sofas, en-suite bathrooms, minibars, worktables and chairs, refrigerators, in-room safes, and " +
                    "coffee making facilities. The rooms are classified into: Standard Room, Deluxe, Junior Suite, Presidential" +
                    " Suite and Executive Suite.\n\n\nRadisson (Formerly Protea Hotel Ikeja) offers a variety of local, African," +
                    " and continental dishes which are served at the restaurant, as well as a bar that houses an array of " +
                    "assorted alcoholic and non-alcoholic drinks. There is a common area where guests can have coffee or tea" +
                    " as well as a buffet service. It owns an outdoor pool where guests can relax with cocktails ordered from" +
                    " the poolside bar. Guests can also undergo fitness exercises at the fully equipped gymnasium, and get a " +
                    "nerve calming massage at the body spa. Four flexible event facilities with complimentary Wi-Fi are available" +
                    " for guests depending on the occasion, whether it is a business or a social gathering.\n\n\nThere are a " +
                    "number of special services available on request for guests. These services include: Free childcare and " +
                    "babysitting (free cribs and rollaway) for guests with kids, a 24-hour health club, airport shuffle, tour" +
                    " and ticket assistance, free onsite parking facilities, a 24/7 business centre, car hire, laundry, and " +
                    "dry-cleaning services. The front desk is accessible to guests on a round-the-clock basis.\n\nRadisson " +
                    "(Formerly Protea Hotel Ikeja) is a luxury hotel in Ikeja, Lagos.\n\n\nTerms and Conditions\nCheck-in: " +
                    "from 2:00 PM (Government –issued ID is required)\nCheck-out: by 11:00AM\nCancellation: Cancellation " +
                    "policies vary according to room type.\nChildren: Children under the age of 12 are allowed to stay free." +
                    "\nPets: Pets are not allowed\nPayment: Cash, Visa, and MasterCard are acceptable forms of payment at the" +
                    " point of arrival.\n\n\nInteresting Places To Visit near Radisson (Formerly Protea Hotel Ikeja)\nIkeja " +
                    "Shopping Mall\nIkeja\nGolf Club\nComputer Village\nLagos State Government Secretariat\nProtea Hotel is " +
                    "4km from Murtala Muhammed International Airport. ",

                    Email = "ikeja@radisson.com",
                    Phone = "+234 1 448 2000",

                    Address = new Address
                    {
                        City = "Lagos", State = "Lagos", Country = "Nigeria", PostalCode = "11111",
                        Street = "42/44 Isaac John Street, GRA, Ikeja, Lagos"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173474/yard/Lagos%20Hotels/Radisson%20Hotel/Radisson_Lagos_Ikeja_epqnsk.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173479/yard/Lagos%20Hotels/Radisson%20Hotel/Radisson_Lagos_Image2_iui1xp.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173477/yard/Lagos%20Hotels/Radisson%20Hotel/Radisson_Lagos_Image1_j5xevk.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173483/yard/Lagos%20Hotels/Radisson%20Hotel/Radisson_Lagos_Image4_vipdkk.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173493/yard/Lagos%20Hotels/Radisson%20Hotel/Radisson_Lagos_Image9_hkkdja.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Room",

                            Description = "Room size 21 m², 1 extra-large double bed, Comfy beds, 8.6 – Based on 62 reviews," +
                            " Providing free toiletries, this double room includes a private bathroom with a bath, a shower and" +
                            " a hairdryer. The air-conditioned double room provides a flat-screen TV with satellite channels, a" +
                            " private entrance, a tea and coffee maker, a seating area as well as city views. The unit offers 1 bed.",

                            Price = 198609M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173574/yard/Lagos%20Hotels/Radisson%20Hotel/RoomTypes/Standard_Room_cvetzb.jpg"
                        },
                        new RoomType
                        {
                            Name = "Premium Room",

                            Description = "Room size 37 m², 1 extra-large double bed, Comfy beds, 8.6 – Based on 62 reviews, " +
                            "Offering free toiletries, this double room includes a private bathroom with a bath, a shower and a " +
                            "hairdryer. The spacious air-conditioned double room offers a flat-screen TV with satellite channels," +
                            " a private entrance, a tea and coffee maker, a seating area as well as pool views. The unit offers" +
                            " 1 bed.",

                            Price = 271397M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173572/yard/Lagos%20Hotels/Radisson%20Hotel/RoomTypes/Premium_Room_guri9u.jpg"
                        },
                        new RoomType
                        {
                            Name = "Junior Suite ",

                            Description = "Room size 40 m², 1 extra-large double bed Comfy beds, 8.6 – Based on 62 reviews, " +
                            "Boasting a private entrance, this air-conditioned suite includes 1 bedroom and 1 bathroom with a" +
                            " bath and a shower. This suite has a tea and coffee maker, flat-screen TV with satellite channels," +
                            " pool views, as well as wine/champagne for guests. The unit has 1 bed.",

                            Price = 405657M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173570/yard/Lagos%20Hotels/Radisson%20Hotel/RoomTypes/Junior_Suite_yqvlua.jpg"
                        },
                        new RoomType
                        {
                            Name = "Suite with Pool View",

                            Description = "Room size 70 m², 1 extra-large double bed Comfy beds, 8.6 – Based on 62 reviews. " +
                            "Featuring a private entrance, this spacious suite also comprises 1 bedroom, a seating area and " +
                            "1 bathroom with a bath and a shower. This suite features air conditioning, flat-screen TV with " +
                            "satellite channels, pool views, as well as wine/champagne for guests. The unit offers 1 bed..",

                            Price = 486213M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173576/yard/Lagos%20Hotels/Radisson%20Hotel/RoomTypes/Suite_with_Pool_View_nyhsfd.jpg"
                        }
                    },
                    IsDelisted = false,
                    Popular = true
                },

                new Hotel
                {
                    Name = "Sigma Base VI",

                    Description = "Attractively located in Lagos, Sigma Base Hotel is a sustainable hotel, which features " +
                    "air-conditioned rooms with free WiFi and free private parking. The hotel provides a spa experience, with " +
                    "its spa facilities, wellness packages and beauty services. Boasting family rooms, this property also " +
                    "provides guests with a year-round outdoor pool.\n\n\nAt the hotel, each unit has a wardrobe, a flat-screen" +
                    " TV, a private bathroom, bed linen and towels. A microwave, a fridge and stovetop are also available, as " +
                    "well as a kettle. All units will provide guests with kitchenware.\n\nAt the hotel, the restaurant is open " +
                    "for dinner, lunch and brunch and specialises in African cuisine.\n\nGuests can relax in the garden at the" +
                    " property.\n\n\nLandmark Beach is 1.9 km from Sigma Base Hotel, while Red Door Gallery is 3.4 km from the" +
                    " property. The nearest airport is Murtala Muhammed International Airport, 25 km from the accommodation." +
                    "\n\n\nCouples particularly like the location — they rated it 9.1 for a two-person trip.\n\nDistance in " +
                    "property description is calculated using © OpenStreetMap\n\nMost popular facilities\nOutdoor swimming " +
                    ".pool\nSpa and wellness centre\nNon-smoking rooms\nRestaurant\nRoom service\nFree WiFi\nFree parking." +
                    "\nFamily rooms\nBar\nProperty highlights\nTop location: Highly rated by recent guests(8.7)\nApartments " +
                    "with:\nTerrace\nFree private parking available on-site\nTravel Sustainable Level 1\n\nThis property has" +
                    " taken 5 out of total 29 sustainability steps to make your stay more sustainable.",

                    Email = "infosigmabase.com",
                    Phone = "07027379355",
                    Address = new Address
                    {
                        City = "Lagos", State = "Lagos", Country = "Nigeria", PostalCode = "106104",
                        Street = "7/9 Molade Okoya Thomas Street, Victoria Island"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173912/yard/Lagos%20Hotels/Sigma%20Base%20VI/Sigma_Base_VictoriaIsland_m7e7du.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173928/yard/Lagos%20Hotels/Sigma%20Base%20VI/Sigma_Base_VictoriaIsland_Image6_gab3ke.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173931/yard/Lagos%20Hotels/Sigma%20Base%20VI/Sigma_Base_VictoriaIsland_Image7_ug7o0t.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173918/yard/Lagos%20Hotels/Sigma%20Base%20VI/Sigma_Base_VictoriaIsland_Image2_loalbg.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173926/yard/Lagos%20Hotels/Sigma%20Base%20VI/Sigma_Base_VictoriaIsland_Image5_v5y6z6.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Single Room",

                            Description = "Apartment size: 25 m², 1 large double bed Comfy beds, 7.7 – Based on 14 reviews, The" +
                            " air-conditioned apartment has 1 bedroom and 1 bathroom with a bath and a shower. Guests can make " +
                            "meals in the kitchenette that features a stovetop, a refrigerator, kitchenware and a microwave. The" +
                            " hotel features a washing machine, a dining area, a wardrobe, a flat-screen TV, as well as city " +
                            "views. The unit has 1 bed.",

                            Price = 123345M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173993/yard/Lagos%20Hotels/Sigma%20Base%20VI/RoomTypes/Single_Room_pjebho.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Room",

                            Description = "Apartment size: 29 m², Bedroom 1: 1 large double bed Living room: 1 sofa bed Comfy " +
                            "beds, 7.7 – Based on 14 reviewsThis air-conditioned apartment consists of 1 living room, 1 separate" +
                            " bedroom and 1 bathroom with a shower. The fully equipped kitchenette features a stovetop, " +
                            "a refrigerator, kitchenware and a microwave. The apartment provides a flat-screen TV, a washing " +
                            "machine, a dining area, a wardrobe as well as city views. The unit offers 2 beds.",

                            Price = 145677M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173988/yard/Lagos%20Hotels/Sigma%20Base%20VI/RoomTypes/Deluxe_Room_mjo0v8.jpg"
                        },
                        new RoomType
                        {
                            Name = "Double-Bedroom",

                            Description = "Apartment size: 35 m², Bedroom 1: 1 large double bed Bedroom 2: 1 large double bed " +
                            "Comfy beds, 7.7 – Based on 14 reviews, Featuring 2 bedrooms and 1 bathroom with a shower, this " +
                            "apartment offers a well-fitted kitchen and a terrace. In the kitchen, guests will find a stovetop," +
                            " a refrigerator, kitchenware and a microwave. The air-conditioned apartment provides a flat-screen" +
                            " TV, a washing machine, a dining area, a wardrobe as well as city views. The unit has 2 beds.",

                            Price = 188435M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173991/yard/Lagos%20Hotels/Sigma%20Base%20VI/RoomTypes/Double_Room_xdivks.jpg"
                        }          
                    },

                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Delight Hotel",

                    Description = "You're eligible for a Genius discount at Delight Apartments! To save at this property, all you" +
                    " have to do is sign in.\n\n\nFeaturing an outdoor swimming pool and views of pool, Delight Apartments is a" +
                    " sustainable apartment set in Lagos, 11 km from Synagogue Church Of all Nations. This property offers access" +
                    " to a balcony, free private parking and free WiFi. The accommodation offers a 24-hour front desk and full-day" +
                    " security for guests.\n\n\nThe units in the apartment complex are equipped with air conditioning, a seating " +
                    "area, a flat-screen TV with streaming services, a kitchen, a dining area and a private bathroom with free" +
                    " toiletries and a walk-in shower. A microwave, a fridge and stovetop are also featured, as well as a kettle." +
                    " At the apartment complex, the units have bed linen and towels.\n\n\nKalakuta Museum is 13 km from the " +
                    "apartment, while National Stadium Lagos is 22 km away. The nearest airport is Murtala Muhammed International," +
                    " 8 km from Delight Apartments, and the property offers a paid airport shuttle service.",

                    Email = "info@delight.com",
                    Phone = "+234 701 485 9495",
                    Address = new Address
                    {
                        City = "Lagos", State = "Lagos", Country = "Nigeria", PostalCode = "10231",
                        Street = "103B, Aiguobasimwin Crescent,off Ikpopan Road, GRA."
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173692/yard/Lagos%20Hotels/Delight%20Hotel/Delight_Hotel_Lagos_whsh0u.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173669/yard/Lagos%20Hotels/Delight%20Hotel/Delight_Hotel_Image1_pevpwj.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173670/yard/Lagos%20Hotels/Delight%20Hotel/Delight_Hotel_Image2_le29q7.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173675/yard/Lagos%20Hotels/Delight%20Hotel/Delight_Hotel_Image4_bmrcmr.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173689/yard/Lagos%20Hotels/Delight%20Hotel/Delight_Hotel_Image10_ougarg.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Single Room Apartment",

                            Description = "Apartment size: 93 m², 1 extra-large double bed Comfy beds, 9.7 – Based on 9 reviews." +
                            " This spacious apartment features 1 living room, 1 separate bedroom and 1 bathroom with a walk-in " +
                            "shower and free toiletries. In the kitchen, guests will find a stovetop, a refrigerator, kitchenware" +
                            " and a microwave. This air-conditioned apartment includes a dining area, a flat-screen TV with " +
                            "streaming services a washing machine and a balcony. The unit has 1 bed.",

                            Price = 121765M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173718/yard/Lagos%20Hotels/Delight%20Hotel/RoomTypes/Single_Room_r3uajs.jpg"
                        },
                        new RoomType
                        {
                            Name = "Double Room Apartment",

                            Description = "Apartment size: 107 m², Bedroom 1: 1 extra-large double bed Bedroom 2: 1 extra -" +
                            " large double bed Comfy beds, 9.7 – Based on 9 reviews Bathrooms: 2.This spacious apartment is" +
                            " comprised of 1 living room, 2 separate bedrooms and 2 bathrooms with a walk-in shower and free" +
                            " toiletries. In the fully equipped kitchen, guests will find a stovetop, a refrigerator, " +
                            "kitchenware and a microwave.This air-conditioned apartment features a dining area, a flat-screen " +
                            "TV with streaming services a washing machine and a balcony. The unit offers 2 beds.",

                            Price = 155645M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173713/yard/Lagos%20Hotels/Delight%20Hotel/RoomTypes/Double_Room_jbogdh.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Room",

                            Description = "This spacious apartment features 1 living room, 2 separate bedrooms and 2 bathrooms " +
                            "with a walk-in shower and free toiletries. In the well-equipped kitchen, guests will find a stovetop," +
                            " a refrigerator, kitchenware and a microwave. The air-conditioned apartment features a flat-screen " +
                            "TV with streaming services, a washing machine, a private entrance, a seating area as well as pool " +
                            "views. The unit has 2 beds.",

                            Price = 175855M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173715/yard/Lagos%20Hotels/Delight%20Hotel/RoomTypes/Executive_Room_eedzcv.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "ParkView Ikoyi",

                    Description = "You're eligible for a Genius discount at George Residence, Parkview Ikoyi! To save at this" +
                    " property, all you have to do is sign in.\n\n\nSituated just 3.7 km from Ikoyi Golf Course, George Residence," +
                    " Parkview Ikoyi provides accommodation in Lagos with access to a garden, a terrace, as well as a 24-hour " +
                    "front desk. It is located 5.9 km from Nike Art Gallery and offers a lift. The aparthotel also features free" +
                    " WiFi, free private parking and facilities for disabled guests.\n\n\nAt the aparthotel, units have air " +
                    "conditioning, a seating area, a flat-screen TV with streaming services, a safety deposit box and a private" +
                    " bathroom with a walk-in shower, bathrobes and slippers. Units come with a kettle, while some rooms also " +
                    "boast a fully equipped kitchen with an oven and a stovetop. At the aparthotel, the units come with bed linen" +
                    " and towels.\n\nGuests are welcome to wind down in the in-house bar or lounge.\n\nSightseeing tours are " +
                    "available in the vicinity of the property. An indoor play area is also available for guests at the aparthotel." +
                    "\n\n\nRed Door Gallery is 6 km from George Residence, Parkview Ikoyi, while National Museum Lagos is 6.9 km" +
                    " from the property. The nearest airport is Murtala Muhammed International, 23 km from the accommodation, and" +
                    " the property offers a paid airport shuttle service.\n\nDistance in property description is calculated using" +
                    " © OpenStreetMap.\n\n\nMost popular facilities\nAirport shuttle\nNon-smoking rooms\nRoom service\nFree " +
                    "WiFi\nFacilities for disabled guests\nFree parking\nFamily rooms\nBar\nBreakfast",

                    Email = "info (at) pvraikoyi.com",
                    Phone = "+234-809-679-9955",
                    Address = new Address
                    {
                        City = "Lagos", State = "Lagos", Country = "Nigeria", PostalCode = "106104", 
                        Street = "6 Olufemi Pedro Street Parkview, Ikoyi"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173818/yard/Lagos%20Hotels/ParkView%20Ikoyi/ParkView_Ikoyi_kzwyzs.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173799/yard/Lagos%20Hotels/ParkView%20Ikoyi/ParkView_Ikoyi_Image1_vtlkz6.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173809/yard/Lagos%20Hotels/ParkView%20Ikoyi/ParkView_Ikoyi_Image5_cytz1u.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173806/yard/Lagos%20Hotels/ParkView%20Ikoyi/ParkView_Ikoyi_Image4_y4k34q.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173811/yard/Lagos%20Hotels/ParkView%20Ikoyi/ParkView_Ikoyi_Image6_psj6b8.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "King Room",

                            Description = "Size 48 m², 1 large double bed, Offering free toiletries and bathrobes, this double" +
                            " room includes a private bathroom with a walk-in shower, a bath and a bidet. The spacious " +
                            "air-conditioned double room offers a flat-screen TV with streaming services, a seating area, a " +
                            "wardrobe and a safe deposit box. The unit offers 1 bed.",

                            Price = 141932M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173852/yard/Lagos%20Hotels/ParkView%20Ikoyi/RoomTypes/King_Room_a35tpk.jpg"
                        },
                        new RoomType
                        {
                            Name = "King Room with Balcony",

                            Description = "Size 50 m², 1 large double bed, The spacious double room provides air conditioning," +
                            " a seating area, a balcony with city views as well as a private bathroom featuring a walk-in shower." +
                            " The unit offers 1 bed.",

                            Price = 161112M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173849/yard/Lagos%20Hotels/ParkView%20Ikoyi/RoomTypes/King_Room_with_Balcony_bz0afp.jpg"
                        },
                        new RoomType
                        {
                            Name = "One-Bedroom Apartment",

                            Description = "Apartment size: 108 m², Bedroom 1: 1 large double bed, Living room: 1 sofa bed " +
                            "Featuring a private entrance, this air-conditioned apartment consists of 2 living rooms, 1 separate" +
                            " bedroom and 1 bathroom with a walk-in shower and a bath. In the fully equipped kitchen, guests will" +
                            " find a stovetop, a refrigerator, kitchenware and an oven. The spacious apartment provides a washing" +
                            " machine, a seating area, a dining area, a wardrobe and a flat-screen TV with streaming services." +
                            " The unit offers 2 beds.",

                            Price = 191800M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173854/yard/Lagos%20Hotels/ParkView%20Ikoyi/RoomTypes/One-Bedroom_Apartment_jgtflq.jpg"
                        },
                        new RoomType
                        {
                            Name = "Penthouse Apartment",

                            Description = "Apartment size: 220 m², Bedroom 1: 1 extra-large double bed, Bedroom 2: 1 large double" +
                            " bed, Bathrooms: 2. Boasting a private entrance, this air-conditioned apartment includes 1 living " +
                            "room, 2 separate bedrooms and 2 bathrooms with a walk-in shower and a bath. In the well-fitted kitchen," +
                            " guests will find a stovetop, a refrigerator, kitchenware and an oven. The spacious apartment offers " +
                            "a flat-screen TV with streaming services, a washing machine, a seating area, a dining area as well " +
                            "as city views. The unit has 2 beds.",

                            Price = 326060M,
                            Discount = 20M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705173857/yard/Lagos%20Hotels/ParkView%20Ikoyi/RoomTypes/Penthouse_Apartment_mmbr94.jpg"
                        }
                    },
                    IsDelisted = false
                },

                 //KANO Hotels
                new Hotel
                {
                    Name = "Prince Hotel",

                    Description = "Prince Hotel is a 3-Star hotel situated at Tamandu Road, off Audu Bako way, Nassarawa GRA, " +
                    "Kano. It boasts a lovely architectural design, exotic greenery and a serene location.\n\n\nPrince Hotel has" +
                    " a total of 295 rooms that come in different room categories like the Standard, Executive, Villa, Double " +
                    "bed Villa, VIP Suite, Royal Suite, Senate Suite, Ambassador Suite, Imperial Suite and the Presidential Suite." +
                    " These spacious rooms come fully equipped with state-of-the-art facilities which include flat-screen " +
                    "television sets, sofas and centre tables, air conditioners, king-size beds, wireless internet access, " +
                    "private bathrooms with shower facilities, telephones, and work desks and tables.\n\n\nA collection of high " +
                    "grade hotel facilities like a restaurant with a menu of diverse tasty dishes plus complimentary breakfast" +
                    " service, a bar/lounge, adequate security, stable power supply, surplus car parking space, an outdoor pool," +
                    " in-house dining facilities, room service, plus laundry and dry cleaning services are available to guests " +
                    "at the Prince Hotel.\n\n\nTerms and Conditions\nCheck in: from 1:00pm\nCheck out: by 12:00pm\nFree " +
                    "cancellation\nNo smoking in rooms.\nPets: not allowed\n\nPrince Hotel is a top-class hotel in Kano, Kano." +
                    "\n\n\nPlaces Of Interest Near Prince Hotel\nSchool of Technology, Kano state Polytechnic\nTajmahal " +
                    "Restaurant\nSani Abacha Sports Stadium\nHalal meat Resto Cafe\nAl-furqan Charitable Foundation\nJalsa\nPrime" +
                    " College.",

                    Email = "info@princehotelng.com",
                    Phone = "+234 (0) 8033773923",
                    Address = new Address
                    {
                        City = "Kano", State = "Kano", Country = "Nigeria", PostalCode = "10235",
                        Street = "13/D Bompai G.R.A, close to doctors clinic, Kano"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172716/yard/Kano%20Hotels/Prince%20Hotel/prince_hotel_Landing_page_1_t4fynz.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172721/yard/Kano%20Hotels/Prince%20Hotel/Prince_hotel_landing_page2_vzidtd.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172720/yard/Kano%20Hotels/Prince%20Hotel/Prince_hotel_landing_page_5_ctze6s.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172718/yard/Kano%20Hotels/Prince%20Hotel/Prince_Hotel_Landing_page_4_tpkqc1.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172717/yard/Kano%20Hotels/Prince%20Hotel/prince-hotel_landing_page_3_i7vv6w.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Room",

                            Description = "The Standard Room comprises of a Double Bed, 2 Bedside Tables, a Desk & Chair. The room" +
                            " is furnished with modern furnishings. Our ultramodern glass bathroom is equipped with magnifying" +
                            " shaving and make up mirror as well as all the amenities you could possible need during your stay.",

                            Price = 48600M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172767/yard/Kano%20Hotels/Prince%20Hotel/RoomTypes/Prince_Hotel_Standard_Room_lkg9xr.jpg"
                        },
                        new RoomType
                        {
                            Name = "VIP Room",

                            Description = "A spacious air conditioned room consist of sleeping area with king size bed with" +
                            " comfortable sofa. This luxuriously decorated room features a flat-screen satellite TV, coffee table " +
                            "and a laptop, a digitalised safe, a dressing table, a bedside table, a small writing table, and a small" +
                            " fridge. The bathroom with shower or a bath tub.",

                            Price = 97200M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172768/yard/Kano%20Hotels/Prince%20Hotel/RoomTypes/Prince_Hotel_VIP_Room_tc8w7y.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Suite",

                            Description = "Our wonderfully appointed Executive flat offers a separate living space with a private" +
                            " dining table, and a full guestroom with a king bed featuring a legendary Vi-spring mattress for the" +
                            " ultimate sleep experience.",

                            Price = 60750M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172764/yard/Kano%20Hotels/Prince%20Hotel/RoomTypes/Prince_Hotel_Executive_Room_wklbsf.jpg"
                        },
                        new RoomType
                        {
                            Name = "King Size Room",

                            Description = "Presidential Suite can accommodate up to 4 persons, consists of 2 bedrooms’ king sized" +
                            " beds and a living room fully furnished, coffee and dining tables as well as a fully equipped kitchen" +
                            " to make you feel at home. The living and bedroom areas are exceptionally decorated providing the " +
                            "guest with a cozy, friendly and relaxing ambience. All Presidential Flats have a furnished balcony " +
                            "or a terrace with view, which are shaded by traditional pergolas.",

                            Price = 204500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172765/yard/Kano%20Hotels/Prince%20Hotel/RoomTypes/Prince_Hotel_King_Size_Room_jlonzv.jpg"
                        },
                        new RoomType
                        {
                            Name = "Royal Room",

                            Description = "The Ambassador Suite are equipped with a living room, dining table, 2 Twin Beds, a " +
                            "guest bathroom, a Desk & Chair coffee table with shower or bathtub and some of them with balcony " +
                            "overlooking hotel garden. Each room is air conditioned and has a flat-screen TV with international" +
                            " TV channels, and a spacious wardrobe.",

                            Price = 182250M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172762/yard/Kano%20Hotels/Prince%20Hotel/RoomTypes/Prince_Hotel_Ambassador_Room_jyuwew.jpg"
                        }
                    },
                    IsDelisted = false
                },


                new Hotel
                {
                    Name = "Tahir Guest Palace",

                    Description = "Tahir Guest Palace is a 3-Star hotel located in the business capital of Northern Nigeria." +
                    " It is situated on 4, Ibrahim Natsugune road, Kano, Nigeria. It is a 19 minutes’ drive from Mallam Aminu" +
                    " Kano International Airport.\n\n\nTahir Guest Palace boasts tastefully furnished rooms that present a " +
                    "luxurious lifestyle. It has a total of over 300 rooms. Each room is broadly categorised into the following:" +
                    " Deluxe Room, Royal Suite, VIP Suite, Chalet, Presidential Suite, Executive Royal Suite and Executive V.I.P" +
                    " Suite. Rooms in each category offer the following amenities: an intercom, a refrigerator, a bathroom, an " +
                    "air conditioning, a cable TV, toiletries, a fridge, complimentary free coffee items, water supply, a " +
                    "king-size bed, a bedside table, a centre table and a lamp.\n\n\nA number of facilities are available at " +
                    "Tahir Guest Palace including a restaurant where guests can get world class cuisines and buffets, free Wi-Fi" +
                    " connection, a bar/lounge for relaxing after a busy day, a conferencing facility where you can have your" +
                    " business meetings. It also offers a swimming pool for guests' use and a gym for those who do workout " +
                    "sessions.\n\n\nTahir Guest Palace offers complimentary services such as laundry/dry cleaning, room service," +
                    " a round-the-clock CCTV surveillance, a 24-hour electricity and security and on-site parking spaces. Car" +
                    " hire services are also available on request.\n\nTahir Guest Palace is a top-class hotel in Nasarawa GRA," +
                    " Kano.\n\n\nTerms and Conditions about Tahir Guest Palace\nCheck in: From 9:00 am.\nCheck out: By 12:00 pm." +
                    "\nChildren: Only kids below 18 can stay for free.\nPets: Pets are not allowed.\nPayment: Visa, MasterCard." +
                    "\n\n\nInteresting Places to Visit near Tahir Guest Palace\nAminu Kano Teaching Hospital (24 minutes’ drive)" +
                    "\nKano Nigerian Railway (12 minutes’ drive)\nKano Golf Club (5 minutes’ drive)\nPizza Hut (2 minutes’ drive)" +
                    "\nBayero University (26 minutes’ drive)\nWell Care Pharmacy (4 minutes’ drive)\nBukavu Army Barracks (23" +
                    " minutes’ drive)\nCilantro restaurant & lounge (3 minutes’ drive).",

                    Email = "reservation@tahirguestpalace.com",
                    Phone = "+234 803 671 7900",

                    Address = new Address
                    {
                        City = "Kano", State = "Kano", Country = "Nigeria", PostalCode = "11111",
                        Street = "No 33 Gashash Road, Along race course road"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172804/yard/Kano%20Hotels/Tahir%20Guest%20Palace/tahir-guest-palace-Landing_page_cwgwha.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172800/yard/Kano%20Hotels/Tahir%20Guest%20Palace/tahir-guest-palace-Landing_page_2_yab070.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172801/yard/Kano%20Hotels/Tahir%20Guest%20Palace/tahir-guest-palace-Landing_page_4_gd6hbu.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Deluxe Room",

                            Description = "Senator Suite Features a wider space, Living Room “Parlour”, 2 LED TV, Small Bar Counter," +
                            " and all the goodies of Superior and Executive suite.",

                            Price = 32000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172841/yard/Kano%20Hotels/Tahir%20Guest%20Palace/RoomTypes/Deluxe_Room_tik0jy.jpg"
                        },
                        new RoomType
                        {
                            Name = "Super Deluxe Room",

                            Description = "Extra-large superior suite room with twin bed. This luxuriously decorated room features" +
                            " a flat-screen satellite TV, coffee table and a laptop, a digitalised safe, a dressing table, a bedside" +
                            " table, a small writing table, and a small fridge. The bathroom with shower or a bath tub.",

                            Price = 40000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172846/yard/Kano%20Hotels/Tahir%20Guest%20Palace/RoomTypes/Super_Deluxe_Room_dfhy1e.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Royal Room",

                            Description = "Executive Suite features a wider space, with Free wifi, Air-conditioner, 1 LED TV with" +
                            " Game facilities, Fridge, Kettle, Hairdryer.",

                            Price = 50000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172843/yard/Kano%20Hotels/Tahir%20Guest%20Palace/RoomTypes/Executive_Room_tzds9q.jpg"
                        },
                        new RoomType
                        {
                            Name = "Chaler Room",

                            Description = "Superior Suite is your best choice for short-long business stay. Your Suite comes with" +
                            " free stationery, shoes polish, and minibar. The living and bedroom areas are exceptionally decorated" +
                            " providing the guest with a cozy, friendly and relaxing ambience.",

                            Price = 82000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172839/yard/Kano%20Hotels/Tahir%20Guest%20Palace/RoomTypes/Chalet_Room_qvoneh.jpg"
                        },
                        new RoomType
                        {
                            Name = "Presidential Room",

                            Description = "The Ambassador Suite are equipped with a living room, dining table, a guest bathroom," +
                            " a Desk & Chair coffee table with shower or bathtub and some of them with balcony overlooking hotel" +
                            " garden. Each room is air conditioned and has a flat-screen TV with international TV channels, and a" +
                            " spacious wardrobe.",

                            Price = 164400M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172844/yard/Kano%20Hotels/Tahir%20Guest%20Palace/RoomTypes/Presidential_Room_pgetne.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Porto Golf Hotel",

                    Description = "Porto Golf Hotels is a spectacular luxurious hotel located in Kano. Porto Golf Hotels is a " +
                    "five stars elegant hotel, this makes our hotel the ideal place for unique moments of relaxation. Porto Golf" +
                    " Hotels is the meeting point of tradition and modern design. The buildings and surroundings of the hotel are" +
                    " those of Nigerian Traditions and culture: paved alleyways, as well as garden.The structure has an outdoor " +
                    "pool, a fully equipped gym, a swimming pool area, a gulf putting area, a restaurant, rest areas, free WIFI," +
                    " ping pong table, also free private parking.\n\n\nThe hotel is a secure oasis of peace and tranquility, a " +
                    "luxurious place to stay, efficiently backed by our dedication in personalised services. This modern day haven" +
                    " is located in Nassarawa local government Area approximately 2km from the Kano municipal Area and next to" +
                    " numerous local cafes, shops and taverns making it ideal for couples, families and nature lovers.\n\nTHE ROOMS" +
                    "\n\nAll our guest-rooms are spacious, comfortable, spotlessly clean and elegantly furnished with the best kind" +
                    " of furniture around the world. Our luxury facilities include complimentary amenities such as ; a flat screen" +
                    " LCD TV, free Wi-Fi, tea/coffee making facilities, and the finest pure white linen and towels.\n\nPorto golf" +
                    " hotels is a top-class hotel in GRA, Kano.\n\nTerms and Conditions about Porto golf hotels\nCheck In- 11:30 " +
                    "am (ID Required)\nCheck Out- 12:00 pm\n\n\nInteresting Places near Porto golf hotels\nEmir's Palace is 6 km " +
                    "from the hotel, while Ancient Kano City Gate and Wall is 6 km away.",

                    Email = "frontdesk@portogolfhotels.com",
                    Phone = "+2348037917049, +2348099959818",
                    Address = new Address
                    {
                        City = "Kano", State = "Kano", Country = "Nigeria", PostalCode = "10231", 
                        Street = "13/D Bompai G.R.A, close to doctors clinic, Kano"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172640/yard/Kano%20Hotels/Porto%20Golf%20Hotel/porto-golf-hotels-_Landing_Page2_s47ijj.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172639/yard/Kano%20Hotels/Porto%20Golf%20Hotel/porto-golf-hotels-_Landing_page1_mu5utw.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172642/yard/Kano%20Hotels/Porto%20Golf%20Hotel/porto-golf-hotels-_Landing_Page3_xrnar8.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172642/yard/Kano%20Hotels/Porto%20Golf%20Hotel/porto-golf-hotels-_Landing_Page4_qmu9vu.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172643/yard/Kano%20Hotels/Porto%20Golf%20Hotel/porto-golf-hotels-_Landing_Page5_hisloi.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Room",

                            Description = "The Standard Room comprises of a Double Bed, 2 Bedside Tables, a Desk & Chair. The room" +
                            " is furnished with modern furnishings. Our ultramodern glass bathroom is equipped with magnifying" +
                            " shaving and make up mirror as well as all the amenities you could possible need during your stay.",

                            Price = 41375M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172683/yard/Kano%20Hotels/Porto%20Golf%20Hotel/RoomTypes/porto-golf-hotel-standard_room_w7qngq.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Room",

                            Description = "Embrace a comfortable stay with the Gold Standard room. This entry-level option provides" +
                            " a solid foundation for a pleasant experience, featuring essential amenities and a cozy atmosphere for " +
                            "travelers seeking a straightforward and welcoming accommodation.",

                            Price = 46900M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172680/yard/Kano%20Hotels/Porto%20Golf%20Hotel/RoomTypes/porto-golf-hotels-_Deluxe_room_lonaf5.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Suite",

                            Description = "Our wonderfully appointed Executive flat offers a separate living space with a private" +
                            " dining table, and a full guestroom with a king bed featuring a legendary Vi-spring mattress for the" +
                            " ultimate sleep experience.",

                            Price = 64175M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172678/yard/Kano%20Hotels/Porto%20Golf%20Hotel/RoomTypes/porto-golf-hotel-_Luxry_room_t6vmmd.jpg"
                        },
                        new RoomType
                        {
                            Name = "Presidential Room",

                            Description = "Presidential Suite can accommodate up to 4 persons, consists of 2 bedrooms’ king sized" +
                            " beds and a living room fully furnished, coffee and dining tables as well as a fully equipped kitchen" +
                            " to make you feel at home. The living and bedroom areas are exceptionally decorated providing the " +
                            "guest with a cozy, friendly and relaxing ambience. All Presidential Flats have a furnished balcony or" +
                            " a terrace with view, which are shaded by traditional pergolas.",

                            Price = 204500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172681/yard/Kano%20Hotels/Porto%20Golf%20Hotel/RoomTypes/porto-golf-hotels-_Presidential_room_bwlbno.jpg"
                        },
                        new RoomType
                        {
                            Name = "Ambassador Room",

                            Description = "The Ambassador Suite are equipped with a living room, dining table, 2 Twin Beds, a " +
                            "guest bathroom, a Desk & Chair coffee table with shower or bathtub and some of them with balcony " +
                            "overlooking hotel garden. Each room is air conditioned and has a flat-screen TV with international TV" +
                            " channels, and a spacious wardrobe.",

                            Price = 66875M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172679/yard/Kano%20Hotels/Porto%20Golf%20Hotel/RoomTypes/porto-golf-hotel-Ambassador_room_danle4.jpg"
                        }
                    },

                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Bristol Palace Hotel",

                    Description = "Bristol Palace Hotel is the leading 5-star certified Hotel in Northern Nigeria, recognized for" +
                    " its ideal conference facilities to accommodate up to 500 people and 114 all-suite with hotel market related" +
                    " pricing situated at 54/56 Guda Abdullahi Road, Farm Centre Kano, Kano, Nigeria.Rooms categories include Senior" +
                    " Twin, Senior Suite, Executive Twin, Executive Suite, Senator Suite and Ambassador Suite. Each room features a" +
                    " spacious bathroom, king-sized beds, cable connected LCD TV sets, refrigerators, elements necessary to cook up" +
                    " a cup of tea and air conditioners.\n\n\nBristol Palace Hotel has 3 restaurants, large entertainment swimming" +
                    " pool area with barbecue weekends, Shisha, branded Chopsticks Restaurant open every day from 3 pm, full English" +
                    " and Nigerian Breakfast Buffet included in accommodation price, full Continental and Nigerian dinner buffet " +
                    "prepared by our award-winning team of Chefs, full à la carte menu for our terraces and 24 hrs.room service. " +
                    "Come and visit Bristol Palace Hotel, it will be your home from home.\n\n\nA number of facilities are available" +
                    " Bristol Palace Hotel.These include free WiFi, a swimming pool, a body spa, 3 restaurants and a bar.Guests at" +
                    " this hotel will have more time to enjoy their stay at the hotel.Guests can also take relaxing dips in the " +
                    "pool.\n\n\nTerms and Conditions\nCheck In: From 12:00 PM\nCheck Out: By 12:00 PM\nCancellation: Cancellation" +
                    " policies vary according to room type.\nChildren: All children are allowed.\nPets: Pets are not allowed." +
                    "\nPayment: Cash, Visa.\n\nPlaces of Interest Near Bristol Palace Hotel\nDala Hills",

                    Email = "frontdesk@portogolfhotels.com",
                    Phone = "(+234) 81 11 353535",
                    Address = new Address
                    {
                        City = "Kano", State = "Kano", Country = "Nigeria", PostalCode = "10231",
                        Street = "54-56 Guda Abdullahi Road,Farm Centre, TarauniKano"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                    new string[] {
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172432/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/bristol-palace-hotel-Landing_page_4_lwzyqk.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172436/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/bristol-palace-hotel-Landing_page_5_uf7upj.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172428/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/bristol-palace-hotel-Landing_Page_3_a4ft1z.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172424/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/bristol-palace-hotel-Landing_page_2_tgbdh1.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172423/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/bristol-palace-hotel-Landing_Page_1_wlblt5.jpg"
                    }
                    ),
                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Senator Room",
                            Description = "Senator Suite Features a wider space, Living Room “Parlour”, 2 LED TV, Small Bar Counter," +
                            " and all the goodies of Superior and Executive suite",
                            Price = 102000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172585/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/RoomTypes/bristol-palace-Senator_Suite_gkb1ob.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Twin Room",
                            Description = "Extra-large superior suite room with twin bed. This luxuriously decorated room features" +
                            " a flat-screen satellite TV, coffee table and a laptop, a digitalised safe, a dressing table, a bedside" +
                            " table, a small writing table, and a small fridge. The bathroom with shower or a bath tub.",
                            Price = 86000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172583/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/RoomTypes/Bristol_Palace-Palace-Palace_room_iq252u.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Suite",
                            Description = "Executive Suite features a wider space, with Free wifi, Air-conditioner," +
                            " 1 LED TV with Game facilities, Fridge, Kettle, Hairdryer.",
                            Price = 86000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172584/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/RoomTypes/bristol-palace-Executive_Room_qgfmto.jpg"
                        },
                        new RoomType
                        {
                            Name = "Superior Room",
                            Description = "Superior Suite is your best choice for short-long business stay. Your Suite comes with" +
                            " free stationery, shoes polish, and minibar. The living and bedroom areas are exceptionally decorated" +
                            " providing the guest with a cozy, friendly and relaxing ambience.",
                            Price = 76500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172582/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/RoomTypes/Bristol-Palace--Superior_Room_mo6frw.jpg"
                        },
                        new RoomType
                        {
                            Name = "Ambassador Room",
                            Description = "The Ambassador Suite are equipped with a living room, dining table, a guest bathroom, " +
                            "a Desk & Chair coffee table with shower or bathtub and some of them with balcony overlooking hotel " +
                            "garden. Each room is air conditioned and has a flat-screen TV with international TV channels, and a" +
                            " spacious wardrobe.",
                            Price = 66875M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172583/yard/Kano%20Hotels/Bristol%20Palace%20Hotel/RoomTypes/bristol-palace-Ambassador_Suite_qgug2o.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Al Jazeera Hotel",

                    Description = "Aljazeerah Hotel Kano is a budget hotel positioned at 33 Gashash road, along race course road," +
                    " Kano, Nigeria. The rooms are divided into Standard, Economy and Deluxe.\n\n\nEach room features a television" +
                    " with cable reception, wardrobe, telephone, bedside table, air conditioner, bed with pillows, work table/chair" +
                    " and a private bathroom.\n\n\nGuest at Aljazeerah Hotel Kano can order varieties of meals at the on-site " +
                    "restaurant.\n\n\nLaundry services, airport pick-up and car hire are additional surcharged services available" +
                    " on request.\n\nThis hotel has a very spacious compound where guests can park.\n\n\nInteresting Places near " +
                    "Aljazeerah Hotel Kano\nMallam Aminu Kano Airport (8.5 km)\nZoo Park (6.5 km)\nShoprite Mall (5.6 km)\nKano" +
                    " Golf Course (1.0 km).",

                    Email = "reservation@aljazeerahotel.com",
                    Phone = "+234 803 671 7900",
                    Address = new Address
                    {
                        City = "Kano", State = "Kano", Country = "Nigeria", PostalCode = "10121", 
                        Street = "No 33 Gashash Road, Along race course road."
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172287/yard/Kano%20Hotels/Aljazeera%20Hotel/Al_Jazeera_Hotel_landing_page_2_gicmgy.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172286/yard/Kano%20Hotels/Aljazeera%20Hotel/Al_Jazeera_Hotel_landing_page_1_yncngc.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172287/yard/Kano%20Hotels/Aljazeera%20Hotel/Al_Jazeera_Hotel_landing_page_3_gqvvku.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172290/yard/Kano%20Hotels/Aljazeera%20Hotel/Al_Jazeera_Hotel_landing_page_4_ukbl0l.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172293/yard/Kano%20Hotels/Aljazeera%20Hotel/Newton_park_1_crjao5.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Senior Room",
                            Description = "Features a wider space, Living Room “Parlour”, 2 LED TV, Small Bar Counter, and all the" +
                            " goodies of Superior and Executive suite.",
                            Price = 122570M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172362/yard/Kano%20Hotels/Aljazeera%20Hotel/RoomTypes/Senior_Room_e7lsuc.jpg"
                        },
                        new RoomType
                        {
                            Name = "Royal Room",
                            Description = "This luxuriously decorated room features a flat-screen satellite TV, coffee table and" +
                            " a laptop, a digitalised safe, a dressing table, a bedside table, a small writing table, and a small" +
                            " fridge. The bathroom with shower or a bath tub.",
                            Price = 123500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172355/yard/Kano%20Hotels/Aljazeera%20Hotel/RoomTypes/Royal_Room_otsntz.jpg"
                        },
                        new RoomType
                        {
                            Name = "Senator Room",
                            Description = "Your best choice for short-long business stay. Your Suite comes with free stationery," +
                            " shoes polish, and minibar. The living and bedroom areas are exceptionally decorated providing the " +
                            "guest with a cozy, friendly and relaxing ambience.",
                            Price = 11050M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172358/yard/Kano%20Hotels/Aljazeera%20Hotel/RoomTypes/Senator_Room_iziemk.jpg"
                        },
                        new RoomType
                        {
                            Name = "King Room",
                            Description = "Equipped with a living room, dining table, a guest bathroom, a Desk & Chair coffee " +
                            "table with shower or bathtub and some of them with balcony overlooking hotel garden. Each room is " +
                            "air conditioned and has a flat-screen TV with international TV channels, and a spacious wardrobe",
                            Price = 130800M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172352/yard/Kano%20Hotels/Aljazeera%20Hotel/RoomTypes/King_Room_tjbv9a.jpg"
                        },
                        new RoomType
                        {
                            Name = "Presidential Suite",
                            Description = "features a wider space, with Free wifi, Air-conditioner, 1 LED TV with Game facilities," +
                            " Fridge, Kettle, Hairdryer.",
                            Price = 157500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705172353/yard/Kano%20Hotels/Aljazeera%20Hotel/RoomTypes/Presidential_Room_bqw2pb.jpg"
                        }
                    },
                    IsDelisted = false
                },

                 //PLATEAU Hotels
                new Hotel
                {
                    Name = "Silk Suites",

                    Description = "Silk Suites is a 3-Star hotel located in the tranquil and serene area of Zaramaganda Rd, Ray" +
                    " field, Jos, Plateau state. It is a place like no other where style and luxury are matched with excellent " +
                    "warmth for guests. Silk Suites is situated centrally and neighbour to major city attractions. Silk Suites is" +
                    " 45 minutes from Yakubu Gowon Airport.\n\n\nThe rooms in Silk Suites are categorised into: Apartment, Deluxe," +
                    " Diplomatic, Superior and Standard. For both leisure and business, each room is exquisitely furnished for your" +
                    " comfort with air conditioners, a fridge, a worktable, and a flat-screen TV. Buffet breakfasts are served daily" +
                    " to the guests, and at no costs.\n\n\nA number of hotel facilities are available for guests at Silk Suites. " +
                    "These include: Parking spaces with the help of a valet, a 24hr supply of electricity, spa and gym areas, a " +
                    "bar that offers a vast selection of beverages and light refreshments, WiFi internet connection, and a " +
                    "swimming pool where guests can take dips for relaxation.\n\n\nExclusive services are available for guests on" +
                    " request, such as: Laundry/ dry cleaning, a 24hr room service, safety deposit boxes and storage rooms, and" +
                    " rolling beds for children. It is an ideal hotel for couples with children as accommodation for babies and" +
                    " kids is free.\n\n\nTerms and Conditions\nCheck-In: From 8:00 AM (based on availability)\nCheck-Out: By 12 PM" +
                    "\nChildren: Allowed, and free.\nPayment: Cash Only\n\n\nInteresting Places to Visit Near Silk Suites\nRayfield" +
                    " Golf Club\nPlateau State Government House",

                    Email = "reservation@silksuites.com.ng",
                    Phone = "+234 7032127930, +234 9098584489",
                    Address = new Address
                    {
                        City = "Jos", State = "Plateau", Country = "Nigeria", PostalCode = "10235",
                        Street = "No.1, Davou Mang Street, Off Zaramaganda Rd Ray field, Jos"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {

                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246290/yard/Plateau%20Hotels/Silk%20Suites/silk-suites-Landing_page_4_sigtvx.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246294/yard/Plateau%20Hotels/Silk%20Suites/silk-suites-Landing_page_5_c1fj1z.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246282/yard/Plateau%20Hotels/Silk%20Suites/silk-suites-Landing_page_2_jdr25h.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246286/yard/Plateau%20Hotels/Silk%20Suites/silk-suites-Landing_page_3_lwkxoc.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246279/yard/Plateau%20Hotels/Silk%20Suites/silk-suites-Landing_page_1_oyprpv.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Room",

                            Description = "Standard  Features a wider space, Living Room “Parlour”, 2 LED TV, Small Bar" +
                            " Counter, and all the goodies of Superior.",

                            Price = 16500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246404/yard/Plateau%20Hotels/Silk%20Suites/RoomTypes/Standard_Room_givzbx.jpg"
                        },
                        new RoomType
                        {
                            Name = "Superior Room",

                            Description = "The Superior Room are equipped with a living room, dining table, a guest bathroom, " +
                            "a Desk & Chair coffee table with shower.",

                            Price = 18500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246408/yard/Plateau%20Hotels/Silk%20Suites/RoomTypes/Superior_Room_vpxlks.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Room",

                            Description = "The Deluxe Room has extra-large superior suite room with twin bed. This luxuriously" +
                            " decorated room features a flat-screen satellite TV, coffee table and a laptop, a digitalised safe," +
                            " a dressing table, a bedside table, a small writing table, and a small fridge. The bathroom with " +
                            "shower or a bath tub.",

                            Price = 20500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246396/yard/Plateau%20Hotels/Silk%20Suites/RoomTypes/Deluxe_Room_jlaboc.jpg"
                        },
                        new RoomType
                        {
                            Name = "Diplomatic Room",

                            Description = "Diplomatic Room features a wider space, with Free wifi, Air-conditioner, 1 LED TV" +
                            " with Game facilities, Fridge, Kettle, Hairdryer..",

                            Price = 28500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246400/yard/Plateau%20Hotels/Silk%20Suites/RoomTypes/Diplomatic_Room_ufclpz.jpg"
                        },
                        new RoomType
                        {
                            Name = "Apartment",

                            Description = "Apartment Room is your best choice for short-long business stay. Your Suite comes with" +
                            " free stationery, shoes polish, and minibar. The living and bedroom areas are exceptionally decorated" +
                            " providing the guest with a cozy, friendly and relaxing ambience.",

                            Price = 40000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246392/yard/Plateau%20Hotels/Silk%20Suites/RoomTypes/Apartment_smee5g.jpg"
                        }
                    },
                    IsDelisted = false,
                    Popular = true
                },


                new Hotel
                {
                    Name = "Maria's Lodge",

                    Description = "Maria’s Lodge is located at No 1 Grey Garden off Tafawa Balewa Street, close to the famous" +
                    " Plateau Riders Park in the West of Mines Area of Jos North Local Government Area Plateau State, Nigeria." +
                    "\n\n\nMaria’s Lodge is right in the heart of Jos, within the most secured area of West of Mines, the " +
                    "neighborhood that host the Police Headquarter, Prison Headquarter and the Department of State Security " +
                    "Services Headquarter.\n\n\nMaria’s Lodge has 24 hours Security Service including Monitors/CCTV, " +
                    "strategically installed to monitor unwanted activities, in and around the Hotel premises. At Maria’s Lodge " +
                    "your, “premium comfort is guaranteed”.\n\n\nOpened since January 2015, as a brand new Hotel, our rates are" +
                    " highly competitive and we have since become a hub for tourists and expatriates, business people who desire" +
                    " nothing but the best. The choice of our location is deliberate, a beautiful landscape and unspoilt " +
                    "environment, easy access to any part of town, either you are in Jos for business or for leisure.\n\n\nMaria’s" +
                    " Lodge in Jos, we are an integral part of the new Jos north, we are only 50 km away from Yakubu Gowon " +
                    "International Airport and convenient to the central part of Jos.\n\n\nReconnect to what’s most important to " +
                    "you and your  life, whether that means having a chilled drink with peanuts at our exclusive Underground Bar," +
                    " or enjoying a spicy peppered chicken or peppered meat at the Main Lounge, or a visit to the Gift Shop for " +
                    "your designer’s fragrance and accessories.\n\n\nWith the all year round cool climate in Jos, we are happy to" +
                    " tailor our space to your needs. Our goal is to make your stay in Jos as stress-free as possible. Maria’s " +
                    "Lodge has 23 tastefully furnished Rooms/Suites and Banquet Hall furnished with the state of the art technology" +
                    " and furniture’s designed to meet all your needs.\n\n\nOur fabulous city center location make’s Maria’s Lodge" +
                    " the perfect place to gather with friends/families and co-workers for your relaxation.\n\n\nTry and discover " +
                    "the cool, warmth, Excitement and sense of belonging at Maria’s Lodge.\n\nWe are here to make you happy.",

                    Email = "reservations@mariaslodge.com",
                    Phone = "0813 667 7557, 0813 408 8789 ",

                    Address = new Address
                    {
                        City = "Jos", State = "Plateau", Country = "Nigeria", PostalCode = "11111",
                        Street = "1 Grey Garden Off Tafawa Balewa Street, Jos"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246008/yard/Plateau%20Hotels/Maria%27s%20Lodge/marias-lodge-Landing_Page_1_y5h9yl.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246005/yard/Plateau%20Hotels/Maria%27s%20Lodge/marias-lodge-Landing_Page_5_cskq1k.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246001/yard/Plateau%20Hotels/Maria%27s%20Lodge/marias-lodge-Landing_Page_4_svciru.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245998/yard/Plateau%20Hotels/Maria%27s%20Lodge/marias-lodge-Landing_Page_3_qqkcf2.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245994/yard/Plateau%20Hotels/Maria%27s%20Lodge/marias-lodge-Landing_Page_2_pifemz.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Room",

                            Description = "Standard room Features a wider space, Living Room “Parlour”, 2 LED TV, Small Bar " +
                            "Counter, and all the goodies of Superior.",

                            Price = 8500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246098/yard/Plateau%20Hotels/Maria%27s%20Lodge/RoomTypes/Standard_Room_kaunqs.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Room",

                            Description = "The Executive Room are equipped with a living room, dining table, a guest bathroom," +
                            " a Desk & Chair coffee table with shower.",

                            Price = 11000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246090/yard/Plateau%20Hotels/Maria%27s%20Lodge/RoomTypes/Executive_Room_yfcdre.jpg"
                        },
                        new RoomType
                        {
                            Name = "Luxury Room",

                            Description = "The Luxury Room has extra-large superior suite room with twin bed. Your Suite comes " +
                            "with free stationery, shoes polish, and minibar.",

                            Price = 15000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246094/yard/Plateau%20Hotels/Maria%27s%20Lodge/RoomTypes/Luxury_Room_wsji7i.jpg"
                        },
                        new RoomType
                        {
                            Name = "Suites",

                            Description = "Suites is your best choice for short-long business stay. Your Suite comes with free" +
                            " stationery, shoes polish, and minibar. The living and bedroom areas are exceptionally decorated" +
                            " providing the guest with a cozy, friendly and relaxing ambience.",

                            Price = 25000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246101/yard/Plateau%20Hotels/Maria%27s%20Lodge/RoomTypes/Suites_Room_j6sewg.jpg"
                        },
                        new RoomType
                        {
                            Name = "Diplomatic Room",

                            Description = "The Diplomatic Room is luxuriously decorated room features a flat-screen satellite TV," +
                            " coffee table and a laptop, a digitalised safe, a dressing table, a bedside table, a small writing " +
                            "table, and a small fridge. The bathroom with shower or a bath tub.",

                            Price = 28500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246086/yard/Plateau%20Hotels/Maria%27s%20Lodge/RoomTypes/Diplomatic_Room_yse5g0.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Qualer Hotel",

                    Description = "Guests are promised a wonderful time trying out the services and facilities that Qualer " +
                    "Apartment has got to offer. Its exquisite furnishings and serene environment make it the ideal place to lodge." +
                    " Qualer Apartment can be found at 3, Monica Close, Rantya Street, Low Cost, Jos, Plateau. It is surrounded by" +
                    " tourist attractions designed to keep travellers amused.\n\n\nThe accommodations have ornate lampshades that" +
                    " keep the rooms well lit. Each spacious and air conditioned room contains king-sized beds with clean bedding," +
                    " safes for storing valuable items, a telephone connected to the front desk, study desks for working or reading" +
                    " and an LCD TV with satellite reception. The en-suite bathroom is modern and comes with a bathtub. The " +
                    "luxurious rooms fall into Standard Deluxe, Business Deluxe, Executive Deluxe, Luxury Deluxe, Double Suite," +
                    " Executive Suite, Luxury Suite and Family Suite categories.\n\n\nQualer Apartment is home to a restaurant " +
                    "and a bar/lounge. The restaurant provides guests with their desired meals. Guests can order drinks at the " +
                    "bar while chilling in the lounge.Car hire, luggage storage, laundry/dry cleaning and a 24-hour room service" +
                    " is available on request. Qualer Apartment has a spacious parking space. This hotel is a guarded complex. " +
                    "A 24-hour power supply is provided throughout the guest’s stay.\n\n\nTerms and ConditionsCheck in: From 2:00" +
                    " pm\nCheck out: By 12:00 pm\nPayment: Visa, MasterCard\nChildren: Kids not older than 12 can stay for free" +
                    "\nPets: Pets are not permitted\n\n\nInteresting Places to Visit near Qualer Apartment\nRayfield Golf Club" +
                    "\nJos National Museum\nSharwama & Grills\nChinese Gardens\nUniversity Of Jos\nJos Zoo\nLaranto Market",

                    Email = "0913-773-5777",
                    Phone = "0913-773-5777",
                    Address = new Address
                    {
                        City = "Jos", State = "Plateau", Country = "Nigeria", PostalCode = "10231", 
                        Street = "3 Monica Close, Rantya street Low Cost , Jos"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246135/yard/Plateau%20Hotels/Qualer%20Hotel/Qualer_Landing_Page_1_ivzouk.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246139/yard/Plateau%20Hotels/Qualer%20Hotel/Qualer_Landing_Page_2_h5rhkq.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246143/yard/Plateau%20Hotels/Qualer%20Hotel/Qualer_Landing_Page_3_zlkktv.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246147/yard/Plateau%20Hotels/Qualer%20Hotel/Qualer_Landing_Page_5_y0q4sd.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246150/yard/Plateau%20Hotels/Qualer%20Hotel/Qualer_Landing_Page4_jautgq.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Deluxe Room",

                            Description = "Features a wider space, Living Room “Parlour”, 2 LED TV, Small Bar Counter, and all " +
                            "the goodies of Superior.",

                            Price = 15000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246244/yard/Plateau%20Hotels/Qualer%20Hotel/RoomTypes/Standard_Deluxe_Room_zrww2f.jpg"
                        },
                        new RoomType
                        {
                            Name = "Business Deluxe Room",

                            Description = "The Business Deluxe Room are equipped with a living room, dining table, a guest bathroom," +
                            " a Desk & Chair coffee table with shower.",

                            Price = 19950M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246248/yard/Plateau%20Hotels/Qualer%20Hotel/RoomTypes/Business_Deluxe_Room_nlntbd.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Deluxe Room",

                            Description = "The Executive Deluxe Room has extra-large superior suite room with twin bed. This " +
                            "luxuriously decorated room features a flat-screen satellite TV, coffee table and a laptop, a " +
                            "digitalised safe, a dressing table, a bedside table, a small writing table, and a small fridge. " +
                            "The bathroom with shower or a bath tub.",

                            Price = 21750M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246252/yard/Plateau%20Hotels/Qualer%20Hotel/RoomTypes/Executive_Deluxe_Room_hzrp6d.jpg"
                        },
                        new RoomType
                        {
                            Name = "Luxury Deluxe Room",

                            Description = "Luxury Deluxe Room features a wider space, with Free wifi, Air-conditioner, 1 LED TV " +
                            "with Game facilities, Fridge, Kettle, Hairdryer.",

                            Price = 23350M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246240/yard/Plateau%20Hotels/Qualer%20Hotel/RoomTypes/Luxury_Deluxe_Room_haynqa.jpg"
                        },
                        new RoomType
                        {
                            Name = "Family Room",

                            Description = "Familys Room is your best choice for short-long business stay. Your Suite comes with " +
                            "free stationery, shoes polish, and minibar. The living and bedroom areas are exceptionally decorated" +
                            " providing the guest with a cozy, friendly and relaxing ambience.",

                            Price = 49950M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705246236/yard/Plateau%20Hotels/Qualer%20Hotel/RoomTypes/Family_Room_z26ewb.jpg"
                        }
                    },

                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Crispan Hotel",

                    Description = "Crispan Suites and Event Centre is a 5-Star luxurious hotel situated in the beautiful City of " +
                    "Jos, Plateau State Nigerian elegantly standing at Plot 9259, Opposite Airforce Base, Off Jonah Davis Jang Way," +
                    " Rayfield.\n\n\nCrispan Suites and Event Centre has 4 room categories into Deluxe, Executive, Royal Suites and" +
                    " Presidential Suite. It is expensively styled and adorned with breathtaking interior decoration matching " +
                    "International Hotel standard. All room categories are fitted with amenities such as Flat Screen TV, Wireless" +
                    " Connection, Table and chairs, A Sofa, Complimentary Toiletries, Water Heater, Towels, Refrigerators, Mirror," +
                    " Lamps, Air conditioners, Intercom System, double and single beds, and lots more.\n\n\nCrispan Suites and " +
                    "Event Centre has State-of-the-art meeting/conference rooms, State of the Art Banquet halls,  Olympic size" +
                    " outdoor pool, A well Equipped Gym, Restaurants  with professionally trained Chefs offering mouth-watering " +
                    "dishes both local and continental , VIP bar/lounge, Bush Bar, Chinese Restaurant, Maximum Security, Parking" +
                    " space for 500 cars, Uninterrupted power supply, Business Centre, Car Hire, Airport Pickup And Drop off, A " +
                    "well-equipped Spa, Laundry services and House Keeping services.\n\n\nPlaces of Interest near Crispan Suites" +
                    " and Event Centre:Old Airport Junction · Air Force Base · Air Force Military Boys School · Pizza and Ice " +
                    "cream Club · Old Government House, Rayfield · New Government House, Little Rayfield · NASCO Group of Companies" +
                    " · NASCO Household · NASCO Fibre · Jos Business School · Plateau Radio Television Corporation Headquarters · " +
                    "NTA College, Jos ·  Rayfield Golf Club ·  Rayfield Resort · Tiger Bar · Mobile Barrack · Rochas Foundation " +
                    "School.\n\n\nCrispan Suites and Event Centre, Jos is a top-class hotel in Jos, Plateau.\n\n\nTerms and " +
                    "Conditions about Crispan Suites and Event Centre, Jos\nCheck in: from 2:00pm(identification required)\n" +
                    "Check out: by 12:00pm\nPets are not allowed.\nSmoking in Rooms not allowed.\nChildren: Babies and Kids stay" +
                    " for free.\nPayment: Cash, Cards.\nCancellation Policy: 24hrs.",

                    Email = "contact@crispanhotel.com",
                    Phone = "0913-773-3369, 0915-020-5777",
                    Address = new Address
                    {
                        City = "Jos", State = "Plateau", Country = "Nigeria", PostalCode = "10231",
                        Street = "Plot 9259, Opposite Airforce Base, Off Jonah Davis Jang Way, Rayfield, Jos"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                    new string[] {
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245824/yard/Plateau%20Hotels/Crispan%20Hotel/Crispan_Landing_Page_3_kvleto.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245815/yard/Plateau%20Hotels/Crispan%20Hotel/Crispan_Landing_Page_5_qne8av.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245811/yard/Plateau%20Hotels/Crispan%20Hotel/Crispan_Landing_Page_4_y1i7gi.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245818/yard/Plateau%20Hotels/Crispan%20Hotel/Crispan_Landing_Page_1_vre58q.jpg"
                    }
                    ),
                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Deluxe Room",
                            Description = "Features a wider space, Living Room “Parlour”, 2 LED TV, Small Bar Counter, and all the" +
                            " goodies of Superior.",

                            Price = 35000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245934/yard/Plateau%20Hotels/Crispan%20Hotel/RoomTypes/Deluxe_Room_undejw.jpg"
                        },
                        new RoomType
                        {
                            Name = "Double Deluxe Room",

                            Description = "The Double Deluxe Room are equipped with a living room, dining table, a guest bathroom," +
                            " a Desk & Chair coffee table with shower.",

                            Price = 37000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245937/yard/Plateau%20Hotels/Crispan%20Hotel/RoomTypes/Double_Deluxe_Room_p2xq2b.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Room",

                            Description = "Extra-large superior suite room with twin bed. This luxuriously decorated room features " +
                            "a flat-screen satellite TV, coffee table and a laptop, a digitalised safe, a dressing table, a bedside" +
                            " table, a small writing table, and a small fridge. The bathroom with shower or a bath tub.",

                            Price = 40000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245941/yard/Plateau%20Hotels/Crispan%20Hotel/RoomTypes/ExecutiveRoom_zagssk.jpg"
                        },
                        new RoomType
                        {
                            Name = "Royal Room",

                            Description = "Executive Suite features a wider space, with Free wifi, Air-conditioner, 1 LED TV with" +
                            " Game facilities, Fridge, Kettle, Hairdryer.",

                            Price = 85000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245948/yard/Plateau%20Hotels/Crispan%20Hotel/RoomTypes/Royal_Room_uceq4i.jpg"
                        },
                        new RoomType
                        {
                            Name = "Presidential Room",

                            Description = "Presidential Room is your best choice for short-long business stay. Your Suite comes " +
                            "with free stationery, shoes polish, and minibar. The living and bedroom areas are exceptionally " +
                            "decorated providing the guest with a cozy, friendly and relaxing ambience.",

                            Price = 250000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245944/yard/Plateau%20Hotels/Crispan%20Hotel/RoomTypes/Presidential_Room_fpalhp.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Hotel Litan",

                    Description = "Hotel Litan is a classy hotel located at 10 Boulevard close, behind Rochas Foundation College," +
                    " Jos, Nigeria. The hotel offers an affordable and comfortable accommodation for visitors/ guests to feel at" +
                    " home, the hotel is situated in a very serene environment and provides a gym where guests can exercise and" +
                    " guests can order meals from the onsite restaurant and drinks from the outdoor bar/ lounge.\n\n\nThe hotel" +
                    " has five types of room, diplomatic room, executive room, luxury room, studio room and the standard room, " +
                    "all rooms provide access to Free Wi-Fi connection and they are fully furnished and single fitted with LCD/ " +
                    "Plasma TV with satellite connection and a fan is provided in each room, other features vary depending on " +
                    "the room type. These features include a king size bed, refrigerators, work table and chair and an ensuite" +
                    " bathroom.\n\n\nThe onsite restaurant offers a variety of meals which guests can choose from via room service," +
                    " room service is also offered 24hrs; payments can be made via cash, visa, and master card. A variety of drinks" +
                    " ranging from alcoholic to non- alcoholic can be enjoyed at the bar/ lounge. Maximum Security is guaranteed." +
                    "\n\n\nAdditional surcharged services available at Hotel Litan are laundry/ dry cleaning, services and " +
                    "concierge services.\n\n\nTerms and Conditions\nCheck In - From 14:00 (ID Required)\nCheckout – by 12:00hrs" +
                    "\nPayment – Cash, visa and Master card\nCancellation – Cancellation policies apply\nPets - Not allowed\n" +
                    "Children – Babies and kids are welcome to stay for free.",

                    Email = "admin@hotellitan.com",
                    Phone = "+2348096412222",
                    Address = new Address
                    {
                        City = "Jos", State = "Plateau", Country = "Nigeria", PostalCode = "10121",
                        Street = "N0.10 Airport Boulevard, behind Rochas Foundation, Off Presidential Drive Old Airport Junction, Jos"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245598/yard/Plateau%20Hotels/Hotel%20Litan/Hotel_Litan_Landing_Page_whfuow.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245595/yard/Plateau%20Hotels/Hotel%20Litan/Hotel_Latin_Landing_Page4_h073kt.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245600/yard/Plateau%20Hotels/Hotel%20Litan/Hotel_Lithan_Landing_Page3_tuz1qp.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245594/yard/Plateau%20Hotels/Hotel%20Litan/Hotel_Latin_Landing_Page2_cwkuyb.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245595/yard/Plateau%20Hotels/Hotel%20Litan/Hotel_Litan_Landing_Page_5_petlrh.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Studio",

                            Description = "Studio room Features a wider space, Living Room “Parlour”, 2 LED TV, Small Bar " +
                            "Counter, and all the goodies of Superior.",

                            Price = 6000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245764/yard/Plateau%20Hotels/Hotel%20Litan/RoomTypes/Studio_Rom_vpqoi4.jpg"
                        },
                        new RoomType
                        {
                            Name = "Luxury",

                            Description = "The Luxury Room are equipped with a living room, dining table, a guest bathroom, a " +
                            "Desk & Chair coffee table with shower.",

                            Price = 9200M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245761/yard/Plateau%20Hotels/Hotel%20Litan/RoomTypes/Luxury_Room_ez2twz.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive",

                            Description = "Executive room features a wider space, with Free wifi, Air-conditioner, 1 LED TV with" +
                            " Game facilities, Fridge, Kettle, Hairdryer.",

                            Price = 11500M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245758/yard/Plateau%20Hotels/Hotel%20Litan/RoomTypes/Executive_Room_uvl4sp.jpg"
                        },
                        new RoomType
                        {
                            Name = "Diplomatic",

                            Description = "Diplomatic Room is your best choice for short-long business stay. Your Suite comes" +
                            " with free stationery, shoes polish, and minibar. The living and bedroom areas are exceptionally " +
                            "decorated providing the guest with a cozy, friendly and relaxing ambience.",

                            Price = 13800M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705245755/yard/Plateau%20Hotels/Hotel%20Litan/RoomTypes/Diplomatic_Room_lpsfuz.jpg"
                        }
                    },
                    IsDelisted = false
                },

                 //RIVERS-PORTHARCOURT Hotels
                new Hotel
                {
                    Name = "LimeWood Hotel",

                    Description = "Limewood Hotel Port Harcourt is located at Plot F 1B, Abacha Road in the reserved area of GRA" +
                    " Phase 3, G.R.A. Port Harcourt 500272 Nigeria. Our 35 room boutique hotel environment is comfortable and " +
                    "exquisite, from our classic rooms right through to our presidential suites the contemporary design and " +
                    "style were inspired to give you a sense of modern comfort with that exquisite feel and ambience.\n\n\nThe " +
                    "International culinary team provides the best continental and national cuisine this part of the country has " +
                    "to offer. Cocktails, wines, spirits and an array of beverages are readily available at our bar which is open" +
                    " all day and night. Other facilities include our exclusive penthouse lounge, gym, and exotic spa, laundry, " +
                    "free WIFI and a well secured parking space. All our guest floors are well secured by access control. Each " +
                    "room is equipped with the finest beds and beddings, tea makers, espresso machines, guest room fridge, " +
                    "electronic safes, reading lamps and electronic door locks. There are also our luxurious signature toiletries" +
                    " available in your bathroom, complimentary of course…\n\n\nOur IPTV is the first of its kind in Port Harcourt." +
                    " There is an endless list of contents be it movies, documentaries, musicals etc. Selection of local and foreign" +
                    " television channels will keep you entertained all through your stay with us.\n\n\nOur staffs are courteous," +
                    " responsive and very friendly; they are trained continuously by the best professionals in hotel and hospitality" +
                    " industry.\n\nLimewood Hotel Port Harcourt is a luxury hotel in Port -Harcourt, Rivers.\n\n\nTerms and " +
                    "Conditions about Limewood Hotel Port Harcourt\n\nCheck in time is 12:00pm\nCheck out time is 2:00pm\nPets are" +
                    " not allowed\nSmoking is not allowed in rooms\nBabies/children are allowed in rooms\nCash and card payments " +
                    "are accepted.\n\nCancellations made within 48 hours’ notice will not be charged, while cancellation made " +
                    "outside 48 hours’ notice will be charged 100% no show policy.\n\n\nInteresting Places nearby\n\nCinemas\n" +
                    "Shopping malls\nChildren’s amusement park\nPolo club\nNight clubs.",

                    Email = "reservations@thelimewoodhotels.com",
                    Phone = "+234 903 000 9808",
                    Address = new Address
                    {
                        City = "Port-Harcourt", State = "Rivers", Country = "Nigeria", PostalCode = "10235",
                        Street = "Plot F 1B, Abacha Road, GRA Phase 3, G.R.A. Port Harcourt 500272, Port -Harcourt"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260406/yard/Rivers%20Hotels/Limewood%20Hotel/Limewood_H1_umlj9s.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260400/yard/Rivers%20Hotels/Limewood%20Hotel/Limewood_Image_lluedq.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260394/yard/Rivers%20Hotels/Limewood%20Hotel/Limewood_H6_tg8f2h.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260389/yard/Rivers%20Hotels/Limewood%20Hotel/Limewood_H5_xzsbwr.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260411/yard/Rivers%20Hotels/Limewood%20Hotel/Limewood_H2_jhj92t.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260381/yard/Rivers%20Hotels/Limewood%20Hotel/Limewood_H3_t2ntpe.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Classic Room",

                            Description = "Enjoy our classic rooms with all the elegancy and comfort that its interior has. It" +
                            " features such essentials as a flat-screen 43″ flat screen TV, WiFi and 1 bathtub with shower option" +
                            " and all guest amenities to give you that comfort and serenity only experienced at Limewood Hotel",

                            Price = 55000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260438/yard/Rivers%20Hotels/Limewood%20Hotel/RoomTypes/Classic_Room_w9cjm4.jpg"
                        },
                        new RoomType
                        {
                            Name = "Superior Room",

                            Description = "This room class is enhanced with an all-round spacious area, which will be more than" +
                            " enough for a company of two. It features such essentials as a flat-screen 43″ TV, WiFi and 1 bathtub" +
                            " with shower option and all guest amenities to give you that special comfort and serenity only " +
                            "experienced at Limewood Hotel",

                            Price = 65000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260459/yard/Rivers%20Hotels/Limewood%20Hotel/RoomTypes/Superior_Room_qeqbeg.jpg"
                        },
                        new RoomType
                        {
                            Name = "Studio Room",

                            Description = "Enjoy our classic suites with all the elegancy and comfort that its interior has… It " +
                            "features such essentials as a flat-screen 45″ TV, WiFi and 2 bathrooms with a living room and 2 " +
                            "bedrooms, big and chic enough to be counted as real suite bedrooms.",

                            Price = 140000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260452/yard/Rivers%20Hotels/Limewood%20Hotel/RoomTypes/Studio_Room_acgsd9.jpg"
                        },
                        new RoomType
                        {
                            Name = "Signature Suite",

                            Description = "Elegant floor to ceiling of unparalleled luxury and comfort. Located on the 3rd Floor, " +
                            "the Signature Suite accommodates a majestic array of facilities and features, including separate living" +
                            " room and dining area, kitchenette and a direct premium access to the Penthouse Lounge. Sophisticated" +
                            " and refined, every aspect of the Signature Suite is carefully crafted to create a sumptuous space fit" +
                            " for royalty.",

                            Price = 200000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260447/yard/Rivers%20Hotels/Limewood%20Hotel/RoomTypes/Signature_Suite_pjqvbo.jpg"
                        },
                        new RoomType
                        {
                            Name = "Presidential Suite",

                            Description = "Above it all, the Presidential Suite offers 120 square feet of elegant comfort. This " +
                            "suite offers a living room and 2 bedrooms with king-size poster bed and sit out deck with panoramic " +
                            "view of Port Harcourt GRA. The master bath offers a deep soaking bathtub with hot water options. " +
                            "The guestroom is also equipped with its own bathtub and shower options.",

                            Price = 250000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260440/yard/Rivers%20Hotels/Limewood%20Hotel/RoomTypes/Presidential_Suite_vqmshd.jpg"
                        }
                    },
                    IsDelisted = false,
                    Popular = true
                },


                new Hotel
                {
                    Name = "Dannic Hotel",

                    Description = "Dannic Hotel Port Harcourt is a budget hotel that offers a perfect blend of relaxation and " +
                    "comfort, delicately blended to meet high taste and standard. This hotel which has been designed for comfort " +
                    "and convenience is located at Plot 33, Circular road, Presidential estate, GRA in Port Harcourt, Rivers State;" +
                    " it is in proximity to major shopping centres, food and entertainment outlets. Free WiFi is available and the" +
                    " hotel features a banquet/conference hall.\n\n\nDannic Hotels Port Harcourt offers 46 stylish, spacious and " +
                    "modern accommodations to ensure a comfortable and enjoyable stay. Each spacious room consists of air cooling " +
                    "units, en-suite bathrooms, toiletries, flat screen TV with cable channels, complimentary bottled water, a sofa," +
                    " duvet, work desk and chair, wall lamp, refrigerator and high-speed Internet access. These rooms are grouped" +
                    " into Standard Room, Victorian, Executive, Galaxy and Royal Suite.\n\n\nDannic Hotel Port Harcourt offers a " +
                    "fantastic range of tempting food option guests can choose from. The on-site restaurant is open for breakfast" +
                    " serving continental or traditional breakfast as well as an a la carte menu. Light snacks are also available " +
                    "at the bar or as a room service alternative. The on-site bar is the perfect setting for a business meeting," +
                    " pre-dinner drinks or evening cocktails. The bar features a selection of beers and ales.\n\n\nDannic Hotels " +
                    "Port Harcourt event hall accommodates up to 150 people and provides all necessary assistance in terms of " +
                    "technical requirements, food & beverage preferences. A range of audio-visual equipment such as LCD projectors," +
                    " overhead projectors, photocopying and email facilities, PA systems and flip charts are available for an " +
                    "additional fee. Teleconferencing and translating facilities can also be arranged at request.\n\n\nLaundry, " +
                    "car hire, and airport shuttle services are available at a surcharge. Services such as concierge services, " +
                    "room service, on-site parking space, round-the-clock security and 24 hours electricity are available at " +
                    "Dannic Hotels Port Harcourt.\n\nDannic Hotels Port Harcourt is a top-class hotel in Port Harcourt, Rivers." +
                    "\n\n\nTerms and Conditions\nCheck In- 2:00 pm (ID Required)\nCheck Out- 12:00 pm\nPet- pets are not " +
                    "allowed\nChildren- Children up to age 8 stay free\nPayment: Cash and Cards payments only\nCancellation:" +
                    " cancellation policy varies according to room types.\n\n\nInteresting Places near Dannic Hotel Port " +
                    "Harcourt\nAir Assault Golf Course (2.7 km)\nPort Harcourt Pleasure Park (2.1 km)\nLiberation Stadium " +
                    "(2.8 km)\nPolo Club (1.8 km)\nEveryday Supermarket (2.3 km)\nGenesis Deluxe Cinemas (2.3 km)\nSogaan " +
                    "Mall (1.6 km) Golf Course (1.5km)",

                    Email = "ph@dannichotels.com",
                    Phone = "+234 803 306 8958",

                    Address = new Address
                    {
                        City = "Port-Harcourt", State = "Rivers", Country = "Nigeria", PostalCode = "11111",
                        Street = "Plot 33, Circular Road, Presidential Estate, GRA,, Port Harcourt"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260075/yard/Rivers%20Hotels/Dannic%20Hotel/Dannic_Hotel_Image_ldzrz4.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260073/yard/Rivers%20Hotels/Dannic%20Hotel/Dannic_Hotel_6_ibwbcu.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260070/yard/Rivers%20Hotels/Dannic%20Hotel/Dannic_Hotel_4_embqth.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260069/yard/Rivers%20Hotels/Dannic%20Hotel/Dannic_Hotel2_efyqew.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260069/yard/Rivers%20Hotels/Dannic%20Hotel/Dannic_Hotel3_zzqy17.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Victorian Room",

                            Description = "Our spacious, stylish and modern rooms include king size bed, air-conditioning; " +
                            "satellite TV, Wi-Fi, occasional chair, functional desk, shower, intercom, mini bar.",

                            Price = 27000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260115/yard/Rivers%20Hotels/Dannic%20Hotel/RoomTypes/Victorian_Room_vcqvyo.png"
                        },
                        new RoomType
                        {
                            Name = "Galaxy Room",

                            Description = "Enjoy a more spacious Room with a King size bed in one of our Galaxy Rooms. This also " +
                            "includes amenities like air-conditioning; satellite TV, Wi-Fi, occasional chair, functional desk, " +
                            "bathtub intercom, mini bar.",

                            Price = 30000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260115/yard/Rivers%20Hotels/Dannic%20Hotel/RoomTypes/Galaxy_Room_pfhgo5.png"
                        },
                        new RoomType
                        {
                            Name = "Executive Room",

                            Description = "Enjoy our Victorian style Room with King size bed in one of our Executive Rooms. This " +
                            "also includes amenities like air-conditioning; satellite TV, Wi-Fi, occasional chair, functional desk," +
                            " shower, intercom, mini bar.",

                            Price = 35000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260114/yard/Rivers%20Hotels/Dannic%20Hotel/RoomTypes/Executive_Room_uuipc9.png"
                        },
                        new RoomType
                        {
                            Name = "Suite Room",

                            Description = "Need a little more space? Our suite Rooms have all the amenities of a Galaxy Room but " +
                            "also feature a separate living room with sofa chair, functional desk and a bedroom with king Size bed.",

                            Price = 45000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260115/yard/Rivers%20Hotels/Dannic%20Hotel/RoomTypes/Suite_Room_gcgnh1.png"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Ogeyi Place Hotel",

                    Description = "Ogeyi Place Hotel (Formerly Le Meridien Ogeyi Place) is a modern hotel located at 45 Tombia " +
                    "Street, GRA Phase II, Port Harcourt, Rivers State, Nigeria. It is located in the serene business district of" +
                    " the oil-rich Niger Delta Region. The comfortable and beautifully designed interior and features complement" +
                    " the magnificent and amazing view of the exterior.\n\n\nOgeyi Place Hotel (Formerly Le Meridien Ogeyi Place)" +
                    " has a total of 86 stylishly furnished rooms and suites of varying categories. Categories of rooms available" +
                    " in this hotel are the Diplomatic Suite, Deluxe Room and Executive Suite. These rooms are fully air-conditioned" +
                    " with Hypo-allergenic premium bedding, minibar, espresso maker, iron/ironing board, cable TV, in-room safe," +
                    " washer/dryer, telephone and a clean private bathroom with shower/tub combination, toiletries, dryer, bathrobes" +
                    " and slippers. Some of these rooms have a City View or a Pool View. Continental breakfast is served daily at a" +
                    " surcharge. Connecting/adjoining rooms are available for families.\n\n\nA couple of modern and awesome " +
                    "facilities are available at Ogeyi Place Hotel (Formerly Le Meridien Ogeyi Place). These facilities include: the" +
                    " Ororo Restaurant, with a buffet serving style and international cuisine, where guests will not just enjoy " +
                    "delicious delicacies but also experience a refreshing feel (Smoking is not allowed here and dressing style is" +
                    " Business Casual); the Ororo Bar, serving snacks and cocktails, grants you a poolside feel(Dressing style is" +
                    " casual and smoking is not permitted);  a free Wi-Fi internet access, an outdoor pool; a fitness centre and " +
                    "spa; and a 24-hour front desk service.\n\n\nIt offers additional services to its guests which include: daily" +
                    " housekeeping, ample parking space, laundry/dry cleaning, hair salon, luggage storage, express check-in/check" +
                    "-out, conference and meeting rooms, and 24-hour business centre. At Ogeyi Place Hotel (Formerly Le Meridien " +
                    "Ogeyi Place)  security procedure is top notch with security personnel around the premises round-the-clock to " +
                    "ensure the safety of its guests.\n\nOgeyi Place Hotel (Formerly Le Meridien Ogeyi Place) is a luxury hotel in" +
                    " Port Harcourt, Rivers.\n\n\nTerms and Conditions\nCheck In: From 3:00 PM (ID Required with Minimum Check-in " +
                    "age of 18)\nCheck Out: By 12:00 PM\nCancellation: Cancellation policies vary according to room type.\nChildren:" +
                    " All children are allowed.\nPets: Pets are not allowed.\nPayment: Credit Card Deposit Required\n\n\nInteresting" +
                    " Places To Visit Near Le Meridien Ogeyi Place\nLiberation Stadium (27-minute Walk)\nSharks Stadium (8.7km)\n" +
                    "University of Port Harcourt (18.3km).",

                    Email = "reservations@ogeyiplace.com",
                    Phone = "+234 84 461 770",
                    Address = new Address
                    {
                        City = "Port-Harcourt", State = "Rivers", Country = "Nigeria", PostalCode = "10231",
                        Street = "45 Tombia Street, G.R.A Phase II, Port Harcourt, Rivers State"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260513/yard/Rivers%20Hotels/Ogeyi%20Place%20Hotel/Ogeyi_place_Hotel_Image_otfukh.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260500/yard/Rivers%20Hotels/Ogeyi%20Place%20Hotel/Ogeyi_H4_uncqxe.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260496/yard/Rivers%20Hotels/Ogeyi%20Place%20Hotel/Ogeyi_H3_tmahdn.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260540/yard/Rivers%20Hotels/Ogeyi%20Place%20Hotel/Ogeyi_H2_x1tz9q.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260506/yard/Rivers%20Hotels/Ogeyi%20Place%20Hotel/Ogeyi_H5_dwuqhh.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Deluxe Room Pool View",

                            Description = "Featuring free toiletries, this double room includes a private bathroom with a walk-in " +
                            "shower, a bath and a bidet. The air-conditioned double room features a flat-screen TV, soundproof walls," +
                            " a tea and coffee maker, a seating area as well as pool views. The unit has 1 bed.",

                            Price = 82400M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260579/yard/Rivers%20Hotels/Ogeyi%20Place%20Hotel/RoomTypes/Deluxe_Room_Pool_View_o0kfpu.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Room City View",

                            Description = "The spacious suite features air conditioning, soundproof walls, as well as a private " +
                            "bathroom boasting a bath and a shower. This suite has a tea and coffee maker, flat-screen TV, pool " +
                            "views, as well as wine/champagne for guests. The unit has 2 beds.",

                            Price = 82400M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260573/yard/Rivers%20Hotels/Ogeyi%20Place%20Hotel/RoomTypes/Deluxe_Room_City_View_a9mfdf.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Suite City View",

                            Description = "The spacious suite offers air conditioning, soundproof walls, as well as a private " +
                            "bathroom boasting a walk-in shower and a bath. This suite has a tea and coffee maker, flat-screen TV," +
                            " pool views, as well as wine/champagne for guests. The unit has 2 beds..",

                            Price = 193600M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260589/yard/Rivers%20Hotels/Ogeyi%20Place%20Hotel/RoomTypes/Executive_Suite_City_View_vk27rz.jpg"
                        }
                    },

                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "Golden Tulip Hotel",

                    Description = "Golden Tulip Port Harcourt Hotel is located at Plot 1C Evo Crescent, GRA Phase II, Port " +
                    "Harcourt, Rivers State.\n\n\nGolden Tulip Port Harcourt Hotel is a luxury hotel in Port Harcourt, Rivers." +
                    "\n\n\nTerms and Conditions about Golden Tulip Port Harcourt Hotel\nCheck in: from 2:00pm\nCheck out: " +
                    "by 12:00pm\nPayment: Cash, Cards\nSmoking: in designated areas\nChildren: free lodgings for kids and babies.",

                    Email = "reservations@goldentuliphotels.com",
                    Phone = "+234 704 235 5124",
                    Address = new Address
                    {
                        City = "Port-Harcourt", State = "Rivers", Country = "Nigeria", PostalCode = "10231",
                        Street = "1C Evo Crescent, GRA Phase II, Port Harcourt"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260285/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/Golden_Tulip_Main_Image_xmu2he.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260274/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/Golden_Tulip_H4_mrrapt.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260294/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/Golden_Tulip_H2_slf9vx.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260290/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/Golden_Tulip_H1_sbpbwd.jpg",
                            "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260274/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/Golden_Tulip_H3_tmvuue.jpg"
                        }),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Standard Room",

                            Description = "1 king bed or 1 double bed, Room size: 22 m²/237 ft²,Outdoor view, Balcony/terrace, " +
                            "Shower, Executive lounge access",

                            Price = 76526M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260344/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/RoomTypes/Standard_Room_eof8vu.jpg"
                        },
                        new RoomType
                        {
                            Name = "Superior Room",

                            Description = "1 double bed, Room size: 22 m²/237 ft², Outdoor view, Balcony/terrace, Shower, " +
                            "Executive lounge access",

                            Price = 89295M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260349/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/RoomTypes/Suoerior_Room_l7aixj.jpg"
                        },
                        new RoomType
                        {
                            Name = "Deluxe Room",

                            Description = "1 double bed, Room size: 22 m²/237 ft², Outdoor view, Balcony/terrace, Shower, Executive" +
                            " lounge access",

                            Price = 111159M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260326/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/RoomTypes/Deluxe_Room_d9d0ij.jpg"
                        },
                        new RoomType
                        {
                            Name = "Executive Suite",

                            Description = "1 double bed, Balcony/terrace, Shower, Executive lounge access, See all room facilities",

                            Price = 146878M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260334/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/RoomTypes/Executive_suite_mgsjzt.jpg"
                        },
                             new RoomType
                        {
                            Name = "Diplomatic Suite",

                            Description = "1 double bed, Balcony/terrace, Shower, Executive lounge access, See all room facilities",

                            Price = 1206800M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260329/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/RoomTypes/Dipolmatic_Suite_p8t16t.jpg"
                        },
                        new RoomType
                        {
                            Name = "Presidential Suite",

                            Description = "2 double bed, Balcony/terrace, Shower, Executive lounge access, See all room facilities",

                            Price = 330000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260339/yard/Rivers%20Hotels/Golden%20Tulip%20Hotel/RoomTypes/Presidential_Suite_xp4k8s.jpg"
                        }
                    },
                    IsDelisted = false
                },

                new Hotel
                {
                    Name = "De Palms Hotel",

                    Description = "De Palms Hotel is located at No. 78 Elelenwo Street, GRA Phase 2, Port Harcourt. Rivers State," +
                    " Nigeria.\n\n\nNumbers of Rooms: 26 Rooms\n\n\nFacilities: Bar, Restaurant, GYM, Swimming Pool, Free WIFI, " +
                    "Pool Bar, complimentary breakfast, fitness center, Event Center.\n\nDe Palms Hotel, Port Harcourt is a luxury" +
                    " hotel in Port Harcourt, Rivers.\n\n\nTerms and Conditions about De Palms Hotel, Port Harcourt\nCheck IN: " +
                    "2PM\nCheck OUT: 12NOON\nNO Pets,\nNo Smoking,\nCash/Card Payment allowed,\nChildren and babies allowed.\n" +
                    "Cancellation policy: No charge deduction 48hours to your arrival, if cancelled 24hours prior to arrival a " +
                    "50% charge on the rate for one night will be charged. if cancelled less than 24hours to arrival, 100% charge" +
                    " on the rate for one night is deducted.\n\n\nInteresting Places to Visit:\nPort Harcourt Pleasure park,\nGarden" +
                    " city amusement park,\nGenesis cinema,\nvine yard.",

                    Email = "support@depalmshotel.ng",
                    Phone = "+234 915 311 1592",
                    Address = new Address
                    {
                        City = "Port-Harcourt", State = "Rivers", Country = "Nigeria", PostalCode = "10121",
                        Street = "78 Elelenwo Street, GRA PHASE 2, 500102, Port Harcourt"
                    },
                    Images = JsonSerializer.Serialize<string[]>(
                        new string[] {
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260166/yard/Rivers%20Hotels/De%20Palms%20Hotel/De_Palm_Hotel_Image_j8yy9x.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260160/yard/Rivers%20Hotels/De%20Palms%20Hotel/De_palm_H4_nlzien.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260159/yard/Rivers%20Hotels/De%20Palms%20Hotel/De_palm_H3_xqo65k.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260158/yard/Rivers%20Hotels/De%20Palms%20Hotel/De_palm_H1_dihket.jpg",
                        "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260158/yard/Rivers%20Hotels/De%20Palms%20Hotel/De_palm_H2_a2wzeh.jpg"
                        }
                    ),

                    RoomTypes = new RoomType[]
                    {
                        new RoomType
                        {
                            Name = "Super Deluxe",

                            Description = "Our super deluxe room is a premium accommodation option, offering an elevated level of " +
                            "comfort and luxury to guests.",

                            Price = 70000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260233/yard/Rivers%20Hotels/De%20Palms%20Hotel/RoomTypes/SuperDeluex_cxfw9t.jpg"
                        },
                        new RoomType
                        {
                            Name = "Royal Room",

                            Description = "Our royal room in the hotel is a premium accommodation option designed to offer guests" +
                            " a regal and luxurious experience fit for royalty.24/7 personalized concierge service to attend to the" +
                            " specific needs and preferences of our guests.",

                            Price = 80000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260229/yard/Rivers%20Hotels/De%20Palms%20Hotel/RoomTypes/Royal_Room_sqxtdk.jpg"
                        },
                        new RoomType
                        {
                            Name = "Ambassadorial",

                            Description = "Our ambassadorial suite is designed to provide a luxurious and comfortable experience " +
                            "for guests with high-profile status, offering a combination of upscale accommodations, personalized " +
                            "services, and a sense of exclusivity.",

                            Price = 180000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260227/yard/Rivers%20Hotels/De%20Palms%20Hotel/RoomTypes/Ambassadorial_Room_ccxtcr.jpg"
                        },
                        new RoomType
                        {
                            Name = "Presidential",

                            Description = "DOur presidential suite is the epitome of luxury and opulence , designed to provide an " +
                            "extravagant and exclusive experience for high-profile guests, often including heads of state, " +
                            "celebrities, or business leaders. The suite is typically the most spacious and lavish accommodation " +
                            "option available in the hotel, offering a combination of sophisticated design, premium amenities, " +
                            "and personalized services.",

                            Price = 300000M,
                            Discount = 5M,
                            Thumbnail = "https://res.cloudinary.com/dqrxujqor/image/upload/v1705260228/yard/Rivers%20Hotels/De%20Palms%20Hotel/RoomTypes/presidential_Room_eqb8gu.jpg"
                        }
                    },
                    IsDelisted = false
                }
            };

            // var hotels = new List<Hotel>();

            // hotels.Add(hotelsArray[0]);
            // hotels.Add(hotelsArray[1]);



            // foreach (Hotel hotel in hotels)
            // {
            //     await context.Hotels.AddAsync(hotel);
            // }

            context.Hotels.AddRange(hotelsArray);

            await context.SaveChangesAsync();
        }
    }
}