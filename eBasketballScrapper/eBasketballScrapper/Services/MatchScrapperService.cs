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

        public BasicMatchInfoDTO GetPlayersNameAndTeam(HtmlNode tableRow)
        {
            var tdToBeWorkedWith = tableRow.Descendants("td").ToList().Where(e => !e.InnerText.Equals("-")).ToList();
            try
            {
                if (tableRow.InnerHtml.Contains("<img "))
                {
                    var tdListWithoutImg = tableRow.Descendants("td").ToList().Where(e => !e.InnerText.Equals("-")).ToList();
                    var playersList = tdListWithoutImg[2].InnerHtml.Split("/")[3].Replace("%28", "(").Replace("%29", ")").Replace("-", " ").Split(" vs ").ToList();
                    var firstPlayer = playersList[0].Split("Esports")[0].Trim();
                    var secondPlayer = playersList[1].Split("Esports")[0].Trim();
                    var firstPlayerTeamName = firstPlayer.Split("(")[0].Trim();
                    var firstPlayerName = firstPlayer.Split("(")[1].Replace(")", "").Trim();
                    var secondPlayerTeamName = secondPlayer.Split("(")[0].Trim();
                    var secondPlayerName = secondPlayer.Split("(")[1].Replace(")", "").Trim();

                    var BasicMatchInfoFromAntiScrapperSystem = new BasicMatchInfoDTO
                    {
                        PlayerA = firstPlayerName,
                        PlayerB = secondPlayerName,
                        TeamA = firstPlayerTeamName,
                        TeamB = secondPlayerTeamName
                    };

                    Console.WriteLine($"First AntiScrapper \nPlayer 1: {BasicMatchInfoFromAntiScrapperSystem.PlayerA}, Player 2: {BasicMatchInfoFromAntiScrapperSystem.PlayerB}, Team A: {BasicMatchInfoFromAntiScrapperSystem.TeamA}, Team B: {BasicMatchInfoFromAntiScrapperSystem.TeamB}");

                    return BasicMatchInfoFromAntiScrapperSystem;
                }

                var tdList = tableRow.Descendants("td").ToList().Where(e => !e.InnerText.Equals("-")).ToList();
                if (!tdList[1].Descendants("a").ToList()[0].InnerHtml.Contains("(") || !tdList[1].Descendants("a").ToList()[1].InnerHtml.Contains("("))
                {
                    var teams = tableRow.Descendants("td").ToList().Where(e => !e.InnerText.Equals("-")).ToList()[2].InnerHtml;
                    var playersList = teams.Split("/")[3].Replace("%28", "(").Replace("%29", ")").Replace("-", " ").Split(" vs ").ToList();
                    var firstPlayer = playersList[0].Split("Esports")[0].Trim();
                    var secondPlayer = playersList[1].Split("Esports")[0].Trim();
                    var firstPlayerTeamName = firstPlayer.Split("(")[0].Trim();
                    var firstPlayerName = firstPlayer.Split("(")[1].Replace(")", "").Trim();
                    var secondPlayerTeamName = secondPlayer.Split("(")[0].Trim();
                    var secondPlayerName = secondPlayer.Split("(")[1].Replace(")", "").Trim();

                    var BasicMatchInfoFromAntiScrapperSystem = new BasicMatchInfoDTO
                    {
                        PlayerA = firstPlayerName,
                        PlayerB = secondPlayerName,
                        TeamA = firstPlayerTeamName,
                        TeamB = secondPlayerTeamName
                    };

                    Console.WriteLine($"Second AntiScrapper \nPlayer 1: {BasicMatchInfoFromAntiScrapperSystem.PlayerA}, Player 2: {BasicMatchInfoFromAntiScrapperSystem.PlayerB}, Team A: {BasicMatchInfoFromAntiScrapperSystem.TeamA}, Team B: {BasicMatchInfoFromAntiScrapperSystem.TeamB}");

                    return BasicMatchInfoFromAntiScrapperSystem;
                }
                var HeadToHead = tdList[1].InnerText.Replace("\n", "").Trim().Split(" v ").ToList();

                //[1] = match, [0] = data, [2] = result


                var playerASplit = HeadToHead[0].Split("(");
                var playerBSplit = HeadToHead[1].Split("(");

                if (playerASplit.Length < 2 || playerBSplit.Length < 2)
                {
                    Console.WriteLine($"Unexpected player data format");
                }

                var playerAName = HeadToHead[0].Split("(")[1].Split(")")[0];
                var playerBName = HeadToHead[1].Split("(")[1].Split(")")[0];
                var teamAName = HeadToHead[0].Split("(")[0].Trim();
                var teamBName = HeadToHead[1].Split("(")[0].Trim();

                var BasicMatchInfo = new BasicMatchInfoDTO
                {
                    PlayerA = playerAName,
                    PlayerB = playerBName,
                    TeamA = teamAName,
                    TeamB = teamBName
                };

                Console.WriteLine($"No AntiScrapper \nPlayer A: {BasicMatchInfo.PlayerA}, Player B: {BasicMatchInfo.PlayerB}, Team A: {BasicMatchInfo.TeamA}, Team B: {BasicMatchInfo.TeamB}");
                return BasicMatchInfo;
            }
            catch
            {
                throw new Exception("Blablabla");
                return null;
            }

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
                GetPlayersNameAndTeam(match);
            }

            return matchList;
        }


    }
}
