using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WoodpeckerBot.Services
{
    public class Random
    {
        private static System.Random _rand = new System.Random();

        public static float NextFloat(int min = 0, int max = Int32.MaxValue)
        {
            return _rand.Next(min, max) / 100;
        }
        public static int Next(int min = 0, int max = Int32.MaxValue)
        {
            return _rand.Next(min, max);
        }
    }
}
