using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AccountDemo1
{
    public sealed class UserInput
    {
        private static readonly string CREDIT = "cr";
        private static readonly string DEBIT = "db";

        public enum InputType
        {
            Credit,
            Debit
        }

        public UserInput(string inputLine)
        {
            ParseInput(inputLine);
        }

        public InputType Type { get; private set; }

        public double Amount { get; private set; }

        private void ParseInput(string inputLine)
        {
            var input = inputLine.Split();

            if (input.Length < 2)
            {
                throw new InvalidOperationException("Error: Use CR or DB plus amount.");
            }

            if (input[0].ToLower() == CREDIT)
            {
                Type = InputType.Credit;
            }
            else if (input[0].ToLower() == DEBIT)
            {
                Type = InputType.Debit;
            }
            else
            {
                throw new InvalidOperationException("Error: Use CR for credit, DB for debit.");
            }

            if (!double.TryParse(input[1], out var amount))
            {
                throw new InvalidOperationException($"Value \"{input[1]}\" is not valid input.");
            }

            Amount = amount;
        }
    }
}
