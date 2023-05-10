using System;
using System.Collections.Generic;
using System.Text;

namespace HurtowniaBazDanych
{
    class RandomeGeneratorWithWeithtedDistrubusion
    {
        private int[] weights;
        
        public RandomeGeneratorWithWeithtedDistrubusion(int count, int maxweights)
        {
            weights = new int[count];
            int sum = 0;
            for (int i = 0; i < count; i++)
            {
                int weight = ThreadSafeRandom.ThisThreadsRandom.Next(maxweights);
                weight++;
                sum += weight;
                weights[i] = sum;
            }
        }

        public RandomeGeneratorWithWeithtedDistrubusion(int[] weights)
        {
            this.weights = new int[weights.Length];
            int sum = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                sum += weights[i];
                this.weights[i] = sum;
            }
        }

        public RandomeGeneratorWithWeithtedDistrubusion(int count, int maxweights, int chunksize)
        {
            weights = new int[count];
            int sum = 0;
            int index = 0;
            int chunkIndex = 0;
            int currentWeight = ThreadSafeRandom.ThisThreadsRandom.Next(maxweights) + 1;
            int nextWeight = ThreadSafeRandom.ThisThreadsRandom.Next(maxweights) + 1;
            while (index < count)
            {
                if(chunkIndex >= chunksize)
                {
                    nextWeight = currentWeight;
                    currentWeight = ThreadSafeRandom.ThisThreadsRandom.Next(maxweights) + 1;
                    chunkIndex = 0;
                }

                sum += currentWeight + ((nextWeight - currentWeight) * (chunkIndex / chunksize));
                weights[index] = sum;

                index++;
                chunkIndex++;
            }
        }

        public int next()
        {
            int pointer = ThreadSafeRandom.ThisThreadsRandom.Next(weights[weights.Length-1]);

            for (int i = 0; i < weights.Length; i++)
            {
                if (pointer < weights[i])
                {
                    return i;
                }
            }
        

            Console.Write("\n" + pointer + " ->");
            for (int i = 0; i < weights.Length; i++)
            {
                Console.Write(" " + weights[i]);
            }
            return -1;
        }
    }
}
