using System;

namespace expense_tracker;

class Program
{
   static int Main(string[] args)
   {
      // handling no input 
      if (args.Length == 0)
      {
         Console.WriteLine("Usage: ");
         return 1;
      }


      // handling input errors and calling the proper meth
      switch (args[0])
      {
         case "add":
            if (args.Length >= 5)
            {

            }
            else
            {
               Console.WriteLine("Arguments missing");
               return 1;
            }
            break;
         case "list":
            ListExpenses();
            break;
         case "summary":
            if (args.Length == 3)
            {
               // process summary with specific month
               if (args[1] == "--month")
               {
                  if (int.TryParse(args[2], out int month))
                  {
                     SummaryMonthExpenses(month);
                  }
                  else
                  {
                     Console.WriteLine("Invalid parameter \"{0}\"", args[2]);
                     return 3;
                  }
               }
               else
               {
                  Console.WriteLine("Invalid argument \"{0}\"", args[1]);
                  return 2;
               }
            }
            else
            {
               SummaryExpenses();
            }
            break;
         case "delete":
            if (args.Length == 3)
            {
               if (args[1] == "--id")
               {
                  if (int.TryParse(args[2], out int id))
                  {
                     DeleteExpense(id);
                  }
                  else
                  {
                     Console.WriteLine("Invalid parameter \"{0}\"", args[2]);
                     return 3;
                  }
               }
               else
               {
                  Console.WriteLine("Invalid argument \"{0}\"", args[1]);
                  return 2;
               }
            }
            else
            {
               Console.WriteLine("Arguments missing");
               return 1;
            }
            break;
      }
      return 0;
   }

   private static void DeleteExpense(int id)
   {
      // delete an expense by id
   }

   private static void SummaryMonthExpenses(int month)
   {
      // summary the expenses for a specific month
   }

   private static void SummaryExpenses()
   {
      // Summary all the expenses of the current year
   }

   private static void ListExpenses()
   {
      // read the file and print all the expenses with their data
   }

   private static void AddExpense(string description, int amount)
   {
      // create an expense obj and add the description, amount, id and date
   }
}
