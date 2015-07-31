namespace ATMApp
{
    using System;
    using System.Linq;
    using ATM.Data;
    using ATM.Models;

    public class ATMMain
    {
        public static void Main()
        {
            var context = new ATMEntities();

            while (true)
            {
                try
                {
                    // Card number: 0963596714
                    // Card PIN: 7206

                    // Read and validate Card Number and Card PIN
                    var cardFound = InputCardNumber(context);
                    InputCardPIN(cardFound);

                    // Read the amount of money and withdraw them
                    Console.Write("--Insert the amount you want to withdraw: ");
                    var amountToWithdraw = decimal.Parse(Console.ReadLine());
                    Withdraw(amountToWithdraw, cardFound);

                    // Adding transaction history entry.
                    context.TransactionHistoryEntries.Add(new TransactionHistoryEntry
                    {
                        CardNumber = cardFound.CardNumber,
                        TransactionDate = DateTime.Now,
                        Amount = amountToWithdraw
                    });
                    Console.Write("--Created transaction history log.");

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static CardAccount InputCardNumber(ATMEntities context)
        {
            Console.Write("--Please, enter your card number: ");
            var candidateCardNumber = Console.ReadLine();

            var cardFound = context.CardAccounts
                .FirstOrDefault(ca => ca.CardNumber.Equals(candidateCardNumber));

            if (cardFound == null)
            {
                throw new InvalidOperationException("--Card number is invalid. Please enter a correct one.");
            }

            Console.WriteLine("--Card number exists.");
            Console.Write("--Insert the PIN of the selected card: ");
            return cardFound;
        }

        private static void InputCardPIN(CardAccount cardFound)
        {
            var cardCandidatePin = Console.ReadLine();
            if (!cardFound.CardPIN.Equals(cardCandidatePin))
            {
                throw new InvalidOperationException("--Card PIN is invalid.");
            }

            Console.WriteLine("--Valid card PIN.");
            Console.WriteLine("--Current card cash: " + cardFound.CardCash.ToString("F"));
        }

        private static void Withdraw(decimal amountToWithdraw, CardAccount cardFound)
        {
            if (cardFound.CardCash - amountToWithdraw < 0)
            {
                throw new ArgumentOutOfRangeException("--Not enough money to withdraw.");
            }

            cardFound.CardCash -= amountToWithdraw;

            Console.Clear();
            Console.WriteLine("--Withdrawn: " + amountToWithdraw.ToString("F"));
            Console.WriteLine("--Current amount: " + cardFound.CardCash.ToString("F"));
        }
    }
}
