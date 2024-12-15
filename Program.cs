using System;
using System.Collections.Generic;
using System.Linq;

namespace RewardsProgram
{
    public class Transaction
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // Sample Data
            var transactions = new List<Transaction>
            {
                new Transaction { CustomerId = 1, Amount = 120, Date = new DateTime(2024, 09, 15) },
                new Transaction { CustomerId = 2, Amount = 80, Date = new DateTime(2024, 09, 18) },
                new Transaction { CustomerId = 1, Amount = 60, Date = new DateTime(2024, 10, 05) },
                new Transaction { CustomerId = 2, Amount = 220, Date = new DateTime(2024, 10, 24) },
                new Transaction { CustomerId = 1, Amount = 200, Date = new DateTime(2024, 11, 14) },
                new Transaction { CustomerId = 2, Amount = 150, Date = new DateTime(2024, 11, 12) }
            };

            // Process Transactions
            var customerRewards = CalculateRewards(transactions);

            // Display Results
            foreach (var customer in customerRewards)
            {
                Console.WriteLine($"Customer {customer.Key}:");
                foreach (var month in customer.Value)
                {
                    Console.WriteLine($"  {month.Key}: {month.Value} points");
                }
                var totalPoints = customer.Value.Values.Sum();
                Console.WriteLine($"  Total: {totalPoints} points\n");
            }
        }

        public static Dictionary<int, Dictionary<string, int>> CalculateRewards(List<Transaction> transactions)
        {            
            return transactions
                .GroupBy(t => t.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(t => t.Date.ToString("yyyy-MM"))
                          .ToDictionary(
                              mg => mg.Key,
                              mg => mg.Sum(t => CalculatePoints(t.Amount))
                          )
                );
        }

        public static int CalculatePoints(decimal amount)
        {
            int points = 0;
            if (amount > 100)
            {
                points += (int)(2 * (amount - 100));
            }
            if (amount > 50)
            {
                points += (int)(1 * Math.Min(amount - 50, 50));
            }
            return points;
        }
    }
}
