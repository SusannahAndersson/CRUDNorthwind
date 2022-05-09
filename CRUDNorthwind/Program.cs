using Figgle;
using System.Data.SqlClient;

namespace SqlCrudNorthwind
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;

            SqlConnection sqlConnection;
            string altConnectionString =
                        @"Data Source=Pandora\SQLEXPRESS; Initial Catalog=Northwind;"
                        + @"Integrated Security=true;" + @"trustservercertificate=true";
            sqlConnection = new SqlConnection(altConnectionString);
            sqlConnection.Open();

            try
            {
                Console.WriteLine("Connected to database successfully\n");

                string menu;
                do
                {
                    Console.WriteLine
                        (
                        FiggleFonts.Ogre.Render("Northwind") +
                        "\n\tMain Menu \n" +
                        "(1) Add customer\n" +
                        "(2) Delete customer\n" +
                        "(3) Update employee address\n" +
                        "(4) Show country sales\n" +
                        "(5) Add new order for new customer\n" +
                        "(6) Delete new order for new customer\n"
                        );
                    int userinput = int.Parse(Console.ReadLine());
                    switch (userinput)
                    {
                        case 1:
                            AddCustomer();
                            break;

                        case 2:
                            DeleteCustomer();
                            break;

                        case 3:
                            UpdateEmployee();
                            break;

                        case 4:
                            ShowCountrySales();
                            break;

                        case 5:
                            AddNewOrderForNewCustomer();
                            break;

                        case 6:
                            DeleteNewOrderForNewCustomer();
                            break;

                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                    Console.WriteLine("\n\tReturn to menu? Write no to exit program");
                    menu = Console.ReadLine();
                }
                while (menu != "no");
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private static void AddCustomer()
        {
            SqlConnection sqlConnection;
            string altConnectionString =
                        @"Data Source=Pandora\SQLEXPRESS; Initial Catalog=Northwind;"
                        + @"Integrated Security=true;" + @"trustservercertificate=true";
            sqlConnection = new SqlConnection(altConnectionString);
            sqlConnection.Open();

            Console.WriteLine("Enter CustomerID");
            var uCustomerID = Console.ReadLine();
            Console.WriteLine("Enter CompanyName");
            string uCompanyName = Console.ReadLine();
            Console.WriteLine("Enter ContactName");
            string uContactName = Console.ReadLine();
            Console.WriteLine("Enter ContactTitle");
            string uContactTitle = Console.ReadLine();
            Console.WriteLine("Enter Address");
            string uAddress = Console.ReadLine();
            Console.WriteLine("Enter City");
            string uCity = Console.ReadLine();
            Console.WriteLine("Enter Region");
            string uRegion = Console.ReadLine();
            Console.WriteLine("Enter PostalCode");
            int uPostalCode = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Country");
            string uCountry = Console.ReadLine();
            Console.WriteLine("Enter Phone");
            int uPhone = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Fax");
            int uFax = int.Parse(Console.ReadLine());

            string insertQuery =
                $"INSERT INTO CUSTOMERS ([CustomerID], [CompanyName], [ContactName], " +
                $"[ContactTitle], [Address],[City], [Region], [PostalCode],[Country],[Phone],[Fax]) " +
                $"VALUES( '" + uCustomerID + "','" + uCompanyName + "', '" + uContactName + "'," +
                $"'" + uContactTitle + "','" + uAddress + "','" + uCity + "','" + uRegion + "', " +
                $"" + uPostalCode + " ,'" + uCountry + "', " + uPhone + " , " + uFax + " )";
            SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
            insertCommand.ExecuteNonQuery();
            Console.WriteLine("Customer added successfully");
            sqlConnection.Close();
        }

        private static void DeleteCustomer()
        {
            SqlConnection sqlConnection;
            string altConnectionString =
                        @"Data Source=Pandora\SQLEXPRESS; Initial Catalog=Northwind;"
                        + @"Integrated Security=true;" + @"trustservercertificate=true";
            sqlConnection = new SqlConnection(altConnectionString);
            sqlConnection.Open();

            Console.WriteLine("Enter CustomerId or ContactName to delete");
            string uDelId = Console.ReadLine();
            string deleteQuery =
                $"DELETE FROM dbo.Customers WHERE [dbo].[Customers].[CustomerID]='{uDelId}'" +
                $"DELETE FROM dbo.Customers WHERE [dbo].[Customers].[ContactName]='{uDelId}'";
            SqlCommand deleteCommand = new SqlCommand(deleteQuery, sqlConnection);
            deleteCommand.ExecuteNonQuery();
            Console.WriteLine("Deleted successfully");
            sqlConnection.Close();
        }

        private static void UpdateEmployee()
        {
            SqlConnection sqlConnection;
            string altConnectionString =
                        @"Data Source=Pandora\SQLEXPRESS; Initial Catalog=Northwind;"
                        + @"Integrated Security=true;" + @"trustservercertificate=true";
            sqlConnection = new SqlConnection(altConnectionString);
            sqlConnection.Open();

            string displayQuery =
                "SELECT * FROM Employees";
            SqlCommand displayCommand = new SqlCommand(displayQuery, sqlConnection);
            SqlDataReader dataReader = displayCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine("EmployeeId: " + dataReader.GetValue(0).ToString());
                Console.WriteLine("Address: " + dataReader.GetValue(7).ToString());
            }
            dataReader.Close();

            Console.WriteLine("Enter EmployeeId to update:");
            int uEmployeeId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new address to update");
            string uEmployeeAddress = Console.ReadLine();
            string updateQuery =
                $"UPDATE Employees SET Address='{uEmployeeAddress}'" +
                $"WHERE EmployeeID='{uEmployeeId}';";
            SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection);
            updateCommand.ExecuteNonQuery();
            Console.WriteLine("Updated successfully");
            sqlConnection.Close();
        }

        private static void ShowCountrySales()
        {
            {
                SqlConnection sqlConnection;
                string altConnectionString =
                            @"Data Source=Pandora\SQLEXPRESS; Initial Catalog=Northwind;"
                            + @"Integrated Security=true;" + @"trustservercertificate=true";
                sqlConnection = new SqlConnection(altConnectionString);
                sqlConnection.Open();

                Console.WriteLine("Enter name of country to display sales:");
                string uShowCountrySales = Console.ReadLine();
                string showCountrySalesQuery =
                    "SELECT emp.FirstName, emp.LastName, SUM(ordd.UnitPrice) AS Total, ord.ShipCountry AS Country "
                    + "FROM Orders ord " + "INNER JOIN [Order Details] ordd " + "ON ordd.OrderID = ord.OrderID " +
                    "INNER JOIN Employees emp " + "ON ord.EmployeeID = emp.EmployeeID "
                    + $"WHERE ord.ShipCountry ='{uShowCountrySales}'"
                    + "GROUP BY emp.LastName, emp.FirstName, ord.ShipCountry "
                    + "ORDER BY Total DESC";
                SqlCommand command = new SqlCommand(showCountrySalesQuery, sqlConnection);

                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string firstName = (string)reader["FirstName"];
                        string lastName = (string)reader["LastName"];
                        decimal total = (decimal)reader["Total"];
                        string country = (string)reader["Country"];
                        Console.WriteLine($"{firstName} {lastName} - {country} - {total}");
                    }
                }
                sqlConnection.Close();
            }
        }

        public static void AddNewOrderForNewCustomer()
        {
            SqlConnection sqlConnection;
            string altConnectionString =
                        @"Data Source=Pandora\SQLEXPRESS; Initial Catalog=Northwind;"
                        + @"Integrated Security=true;" + @"trustservercertificate=true";
            sqlConnection = new SqlConnection(altConnectionString);
            sqlConnection.Open();

            string addNew =
                "INSERT INTO Customers" +
                "(CustomerID, CompanyName, ContactName, ContactTitle, Address, City, PostalCode, Country, Phone)" +
                "VALUES" +
                $"('AAAAA', 'Fictitious Horses', 'Anna Andersson', 'Sales Representative', 'Obere Str. 55', 'Berlin', '12209', 'Germany', '030-1074321');" +
                "INSERT INTO Orders " +
                "(CustomerID, EmployeeID, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipCountry, ShipPostalCode) " +
                "VALUES" +
                $"('AAAAA', '1', '1', '10', 'Fictitious Horses', 'Obere Str. 55', 'Berlin', 'Germany', '12209');" +
                $"INSERT INTO[Order Details]" +
                $"(OrderID, ProductID, UnitPrice, Quantity, Discount)" +
                $"VALUES" +
                $"(SCOPE_IDENTITY(), 7, 30, 1, 0);";

            SqlCommand addNewCommand = new SqlCommand(addNew, sqlConnection);
            addNewCommand.ExecuteNonQuery();
            Console.WriteLine($"Created new order for new customer successfully");
            sqlConnection.Close();
        }

        public static void DeleteNewOrderForNewCustomer()
        {
            SqlConnection sqlConnection;
            string altConnectionString =
                        @"Data Source=Pandora\SQLEXPRESS; Initial Catalog=Northwind;"
                        + @"Integrated Security=true;" + @"trustservercertificate=true";
            sqlConnection = new SqlConnection(altConnectionString);
            sqlConnection.Open();

            string deleteNew =
                $"DELETE FROM [Order Details] WHERE OrderID " +
                $"IN (SELECT o.OrderID FROM Orders o WHERE o.CustomerID = 'AAAAA');" +
                $"DELETE FROM Orders WHERE CustomerID = 'AAAAA';" +
                $"DELETE FROM Customers WHERE CustomerID = 'AAAAA';";
            SqlCommand deleteNewCommand = new SqlCommand(deleteNew, sqlConnection);
            deleteNewCommand.ExecuteNonQuery();
            Console.WriteLine($"Deleted new order for new customer successfully");
            sqlConnection.Close();
        }
    }
}