using System.ComponentModel.DataAnnotations;

namespace eBasketballScrapper.Core.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public int PlayerA { get; set; }

        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public int PlayerB { get; set; }
        public DateTime MatchDate { get; set; }
        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
        public string Url { get; set; }
    }
}