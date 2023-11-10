using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Domains;

namespace SakeFigureShop.Data
{
    public class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider, string username = "admin@example.com", string pw = "Admin@1234")
        {
            using (
                var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>())
            ) {
                var adminUid = await EnsureUser(serviceProvider, username, pw);
                var userUid = await EnsureUser(serviceProvider, "user@example.com", "User@1234");

                await EnsureRole(serviceProvider, adminUid, "Admin");
                await EnsureRole(serviceProvider, userUid, "User");

                var isBrandsAdded = await EnsureBrands(context);
                var isFlimsAdded = await EnsureFilms(context);
                var isProductsAdded = await EnsureProducts(context);
            }
        }
        private static async Task<bool> EnsureBrands(
            ApplicationDbContext context)
        {
            try
            {
                var brands = new List<Brand> {
                    new Brand{Name = "Good Smile Company"},
                    new Brand{Name = "Wonderful Works"},
                    new Brand{Name = "Alter"},
                    new Brand{Name = "Max Factory"},
                    new Brand{Name = "Taito"},
                    new Brand{Name = "Bandai"},
                    new Brand{Name = "Sega"},
                    new Brand{Name = "FuRyu"},
                    new Brand{Name = "Banpresto"},
                };

                brands.ForEach(brand => {
                    var b = context.Brands.Where(b => b.Name == brand.Name).FirstOrDefault();

                    if (b == null)
                    {
                        context.Brands.Add(brand);
                    }
                });
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task<bool> EnsureFilms(
            ApplicationDbContext context)
        {
            try
            {
                var films = new List<Film> {
                    new Film{Name = "One Piece"},
                    new Film{Name = "Chainsaw Man"},
                    new Film{Name = "Bullbuster"},
                    new Film{Name = "Jujutsu Kaisen"},
                    new Film{Name = "Naruto"},
                    new Film{Name = "Conan"},
                    new Film{Name = "Overlord"},
                    new Film{Name = "Bleach"},
                    new Film{Name = "Sword Art Online"},
                    new Film{Name = "SPY x FAMILY"},
                    new Film{Name = "Blue Lock"},
                    new Film{Name = "Boku no Hero Academia (My Hero Academia)"},
                    new Film{Name = "NieR: Automata"},
                    new Film{Name = "Dragon Ball"}
                };

                films.ForEach(film => {
                    var f = context.Films.Where(b => b.Name == film.Name).FirstOrDefault();

                    if (f == null)
                    {
                        context.Films.Add(film);
                    }
                });
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task<bool> EnsureProducts(
            ApplicationDbContext context)
        {
            try
            {
                var products = new List<Product> {
                    new Product{ 
                        Name = "Mô hình nhân vật Robin The grandline Lady Wanokuni vol.6",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Robin\n" +
                        "Anime: One piece\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 300000,
                        Quantity = 12,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3nc3k1eab",
                        BrandId = 9,
                        FilmId = 1
                    },
                    new Product{
                        Name = "Mô hình nhân vật Marco The Grandline Men Wanokuni vol.18",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Marco\n" +
                        "Anime: One piece\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 300000,
                        Quantity = 8,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3nbsbhu58",
                        BrandId = 9,
                        FilmId = 1
                    },
                    new Product{
                        Name = "Mô hình nhân vật Kid The Grandline Men Wanokuni vol.15",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Kid\n" +
                        "Anime: One piece\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 300000,
                        Quantity = 9,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3nbzchf1b",
                        BrandId = 9,
                        FilmId = 1
                    },
                    new Product{
                        Name = "Mô hình nhân vật Law The Grandline Men Wanokuni vol.14",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Law\n" +
                        "Anime: One piece\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 300000,
                        Quantity = 11,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3nbtq7n30",
                        BrandId = 9,
                        FilmId = 1
                    },
                    new Product{
                        Name = "Mô hình nhân vật Power",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Power\n" +
                        "Anime: Chainsaw Man\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 400000,
                        Quantity = 5,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3ncygojcb",
                        BrandId = 5,
                        FilmId = 2
                    },
                    new Product{
                        Name = "Mô hình nhân vật Denji",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Denji\n" +
                        "Anime: Chainsaw Man\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 350000,
                        Quantity = 8,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3ncrfub0c",
                        BrandId = 7,
                        FilmId = 2
                    },
                    new Product{
                        Name = "Mô hình nhân vật Makima",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Makima\n" +
                        "Anime: Chainsaw Man\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 400000,
                        Quantity = 3,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3nc96gj8f",
                        BrandId = 8,
                        FilmId = 2
                    },
                    new Product{
                        Name = "Mô hình nhân vật Aki Hayakawa Chain Spirits Vol.2",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Aki Hayakawa\n" +
                        "Anime: Chainsaw Man\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 350000,
                        Quantity = 3,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lh0b36xss9v7b3",
                        BrandId = 9,
                        FilmId = 2
                    },
                    new Product{
                        Name = "Mô hình nhân vật Denji",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Denji\n" +
                        "Anime: Chainsaw Man\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 1230000,
                        Quantity = 0,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3ncesqbbf",
                        BrandId = 7,
                        FilmId = 2
                    },
                    new Product{
                        Name = "Mô hình nhân vật Denji Vibration Star",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Denji\n" +
                        "Anime: Chainsaw Man\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 1000000,
                        Quantity = 13,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3ncesqbbf",
                        BrandId = 9,
                        FilmId = 2
                    },
                    new Product{
                        Name = "Mô hình nhân vật Shoto Todoroki",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Shoto Todoroki\n" +
                        "Anime: My hero Academia\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 750000,
                        Quantity = 12,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgyy617viwj682",
                        BrandId = 9,
                        FilmId = 12
                    },
                    new Product{
                        Name = "Mô hình nhân vật Izuku Midoriya",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Izuku Midoriya\n" +
                        "Anime: My hero Academia\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 750000,
                        Quantity = 12,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lh0cix53odf71a",
                        BrandId = 9,
                        FilmId = 12
                    },
                    new Product{
                        Name = "Mô hình nhân vật Mirko",
                        Description = "Hàng chính hãng Nhật Bản\n" +
                        "Nhân vật: Mirko\n" +
                        "Anime: My hero Academia\n" +
                        "Mọi thông tin chi tiết xin liên hệ f.b: facebook.com/sakefigure",
                        Price = 500000,
                        Quantity = 12,
                        ThumbnailImageUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lgzyg3nbpicy00",
                        BrandId = 9,
                        FilmId = 12
                    }
                };

                products.ForEach(product => {
                    var p = context.Products.Where(p => product.Name == p.Name).FirstOrDefault();
                    
                    if (p == null)
                    {
                        context.Products.Add(product);
                    }
                });
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        private static async Task<string> EnsureUser(
            IServiceProvider serviceProvider,
            string username,
            string initPw) 
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new User { 
                    UserName = username,
                    Email = username,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, initPw);
            }
            if (user == null)
            {
                throw new Exception("User did not created. Password Policy?");
            }


            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider,
            string uid,
            string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult ir;

            if (await roleManager.RoleExistsAsync(role) == false)
            {
                ir = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(uid);
            if (user == null)
            {
                throw new Exception("User not existing.");
            }

            ir = await userManager.AddToRoleAsync(user, role);

            return ir;
        }
    }
}
