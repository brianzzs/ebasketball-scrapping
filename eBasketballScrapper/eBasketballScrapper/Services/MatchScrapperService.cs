using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBasketballScrapper.Core.Entities;
using HtmlAgilityPack;
namespace eBasketballScrapper.Application.Services
{
    public class MatchScrapperService
    {
        public MatchScrapperService() { }

        public async Task<string> CallUrl(string url, HttpClient client)
        {
            Console.WriteLine($"Starting Request for URL: {url}");

            var response = await client.GetStringAsync(url);

            return response;

        }

        public async Task<Dictionary<string, Dictionary<string, string>>> GetPlayersNames(HtmlNode tableRow)
        {
            //playerNumber 0 would be "Home"
            //playerNumber 1 would be "Away"
            Dictionary<string, Dictionary<string, string>> match = new Dictionary<string, Dictionary<string, string>>();

            var HeadToHead = tableRow.Descendants("td").ToList().Where(e => !e.InnerText.Equals("-")).ToList()[1].InnerText.Replace("\n", "").Trim().Split(" v ").ToList();
            var playerAName = HeadToHead[0].Split("(")[1].Split(")").FirstOrDefault();
            var playerBName = HeadToHead[1].Split("(")[1].Split(")").FirstOrDefault();
            var teamAName = HeadToHead[0].Split("(").FirstOrDefault();
            var teamBName = HeadToHead[1].Split("(").FirstOrDefault();

            match[playerAName] = new Dictionary<string, string>
                {
                    { "Player", playerAName },
                    { "Team", teamAName }
                };

            match[playerBName] = new Dictionary<string, string>
                {
                    { "Player", playerBName },
                    { "Team", teamBName }
                };
            return match;
        }

        public async Task<List<Game>> ParseMatchTable(string response)
        {

            var matchList = new List<Game>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            var matches = doc.DocumentNode.Descendants("table").ToList()[0].Descendants("tr").ToList();
            matches.RemoveAt(0);
            foreach (var match in matches)
            {
                var playerName = GetPlayersNames(match);
            }

            return matchList;
        }


    }
}
