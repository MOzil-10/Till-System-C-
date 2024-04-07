using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        int change = 0;
        int tillBalance = 500; // Initial till balance
        StringBuilder output = new StringBuilder(); // Initialize output string

        try
        {
            string[] lines = File.ReadAllLines(@"input.txt");
            foreach (string line in lines)
            {
                Console.WriteLine($"Processing line: {line}");
                
                string[] transactionsAndPayment = line.Split(",");
                string[] transactions = transactionsAndPayment[0].Split(";");
                string[] amountsStr = transactionsAndPayment[transactionsAndPayment.Length - 1].Split("-");

                int transactionTotal = ProcessTotalTransaction(transactions);
                int paid = ProcessPaidAmount(amountsStr);
                change = paid - transactionTotal;

                output.AppendLine($"Till Start: R{tillBalance}");
                output.AppendLine($"Transaction Total: R{transactionTotal}");
                output.AppendLine($"Paid: R{paid}");
                output.AppendLine($"Change Total: R{change}");
                output.AppendLine($"Change Breakdown: {(change == 0 ? "0" : $"R{ChangeBreakDown(change)}")}");

                // Update till balance for the next transaction
                tillBalance += paid - change;
            }
            output.AppendLine($"Remaining Balance in Till: R{tillBalance}");

            File.WriteAllText("output.txt", output.ToString()); // Write output to output.txt
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading input file: " + ex.Message);
        }
    }

    static string ChangeBreakDown(int change)
    {
        int[] note = { 50, 20, 10, 5, 2, 1 };
        int[] count = new int[note.Length];
        StringBuilder changeBreakDownStr = new StringBuilder();

        for (int i = 0; i < note.Length; i++)
        {
            while (change >= note[i])
            {
                change -= note[i];
                count[i]++;
            }
        }

        bool first = true;
        for (int i = 0; i < note.Length; i++)
        {
            for (int j = 0; j < count[i]; j++)
            {
                if (!first)
                {
                    changeBreakDownStr.Append("-");
                }
                else
                {
                    first = false;
                }
                changeBreakDownStr.Append(note[i]);
            }
        }
        return changeBreakDownStr.ToString();
    }

    static int ProcessPaidAmount(string[] amountsStr)
    {
        int paid = 0;
        foreach (string amount in amountsStr)
        {
            string paidString;
            if (amount.Length >= 2)
            {
                paidString = amount.Trim().Substring(1);
                paid += int.Parse(paidString);
            }
            else
            {
                // Handle invalid amount format
                Console.WriteLine("Error: Invalid amount format - " + amount);
            }
        }
        return paid;
    }

    static int ProcessTotalTransaction(string[] transactions)
    {
        int totalTransaction = 0;
        foreach (string transaction in transactions)
        {
            string[] parts = transaction.Split(" ");
            if (parts.Length > 1)
            {
                string amount = parts[parts.Length - 1].Trim().Substring(1);
                totalTransaction += int.Parse(amount);
            }
            else
            {
                // Handle invalid transaction format
                Console.WriteLine("Error: Invalid transaction format - " + transaction);
            }
        }
        return totalTransaction;
    }
}
