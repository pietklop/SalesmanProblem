﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesmanProblem
{
    public class DistanceMatrix
    {
        public readonly int[,] CityToCity;
        public const int AMOUNT = 15;

        public DistanceMatrix()
        {
            CityToCity = new int[AMOUNT, AMOUNT];
            string[] x = // minimum is 291
            @"00 29 82 46 68 52 72 42 51 55 29 74 23 72 46
            29 00 55 46 42 43 43 23 23 31 41 51 11 52 21
            82 55 00 68 46 55 23 43 41 29 79 21 64 31 51
            46 46 68 00 82 15 72 31 62 42 21 51 51 43 64
            68 42 46 82 00 74 23 52 21 46 82 58 46 65 23
            52 43 55 15 74 00 61 23 55 31 33 37 51 29 59
            72 43 23 72 23 61 00 42 23 31 77 37 51 46 33
            42 23 43 31 52 23 42 00 33 15 37 33 33 31 37
            51 23 41 62 21 55 23 33 00 29 62 46 29 51 11
            55 31 29 42 46 31 31 15 29 00 51 21 41 23 37
            29 41 79 21 82 33 77 37 62 51 00 65 42 59 61
            74 51 21 51 58 37 37 33 46 21 65 00 61 11 55
            23 11 64 51 46 51 51 33 29 41 42 61 00 62 23
            72 52 31 43 65 29 46 31 51 23 59 11 62 00 59
            46 21 51 64 23 59 33 37 11 37 61 55 23 59 00".Trim().Split(new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < x.Count(); i++)
            {
                string[] y = x[i].Trim().Split(' ');
                for (int j = 0; j < y.Count(); j++)
                    CityToCity[i, j] = int.Parse(y[j]);
            }

        }

    }
}