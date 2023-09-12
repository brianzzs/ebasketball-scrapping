using eBasketballScrapper.Core.Entities;
using HtmlAgilityPack;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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


                return BasicMatchInfoFromAntiScrapperSystem;
            }
            var HeadToHead = tdList[1].InnerText.Replace("\n", "").Trim().Split(" v ").ToList();

            var playerASplit = HeadToHead[0].Split("(");
            var playerBSplit = HeadToHead[1].Split("(");
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
            return BasicMatchInfo;
        }

        public BasicMatchInfoDTO GetMatchInfo(HtmlNode tableRow)
        {
            var tdToBeWorkedWith = tableRow.Descendants("td").ToList().FirstOrDefault();
            DateTime matchDate = DateTime.Parse(tdToBeWorkedWith.GetAttributeValue("data-dt", ""));
            string baseUrl = "https://betsapi.com";
            var url = tableRow.Descendants("a").ToList().LastOrDefault().GetAttributeValue("href", "");
            int id = Int32.Parse(url.Split("/")[2]);
            CultureInfo userCulture = CultureInfo.CurrentCulture;

            string formattedDateTime = matchDate.ToString(userCulture.DateTimeFormat.ShortDatePattern + " " + userCulture.DateTimeFormat.LongTimePattern);

            var MatchInfo = new BasicMatchInfoDTO()
            {
                Id = id,
                MatchDate = DateTime.Parse(formattedDateTime),
                Url = baseUrl + url,
            };

            return MatchInfo;

        }

        public BasicMatchInfoDTO GetScores(HtmlNode tableRow)
        {
            var tdToBeWorkedWith = tableRow.Descendants("td").ToList().LastOrDefault();
            var scoresList = tdToBeWorkedWith.InnerText.Replace("\n", "").Trim().Split("-").ToList();

            if (scoresList.Contains("View"))
            {
                var noScoreOnTd = new BasicMatchInfoDTO()
                {
                    ScoreA = 0,
                    ScoreB = 0,
                };
                return noScoreOnTd;
            }
            var scores = new BasicMatchInfoDTO()
            {
                ScoreA = Int32.Parse(scoresList[0]),
                ScoreB = Int32.Parse(scoresList[1]),
            };
            return scores;
        }

        public List<Game> ParseMatchTable(string response)
        {

            var matchList = new List<Game>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            var matches = doc.DocumentNode.Descendants("table").ToList()[0].Descendants("tr").ToList();
            matches.RemoveAt(0);
            foreach (var match in matches)
            {

                var HeadToHeadInfo = GetPlayersNameAndTeam(match);
                var MatchInfo = GetMatchInfo(match);
                var scores = GetScores(match);
                var game = new Game()
                {
                    Id = MatchInfo.Id,
                    PlayerA = HeadToHeadInfo.PlayerA,
                    TeamA = HeadToHeadInfo.TeamA,
                    ScoreA = scores.ScoreA,
                    PlayerB = HeadToHeadInfo.PlayerB,
                    TeamB = HeadToHeadInfo.TeamB,
                    ScoreB = scores.ScoreB,
                    MatchDate = MatchInfo.MatchDate,
                    Url = MatchInfo.Url,
                };
                matchList.Add(game);
            }


            return matchList;
        }


    }
}
