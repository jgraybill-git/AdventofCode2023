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
        bool UseTestingFile = false;
        public Day5()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}.txt";
            if (UseTestingFile)
            {
                InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}Testing.txt";
            }
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

            double minSeedDistance = double.MaxValue;
            List<Tuple<double, double, int>> mapAtLevel = new List<Tuple<double, double, int>>();

            foreach (var seedRange in seedPairs)
            {
                var minSeed = seedRange.Item1;
                var maxSeed = minSeed + seedRange.Item2 - 1;

                List<Tuple<double, double>> mapped = MapRange(minSeed, maxSeed, seedSoil);

                foreach (var t in mapped)
                {
                    mapAtLevel.Add(new Tuple<double, double, int>(t.Item1, t.Item2, 1));
                    //Console.WriteLine("MAPPED: " + t.Item1 + "----" + t.Item2);
                }

                ///===
                List<Tuple<double, double, int>> toAdd = new List<Tuple<double, double, int>>();
                foreach (var range in mapAtLevel.Where(x => x.Item3 == 1))
                {
                    mapped = MapRange(range.Item1, range.Item2, soilFertilizer);

                    foreach (var t in mapped)
                    {
                        toAdd.Add(new Tuple<double, double, int>(t.Item1, t.Item2, 2));
                        //Console.WriteLine("MAPPED: " + t.Item1 + "----" + t.Item2);
                    }
                }
                foreach(var item in toAdd)
                {
                    mapAtLevel.Add(item);
                }
                toAdd.Clear();

                ///===
                foreach (var range in mapAtLevel.Where(x => x.Item3 == 2))
                {
                    mapped = MapRange(range.Item1, range.Item2, fertilizerWater);

                    foreach (var t in mapped)
                    {
                        toAdd.Add(new Tuple<double, double, int>(t.Item1, t.Item2, 3));
                        //Console.WriteLine("MAPPED: " + t.Item1 + "----" + t.Item2);
                    }
                }
                foreach (var item in toAdd)
                {
                    mapAtLevel.Add(item);
                }
                toAdd.Clear();

                ///===
                foreach (var range in mapAtLevel.Where(x => x.Item3 == 3))
                {
                    mapped = MapRange(range.Item1, range.Item2, waterLight);

                    foreach (var t in mapped)
                    {
                        toAdd.Add(new Tuple<double, double, int>(t.Item1, t.Item2, 4));
                        //Console.WriteLine("MAPPED: " + t.Item1 + "----" + t.Item2);
                    }
                }
                foreach (var item in toAdd)
                {
                    mapAtLevel.Add(item);
                }
                toAdd.Clear();

                ///===
                foreach (var range in mapAtLevel.Where(x => x.Item3 == 4))
                {
                    mapped = MapRange(range.Item1, range.Item2, lightTemperature);

                    foreach (var t in mapped)
                    {
                        toAdd.Add(new Tuple<double, double, int>(t.Item1, t.Item2, 5));
                        //Console.WriteLine("MAPPED: " + t.Item1 + "----" + t.Item2);
                    }
                }
                foreach (var item in toAdd)
                {
                    mapAtLevel.Add(item);
                }
                toAdd.Clear();

                ///===
                foreach (var range in mapAtLevel.Where(x => x.Item3 == 5))
                {
                    mapped = MapRange(range.Item1, range.Item2, temperatureHumidity);

                    foreach (var t in mapped)
                    {
                        toAdd.Add(new Tuple<double, double, int>(t.Item1, t.Item2, 6));
                        //Console.WriteLine("MAPPED: " + t.Item1 + "----" + t.Item2);
                    }
                }
                foreach (var item in toAdd)
                {
                    mapAtLevel.Add(item);
                }
                toAdd.Clear();

                ///===
                foreach (var range in mapAtLevel.Where(x => x.Item3 == 6))
                {
                    mapped = MapRange(range.Item1, range.Item2, humidityLocation);

                    foreach (var t in mapped)
                    {
                        toAdd.Add(new Tuple<double, double, int>(t.Item1, t.Item2, 7));
                        //Console.WriteLine("MAPPED: " + t.Item1 + "----" + t.Item2);
                    }
                }
                foreach (var item in toAdd)
                {
                    mapAtLevel.Add(item);
                }
                toAdd.Clear();

                foreach (var th in mapAtLevel.Where(x => x.Item3 == 7).ToList())
                {
                    //Console.WriteLine("=====================" + th.Item1 + " " + th.Item2);
                }

            }
            
            return Convert.ToInt32(mapAtLevel.Where(x => x.Item3 == 7).Select(x => x.Item1).Min()); //Convert.ToInt32(minSeedDistance);
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

        public int Problem2Old()
        {
            string? line;
            List<double> seeds = new List<double>();
            List<List<double>> soilSeed = new List<List<double>>();
            List<List<double>> fertilizerSoil = new List<List<double>>();
            List<List<double>> waterFertilizer = new List<List<double>>();
            List<List<double>> lightWater = new List<List<double>>();
            List<List<double>> temperatureLight = new List<List<double>>();
            List<List<double>> humidityTemperature = new List<List<double>>();
            List<List<double>> locationHumidity = new List<List<double>>();

            List<List<double>> currentMap = new List<List<double>>();

            List<string> maps = new List<string>()
                {
                    "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light",
                    "light-to-temperature", "temperature-to-humidity", "humidity-to-location"
                };

            List<Tuple<double, double>> seedPairs = new List<Tuple<double, double>>();

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

                        for(int i = 0; i < fileSeeds.Count - 1; i+=2)
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
                                    currentMap = soilSeed;
                                    break;
                                case 1:
                                    currentMap = fertilizerSoil;
                                    break;
                                case 2:
                                    currentMap = waterFertilizer;
                                    break;
                                case 3:
                                    currentMap = lightWater;
                                    break;
                                case 4:
                                    currentMap = temperatureLight;
                                    break;
                                case 5:
                                    currentMap = humidityTemperature;
                                    break;
                                case 6:
                                    currentMap = locationHumidity;
                                    break;
                            }
                        }
                    }

                    MatchCollection matches = numbers.Matches(line);
                    if (matches.Count == 3)
                    {
                        double rangeLen = Convert.ToDouble(matches.ElementAt(2).Value);
                        double dest = Convert.ToDouble(matches.ElementAt(1).Value);
                        double source = Convert.ToDouble(matches.ElementAt(0).Value);


                        currentMap.Add(new List<double>() { source, dest, rangeLen });
                        
                    }
                }
            }

            List<List<List<double>>> mappings = new List<List<List<double>>>()
            {
                locationHumidity,
                humidityTemperature,
                temperatureLight,
                lightWater,
                waterFertilizer,
                fertilizerSoil,
                soilSeed
            };

            double minSeedDistance = double.MaxValue;

            List<List<double>> orderedMap = mappings.ElementAt(0).OrderBy(x => x.ElementAt(0)).ToList();

            var layerMatched = false;
            foreach (var valRange in orderedMap)
            {
                double sourceStart = valRange.ElementAt(0);
                double destStart = valRange.ElementAt(1);
                double destEnd = destStart + valRange.ElementAt(2);
                int layer = 1;
                for (double x = destStart; x < destEnd; x++)
                {
                    
                    var mappingResult = PassToNextMap(destStart, layer, mappings);
                    //while (layer <= 6 && !mappingResult.Item1)
                    //{
                    //    layer += 1;
                    //    mappingResult = PassToNextMap(destStart, layer, mappings);
                    //    if (mappingResult.Item1)
                    //    {
                    //        Console.WriteLine("res found starting layer " + layer);
                    //    }

                    //}

                    // Console.WriteLine(mappingResult.Item2);
                    var num = mappingResult.Item2;
                    if (mappingResult.Item1)
                    {
                        layerMatched = true;
                    }
                }
                
            }



            return 0; //Convert.ToInt32(minSeedDistance);
        }


        public int Problem2Progress()
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

            List<Tuple<double, double>> seedPairs = new List<Tuple<double, double>>();

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
                soilFertilizer,
                fertilizerWater,
                waterLight,
                lightTemperature,
                temperatureHumidity,
                humidityLocation
            };

            double minSeedDistance = double.MaxValue;

            List<List<double>> orderedMap = mappings.ElementAt(0).OrderBy(x => x.ElementAt(0)).ToList();

            var seedRanges = new List<IEnumerable<double>>();
            List<Tuple<double, double>> sourceOverlap = new List<Tuple<double, double>>();
            List<Tuple<double, double>> mappedOverlap = new List<Tuple<double, double>>();

            List<Tuple<double, double>> soilRanges = new List<Tuple<double, double>>();
            List<Tuple<double, double>> fertilizerRanges = new List<Tuple<double, double>>();
            List<Tuple<double, double>> waterRanges = new List<Tuple<double, double>>();
            List<Tuple<double, double>> lightRanges = new List<Tuple<double, double>>();
            List<Tuple<double, double>> temperatureRanges = new List<Tuple<double, double>>();
            List<Tuple<double, double>> humidityRanges = new List<Tuple<double, double>>();
            List<Tuple<double, double>> locationRanges = new List<Tuple<double, double>>();

            List<Tuple<double, double>> retainedOptions = new List<Tuple<double, double>>();
            var resultList = new List<Tuple<bool, double>>();
            foreach(var seedRange in seedPairs)
            {
                double minSeed = seedRange.Item1;
                double maxSeed = seedRange.Item1 + seedRange.Item2;

                foreach(var map in seedSoil)
                {
                    double minRange = map.ElementAt(0);
                    double maxRange = minRange + map.ElementAt(2);
                    double offset = map.ElementAt(1) - minRange;

                    var commonality = UnionRanges(minSeed, maxSeed, minRange, maxRange, offset);
                    var unmapped = LeftUnionRanges(minSeed, maxSeed, minRange, maxRange);

                    // Save mapped values
                    retainedOptions.Add(new Tuple<double, double>(commonality.Item3, commonality.Item4));

                    // Save unmapped seed numbers
                    if(unmapped.Item1 != -1)
                    {
                        retainedOptions.Add(new Tuple<double, double>(unmapped.Item1, unmapped.Item2));
                    }
                    if(unmapped.Item3 != -1)
                    {
                        retainedOptions.Add(new Tuple<double, double>(unmapped.Item3, unmapped.Item4));
                    }

                    resultList.Add(ProcessChildMappings(retainedOptions, mappings, 0));
                }

                

            }


            //retainedOptions = retainedOptions.Distinct().OrderByDescending(x => x.Item3).ToList();

            for(int fd = 0; fd < 100; fd++)
            {
                Console.WriteLine(locationRanges[fd].Item1 + " " + locationRanges[fd].Item2);
            }

            minSeedDistance = locationRanges.Select(x => x.Item1).Min();
            //foreach (var seed in seeds)



            return Convert.ToInt32(minSeedDistance);
        }

        private Tuple<bool, double> ProcessChildMappings(List<Tuple<double, double>> ranges,
            List<List<List<double>>> mappings,
            int layer)
        {
            var map = mappings[layer];
            ranges = ranges.Distinct().ToList();
            List<double> rangeResults = new List<double>();

            foreach(var range in ranges.OrderBy(x => x.Item1))
            {
                double minRangeIn = range.Item1;
                double maxRangeIn = range.Item2;
                List<double> mappingResults = new List<double>();
                foreach (var mapping in map)
                {
                    
                    List<Tuple<double, double>> newRanges = new List<Tuple<double, double>>();
                    List<Tuple<double, double>> removals = new List<Tuple<double, double>>();
                    double minRange = mapping.ElementAt(0);
                    double maxRange = minRange + mapping.ElementAt(2);
                    double offset = mapping.ElementAt(1) - minRange;
                    
                    var commonality = UnionRanges(minRangeIn, maxRangeIn, minRange, maxRange, offset);
                    var unmapped = LeftUnionRanges(minRangeIn, maxRangeIn, minRange, maxRange);

                    if(layer == 5 && commonality.Item1 != -1)
                    {
                        return new Tuple<bool, double>(true, commonality.Item1);
                    }

                    // Save mapped values
                    if (commonality.Item1 != -1)
                    {
                        newRanges.Add(new Tuple<double, double>(commonality.Item3, commonality.Item4));
                        removals.Add(new Tuple<double, double>(minRangeIn, maxRangeIn));
                    }

                    // Save unmapped seed numbers
                    if (unmapped.Item1 != -1)
                    {
                        newRanges.Add(new Tuple<double, double>(unmapped.Item1, unmapped.Item2));
                    }
                    if (unmapped.Item3 != -1)
                    {
                        newRanges.Add(new Tuple<double, double>(unmapped.Item3, unmapped.Item4));
                    }

                    foreach(var newRange in ranges)
                    {
                        if(!newRanges.Contains(newRange) && !removals.Contains(newRange))
                        {
                            newRanges.Add(newRange);
                        }
                    }

                    if(layer + 1 <= 5)
                    {
                        var mappingResult = ProcessChildMappings(newRanges, mappings, layer + 1);
                        if (mappingResult.Item1)
                        {
                            mappingResults.Add(mappingResult.Item2);
                        }
                    }
                    
            
                }

                if (mappingResults.Any())
                {
                    rangeResults.Add(mappingResults.Min());
                }
            }
            if (rangeResults.Any())
            {
                return new Tuple<bool, double>(true, rangeResults.Min());
            }
            return new Tuple<bool, double>(false, -1);

        }


        private Tuple<double, double, double, double> UnionRanges(double lstOneStart, double lstOneEnd, double lstTwoStart, double lstTwoEnd
            , double lstTwoMappingOffset)
        {
            // Source range start between second range and ends after
            if (lstOneEnd >= lstTwoEnd && (lstOneStart > lstTwoStart && lstOneStart < lstTwoEnd))
            {
                return new Tuple<double, double, double, double>(lstOneStart, lstTwoEnd, lstOneStart + lstTwoMappingOffset, lstTwoEnd + lstTwoMappingOffset);
            }
            // Source range starts before second range and ends between
            else if (lstOneStart <= lstTwoStart && (lstOneEnd > lstTwoStart && lstOneEnd < lstTwoEnd))
            {
                return new Tuple<double, double, double, double>(lstTwoStart, lstOneEnd, lstTwoStart + lstTwoMappingOffset, lstTwoEnd + lstTwoMappingOffset);
            }
            // Source range is within second range
            else if (lstOneStart > lstTwoStart && lstOneEnd < lstTwoEnd)
            {
                return new Tuple<double, double, double, double>(lstOneStart, lstOneEnd, lstOneStart + lstTwoMappingOffset, lstOneEnd + lstTwoMappingOffset);
            }
            // Source range contains all of second range
            else if (lstOneStart < lstTwoStart && lstOneEnd > lstTwoEnd)
            {
                return new Tuple<double, double, double, double>(lstTwoStart, lstTwoEnd, lstTwoStart + lstTwoMappingOffset, lstTwoEnd + lstTwoMappingOffset);
            }
            // They are the same range
            else if(lstOneStart == lstTwoStart && lstOneEnd == lstTwoEnd)
            {
                return new Tuple<double, double, double, double>(lstOneStart, lstOneEnd, lstOneStart + lstTwoMappingOffset, lstOneEnd + lstTwoMappingOffset);
            }
            // No overlap
            else
            {
                return new Tuple<double, double, double, double>(-1, -1, -1, -1);
            }

        }

        private Tuple<double, double, double, double> LeftUnionRanges(double lstOneStart, double lstOneEnd, double lstTwoStart, double lstTwoEnd)
        {
            // Source range start between second range and ends after
            if (lstOneEnd > lstTwoEnd && (lstOneStart > lstTwoStart && lstOneStart < lstTwoEnd))
            {
                return new Tuple<double, double, double, double>(lstTwoEnd, lstOneEnd, -1, -1);
            }
            // Source range starts before second range and ends between
            else if (lstOneStart < lstTwoStart && (lstOneEnd > lstTwoStart && lstOneEnd < lstTwoEnd))
            {
                return new Tuple<double, double, double, double>(lstOneStart, lstTwoStart, -1, -1);
            }
            // Source range is within second range
            else if (lstOneStart > lstTwoStart && lstOneEnd < lstTwoEnd)
            {
                return new Tuple<double, double, double, double>(-1, -1, -1, -1);
            }
            // Source range contains all of second range
            else if (lstOneStart < lstTwoStart && lstOneEnd > lstTwoEnd)
            {
                return new Tuple<double, double, double, double>(lstOneStart, lstTwoStart, lstTwoEnd, lstOneEnd);
            }
            // They are the same range
            else if (lstOneStart == lstTwoStart && lstOneEnd == lstTwoEnd)
            {
                return new Tuple<double, double, double, double>(-1, -1, -1, -1);
            }
            // No overlap
            else
            {
                return new Tuple<double, double, double, double>(lstOneStart, lstOneEnd, -1, -1);
            }

        }

        private Tuple<bool, double> PassToNextMap(double value, int mapIndex, List<List<List<double>>> mappings)
        {
            if(mapIndex > 6)
            {
                return new Tuple<bool, double>(false, value);
            }

            var mapRanges = mappings.ElementAt(mapIndex).OrderBy(x => x.ElementAt(0)).ToList();

            foreach(var range in mapRanges)
            {
                double destStart = range.ElementAt(0);
                double destRangeEnd = destStart + range.ElementAt(2);
                double sourceStart = range.ElementAt(1);

              
                if (value >= sourceStart && value < destRangeEnd)
                {
                    if(mapIndex == 6)
                    {
                        return new Tuple<bool, double>(true, value);
                    }
                    var mappedValue = destStart + (value - sourceStart);
                    return PassToNextMap(mappedValue, mapIndex + 1, mappings);
                }
            }

            return PassToNextMap(value, mapIndex + 2, mappings);

        }
    }
}

