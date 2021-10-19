using LinenAndBird.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;


namespace LinenAndBird.DataAccess
{
    public class OrdersRepository
    {
        //static List<Order> _orders = new List<Order>();

        const string _connectionString = "Server=localhost;Database=LinenAndBird;Trusted_Connection=True;";

        internal void Add(Order order)
        {
            
            // Create a connection //
            using var db = new SqlConnection(_connectionString);


            //order.Id = Guid.NewGuid(); // for a unique identifier

            // Write Query Command //
                        var sql = @"INSERT INTO [dbo].[Orders]
			                        ([BirdId]
                                    ,[HatId]
                                    ,[Price])
                            Output inserted.Id
                            VALUES
                                     (@BirdId
                                     ,@HatId
                                     ,@Price)";

            // Execute the Query // We want the upper left row //

            // Bc there are so many to set, we made it a variable for cleaner code in the ExecuteScaler below //
            var parameters = new 
            { 
                BirdId = order.Bird.Id,
                HatId = order.Hat.Id,
                Price = order.Price 
            };

            var id = db.ExecuteScalar<Guid>(sql, parameters);

            order.Id = id;


            //_orders.Add(order);
        }

        internal IEnumerable<Order> GetAll()
        {

            // Create a connection // 
            using var db = new SqlConnection(_connectionString);

            // Sql command //
            var sql = @"select * 
                        from Orders o
	                        join Birds b
		                        on b.Id = o.BirdId
	                        join Hats h
		                        on h.Id = o.HatId";

            // Executing Query //
            //  MULTI-MAPPING // - One row in SQL data and turning it into more than one C# object type //
            // In this query, we have orders, hats, and birds. What we wanna do is turn each row of this SQL result set into one Order, on Bird, and one Hat //

            // First set of Arguments passed in are what we are getting back, Order, Bird, Hat. Then the last argument is saying that all of those things, at the end of the day really belong in this argument // We are going to return one order at the end of the day // What we really want back is one Order Class //
            // After the SQL command, we need to pass in a function that takes in an order, a bird, and a hat, and returns to it an order //
            // It's our job to take an order, a bird, and a hat and combine those into one order //

            var results = db.Query<Order, Bird, Hat, Order>(sql, Map, splitOn: "Id");

            return results;

            // MULTI-MAPPING //

        }

        internal Order Get(Guid id)
        {

            // Create a connection // 
            using var db = new SqlConnection(_connectionString);

            // Sql command //
            var sql = @"select * 
                        from Orders o
	                        join Birds b
		                        on b.Id = o.BirdId
	                        join Hats h
		                        on h.Id = o.HatId
                        where o.id = @id"; 

            // multi-mapping doesn't work for any other kind of dapper call so we take the collection and turn it into one item ourselves //
            // With multi-mapping, the map function is passed in second and the params are passed in third //
            var orders = db.Query<Order, Bird, Hat, Order>(sql, Map, new { id = id }, splitOn: "Id"); // Tells dapper to split on a column. In this case it's "Id" 


            return orders.FirstOrDefault(); // Give me just the first order matching the id from the collection above // 
        }

        Order Map(Order order, Bird bird, Hat hat) // We previously wrote this in our Get and GetAll functions. but made a function for it so we could just call it and clean up the code //
        {
            order.Bird = bird; // Set Bird property on the order to be the bird property that dapper passed in. //
            order.Hat = hat; // Set Hat property on the order to be the hat property that dapper passed in. //
            return order; // Bc we need to return an order, we need to return the order that got passed in //
        }

        
    }
}
