using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day4 : IDay
    {
        private int DayNum = 4;
        private string InputFile;

        public Day4()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}.txt";
        }

        public int Problem1()
        {
            string? line;
            int totalPoints = 0;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                Regex numbers = new Regex(@"\d+");

                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] lineParts = line.Split(':');
                    string[] gameInfo = lineParts[1].Split('|');
                    List<int> winning = new List<int>();
                    List<int> have = new List<int>();
                    MatchCollection winningNumbers = numbers.Matches(gameInfo[0]);
                    MatchCollection haveNumbers = numbers.Matches(gameInfo[1]);

                    foreach(Match match in winningNumbers)
                    {
                        winning.Add(Convert.ToInt32(match.Value));
                    }

                    foreach (Match match in haveNumbers)
                    {
                        have.Add(Convert.ToInt32(match.Value));
                    }

                    int cardScore = 0;
                    foreach(var num in have)
                    {
                        if (winning.Contains(num))
                        {
                            cardScore = cardScore == 0 ? 1 : cardScore * 2;
                            
                        }
                    }

                    totalPoints += cardScore;
                }
            }

            return totalPoints;
        }

        public int Problem2()
        {
            string? line;
            Dictionary<int, int> cardStock = new Dictionary<int, int>();
            Dictionary<int, int> cardMatchCount = new Dictionary<int, int>();
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                Regex numbers = new Regex(@"\d+");
                int cardNumber = 1;

                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] lineParts = line.Split(':');
                    string[] gameInfo = lineParts[1].Split('|');
                    List<int> winning = new List<int>();
                    List<int> have = new List<int>();
                    MatchCollection winningNumbers = numbers.Matches(gameInfo[0]);
                    MatchCollection haveNumbers = numbers.Matches(gameInfo[1]);

                    foreach (Match match in winningNumbers)
                    {
                        winning.Add(Convert.ToInt32(match.Value));
                    }

                    foreach (Match match in haveNumbers)
                    {
                        have.Add(Convert.ToInt32(match.Value));
                    }

                    int matchCount = 0;
                    foreach (var num in have)
                    {
                        if (winning.Contains(num))
                        {
                            matchCount++;

                        }
                    }

                    cardStock.Add(cardNumber, 1);
                    cardMatchCount.Add(cardNumber, matchCount);                  
                    cardNumber++;
                }

                foreach (var card in cardStock)
                {
                    PlayCard(card.Key, ref cardStock, ref cardMatchCount);
                }
            }

            return cardStock.Select(x => x.Value).Sum();
        }

        private void PlayCard(int cardNumber, ref Dictionary<int, int> cardStock, ref Dictionary<int, int> cardMatchCount)
        {
            int gameMatches = cardMatchCount.Where(x => x.Key == cardNumber).First().Value;

            for (int i = 0; i < cardStock[cardNumber]; i++)
            {
                foreach (var matchNum in Enumerable.Range(cardNumber + 1, gameMatches))
                {
                    cardStock[matchNum]++;
                }
            }
        }
    }
}

