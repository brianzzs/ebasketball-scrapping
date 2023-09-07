
using eBasketballScrapper.Application.Services;

HttpClient client = new HttpClient();

var page = 1;

var service = new MatchScrapperService();


while (page <= 30)
{
    var url = $"https://betsapi.com/le/23105/Ebasketball-Battle--4x5mins/p.{page}";
    var response = service.CallUrl(url, client).Result;
    var teamName = await service.ParseMatchTable(response);
    System.Console.WriteLine(teamName);
    page++;
}

