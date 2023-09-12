using eBasketballScrapper.Application.Services;
using eBasketballScrapper.Infrastructure.Data;


HttpClient client = new HttpClient();
var service = new MatchScrapperService();

var page = 1500;

while (page >= 1)
{
    using var context = new eBasketballDbContext();
    var url = $"https://betsapi.com/le/23105/Ebasketball-Battle--4x5mins/p.{page}";
    var response = service.CallUrl(url, client).Result;
    var games = service.ParseMatchTable(response);

    // Just to see if our backlog is different from what is being inserted on the database
    var select = from match in context.Matches
                 select match.Id;

    foreach (var match in games)
    {
        var connection = new eBasketballDbContext();
        if (!select.Contains(match.Id))
        {
            Console.WriteLine($"Adding Match {match.Url} to the Database");
            connection.Matches.Add(match);
        }
        else
        {
            Console.WriteLine("Match already on the Database");
            continue;
        }

        connection.SaveChanges();
    }
    context.SaveChanges();
    page--;
    Console.WriteLine($"Page: {page}");
}

