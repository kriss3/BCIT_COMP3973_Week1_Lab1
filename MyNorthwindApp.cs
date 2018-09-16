using System;
using NorthwindApp.NW;
using System.Linq;
using static System.Console;

namespace NorthwindApp
{
    /// <summary>
    /// COMP3973 Week 1 Lab 1
    /// Krzysztof Szczurowski: A01013054
    /// </summary>
    class MyNorthwindApp
    {
		static String answer = String.Empty;
        static NorthwindContext ctx = new NorthwindContext();
        static void Main(string[] args)
        {
            WriteLine($"Hello  {Environment.UserName} - Welcome to Lab 1 of COMP 3973\nPress key to Start!");
			ReadKey();
			Clear();
            WriteLine("Choose from below options: ");
            do
            {
                WriteLine();
                WriteLine("[1] - Get TOP 15 Customers");
                WriteLine("[2] - Insert New Recod to Category Table");
                WriteLine("[3] - Update Record in Category Table");
                WriteLine("[4] - Delete record in Category Table");
                WriteLine("[0] - Exit");
				WriteLine("=====================================");
				Write("Your choice: \t");
                answer = ReadLine().ToString();
                switch (answer)
                {
                    case "1":
                        GetTopCustomers();
                        break;
					case "2":
						InsertCategory();
						break;
					case "3":
						UpdateCategory();
						break;
					case "4":
						DeleteCategory();
						break;
                    case "5":
						GetCategoriesByFirstChar("C");
						break;
					default:
						ForegroundColor = ConsoleColor.Green;
						Write($"Thank you {Environment.UserName} for playig. Please, press any key to exit!");
						break;
				}
            }
            while (answer != "0");
            Console.ReadLine();
        }

        static void GetTopCustomers()
        {
            var ctx = new NorthwindContext();
			Clear();
            var qry = ctx.Customers.Take(15);
            var counter = 1;
            WriteLine($"{"Id",2} {"Cust ID",-10}\t{"Company Name",-35}\t{"Contact Name",-20}\t{"Country",-10}\t{"City",-10}");
            WriteLine("===========================================================================================================");
            foreach (var cust in qry)
            {
				WriteLine($"{counter,2}. {cust.CustomerId,-10}\t{cust.CompanyName,-35}\t{cust.ContactName,-20}\t{cust.Country,-10}\t{cust.City,-10}");
                counter++;
            }
			ForegroundColor = ConsoleColor.Green;
			Write("Press any key to continue...");
			ForegroundColor = ConsoleColor.Gray;
			answer = ReadLine().ToString();
			Clear();
        }

        static void GetCategoriesByFirstChar(string starts)
        {
			Clear();
            var qry = ctx.Categories.
                Where(c => c.CategoryName.StartsWith(starts));
			WriteLine();
			foreach (var item in ctx.Categories)
            {
                WriteLine(item.CategoryId + "\t" + item.CategoryName + "\t" + item.Description );
            }
			WriteLine();
			WriteLine("Please, enter any key to continue.");
			ReadLine();
			Clear();
        }

        static void InsertCategory()
        {
			Clear();
			ForegroundColor = ConsoleColor.Green;
			Write("Enter new Category Name: ");
			ForegroundColor = ConsoleColor.Gray;
			var name = ReadLine();
			ForegroundColor = ConsoleColor.Green;
			Write("Enter new Category Description: ");
			ForegroundColor = ConsoleColor.Gray;
			var desc = ReadLine();
			
            var ctx = new NorthwindContext();
            var newCat = new Categories()
            {
                CategoryName = name,
                Description = desc
            };
            ctx.Categories.Add(newCat);
            var results = ctx.SaveChanges();
			if (results > 0)
			{
				Write(Environment.NewLine);
				ForegroundColor = ConsoleColor.Green;
				Write("Success ! \nNew Category Saved to Database. Please, press any key to return to the Main Menu.");
				ForegroundColor = ConsoleColor.Gray;
				ReadKey();
			}
			Clear();
        }

        static void UpdateCategory()
        {
			Clear();
			ForegroundColor = ConsoleColor.Green;
			Write("Enter Category Id to update: ");
			ForegroundColor = ConsoleColor.Gray;
			var id = int.Parse(ReadLine());

			ForegroundColor = ConsoleColor.Green;
			Write("Enter new Category Name: ");
			ForegroundColor = ConsoleColor.Gray;
			var newName = ReadLine();

			ForegroundColor = ConsoleColor.Green;
			Write("Enter new Category Description: ");
			ForegroundColor = ConsoleColor.Gray;
			var newDesc = ReadLine();

            var category2Update = ctx.Categories.Find(id);
			if (category2Update != null)
			{
				category2Update.CategoryName = newName;
				category2Update.Description = newDesc;
			}
			else
			{
				Write(Environment.NewLine);
				ForegroundColor = ConsoleColor.Red;
				Write($"Record with Id: {id} was not found. Please, try again.");
				ForegroundColor = ConsoleColor.Gray;
			}
			
            var results = ctx.SaveChanges();
			if (results > 0)
			{
				ForegroundColor = ConsoleColor.Green;
				Write($"Success!\nCategory Id: {id} was updated successfully. Press any key to return to Main Menu.");
				ForegroundColor = ConsoleColor.Gray;
				ReadKey();
			}
			Clear();
        }

        static void DeleteCategory()
		{ 
			Clear();
			ForegroundColor = ConsoleColor.Green;
			Write("Enter Category Id to delete: ");
			ForegroundColor = ConsoleColor.Gray;
			var id = int.Parse(ReadLine());
            var cat2Del = ctx.Categories.Find(id);
			if (cat2Del != null)
			{
				try
				{
					ctx.Remove(cat2Del);
					var results = ctx.SaveChanges();
					if (results > 0)
					{
						ForegroundColor = ConsoleColor.Green;
						Write($"Success!\nRecord with Category Id: {id} was removed successfully. Press any key to return to Main Menu.");
						ForegroundColor = ConsoleColor.Gray;
						ReadKey();
					}
				}
				catch (Exception ex)
				{
					ForegroundColor = ConsoleColor.Red;
					Write($"Failed to remove Category with Id: {id}. Please, try again or contact Staff member.");
					ForegroundColor = ConsoleColor.Gray;
					//log to file with ex.Message;
				}
			}
			Clear();
        }

        static void GetProductsByCategory()
        {
            var qry = ctx.Products.Select(p=> new { p.ProductId, p.ProductName, p.Category.CategoryName,  p.Category.Products});

			foreach (var item in qry)
			{
				Console.WriteLine(
				item.ProductId + "\t" +
				item.ProductName + "\t" +
				item.CategoryName);
			}
		}
    }
}
