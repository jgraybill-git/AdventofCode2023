using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day5 : IDay
    {
        private int DayNum = 5;
        private string InputFile;
        bool UseTestingFile = true;
        public Day5()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            List<double> seeds = new List<double>();
            List<List<double>> seedSoil = new List<List<double>>();
            List<List<double>> soilFertilizer = new List<List<double>>();
            List<List<double>> fertilizerWater = new List<List<double>>();
            List<List<double>> waterLight = new List<List<double>>();
            List<List<double>> lightTemperature = new List<List<double>>();
            List<List<double>> temperatureHumidity = new List<List<double>>();
            List<List<double>> humidityLocation = new List<List<double>>();

            List<List<double>> currentMap = new List<List<double>>();

            List<string> maps = new List<string>()
                {
                    "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light",
                    "light-to-temperature", "temperature-to-humidity", "humidity-to-location"
                };
            Regex numbers = new Regex(@"\d+");
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line.Trim()))
                    {
                        continue;
                    }

                    if (line.Contains("seeds:"))
                    {
                        List<string> fileSeeds = line.Split(':')[1].Trim().Split(' ').ToList();
                        foreach(var seed in fileSeeds)
                        {
                            seeds.Add(Convert.ToDouble(seed));
                        }
                        continue;
                    }

                    foreach (var map in maps)
                    {
                        if ($"{map} map:".ToLower() == line.Trim().ToLower())
                        {
                            switch (maps.IndexOf(map))
                            {
                                case 0:
                                    currentMap = seedSoil;
                                    break;
                                case 1:
                                    currentMap = soilFertilizer;
                                    break;
                                case 2:
                                    currentMap = fertilizerWater;
                                    break;
                                case 3:
                                    currentMap = waterLight;
                                    break;
                                case 4:
                                    currentMap = lightTemperature;
                                    break;
                                case 5:
                                    currentMap = temperatureHumidity;
                                    break;
                                case 6:
                                    currentMap = humidityLocation;
                                    break;
                            }
                        }
                    }

                    MatchCollection matches = numbers.Matches(line);
                    if (matches.Count == 3)
                    {
                        double rangeLen = Convert.ToDouble(matches.ElementAt(2).Value);
                        double dest = Convert.ToDouble(matches.ElementAt(0).Value);
                        double source = Convert.ToDouble(matches.ElementAt(1).Value);

                        currentMap.Add(new List<double>() { source, dest, rangeLen });
                    }
                }
            }

            List<List<List<double>>> mappings = new List<List<List<double>>>()
            {
                seedSoil,
                soilFertilizer,
                fertilizerWater,
                waterLight,
                lightTemperature,
                temperatureHumidity,
                humidityLocation
            };

            double minSeedDistance = double.MaxValue;
            foreach (var seed in seeds)
            {
                double mapVal = seed;
                foreach (var mapping in mappings)
                {
                    foreach (var valRange in mapping)
                    {
                        double sourceStart = valRange.ElementAt(0);
                        if (mapVal >= sourceStart && mapVal < sourceStart + valRange.ElementAt(2))
                        {
                            double mapPos = mapVal - sourceStart;
                            mapVal = valRange.ElementAt(1) + mapPos;
                            break;
                        }
                    }
                }

                minSeedDistance = Math.Min(minSeedDistance, mapVal);
                
            }

            return Convert.ToInt32(minSeedDistance);
        }

        public int Problem2()
        {
            string? line;
            List<double> seeds = new List<double>();
            List<List<double>> seedSoil = new List<List<double>>();
            List<List<double>> soilFertilizer = new List<List<double>>();
            List<List<double>> fertilizerWater = new List<List<double>>();
            List<List<double>> waterLight = new List<List<double>>();
            List<List<double>> lightTemperature = new List<List<double>>();
            List<List<double>> temperatureHumidity = new List<List<double>>();
            List<List<double>> humidityLocation = new List<List<double>>();

            List<List<double>> currentMap = new List<List<double>>();
            List<Tuple<double, double>> seedPairs = new List<Tuple<double, double>>();

            List<string> maps = new List<string>()
                {
                    "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light",
                    "light-to-temperature", "temperature-to-humidity", "humidity-to-location"
                };
            Regex numbers = new Regex(@"\d+");
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line.Trim()))
                    {
                        continue;
                    }

                    if (line.Contains("seeds:"))
                    {
                        List<string> fileSeeds = line.Split(':')[1].Trim().Split(' ').ToList();
                        for (int i = 0; i < fileSeeds.Count - 1; i += 2)
                        {
                            double seed = Convert.ToDouble(fileSeeds[i]);
                            double seedCount = Convert.ToDouble(fileSeeds[i + 1]);
                            seedPairs.Add(new Tuple<double, double>(seed, seedCount));
                        }
                        continue;
                    }

                    foreach (var map in maps)
                    {
                        if ($"{map} map:".ToLower() == line.Trim().ToLower())
                        {
                            switch (maps.IndexOf(map))
                            {
                                case 0:
                                    currentMap = seedSoil;
                                    break;
                                case 1:
                                    currentMap = soilFertilizer;
                                    break;
                                case 2:
                                    currentMap = fertilizerWater;
                                    break;
                                case 3:
                                    currentMap = waterLight;
                                    break;
                                case 4:
                                    currentMap = lightTemperature;
                                    break;
                                case 5:
                                    currentMap = temperatureHumidity;
                                    break;
                                case 6:
                                    currentMap = humidityLocation;
                                    break;
                            }
                        }
                    }

                    MatchCollection matches = numbers.Matches(line);
                    if (matches.Count == 3)
                    {
                        double rangeLen = Convert.ToDouble(matches.ElementAt(2).Value);
                        double dest = Convert.ToDouble(matches.ElementAt(0).Value);
                        double source = Convert.ToDouble(matches.ElementAt(1).Value);

                        currentMap.Add(new List<double>() { source, dest, rangeLen });
                    }
                }
            }

            List<List<List<double>>> mappings = new List<List<List<double>>>()
            {
                seedSoil,
                soilFertilizer,
                fertilizerWater,
                waterLight,
                lightTemperature,
                temperatureHumidity,
                humidityLocation
            };

            List<Tuple<double, double, int, string, string>> mapAtLevel = new List<Tuple<double, double, int, string, string>>();
            List<Tuple<double, double, int, string, string>> addToLevel = new List<Tuple<double, double, int, string, string>>();
            List<double> minSeedResult = new List<double>();
            foreach (var seedRange in seedPairs)
            {
                mapAtLevel.Clear();
                var minSeed = seedRange.Item1;
                var maxSeed = minSeed + seedRange.Item2 - 1;
                int currentMapIndex = 1;

                List<Tuple<double, double>> mapped = MapRange(minSeed, maxSeed, seedSoil);
                var seedGuid = Guid.NewGuid().ToString();
                foreach (var item in mapped)
                {
                    mapAtLevel.Add(new Tuple<double, double, int, string, string>(item.Item1, item.Item2, currentMapIndex, "-1", seedGuid));
                }

                
                foreach(var map in mappings.Where(x => mappings.IndexOf(x) != 0))
                {
                    //var rangeParentID = mapAtLevel.Where(x => x.Item3 == currentMapIndex).First().Item5;
                    foreach (var range in mapAtLevel.Where(x => x.Item3 == currentMapIndex))
                    {
                        mapped = MapRange(range.Item1, range.Item2, map);
                        //foreach (var rangeRes in mapped)
                        //{
                        //    addToLevel.Add(new Tuple<double, double, int, string, string>(rangeRes.Item1, rangeRes.Item2, currentMapIndex + 1, rangeParentID, Guid.NewGuid().ToString()));
                        //}
                    }
                    foreach (var item in addToLevel)
                    {
                        mapAtLevel.Add(item);
                    }
                    addToLevel.Clear();
                    currentMapIndex++;
                }

                minSeedResult.Add(mapAtLevel.Where(x => x.Item3 == 7).Select(x => x.Item1).Min());

            }

            var minSeedRange = mapAtLevel.Where(x => x.Item1 == minSeedResult.Min()).First();
            double minSeedDistance = double.MaxValue;

            for(double i = minSeedRange.Item1; i < minSeedRange.Item2; i++)
            {
                var parentGuid = minSeedRange.Item4;
                var parentLevel = minSeedRange.Item3;
                for(int j = parentLevel; j > 0; j--)
                {
                    if(!mapAtLevel.Where(x => x.Item3 == j - 1 && x.Item4 == parentGuid).Any())
                    {
                        break;
                    }
                    parentGuid = mapAtLevel.Where(x => x.Item3 == j && x.Item4 == parentGuid).First().Item4;
                }

                if(1 == 5)
                {
                    int fdsa = 432;
                }
            }
            


            return Convert.ToInt32(minSeedResult.Min());
        }


        private List<Tuple<double, double>> MapRange(double srcStart, double srcEnd, List<List<double>> dstRanges)
        {
            List<Tuple<double, double>> mappings = new List<Tuple<double, double>>();
            List<Tuple<double, double, double>> srcMapped = new List<Tuple<double, double, double>>();

            foreach (List<double> dstRange in dstRanges)
            {
                double dstStart = dstRange.ElementAt(0);
                double dstEnd = dstStart + dstRange.ElementAt(2) - 1;
                double offset = dstRange.ElementAt(1) - dstStart;

                // |--------------|
                // |------------------|
                if (srcStart == dstStart && srcEnd < dstEnd)
                {
                    srcMapped.Add(new Tuple<double, double, double>(srcStart, srcEnd, offset));
                }
                //     |--------------|
                // |------------------|
                else if (srcStart > dstStart && srcEnd == dstEnd)
                {
                    srcMapped.Add(new Tuple<double, double, double>(srcStart, srcEnd, offset));
                }
                // |--------------|
                //      |------------------|
                else if (srcStart < dstStart & srcEnd > dstStart && srcEnd <= dstEnd)
                {
                    srcMapped.Add(new Tuple<double, double, double>(dstStart, srcEnd, offset));
                }
                //           |--------------|
                // |------------------|
                else if (srcEnd > dstEnd && srcStart >= dstStart && srcStart < dstEnd)
                {
                    srcMapped.Add(new Tuple<double, double, double>(srcStart, dstEnd, offset));
                }
                // |--------------|
                //                |------------------|
                else if (srcEnd == dstStart)
                {
                    srcMapped.Add(new Tuple<double, double, double>(srcEnd, srcEnd, offset));
                }
                //                    |--------------|
                // |------------------|
                else if (srcStart == dstEnd)
                {
                    srcMapped.Add(new Tuple<double, double, double>(srcStart, srcStart, offset));
                }
                //   |--------------|         OR      |------------------|
                // |------------------|               |------------------|
                else if (srcStart >= dstStart && srcEnd <= dstEnd)
                {
                    srcMapped.Add(new Tuple<double, double, double>(srcStart, srcEnd, offset));
                }
                // |--------------|                       OR           |------------------|
                //                   |----------------|                                      |------------------|
                else if (srcEnd < dstStart || srcEnd < dstStart)
                {
                    // UNMAPPED
                }
            }

            var orderedMappedRanges = srcMapped.OrderBy(x => x.Item1).ToList();

            for(int i = 0; i < orderedMappedRanges.Count; i++)
            {
                var srcRangeMapped = orderedMappedRanges.ElementAt(i);

                var nextRangeMapped = new Tuple<double, double, double>(-1, -1, -1);

                if(orderedMappedRanges.Count > 1 && i <= orderedMappedRanges.Count - 2)
                {
                    nextRangeMapped = orderedMappedRanges.ElementAt(i + 1);
                }

                // Add unmapped cases between ordered ranges
                if (i == 0 && srcRangeMapped.Item1 > srcStart)
                {
                    mappings.Add(new Tuple<double, double>(srcStart, srcRangeMapped.Item1 - 1));
                }
                else if(nextRangeMapped.Item1 != -1 && i == orderedMappedRanges.Count - 2 && nextRangeMapped.Item2 < srcEnd)
                {
                    mappings.Add(new Tuple<double, double>(nextRangeMapped.Item2 + 1, srcEnd));
                }
                else if(nextRangeMapped.Item1 != -1 &&  nextRangeMapped.Item1 - srcRangeMapped.Item2 > 1)
                {
                    mappings.Add(new Tuple<double, double>(srcRangeMapped.Item2 + 1, nextRangeMapped.Item1 -1));
                }

                mappings.Add(new Tuple<double, double>(srcRangeMapped.Item1 + srcRangeMapped.Item3,
                    srcRangeMapped.Item2 + srcRangeMapped.Item3));
            }

            if(orderedMappedRanges.Count == 0)
            {
                mappings.Add(new Tuple<double, double>(srcStart, srcEnd));
            }

            return mappings;
        }

        // Temporary brute force
        public int Problem2BruteForce()
        {
            string? line;
            List<double> seeds = new List<double>();
            List<double> seeds2 = new List<double>();
            List<List<double>> seedSoil = new List<List<double>>();
            List<List<double>> soilFertilizer = new List<List<double>>();
            List<List<double>> fertilizerWater = new List<List<double>>();
            List<List<double>> waterLight = new List<List<double>>();
            List<List<double>> lightTemperature = new List<List<double>>();
            List<List<double>> temperatureHumidity = new List<List<double>>();
            List<List<double>> humidityLocation = new List<List<double>>();

            List<List<double>> currentMap = new List<List<double>>();

            List<string> maps = new List<string>()
                {
                    "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light",
                    "light-to-temperature", "temperature-to-humidity", "humidity-to-location"
                };
            Regex numbers = new Regex(@"\d+");
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line.Trim()))
                    {
                        continue;
                    }

                    if (line.Contains("seeds:"))
                    {
                        List<string> fileSeeds = line.Split(':')[1].Trim().Split(' ').ToList();
                        
                        for (int fs = 0; fs < fileSeeds.Count - 1; fs += 2)
                        {
                            double seed = Convert.ToDouble(fileSeeds[fs]);
                            double seedCount = Convert.ToDouble(fileSeeds[fs + 1]);
                            for(double seedI = 0; seedI < seedCount; seedI++)
                            {
                                if(fs >= 10)
                                {
                                    seeds2.Add(seed + seedI);
                                }
                                else
                                {
                                    seeds.Add(seed + seedI);
                                }
                                
                                if(seedI % 10000 == 0)
                                {
                                    Console.WriteLine(fs + " at " + seedI + " / " + seedCount);
                                }
                            }
                            Console.WriteLine("Done building seed list " + fs);
                        }
                        continue;
                    }

                    foreach (var map in maps)
                    {
                        if ($"{map} map:".ToLower() == line.Trim().ToLower())
                        {
                            switch (maps.IndexOf(map))
                            {
                                case 0:
                                    currentMap = seedSoil;
                                    break;
                                case 1:
                                    currentMap = soilFertilizer;
                                    break;
                                case 2:
                                    currentMap = fertilizerWater;
                                    break;
                                case 3:
                                    currentMap = waterLight;
                                    break;
                                case 4:
                                    currentMap = lightTemperature;
                                    break;
                                case 5:
                                    currentMap = temperatureHumidity;
                                    break;
                                case 6:
                                    currentMap = humidityLocation;
                                    break;
                            }
                        }
                    }

                    MatchCollection matches = numbers.Matches(line);
                    if (matches.Count == 3)
                    {
                        double rangeLen = Convert.ToDouble(matches.ElementAt(2).Value);
                        double dest = Convert.ToDouble(matches.ElementAt(0).Value);
                        double source = Convert.ToDouble(matches.ElementAt(1).Value);

                        currentMap.Add(new List<double>() { source, dest, rangeLen });
                    }
                }
            }

            List<List<List<double>>> mappings = new List<List<List<double>>>()
            {
                seedSoil,
                soilFertilizer,
                fertilizerWater,
                waterLight,
                lightTemperature,
                temperatureHumidity,
                humidityLocation
            };

            double minSeedDistance = double.MaxValue;
            foreach (var seed in seeds)
            {
                double mapVal = seed;
                foreach (var mapping in mappings)
                {
                    foreach (var valRange in mapping)
                    {
                        double sourceStart = valRange.ElementAt(0);
                        if (mapVal >= sourceStart && mapVal < sourceStart + valRange.ElementAt(2))
                        {
                            double mapPos = mapVal - sourceStart;
                            mapVal = valRange.ElementAt(1) + mapPos;
                            break;
                        }
                    }
                }

                minSeedDistance = Math.Min(minSeedDistance, mapVal);
            }

            foreach (var seed in seeds2)
            {
                double mapVal = seed;
                foreach (var mapping in mappings)
                {
                    foreach (var valRange in mapping)
                    {
                        double sourceStart = valRange.ElementAt(0);
                        if (mapVal >= sourceStart && mapVal < sourceStart + valRange.ElementAt(2))
                        {
                            double mapPos = mapVal - sourceStart;
                            mapVal = valRange.ElementAt(1) + mapPos;
                            break;
                        }
                    }
                }

                minSeedDistance = Math.Min(minSeedDistance, mapVal);

            }

            return Convert.ToInt32(minSeedDistance);
        }

    }
}

