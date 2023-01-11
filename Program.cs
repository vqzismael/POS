
using System;
using System.Configuration;

namespace POS
{
    class Program
    {
        static void Main(string[] args){
            //create a random total price
            decimal price = new Random().next(0,100000)/100m;
            Console.WriteLine($"The total Price is {price:C}");
            string amount;
            // reading amount given from the customer
            Console.WriteLine("type the payment amount, then press Enter.");
            amount = Console.ReadLine();

            decimal dAmount = 0;
            while(!decimal.TryParse(amount, out dAmount) || dAmount < price){
                Console.WriteLine($"this is not a valid input. Please enter a greater than {price:C} numeric value.");
                amount = Console.ReadLine();
            }

            // Calculating change
            price = dAmount - price;
            Console.WriteLine($"The calculated change is {price:C}");

            //Calculating needed number of coin/bill to the customer
            Change.CalculateChange(price);
            Console.ReadKey();
        }
    }

    // class which contains the static method to calculate number of coins and bills to return
    class Change
    {
        public static void CalculateChange(decimal price){
            string configCurrency = ConfigurationManager.AppSettings["currency"];
            var denArray = Currency.GetCurrency(configCurrency);

            Array.Sort<decimal>(denArray, new Comparison<decimal>((d1, d2) => d2.CompareTo(d1)));

            for(int i = 0; i < denArray.Lenght; i++){
                var denomination = denArray[i];
                int totalDen (int)(price / denomination);

                if(totalDen > 0){
                    Console.WriteLine($"{denomination:C} coin/bill: {totalDen}");
                    price %= denomination;
                }
            }
        }
    }

    // Class where the denominations are set, according to the country currency
    class Currency{
        public static decimal[] usdCurrency = new decimal[] {0.01m, 0.05m, 0.10m, 0.25m, 0.50m, 1.00m, 2.00m, 5.00m, 10.00m, 20.00m, 50.00m, 100.00m};
        public static decimal[] mxnCurrency = new decimal[] {0.05m, 0.10m, 0.20m, 0.50m, 1.00m, 2.00m, 5.00m, 10.00m, 20.00m, 50.00m, 100.00m, 200.00m, 500.00m, 1000.00m};
        
        //setting denominationsacording the configration value 'currency' in app.config
        public static decimal[] Getcurrency(string configValue){
            switch (configValue)
            {
                case "USD":
                return usdCurrency;
                case "MXN":
                return mxnCurrency;
                default:
                return usdCurrency;
            }
        }
        }
}