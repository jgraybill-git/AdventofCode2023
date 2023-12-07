using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day7 : IDay
    {
        private int DayNum = 7;
        private string InputFile;
        private bool UseTestingFile = false;
        public Day7()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            List<string> inputHands = new List<string>();
            char[] cards = new char[] { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
            List<HandCategory> categories = new List<HandCategory>()
            {
                HandCategory.HighCard,
                HandCategory.OnePair,
                HandCategory.TwoPair,
                HandCategory.ThreeOfAKind,
                HandCategory.FullHouse,
                HandCategory.FourOfAKind,
                HandCategory.FiveOfAKind
            };

            List<int> bids = new List<int>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    inputHands.Add(line.Split(' ')[0]);
                    bids.Add(Convert.ToInt32(line.Split(' ')[1]));
                }
            }

            List<Tuple<string, int, HandCategory>> hands = new List<Tuple<string, int, HandCategory>>();

            for(int i = 0; i < inputHands.Count; i++)
            {
                Dictionary<char, int> handCardCounts = new Dictionary<char, int>();
                foreach(var card in cards)
                {
                    handCardCounts.Add(card, inputHands[i].Where(x => x.Equals(card)).Count());
                }
                HandCategory category = AssignHandCategory(handCardCounts);
                hands.Add(new Tuple<string, int, HandCategory>(inputHands[i], bids[i], category));
            }

            List<Tuple<string, int, HandCategory>> rankedHands = new List<Tuple<string, int, HandCategory>>();
            //int rankAssignment = 1;

            foreach(var category in categories)
            {
                var categoryHands = hands.Where(x => x.Item3 == category).ToList();
                OrderCategory(cards, ref categoryHands);

                foreach(var hand in categoryHands)
                {
                    rankedHands.Add(new Tuple<string, int, HandCategory>(hand.Item1, hand.Item2, hand.Item3));
                    //rankAssignment++;
                }
            }

            int winnings = 0;

            for(int i = 0; i < rankedHands.Count; i++)
            {
                winnings += (rankedHands[i].Item2 * (i + 1));
            }

            return winnings;
        }

        public int Problem2()
        {
            string? line;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {

                }
            }

            return 0;
        }

        private void OrderCategory(char[] cards, ref List<Tuple<string, int, HandCategory>> hands)
        {
            List<Tuple<string, int, HandCategory>> orderedHands = new List<Tuple<string, int, HandCategory>>();
            char[] cardsAsc = new char[cards.Length];

            int ascIndex = 0;
            for(int i = cards.Length - 1; i >= 0; i--)
            {
                cardsAsc[ascIndex] = cards[i];
                ascIndex++;
            }

            foreach(char card1 in cardsAsc)
            {
               foreach(char card2 in cardsAsc)
               {
                    foreach (char card3 in cardsAsc)
                    {
                        foreach (char card4 in cardsAsc)
                        {
                            foreach (char card5 in cardsAsc)
                            {
                                string hand = $"{card1}{card2}{card3}{card4}{card5}";
                                orderedHands.AddRange(hands.Where(x => x.Item1.Equals(hand)));
                            }
                        }
                    }
                } 
            }

            hands = orderedHands;
        }

        private HandCategory AssignHandCategory(Dictionary<char, int> hand)
        {
            if(hand.Values.Where(x => x == 5).Any())
            {
                return HandCategory.FiveOfAKind;
            }
            else if(hand.Values.Where(x => x == 4).Any())
            {
                return HandCategory.FourOfAKind;
            }
            else if (hand.Values.Where(x => x == 3).Any())
            {
                return hand.Values.Contains(2) ? HandCategory.FullHouse : HandCategory.ThreeOfAKind;
            }

            int pairCount = hand.Values.Where(x => x == 2).Count();
            switch (pairCount)
            {
                case 1:
                    return HandCategory.OnePair;
                case 2:
                    return HandCategory.TwoPair;
                default:
                    return HandCategory.HighCard;
            }
            
        }
    }

    enum HandCategory
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard
    }
}

