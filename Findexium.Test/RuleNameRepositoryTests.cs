using Microsoft.EntityFrameworkCore;
using P7CreateRestApi;
using Xunit;
using Dot.Net.WebApi.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Controllers;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using P7CreateRestApi.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Findexium.Test
{
    public class RuleNameRepositoryTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        //private readonly IJwtTokenService _jwtTokenService;
        //private readonly IServiceProvider _serviceProvider;

        public RuleNameRepositoryTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            //_serviceProvider = factory.Services;
            //_jwtTokenService = _serviceProvider.GetRequiredService<IJwtTokenService>();
        }

        [Fact]
        public async Task ValidateWithAdminAcount_ShouldRetourneOk()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            var jwtTokenService = scopedProvider.GetRequiredService<IJwtTokenService>();

            // Simuler un token JWT pour Admin (exemple simple)
            var user = new User
            {
                UserName = "User2",
                Password = "2345Pw!",
                Role = "Admin",
                Email = "mail@example.com",
                FullName = "User2 FullName",
                EmailConfirmed = true
            };//User Admin
            var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
            var result = await userManager.CreateAsync(user, "2345Pw!");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, "Admin");

            var token = jwtTokenService.GenerateJwtToken(user);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/trade/list")
            {
                Headers = { { "Authorization", "Bearer " + token } }
            };

            // Assert
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ValidateWithNonAutoriseAcount_Retourne403()
        {
            using var scope = _factory.Services.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            var jwtTokenService = scopedProvider.GetRequiredService<IJwtTokenService>();

            // Simuler un token JWT pour Admin (exemple simple)
            var user = new User
            {
                UserName = "User1",
                Password = "1234Pw!",
                Role = "User",
                Email = "mail@example.com",
                FullName = "User1 FullName",
                EmailConfirmed = true
            };
            var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
            var result = await userManager.CreateAsync(user, "1234Pw!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }

            var token = jwtTokenService.GenerateJwtToken(user);

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/trade/list")
            {
                Headers = { { "Authorization", "Bearer " + token } }
            };

            var response = await _client.SendAsync(request);

            // Assert : Doit renvoyer 403 Forbidden
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task ValidateWithoutAuthentication_ShouldReturn401()
        {
            // Arrange : Création d'une requête GET vers l'endpoint sans ajouter d'en-tête d'authentification.
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/trade/list");

            // Act : Exécution de la requête.
            var response = await _client.SendAsync(request);

            // Assert : On s'attend à recevoir un code 401 Unauthorized.
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        //CRUD
        private DbContextOptions<LocalDbContext> CreateSqlDatabaseOptions()
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase("TestDatabaseP7")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            return options;
        }
        
        [Fact]
        public async void AddAndDeletRuleName_ShouldRemoveRuleNameFromDB()
        {
            //Arrange
            var options = CreateSqlDatabaseOptions();
            LocalDbContext _context = new LocalDbContext(options);
            _context.Database.EnsureDeleted();//delete data base befor creat if to make sure it was clear
            _context.Database.EnsureCreated();//creat database
            var _ruleNameService = new RuleNameRepository(_context);

            var rule = new RuleName
            {
                Name = "name",
                Description = "description rule",
                Json = "string",
                Template = "string",
                SqlStr = "string",
                SqlPart = "string"
            };
            //Act : add rulename to DB
            _ruleNameService.Add(rule);
            await _context.SaveChangesAsync();

            //Assert : confirm the addition
            Assert.Equal(1, await _context.RuleNames.CountAsync());
            var ruleAdded = await _context.RuleNames.FirstAsync();
            Assert.Equal("description rule", ruleAdded.Description);
            Assert.Equal(1, ruleAdded.Id);

            //Act2 : delet rulename
            _ruleNameService.Delete(1);
            await _context.SaveChangesAsync();

            //Assert 2 : make sure the rulename is no longuer in DB
            Assert.Equal(0, await _context.RuleNames.CountAsync());

            //delet DB
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async void AddAndUpdateRuleName_ShouldUpdateRuleNameInDB()
        {
            //Arrange
            var options = CreateSqlDatabaseOptions();
            LocalDbContext _context = new LocalDbContext(options);
            _context.Database.EnsureDeleted();//delete data base befor creat if to make sure it was clear
            _context.Database.EnsureCreated();//creat database
            var _ruleNameService = new RuleNameRepository(_context);

            var rule1 = new RuleName
            {
                Name = "name",
                Description = "description rule",
                Json = "string",
                Template = "string",
                SqlStr = "string",
                SqlPart = "string"
            };
            var rule2 = new RuleName
            {
                Name = "2name",
                Description = "2description rule",
                Json = "2string",
                Template = "2string",
                SqlStr = "2string",
                SqlPart = "2string"
            };

            //Act : add rulename to DB
            _ruleNameService.Add(rule1);
            await _context.SaveChangesAsync();

            //Assert : confirm the addition
            Assert.Equal(1, await _context.RuleNames.CountAsync());
            var ruleAdded = await _context.RuleNames.FirstAsync();
            Assert.Equal("description rule", ruleAdded.Description);
            Assert.Equal(1, ruleAdded.Id);

            //Act2 : update rulename
            _ruleNameService.Update(1, rule2);
            await _context.SaveChangesAsync();

            //Assert 2 : make sure the rulename was update in DB
            Assert.Equal(1, await _context.RuleNames.CountAsync());
            var rule2Added = await _context.RuleNames.FirstAsync();
            Assert.Equal("2description rule", rule2Added.Description);
            Assert.Equal(1, rule2Added.Id);

            //delet DB
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async void AddAndReadRuleName_ShouldReturnRuleNameDataFromDB()
        {
            //Arrange
            var options = CreateSqlDatabaseOptions();
            LocalDbContext _context = new LocalDbContext(options);
            _context.Database.EnsureDeleted();//delete data base befor creat if to make sure it was clear
            _context.Database.EnsureCreated();//creat database
            var _ruleNameService = new RuleNameRepository(_context);

            var rule = new RuleName
            {
                Name = "this is the name",
                Description = "this is the description rule",
                Json = "this is the  Json",
                Template = "this is the Template",
                SqlStr = "this is the SqlStr",
                SqlPart = "this is the SqlPart"
            };
            //Act : add rulename to DB
            _ruleNameService.Add(rule);
            await _context.SaveChangesAsync();

            //Assert : confirm the addition
            Assert.Equal(1, await _context.RuleNames.CountAsync());
            var ruleAdded = await _context.RuleNames.FirstAsync();
            Assert.Equal("this is the description rule", ruleAdded.Description);
            Assert.Equal(1, ruleAdded.Id);

            //Assert2 : confirm the data
            Assert.Equal(rule, _ruleNameService.FindById(1));
            Assert.Equal("this is the description rule", _ruleNameService.FindByName("this is the name").Description);

            //delet DB
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async void ValidateRuleName_ShouldAddRuleNameToDBFromController()
        {
            //Arrange
            var options = CreateSqlDatabaseOptions();
            LocalDbContext _context = new LocalDbContext(options);
            _context.Database.EnsureDeleted();//delete data base befor creat if to make sure it was clear
            _context.Database.EnsureCreated();//creat database
            var _ruleNameService = new RuleNameRepository(_context);
            var _controller = new RuleNameController(_ruleNameService);

            var rule = new RuleName
            {
                Name = "Validate name",
                Description = "description rule",
                Json = "string",
                Template = "string",
                SqlStr = "string",
                SqlPart = "string"
            };

            // Act
            var result = await _controller.Validate(rule) as OkObjectResult;
            var rules = result.Value as List<RuleName>;

            // Assert
            Assert.NotNull(rules);
            Assert.Single(rules);
            Assert.Equal("Validate name", rules[0].Name);

            //delet DB
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}