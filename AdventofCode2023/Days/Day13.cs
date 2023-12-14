namespace AdventofCode2023.Days
{
    public class Day13 : IDay
    {
        private int DayNum = 13;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day13()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string[] lines = File.ReadAllLines(InputFile);
            List<List<string>> patterns = new List<List<string>>();
            List<string> pattern = new List<string>();

            for(int lineNum = 0; lineNum < lines.Length; lineNum++)
            {
                if (String.IsNullOrEmpty(lines[lineNum]))
                {
                    patterns.Add(new List<string>(pattern));
                    pattern.Clear();
                    continue;
                }

                pattern.Add(lines[lineNum]);
            }

            if(pattern.Count > 0)
            {
                patterns.Add(new List<string>(pattern));
            }

            int sum = 0;

            foreach(var p in patterns)
            {
                int colCountLeftOfSplit = 0;
                int colCountAboveSplit = 0;
                bool verticalSplit = false;

                for(int i = 0; i < p.ElementAt(0).Length - 1; i++)
                {
                    string col1 = String.Join(String.Empty, p.Select(x => x[i]).ToList());
                    string col2 = String.Join(String.Empty, p.Select(x => x[i + 1]).ToList());

                    if (col1 == col2)
                    {
                        if(ReflectsOutwards(Orientation.Vertical, p, i, 1))
                        {
                            colCountLeftOfSplit = i;
                            sum += (i + 1);
                            verticalSplit = true;
                        }
                    }
                }


                if (!verticalSplit)
                {
                    for (int i = 0; i < p.Count - 1; i++)
                    {
                        string row1 = p[i];
                        string row2 = p[i + 1];

                        if (row1 == row2)
                        {
                            if (ReflectsOutwards(Orientation.Horizontal, p, i, 1))
                            {
                                sum += ((i + 1) * 100);
                                colCountAboveSplit = i;
                            }
                        }
                    }
                }

            }


            return sum;
        }

        public int Problem2()
        {
            string[] lines = File.ReadAllLines(InputFile);
            List<List<string>> patterns = new List<List<string>>();
            List<string> pattern = new List<string>();
            List<Tuple<Orientation, int>> patternOriginalAnswers = new List<Tuple<Orientation, int>>();

            for (int lineNum = 0; lineNum < lines.Length; lineNum++)
            {
                if (String.IsNullOrEmpty(lines[lineNum]))
                {
                    patterns.Add(new List<string>(pattern));
                    pattern.Clear();
                    continue;
                }

                pattern.Add(lines[lineNum]);
            }

            if (pattern.Count > 0)
            {
                patterns.Add(new List<string>(pattern));
            }

            

            // Part 1 answers
            foreach (var p in patterns)
            {
                bool verticalSplit = false;

                for (int i = 0; i < p.ElementAt(0).Length - 1; i++)
                {
                    string col1 = String.Join(String.Empty, p.Select(x => x[i]).ToList());
                    string col2 = String.Join(String.Empty, p.Select(x => x[i + 1]).ToList());

                    if (col1 == col2)
                    {
                        if (ReflectsOutwards(Orientation.Vertical, p, i, 1))
                        {
                            patternOriginalAnswers.Add(new Tuple<Orientation, int>(Orientation.Vertical, i));
                            verticalSplit = true;
                        }
                    }
                }


                if (!verticalSplit)
                {
                    for (int i = 0; i < p.Count - 1; i++)
                    {
                        string row1 = p[i];
                        string row2 = p[i + 1];

                        if (row1 == row2)
                        {
                            if (ReflectsOutwards(Orientation.Horizontal, p, i, 1))
                            {
                                patternOriginalAnswers.Add(new Tuple<Orientation, int>(Orientation.Horizontal, i));
                            }
                        }
                    }
                }
            }

            int patternIndex = 0;
            int sum = 0;

            foreach (var p in patterns)
            {
                Tuple<Orientation, int> patternOriginalSolution = patternOriginalAnswers.ElementAt(patternIndex);
                bool reflectionFound = false;

                for (int i = 0; i < p.ElementAt(0).Length - 1; i++)
                {
                    if (reflectionFound)
                    {
                        break;
                    }

                    string col1 = String.Join(String.Empty, p.Select(x => x[i]).ToList());
                    string col2 = String.Join(String.Empty, p.Select(x => x[i + 1]).ToList());

                    // These two cols are one char apart (the blur), try a normal reclection on other rows
                    if(OffByOne(col1, col2) != -1)
                    {
                        if (ReflectsOutwardsWithBlur(Orientation.Vertical, p, i, 1, true))
                        {
                            sum += (i + 1);
                            reflectionFound = true;
                        }
                    }

                    // Cols identical, can't reflect here again
                    if (col1 == col2 &&
                        !(patternOriginalSolution.Item1 == Orientation.Vertical && patternOriginalSolution.Item2 == i))
                    {
                        if (ReflectsOutwardsWithBlur(Orientation.Vertical, p, i, 1, false))
                        {
                            sum += (i + 1);
                            reflectionFound = true;
                        }
                    }
                }


                if (!reflectionFound)
                {
                    for (int i = 0; i < p.Count - 1; i++)
                    {
                        if (reflectionFound)
                        {
                            break;
                        }

                        string row1 = p[i];
                        string row2 = p[i + 1];

                        // These two cols are one char apart (the blur), try a normal reclection on other rows
                        if (OffByOne(row1, row2) != -1)
                        {
                            if (ReflectsOutwardsWithBlur(Orientation.Horizontal, p, i, 1, true))
                            {
                                sum += ((i + 1) * 100);
                                reflectionFound = true;
                            }
                        }

                        // Cols identical, can't reflect here again
                        if (row1 == row2 &&
                            !(patternOriginalSolution.Item1 == Orientation.Horizontal && patternOriginalSolution.Item2 == i))
                        {
                            if (ReflectsOutwardsWithBlur(Orientation.Horizontal, p, i, 1, false))
                            {
                                sum += ((i + 1) * 100);
                                reflectionFound = true;
                            }
                        }
                    }
                }

                patternIndex++;
            }

            return sum;
        }


        private bool ReflectsOutwards(Orientation orientation, List<string> pattern, int index, int offset)
        {

            string segment1 = String.Empty;
            string segment2 = String.Empty;

            if (orientation == Orientation.Vertical)
            {
                if (index - offset < 0 || index + offset + 1 >= pattern.ElementAt(0).Length)
                {
                    return true;
                }

                segment1 = String.Join(String.Empty, pattern.Select(x => x[index - offset]).ToList());
                segment2 = String.Join(String.Empty, pattern.Select(x => x[index + offset + 1]).ToList());
            }
            else
            {
                if (index - offset < 0 || index + offset + 1 >= pattern.Count)
                {
                    return true;
                }

                segment1 = pattern.ElementAt(index - offset);
                segment2 = pattern.ElementAt(index + offset + 1);
            }

            if(segment1 == segment2)
            {
                return ReflectsOutwards(orientation, pattern, index, ++offset);
            }
            else
            {
                return false;
            }
        }

        private bool ReflectsOutwardsWithBlur(Orientation orientation, List<string> pattern, int index, int offset, bool blurSet)
        {

            string segment1 = String.Empty;
            string segment2 = String.Empty;

            if (orientation == Orientation.Vertical)
            {
                if (index - offset < 0 || index + offset + 1 >= pattern.ElementAt(0).Length)
                {
                    return true;
                }

                segment1 = String.Join(String.Empty, pattern.Select(x => x[index - offset]).ToList());
                segment2 = String.Join(String.Empty, pattern.Select(x => x[index + offset + 1]).ToList());
            }
            else
            {
                if (index - offset < 0 || index + offset + 1 >= pattern.Count)
                {
                    return true;
                }

                segment1 = pattern.ElementAt(index - offset);
                segment2 = pattern.ElementAt(index + offset + 1);
            }

            if (segment1 == segment2)
            {
                return ReflectsOutwardsWithBlur(orientation, pattern, index, ++offset, blurSet);
            }
            else if(OffByOne(segment1, segment2) != -1 && !blurSet)
            {
                return ReflectsOutwardsWithBlur(orientation, pattern, index, ++offset, true);
            }
            else
            {
                return false;
            }
        }

        private int OffByOne(string a, string b)
        {
            int diffCount = 0;
            int diffIndex = 0;
            if(a.Length != b.Length)
            {
                return -1;
            }

            for(int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    diffIndex = i;
                    diffCount++;
                }
            }

            if(diffCount == 1)
            {
                return diffIndex;
            }

            return -1;
        }

        private enum Orientation
        {
            None,
            Horizontal,
            Vertical
        }
    }
}

