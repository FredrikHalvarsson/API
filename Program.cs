using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Return all people
            app.MapGet("/people", async (ApplicationDbContext context) =>
            {
                var people = await context.People
                .Include(p =>p.Interests)
                    .ThenInclude(i=>i.Interest)
                .Include(p => p.Interests)
                    .ThenInclude(i => i.Links)
                .ToListAsync();
                if (people == null || !people.Any())
                {
                    return Results.NotFound("No people found");
                }
                return Results.Ok(people);
            });
            // Create person
            app.MapPost("people", async (Person person, ApplicationDbContext context) =>
            {
                context.People.Add(person);
                await context.SaveChangesAsync();
                return Results.Created($"/people/{person.PersonId}", person);
            });

            // Get person by Id
            app.MapGet("/people/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var person = await context.People
                .Include(p => p.Interests)
                    .ThenInclude(i => i.Interest)
                .Include(p => p.Interests)
                    .ThenInclude(i=>i.Links)
                .FirstOrDefaultAsync(p => p.PersonId == id);
                if (person == null)
                {
                    return Results.NotFound("Person not found");
                }
                return Results.Ok(person);
            });

            // Edit person
            app.MapPut("/people/{id:int}", async (int id, Person updatedPerson, ApplicationDbContext context) =>
            {
                var person = await context.People.FindAsync(id);
                if (person == null)
                {
                    return Results.NotFound("Person not found");
                }
                person.FirstName = updatedPerson.FirstName;
                person.LastName = updatedPerson.LastName;
                person.Email = updatedPerson.Email;
                person.PhoneNumber = updatedPerson.PhoneNumber;
                context.People.Update(person);
                await context.SaveChangesAsync();
                return Results.Ok(person);
            });

            // Delete person
            app.MapDelete("/people/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var person = await context.People.FindAsync(id);

                if (person == null)
                {
                    return Results.NotFound("Person not found");
                }
                context.People.Remove(person);
                await context.SaveChangesAsync();
                return Results.Ok($"Person with ID: {id} was deleted");
            });

            /////////////////////////////////////////////////////////////////////////////
            //                             -/ Interests /-                             //

            // Return all interests
            app.MapGet("/interests", async (ApplicationDbContext context) =>
            {
                var interests = await context.Interests.ToListAsync();
                if (interests == null || !interests.Any())
                {
                    return Results.NotFound("No interests found");
                }
                return Results.Ok(interests);
            });
            // Create interest
            app.MapPost("interests", async (Interest interest, ApplicationDbContext context) =>
            {
                context.Interests.Add(interest);
                await context.SaveChangesAsync();
                return Results.Created($"/interests/{interest.InterestId}", interest);
            });

            // Delete interest
            app.MapDelete("/interests/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var interest = await context.Interests.FindAsync(id);

                if (interest == null)
                {
                    return Results.NotFound("Interest not found");
                }
                context.Interests.Remove(interest);
                await context.SaveChangesAsync();
                return Results.Ok($"Interest with ID: {id} was deleted");
            });

            /////////////////////////////////////////////////////////////////////////////
            //                        -/ PersonWithInterests /-                        //

            // Return all PersonWithInterests
            app.MapGet("/personwithinterests", async (ApplicationDbContext context) =>
            {
                var personwithinterests = await context.PeopleWithInterests
                .Include(pi=>pi.Interest)
                .ToListAsync();
                if (personwithinterests == null || !personwithinterests.Any())
                {
                    return Results.NotFound("No personwithinterests found");
                }
                return Results.Ok(personwithinterests);
            });

            // Create PersonWithInterests
            app.MapPost("personwithinterests", async (PersonWithInterests personWithInterests, ApplicationDbContext context) =>
            {
                context.PeopleWithInterests.Add(personWithInterests);
                await context.SaveChangesAsync();
                return Results.Created($"/interests/{personWithInterests.Id}", personWithInterests);
            });

            // Delete PersonWithInterests
            app.MapDelete("/personwithinterests/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var personWithInterests = await context.PeopleWithInterests.FindAsync(id);

                if (personWithInterests == null)
                {
                    return Results.NotFound("PersonWithInterests not found");
                }
                context.PeopleWithInterests.Remove(personWithInterests);
                await context.SaveChangesAsync();
                return Results.Ok($"PersonWithInterests with ID: {id} was deleted");
            });

            /////////////////////////////////////////////////////////////////////////////
            //                               -/ Links /-                               //

            // Create Link
            app.MapPost("links", async (Link link, ApplicationDbContext context) =>
            {
                context.Links.Add(link);
                await context.SaveChangesAsync();
                return Results.Created($"/interests/{link.LinkId}", link);
            });

            // Delete Link
            app.MapDelete("/links/{id:int}", async (int id, ApplicationDbContext context) =>
            {
                var link = await context.Links.FindAsync(id);

                if (link == null)
                {
                    return Results.NotFound("Link not found");
                }
                context.Links.Remove(link);
                await context.SaveChangesAsync();
                return Results.Ok($"Sink with ID: {id} was deleted");
            });

            app.Run();
        }
    }
}
