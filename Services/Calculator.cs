using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityBot.Services;

namespace UtilityBot.Services
{
    public class Calculator : IAddition
    {
        double IAddition.AddNumbers(string numbers)
        {
            string[] nums = numbers.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            double[] doubleNumbers = new double[nums.Length];

            double sum = 0;

            for (int i = 0; i < nums.Length; i++)
            {
                if (Double.TryParse(nums[i], out doubleNumbers[i]))
                {
                    doubleNumbers[i] = Double.Parse(nums[i]);

                    sum += doubleNumbers[i];
                }
                else
                {
                    throw new ArgumentException($"Введенное значение {nums[i]} не является числом");
                }
            }
            
            return sum;
        }
    }
}
