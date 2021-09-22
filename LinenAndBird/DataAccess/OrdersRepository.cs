using LinenAndBird.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace LinenAndBird.DataAccess
{
    public class OrdersRepository
    {
        string _connectionString;
        //this is asking asp.net for the application configuration 
        public OrdersRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("LinenAndBird");
        }
        internal IEnumerable<Order> GetAll()
        {
            //create a connection
            using var db = new SqlConnection(_connectionString);

            var sql = @"Select * 
                          From Orders o 
                            join Birds b
                                on b.Id = o.BirdId
                            join Hats h
                                on h.Id = o.HatId";

            //takes one row of sql data and maps into more than one c# object
            //last argument tells c# that it all belongs to the Order object
            var results = db.Query<Order, Bird, Hat, Order>(sql, (order, bird, hat) =>
            {
                order.Bird = bird; //order.Bird will equal the bird object we created from the sql
                order.Hat = hat;   //order.Hat will equal the hat object we created from the sql 
                return order;
            }, splitOn: "Id"); //the default value, but explicitly writing it so that we know about it -- tells which columns to split the sql data by so each chunk can map into our objects

            return results;

        }

        internal void Add(Order order)
        {
            //Create a connection
            // using 'disposes' the variable, and must be disposable in the first place (i.e. has a .Dispose() method), when the current code block completes
            using var db = new SqlConnection(_connectionString);

            var sql = @"INSERT INTO [dbo].[Orders]
                               ([BirdId]
                               ,[HatId]
                               ,[Price])
                         Output inserted.Id
                         VALUES
                               (@BirdId
                               ,@HatId
                               ,@Price)";

            var parameters = new
            {
                BirdId = order.Bird.Id,
                HatId = order.Hat.Id,
                Price = order.Price
            };

            var id = db.ExecuteScalar<Guid>(sql, parameters);

            order.Id = id;


            //order.Id = Guid.NewGuid();
            //_orders.Add(order);
        }

        internal Order Get(Guid id)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Select * 
                          From Orders o 
                            join Birds b
                                on b.Id = o.BirdId
                            join Hats h
                                on h.Id = o.HatId
                        where o.id = @id";

            //multimapping doesn't work for any other kind of dapper call
            //so we take the collection and turn it into one item ourselves
            var orders = db.Query<Order, Bird, Hat, Order>(sql, Map,
                new { id }, //third argument is different, explains what maps to @id
                splitOn: "Id");

            return orders.FirstOrDefault();
        }

        Order Map(Order order, Bird bird, Hat hat)
        {
            order.Bird = bird;
            order.Hat = hat;
            return order;
        }
    }
}