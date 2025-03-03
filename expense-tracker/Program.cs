using System;
using System.IO;
using System.Text;

namespace expense_tracker;

class Program
{
   const string FILE_NAME = "expenses.csv";
   static string filePath = string.Empty;
   static Dictionary<int, Expense> expenses = new Dictionary<int, Expense>();
   static int Main(string[] args)
   {
      // handling no input or too many inputs 
      if (args.Length == 0 || args.Length > 5)
      {
         Console.WriteLine("Usage: ");
         return 1;
      }

      CreateEmptyFile(FILE_NAME);

      LoadExpensesFromFile();

      // handling input errors and calling the proper meth
      switch (args[0])
      {
         case "add":
            if (args.Length == 5)
            {
               AddExpense(args);
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

   private static void AddExpense(string[] args)
   {
      if (args[1] == "--description")
      {
         if (args[3] == "--amount")
         {
            if (decimal.TryParse(args[4], out decimal result) && result > 0)
            {
               var expense = new Expense()
               {
                  id = expenses.Count + 1,
                  date = DateOnly.FromDateTime(DateTime.Now),
                  description = args[2],
                  amount = result
               };

               expenses.Add(expenses.Count + 1, expense);

               SaveExpensesToFile(expense);
            }
            else
            {
               Console.WriteLine("\"{0}\" is not a valid amount", args[4]);
               return;
            }
         }
         else
         {
            Console.WriteLine("Invalid argument \"{0}\"", args[3]);
            return;
         }
      }
      else
      {
         Console.WriteLine("Invalid argument \"{0}\"", args[1]);
         return;
      }

   }

   private static void LoadExpensesFromFile()
   {
      // initial method to load the expenses stored in the csv file
      // if there isn't any file, call CreateEmptyFile()
      // if there's one, read it and deserealize the data

   }

   private static void SaveExpensesToFile(Expense expense)
   {
      // convert the data into bytes array
      // append to the file
      string data = $"{expense.id}, {expense.date.ToString()}, {expense.description}, {expense.amount.ToString()}\n";
      byte[] bytes = Encoding.UTF8.GetBytes(data);
      try
      {
         File.AppendAllBytes(filePath, bytes);
      }
      catch
      {
         Console.WriteLine("Cannot write in the file");
         return;
      }
   }

   private static void CreateEmptyFile(string filename)
   {
      string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/expenses";

      if (!Directory.Exists(folderPath))
      {
         Directory.CreateDirectory(folderPath);
      }

      filePath = Path.Combine(folderPath, filename);

      if (File.Exists(filePath))
      {
         Console.WriteLine("The file already exits");
         return;
      }
      else
      {
         byte[] bytes = Encoding.UTF8.GetBytes("Id, Date, Description, Amount\n");
         File.WriteAllBytes(filePath, bytes);
      }
   }

}
