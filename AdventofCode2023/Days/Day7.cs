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
            char[] cards = new char[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

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
                HandCategory category = AssignHandCategory(handCardCounts, false);
                hands.Add(new Tuple<string, int, HandCategory>(inputHands[i], bids[i], category));
            }

            List<Tuple<string, int, HandCategory>> rankedHands = new List<Tuple<string, int, HandCategory>>();

            foreach(var category in categories)
            {
                var categoryHands = hands.Where(x => x.Item3 == category).ToList();
                OrderCategory(cards, ref categoryHands);

                foreach(var hand in categoryHands)
                {
                    rankedHands.Add(new Tuple<string, int, HandCategory>(hand.Item1, hand.Item2, hand.Item3));
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
            List<string> inputHands = new List<string>();
            char[] cards = new char[] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

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

            for (int i = 0; i < inputHands.Count; i++)
            {
                Dictionary<char, int> handCardCounts = new Dictionary<char, int>();
                foreach (var card in cards)
                {
                    handCardCounts.Add(card, inputHands[i].Where(x => x.Equals(card)).Count());
                }
                HandCategory category = AssignHandCategory(handCardCounts, true);
                hands.Add(new Tuple<string, int, HandCategory>(inputHands[i], bids[i], category));
            }

            List<Tuple<string, int, HandCategory>> rankedHands = new List<Tuple<string, int, HandCategory>>();

            foreach (var category in categories)
            {
                var categoryHands = hands.Where(x => x.Item3 == category).ToList();
                OrderCategory(cards, ref categoryHands);

                foreach (var hand in categoryHands)
                {
                    rankedHands.Add(new Tuple<string, int, HandCategory>(hand.Item1, hand.Item2, hand.Item3));
                }
            }

            int winnings = 0;

            for (int i = 0; i < rankedHands.Count; i++)
            {
                winnings += (rankedHands[i].Item2 * (i + 1));
            }

            return winnings;
        }

        private void OrderCategory(char[] cards, ref List<Tuple<string, int, HandCategory>> hands)
        {
            List<Tuple<string, int, HandCategory>> orderedHands = new List<Tuple<string, int, HandCategory>>();

            foreach(char card1 in cards)
            {
               foreach(char card2 in cards)
               {
                    foreach (char card3 in cards)
                    {
                        foreach (char card4 in cards)
                        {
                            foreach (char card5 in cards)
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

        private HandCategory AssignHandCategory(Dictionary<char, int> hand, bool jIsJoker)
        {
            int jokerCount = 0;
            if (jIsJoker)
            {
                jokerCount = hand['J'];
            }

            if(hand.Values.Where(x => x == 5).Any())
            {
                return HandCategory.FiveOfAKind;
            }
            else if(hand.Values.Where(x => x == 4).Any())
            {
                if(jIsJoker && (jokerCount == 1 || jokerCount == 4))
                {
                    return HandCategory.FiveOfAKind;
                }

                return HandCategory.FourOfAKind;
            }
            else if (hand.Values.Where(x => x == 3).Any())
            {
                if (jIsJoker && jokerCount > 0)
                {
                    if(jokerCount == 2)
                    {
                        return HandCategory.FiveOfAKind;
                    }
                    else if(jokerCount == 1)
                    {
                        return HandCategory.FourOfAKind;
                    }
                    else if(jokerCount == 3 && hand.Values.Contains(2))
                    {
                        return HandCategory.FiveOfAKind;
                    }
                    else
                    {
                        return HandCategory.FourOfAKind;
                    }
                }

                return hand.Values.Contains(2) ? HandCategory.FullHouse : HandCategory.ThreeOfAKind;
            }

            int pairCount = hand.Values.Where(x => x == 2).Count();

            if (jIsJoker && jokerCount > 0)
            {
                if(jokerCount == 1)
                {
                    switch (pairCount)
                    {
                        case 1:
                            return HandCategory.ThreeOfAKind;
                        case 2:
                            return HandCategory.FullHouse;
                        default:
                            return HandCategory.OnePair;
                    }
                }
                else if (jokerCount == 2) // It's one of the pairs
                {
                    switch (pairCount)
                    {
                        case 1:
                            return HandCategory.ThreeOfAKind;
                        case 2:
                            return HandCategory.FourOfAKind;
                        default:
                            return HandCategory.ThreeOfAKind;
                    }
                }
            }

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

