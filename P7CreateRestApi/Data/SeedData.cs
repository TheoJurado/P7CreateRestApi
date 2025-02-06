using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.AspNetCore.Identity;

namespace P7CreateRestApi.Data
{
    public class SeedData
    {
        //public static void Initialize(IServiceProvider serviceProvider)
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new LocalDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<LocalDbContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            bool alreadyFeeded = context.Users.Any() && context.Trades.Any() && context.Ratings.Any();
            if (alreadyFeeded)
            {
                return;
            }/**/


            var result1 = await roleManager.CreateAsync(new Role { Name = "Admin" });
            var result2 = await roleManager.CreateAsync(new Role { Name = "User" });
            async Task CreateUser(string username, string password, string role)
            {
                if (await userManager.FindByNameAsync(username) == null)
                {
                    var user = new User
                    {
                        UserName = username,
                        Password = password,
                        Role = role,
                        Email = $"{username.ToLower()}@example.com",
                        FullName = $"{username} FullName",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
            await CreateUser("User1", "1234Pw!", "User");
            await CreateUser("User2", "2345Pw!", "Admin");
            await CreateUser("User3", "3456Pw!", "User");
            var result6 = await context.SaveChangesAsync();




            Trade trade1 = new Trade
            {
                Account = "AccountA",
                AccountType = "Retail",
                BuyQuantity = 100,
                SellQuantity = null,
                BuyPrice = 50.5,
                SellPrice = null,
                TradeDate = new DateTime(2024,5,18),
                TradeSecurity = "SecurityA",
                TradeStatus = "Pending",
                Trader = "Trader1",
                Benchmark = "BenchmarkA",
                Book = "BookA",
                CreationName = "Admin",
                CreationDate = new DateTime(2024, 5, 3),
                RevisionName = "RevisionA",
                RevisionDate = null,
                DealName = "DealA",
                DealType = "TypeA",
                SourceListId = "SL001",
                Side = "Buy"
            };
            Trade trade2 = new Trade
            {
                Account = "AccountB",
                AccountType = "Institutional",
                BuyQuantity = null,
                SellQuantity = 200,
                BuyPrice = null,
                SellPrice = 75.3,
                TradeDate = new DateTime(2024, 8, 15),
                TradeSecurity = "SecurityB",
                TradeStatus = "Completed",
                Trader = "Trader2",
                Benchmark = "BenchmarkB",
                Book = "BookB",
                CreationName = "System",
                CreationDate = new DateTime(2024, 8, 13),
                RevisionName = "Manager1",
                RevisionDate = new DateTime(2024, 8, 27),
                DealName = "DealB",
                DealType = "TypeB",
                SourceListId = "SL002",
                Side = "Sell"
            };
            Trade trade3 = new Trade
            {
                Account = "AccountC",
                AccountType = "Corporate",
                BuyQuantity = 150,
                SellQuantity = null,
                BuyPrice = 60.0,
                SellPrice = null,
                TradeDate = new DateTime(2024, 11, 4),
                TradeSecurity = "SecurityC",
                TradeStatus = "InProgress",
                Trader = "Trader3",
                Benchmark = "BenchmarkC",
                Book = "BookC",
                CreationName = "Admin",
                CreationDate = new DateTime(2024, 10, 22),
                RevisionName = "Supervisor1",
                RevisionDate = new DateTime(2024, 11, 28),
                DealName = "DealC",
                DealType = "TypeC",
                SourceListId = "SL003",
                Side = "Buy"
            };
            context.Trades.AddRange(trade1, trade2, trade3);
            context.SaveChanges();

            RuleName ruleName1 = new RuleName
            {
                Name = "Nom1",
                Description = "description1",
                Json = "J1",
                Template = "T1",
                SqlStr = "SS1",
                SqlPart = "SP1"
            };
            RuleName ruleName2 = new RuleName
            {
                Name = "Nom2",
                Description = "description2",
                Json = "J2",
                Template = "T2",
                SqlStr = "SS2",
                SqlPart = "SP2"
            };
            RuleName ruleName3 = new RuleName
            {
                Name = "Nom3",
                Description = "description3",
                Json = "J3",
                Template = "T3",
                SqlStr = "SS3",
                SqlPart = "SP3"
            };
            context.RuleNames.AddRange(ruleName1, ruleName2, ruleName3);
            context.SaveChanges();

            Rating rating1 = new Rating
            {
                MoodyRating = "MR1",
                SandPRating = "SPR1",
                FitchRating = "FR1",
                OrderNumber = 1
            };
            Rating rating2 = new Rating
            {
                MoodyRating = "MR2",
                SandPRating = "SPR2",
                FitchRating = "FR2",
                OrderNumber = 2
            };
            Rating rating3 = new Rating
            {
                MoodyRating = "MR3",
                SandPRating = "SPR3",
                FitchRating = "FR3",
                OrderNumber = 3
            };
            context.Ratings.AddRange(rating1, rating2, rating3);
            context.SaveChanges();

            CurvePoint curve1 = new CurvePoint
            {
                CurveId = 1,
                AsOfDate = new DateTime(2024, 1, 1),
                Term = 100,
                CurvePointValue = 1.1,
                CreationDate = new DateTime(2024, 1, 10)
            };
            CurvePoint curve2 = new CurvePoint
            {
                CurveId = 2,
                AsOfDate = new DateTime(2024, 2, 2),
                Term = 200,
                CurvePointValue = 2.2,
                CreationDate = new DateTime(2024, 2, 20)
            };
            CurvePoint curve3 = new CurvePoint
            {
                CurveId = 3,
                AsOfDate = new DateTime(2024, 3, 3),
                Term = 300,
                CurvePointValue = 3.3,
                CreationDate = new DateTime(2024, 3, 30)
            };
            context.Curves.AddRange(curve1, curve2, curve3);
            context.SaveChanges();

            BidList bid1 = new BidList
            {
                Account = "AccountX",
                BidType = "Type1",
                BidQuantity = 1000,
                AskQuantity = 500,
                Bid = 45.5,
                Ask = 46.2,
                Benchmark = "BenchmarkX",
                BidListDate = new DateTime(2024, 11, 1),
                Commentary = "First bid example",
                BidSecurity = "SecurityX",
                BidStatus = "Open",
                Trader = "TraderA",
                Book = "Book1",
                CreationName = "System",
                CreationDate = new DateTime(2024, 10, 1),
                RevisionName = "Revision1",
                RevisionDate = null,
                DealName = "DealX",
                DealType = "Standard",
                SourceListId = "SLX01",
                Side = "Buy"
            };

            BidList bid2 = new BidList
            {
                Account = "AccountY",
                BidType = "Type2",
                BidQuantity = 2000,
                AskQuantity = null,
                Bid = 50.0,
                Ask = null,
                Benchmark = "BenchmarkY",
                BidListDate = new DateTime(2024, 2, 20),
                Commentary = "Second bid example",
                BidSecurity = "SecurityY",
                BidStatus = "Closed",
                Trader = "TraderB",
                Book = "Book2",
                CreationName = "Admin",
                CreationDate = new DateTime(2024, 2, 2),
                RevisionName = "ManagerB",
                RevisionDate = new DateTime(2024, 2, 22),
                DealName = "DealY",
                DealType = "Advanced",
                SourceListId = "SLY02",
                Side = "Sell"
            };

            BidList bid3 = new BidList
            {
                Account = "AccountZ",
                BidType = "Type3",
                BidQuantity = 1500,
                AskQuantity = 1200,
                Bid = 48.75,
                Ask = 49.5,
                Benchmark = "BenchmarkZ",
                BidListDate = new DateTime(2024, 3, 30),
                Commentary = "Third bid example",
                BidSecurity = "SecurityZ",
                BidStatus = "Pending",
                Trader = "TraderC",
                Book = "Book3",
                CreationName = "Automation",
                CreationDate = new DateTime(2024, 3, 1),
                RevisionName = "SupervisorC",
                RevisionDate = new DateTime(2024, 4, 1),
                DealName = "DealZ",
                DealType = "Custom",
                SourceListId = "SLZ03",
                Side = "Buy"
            };

            context.Bids.AddRange(bid1, bid2, bid3);
            context.SaveChanges();
        }
    }
}
