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
         Console.WriteLine("Add:  add --description <description> --amount <amount>");
         Console.WriteLine("list: list");
         Console.WriteLine("Summary:  Summary");
         Console.WriteLine("Summary for a specific month: summary --month <monthNumber>");
         Console.WriteLine("Delete:  delete --id <id>");
         return 1;
      }

      CreateEmptyFile(FILE_NAME);

      LoadExpensesFromFile();

      // handling input errors and calling the proper meth

      switch (args[0].ToLower())
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

   private static void OverwriteFile(string filename)
   {
      string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/expenses";

      if (!Directory.Exists(folderPath))
      {
         Directory.CreateDirectory(folderPath);
      }

      filePath = Path.Combine(folderPath, filename);


      byte[] bytes = Encoding.UTF8.GetBytes("Id, Date, Description, Amount\n");
      File.WriteAllBytes(filePath, bytes);

      foreach (var exp in expenses)
      {
         SaveExpensesToFile(exp.Value);
      }
   }

   private static void DeleteExpense(int id)
   {
      // delete an expense by id
      if (expenses.Remove(id))
      {
         // rewrite the expenses in the file
         OverwriteFile(FILE_NAME);
         Console.WriteLine("Expense deleted successfully");
      }
      else
      {
         Console.WriteLine("Expense not found at id: \"{0}\"", id);
      }

   }

   private static void SummaryMonthExpenses(int month)
   {
      // summary the expenses for a specific month
      string monthName = String.Empty;
      if (month < 1 || month > 12)
      {
         Console.WriteLine("Invalid Month");
         return;
      }
      decimal total = 0;
      foreach (var exp in expenses)
      {
         var amount = exp.Value.amount;
         if (exp.Value.date.Month == month)
         {
            total += amount;
            monthName = exp.Value.date.ToString("MMMM");
         }
      }

      Console.WriteLine("Total expenses for {1}: ${0}", total, monthName);
   }

   private static void SummaryExpenses()
   {
      // Summary all the expenses of the current year
      decimal total = 0;
      foreach (var exp in expenses)
      {
         var amount = exp.Value.amount;
         total += amount;
      }

      Console.WriteLine("Total expenses: ${0}", total);
   }

   private static void ListExpenses()
   {
      // read the file and print all the expenses with their data

      Console.WriteLine("Id      Date        Description       Amount");
      int padMeasure = 0;
      foreach (var exp in expenses)
      {
         var value = exp.Value;

         padMeasure = value.description.Length < 11 ? 16 : 10;
         Console.Write("{0}".PadRight(8), value.id);
         Console.Write("{0}".PadRight(10), value.date);
         Console.Write("{0}".PadRight(padMeasure), value.description);
         Console.Write("${0}", value.amount);
         Console.WriteLine("");
      }

   }

   private static void AddExpense(string[] args)
   {
      if (args[1] == "--description")
      {
         if (args[3] == "--amount")
         {
            if (decimal.TryParse(args[4], out decimal result) && result > 0)
            {
               var key = expenses.Count + 1;
               foreach (var exp in expenses)
               {
                  if (key == exp.Value.id)
                  {
                     key++;
                  }
               }
               var expense = new Expense()
               {
                  id = key,
                  date = DateOnly.FromDateTime(DateTime.Now),
                  description = args[2],
                  amount = result
               };

               expenses.Add(key, expense);

               SaveExpensesToFile(expense);
               Console.WriteLine($"Expense added successfully ID: {key}");
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
      try
      {
         using (StreamReader sr = new StreamReader(filePath))
         {
            string line = sr.ReadLine() == null ? string.Empty : ""; // skipping the first line

            while ((line = sr.ReadLine()) != null)
            {
               if (int.TryParse(line.Substring(0, line.IndexOf(',')), out int id))
               {
                  string[] strings = line.Split(',');

                  DateOnly.TryParse(strings[1], out DateOnly date);

                  decimal.TryParse(strings[strings.Length - 1], out decimal amount);

                  expenses.Add(id, new Expense()
                  {
                     id = id,
                     date = date,
                     description = strings[2].Trim(),
                     amount = amount
                  });

               }
            }
         }

      }
      catch (Exception e)
      {
         Console.WriteLine("couldn't read the file");
         Console.WriteLine(e);
      }
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
         return;
      }
      else
      {
         byte[] bytes = Encoding.UTF8.GetBytes("Id, Date, Description, Amount\n");
         File.WriteAllBytes(filePath, bytes);
      }
   }

}
