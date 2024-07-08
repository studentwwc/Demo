using System;

namespace Common
{
    public class PETool
    {
        public static int GetRandom(int min,int max,Random random=null)
        {
            if (random == null)
            {
                random = new Random();
            }

            return random.Next(min, max);
        }
    }
}