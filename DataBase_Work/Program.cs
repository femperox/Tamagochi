using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DataBase_Work
{
    class Program
    {

        // удаление таблицы
        static async Task DropTableAsync(string conString, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand($"DROP TABLE {tableName}", connection);
                await command.ExecuteNonQueryAsync();
                Console.WriteLine($"Таблица {tableName} удалена");
            }
        }

        // удаление процедуры
        static async Task DropProcAsync(string conString, string procName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand($"DROP PROCEDURE {procName}", connection);
                await command.ExecuteNonQueryAsync();
                Console.WriteLine($"Процедура {procName} удалена");
            }
        }

        // создать таблиц айтемов
        static async Task CreateItemTableAsync(string conString)
        {
            string procInserting = @"CREATE PROCEDURE [dbo].[InsertItem]
                                @type nvarchar(20),
                                @name nvarchar(30),
                                @price int,
                                @health_stat int,   
                                @hunger_stat int,
                                @fun_stat int,
                                @clean_stat int
                            AS
                                INSERT INTO Items (Type, Name, Price, Health_stat, Hunger_stat, Fun_stat, Clean_stat)
                                VALUES (@type, @name, @price, @health_stat, @hunger_stat, @fun_stat, @clean_stat)
   
                                SELECT SCOPE_IDENTITY()
                            GO";

            string procGetting = @"CREATE PROCEDURE [dbo].[GetItem]
                                @name nvarchar(30)
                                AS
                                    SELECT * FROM Items WHERE Name LIKE '%' + @name + '%';
                                ";
                              


            using (SqlConnection con = new SqlConnection(conString))
            {
                await con.OpenAsync();

                SqlCommand command = new SqlCommand();
                command.CommandText = "CREATE TABLE Items (Id INT PRIMARY KEY IDENTITY, Type NVARCHAR(20) NOT NULL, Name NVARCHAR(30) NOT NULL, Price INT NOT NULL," +
                                      "Health_stat INT NOT NULL, Hunger_stat INT NOT NULL, Fun_stat INT NOT NULL, Clean_stat INT NOT NULL)";
                command.Connection = con;
                await command.ExecuteNonQueryAsync();

                Console.WriteLine("Таблица Items создана");

                command.CommandText = procInserting;
                await command.ExecuteNonQueryAsync();

                command.CommandText = procGetting;
                await command.ExecuteNonQueryAsync();

                Console.WriteLine("Хранимые процедуры для таблицы Items добавлены в базу данных.");

            }
        }

        // создать таблицу-инвентарь
        static async Task CreateItemInventoryAsync(string conString)
        {
            string procInserting = @"CREATE PROCEDURE [dbo].[InsertItemInventory]
                                @name nvarchar(30),
                                @amout int
                            AS
                                DECLARE @item_id int                                
                                SELECT @item_id=Id FROM Items WHERE @name = Name

                                DECLARE @count int
                                SELECT @count = COUNT(*) FROM Inventory WHERE @item_id = Item_id

                           BEGIN
                                IF (@count = 0) BEGIN
                                    INSERT INTO Inventory (Item_id, Amout)
                                    VALUES (@item_id, @amout)
   
                                    SELECT SCOPE_IDENTITY()
                                END
                                ELSE BEGIN
                                    UPDATE Inventory SET Amout = Amout + @amout WHERE Item_id = @item_id
                                    SELECT @item_id
                                END
                                
                            END";

            string procGetting = @"CREATE PROCEDURE [dbo].[GetItemInventory]
                                @name nvarchar(30)
                                AS
                                    DECLARE @item_id int                                
                                    SELECT TOP 1 @item_id=Id FROM Items WHERE @name = Name
                               BEGIN     
                                    SELECT * FROM Inventory WHERE @item_id = Item_id;
                               END
                                ";



            using (SqlConnection con = new SqlConnection(conString))
            {
                await con.OpenAsync();

                SqlCommand command = new SqlCommand();
                command.CommandText = "CREATE TABLE Inventory (Id INT PRIMARY KEY IDENTITY, Item_id INT FOREIGN KEY REFERENCES Items(Id), Amout INT NOT NULL)";
                command.Connection = con;
                await command.ExecuteNonQueryAsync();

                Console.WriteLine("Таблица Inventory создана");

                command.CommandText = procInserting;
                await command.ExecuteNonQueryAsync();

                command.CommandText = procGetting;
                await command.ExecuteNonQueryAsync();

                Console.WriteLine("Хранимые процедуры для таблицы Inventory добавлены в базу данных.");

            }
        }

        // создать айтем
        static async Task InsertItemAsync(string conString, string type, string name, int price = 0, int health_stat = 0, int hunger_stat = 0, int fun_stat = 0, int clean_stat = 0)
        {
            string sqlExpression = "InsertItem";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;


                if ((!type.Equals("Food")) && (!type.Equals("Toy")) && (!type.Equals("Soap"))) throw new Exception("Error: Item Type");

                SqlParameter typeParam = new SqlParameter
                {
                    ParameterName = "@type",
                    Value = type
                };
                command.Parameters.Add(typeParam);

            
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);

                SqlParameter priceParam = new SqlParameter
                {
                    ParameterName = "@price",
                    Value = price
                };
                command.Parameters.Add(priceParam);

                SqlParameter health_statParam = new SqlParameter
                {
                    ParameterName = "@health_stat",
                    Value = health_stat
                };
                command.Parameters.Add(health_statParam);

                SqlParameter hunger_statParam = new SqlParameter
                {
                    ParameterName = "@hunger_stat",
                    Value = hunger_stat
                };
                command.Parameters.Add(hunger_statParam);

                SqlParameter fun_statParam = new SqlParameter
                {
                    ParameterName = "@fun_stat",
                    Value = fun_stat
                };
                command.Parameters.Add(fun_statParam);

                SqlParameter clean_statParam = new SqlParameter
                {
                    ParameterName = "@clean_stat",
                    Value = clean_stat
                };
                command.Parameters.Add(clean_statParam);

                var id = await command.ExecuteScalarAsync();

                Console.WriteLine($"Id добавленного объекта: {id}");

            }
        }

        // получить айтем по имени
        static async Task GetItemAsync(string conString, string name="")
        {
            string sqlExpression = "GetItem";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"{reader.GetName(0)}\t\t{reader.GetName(2)}\t\t{reader.GetName(1)}");

                        while (await reader.ReadAsync())
                        {
                            int id = reader.GetInt32(0);
                            string type = reader.GetString(2);
                            name = reader.GetString(1);
                            Console.WriteLine($"{id}\t\t{type}\t\t{name}");
                        }
                    }
                }

            }

        }

        static async Task InsertItemInventoryAsync(string conString, string name, int amout)
        {
            string sqlExpression = "InsertItemInventory";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);

                SqlParameter amoutParam = new SqlParameter
                {
                    ParameterName = "@amout",
                    Value = amout
                };
                command.Parameters.Add(amoutParam);


                var id = await command.ExecuteScalarAsync();

                Console.WriteLine($"Id добавленного объекта: {id}");

            }
        }

        static async Task Main(string[] args)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=adonetdb;Trusted_Connection=True;";


            await InsertItemInventoryAsync(connectionString, "Simple Soap", 2);

        }
    }
}
