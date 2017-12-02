using System.Linq;
using System.Threading.Tasks;
using Bogus;
using ImageManager.Data.Domains;
using Microsoft.AspNetCore.Identity;

namespace ImageManager.Data.Seeds
{
    public class Seeder
    {
        private readonly NeptuneContext _context;
        private readonly UserManager<User> _userManager;

        public Seeder(UserManager<User> userManager, NeptuneContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task InitializeAsync()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var imageFaker = new Faker<Image>()
                .RuleFor(o => o.Path, f => f.Internet.Avatar())
                .RuleFor(o => o.Name, f => f.Person.FirstName)
                .RuleFor(o => o.Description, f => f.Lorem.Sentences(2))
                .RuleFor(o => o.DateCreated, f => f.Date.Past());

            var categoryFaker = new Faker<Category>()
                .RuleFor(o => o.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(o => o.DateCreated, f => f.Date.Past());

            var category = categoryFaker.Generate(10).ToList();

            var albumFaker = new Faker<Album>()
                .RuleFor(o => o.Name, f => f.Person.FirstName)
                .RuleFor(o => o.Description, f => f.Lorem.Sentences(5))
                .RuleFor(o => o.Category, f => f.PickRandom(category))
                .RuleFor(o => o.Images, f => imageFaker.Generate(f.Random.Number(5, 20)))
                .RuleFor(o => o.DateCreated, f => f.Date.Past());

            var userFaker = new Faker<User>()
                .RuleFor(o => o.Name, f => $"{f.Person.FirstName} {f.Person.LastName}")
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.Role, f => Role.None);

            var users = userFaker.Generate(50).ToList();
            users[0].UserName = "admin";
            users[0].Role = Role.Admin;
            users[0].Albums = albumFaker.Generate(10).ToList();
            users[1].UserName = "user";
            users[1].Role = Role.None;
            users[1].Albums = albumFaker.Generate(10).ToList();
            users.ForEach(async x =>
            {
                x.DateCreated = new Faker().Date.Past();
                x.DateModified = x.DateCreated;
                await _userManager.CreateAsync(x, "123");
            });

            await _context.SaveChangesAsync();
        }
    }
}