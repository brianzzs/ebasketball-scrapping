using System.ComponentModel.DataAnnotations;

namespace eBasketballScrapper.Core.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public int Player_A { get; set; }
        public int Player_B { get; set; }
        public DateTime MatchDate { get; set; }
        public int Score_A { get; set; }
        public int Score_B { get; set; }
        public string Url { get; set; }
    }
}