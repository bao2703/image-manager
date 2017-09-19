using System.Linq;
using System.Threading.Tasks;
using Bogus;
using ImageManager.Data.Domains;
using Microsoft.AspNetCore.Identity;

namespace ImageManager.Data.Seeds
{
    public class Seeder
    {
        private readonly UserManager<User> _userManager;

        public Seeder(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task InitializeAsync(NeptuneContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var imageFaker = new Faker<Image>()
                .RuleFor(o => o.Path, f => f.Internet.Avatar())
                .RuleFor(o => o.Description, f => f.Lorem.Sentence());

            var albumFaker = new Faker<Album>()
                .RuleFor(o => o.Name, f => f.Person.FirstName)
                .RuleFor(o => o.Description, f => f.Lorem.Sentence())
                .RuleFor(o => o.Images, f => imageFaker.Generate(f.Random.Number(5, 10)));

            var userFaker = new Faker<User>()
                .RuleFor(o => o.Name, f => $"{f.Person.FirstName} {f.Person.LastName}")
                .RuleFor(o => o.UserName, f => f.Person.UserName)
                .RuleFor(o => o.Email, f => f.Person.Email);

            var users = userFaker.Generate(100).ToList();
            users[0].UserName = "admin";
            users[0].Albums = albumFaker.Generate(10).ToList();
            users[0].Images = imageFaker.Generate(100).ToList();
            users.ForEach(async x => await _userManager.CreateAsync(x, "123"));

            await context.SaveChangesAsync();
        }
    }
}