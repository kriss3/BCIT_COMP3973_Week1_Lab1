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
            var  welcomeMsg = $"Hello {Environment.UserName} - Welcome to Lab 1 of COMP 3973";
            WriteLine();
            ForegroundColor = ConsoleColor.DarkMagenta;
            WriteLine(String.Format("{0," + ((WindowWidth / 2) + (welcomeMsg.Length / 2)) + "}", welcomeMsg));
            ForegroundColor = ConsoleColor.Gray;
			ReadKey();
			Clear();
            WriteLine("Choose from below options: ");
            do
            {
                WriteLine();
                WriteLine("[1] - Get TOP 15 Customers");
                WriteLine("[2] - Insert New Record to Customer Table");
                WriteLine("[3] - Update Record in Customer Table");
                WriteLine("[4] - Delete record from Customer Table");
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
						InsertCustomer();
						break;
					case "3":
						UpdateCustomer();
						break;
					case "4":
						DeleteCustomer();
						break;
                    case "5":
                        GetCustomersByContactName("C");
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
            using (var ctx = new NorthwindContext())
            {
                try
                {
                    Clear();
                    var qry = ctx.Customers.Take(15);
                    var counter = 1;
                    WriteLine();
                    Helper.DrawGetTopCustomerHeader();
                    Helper.DrawSeparator(110);
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
                catch (Exception ex)
                {
                    Helper.DisplayErrorMessage(ex.Message);
                }
            }
        }

        static void GetCustomersByContactName(string starts)
        {
			Clear();
            var qry = ctx.Customers.
                Where(c => c.ContactName.StartsWith(starts));
            var counter = 1;
			WriteLine();
			foreach (var cust in qry)
            {
                WriteLine($"{counter,2}.\t{cust.CustomerId,5}\t{cust.ContactName,15} \t{cust.CompanyName,20}");
                counter++;
            }
			WriteLine();
			WriteLine("Please, enter any key to continue.");
			ReadLine();
			Clear();
        }

        static void InsertCustomer()
        {
			Clear();
			ForegroundColor = ConsoleColor.Green;
			Write("Enter new Customer Id: ");
			ForegroundColor = ConsoleColor.Gray;
			var custId = ReadLine();
			ForegroundColor = ConsoleColor.Green;
			Write("Enter new Company Name: ");
			ForegroundColor = ConsoleColor.Gray;
			var compName = ReadLine();
            ForegroundColor = ConsoleColor.Green;
            Write("Enter new Contact Name: ");
            ForegroundColor = ConsoleColor.Gray;
            var contactName = ReadLine();

            var newCust = new Customers()
            {
                CustomerId = custId.ToUpper(),
                CompanyName = compName,
                ContactName = contactName
            };

            try
            {
                ctx.Customers.Add(newCust);
                var results = ctx.SaveChanges();
                WriteLine();
                ForegroundColor = ConsoleColor.Green;
                Write("Success ! \nNew Customer Saved to Database. Please, press any key to return to the Main Menu.");
                ForegroundColor = ConsoleColor.Gray;
                ReadKey();
                Clear();
            }
            catch (Exception ex)
            {
                Helper.DisplayErrorMessage(ex.Message + "\n" + ex.InnerException.Message);
                ReadKey();
                Clear();
            }
        }

        static void UpdateCustomer()
        {
			Clear();
			ForegroundColor = ConsoleColor.Green;
			Write("Enter Customer Id to update: ");
			ForegroundColor = ConsoleColor.Gray;
            var customerId = ReadLine();

			ForegroundColor = ConsoleColor.Green;
			Write("Enter new Contact Name: ");
			ForegroundColor = ConsoleColor.Gray;
			var newContactName = ReadLine();

			ForegroundColor = ConsoleColor.Green;
			Write("Enter new Company Name: ");
			ForegroundColor = ConsoleColor.Gray;
			var newCompanyName = ReadLine();

            var customer2Update = ctx.Customers.Find(customerId.ToUpper());
			if (customer2Update != null)
			{
				customer2Update.ContactName = newContactName;
				customer2Update.CompanyName = newCompanyName;
			}
			else
			{
                WriteLine();
				ForegroundColor = ConsoleColor.Red;
				Write($"Record with Id: {customerId} was not found. Please, try again.");
				ForegroundColor = ConsoleColor.Gray;
			}

            try
            {
                ctx.SaveChanges();
                WriteLine();
                ForegroundColor = ConsoleColor.Green;
                Write($"Success!\nCustomer Id: {customerId} was updated successfully. Press any key to return to Main Menu.");
                ForegroundColor = ConsoleColor.Gray;
                ReadKey();
            }
            catch (Exception ex)
            {
                Helper.DisplayErrorMessage(ex.Message);
            }
            finally
            {
                Clear();
            }
        }

        static void DeleteCustomer()
		{ 
			Clear();
			ForegroundColor = ConsoleColor.Green;
			Write("Enter Customer Id to delete: ");
			ForegroundColor = ConsoleColor.Gray;
            var id = ReadLine();
            var customer2Delete = ctx.Customers.Find(id.ToUpper());
			if (customer2Delete != null)
			{
				try
				{
					ctx.Remove(customer2Delete);
                    ctx.SaveChanges();
                    WriteLine();
					ForegroundColor = ConsoleColor.Green;
                    WriteLine();
					Write($"Success!\nRecord with Customer Id: {id.ToUpper()} was removed successfully. Press any key to return to Main Menu.");
					ForegroundColor = ConsoleColor.Gray;
					ReadKey();
                    Clear();
				}
				catch (Exception ex)
				{
                    Helper.DisplayErrorMessage(ex.Message);
					//log to file with ex.Message;
				}
			}
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
