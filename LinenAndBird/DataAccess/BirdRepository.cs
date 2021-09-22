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
    public class BirdRepository
    {
        // use 'Using Microsoft.Data.SqlClient;' up above first
        // must have a connection to the database if you want to use ado.net:
        // use connectionstrings.com/sql-server/ for documentation
        readonly string _connectionString;

        // http request => IConfiguration => BirdRepository => BirdController

        //this is asking asp.net for the application configuration
        public BirdRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("LinenAndBird");
        }

        internal IEnumerable<Bird> GetAll()
        {
            //many-to-many relationship
            //using 2 queries to get all the birds and their accessories
            // Dapper way:
            using var db = new SqlConnection(_connectionString);
            var birds = db.Query<Bird>(@"Select * From Birds");

            var accessorySql = @"Select * From BirdAccessories";
            var accessories = db.Query<BirdAccessory>(accessorySql);

            foreach (var bird in birds)
            {
                bird.Accessories = accessories.Where(accessory => accessory.BirdId == bird.Id);
            }

            return birds;

            // ADO - 
            //connection.Open();

            //establish a connection
            //var command = connection.CreateCommand();
            //this is what tells sql what we want to do
            //command.CommandText = @"Select * 
            //                      From Birds";
            //then run command against connection
            //execute reader is for when we care about getting all the results of out query
            //this command only ever holds 1 row of data. when it starts it hold 0 rows of data
            //var reader = command.ExecuteReader();

            //make place to store each bird
            //var birds = new List<Bird>();

            //.Read() is a boolean value on each read row.
            //data readers are weird, only get one row from the results at a time
            //while (reader.Read()) //"while there is a next record, keep doing this..."
            //{

            //MAPPING DATA FROM THE RELATIONAL MODEL TO THE OBJECT MODEL (like ORM does for free)
            //after this explicit example we will use MapFromReader to save effort
            //var bird = new Bird();
            //bird.Id = reader.GetGuid(0); //ordinal call works great but you need to know where the column lives. No need to express data types this way.
            //bird.Size = reader["Size"].ToString(); //if specifically naming the column you want, you then need to convert it to the appropriate data type
            //Enum.TryParse<BirdType>(reader["Type"].ToString(),out var birdType);
            //bird.Type = (BirdType)reader["Type"]; //explicit cast when using custom type. if however the types are incorrect and not matching then things blow up
            //bird.Color = reader["Color"].ToString();
            //bird.Name = reader["Name"].ToString();

            //var bird = MapFromReader(reader);

            //each bird goes in the list to return later
            //    birds.Add(bird);
            //}

            //return birds;
        }

        internal Bird Update(Guid id, Bird bird)
        {
            // Dapper Way:
            using var db = new SqlConnection(_connectionString);
            var sql = @"update Birds 
                        Set Color = @color,
                            Name = @name,
                            Type = @type,
                            Size = @size
                        output inserted.*
                        Where id = @id";

            bird.Id = id;
            //if we have an object already that maps what we need, we can plug it in.
            //anonymous data type is not necessary and we can use the bird calls.
            var updatedBird = db.QuerySingleOrDefault(sql, bird);

            return updatedBird;

            // ADO way:
            //connection.Open();

            //var cmd = connection.CreateCommand();
            //cmd.CommandText = @"update Birds
            //Set Color = @color,
            //Name = @name,
            //Type = @type,
            //Size = @size
            //output inserted.*
            //Where Id = @id";

            //bird comes from the http request in the controller
            //note, capitalization dn have to match, just the word needs to match
            //cmd.Parameters.AddWithValue("Type", bird.Type);
            //cmd.Parameters.AddWithValue("Color", bird.Color);
            //cmd.Parameters.AddWithValue("Size", bird.Size);
            //cmd.Parameters.AddWithValue("Name", bird.Name);
            //cmd.Parameters.AddWithValue("Id", bird.Id);

            //execution of the sql against the database
            //var reader = cmd.ExecuteReader();

            //working with the results
            //if (reader.Read())
            //{
            //var updatedBird =  MapFromReader(reader);
            //return updatedBird;
            //}
            //return null;

        }

        internal void Remove(Guid id)
        {
            // Dapper way:
            using var db = new SqlConnection(_connectionString);

            var sql = @"Delete
                        From Birds 
                        Where Id = @id";

            db.Execute(sql, new { id });

            // ADO way:
            //connection.Open();

            //var cmd = connection.CreateCommand();
            //cmd.CommandText = @"Delete
            //                  From Birds 
            //                  Where Id = @id";

            //cmd.Parameters.AddWithValue("id", id);

            //cmd.ExecuteNonQuery(); //run the cmd then forget about it

        }

        internal void Add(Bird newBird)
        {
            // Dapper way:
            using var db = new SqlConnection(_connectionString);

            var sql = @"insert into Birds(Type,Color,Size,Name)
                                values(@Type, @Color, @Size, @Name)";

            //execute scalar, grab the guid data type from the sql, where the newBird properties match the variables (the @ sql parameters)

            var id = db.ExecuteScalar<Guid>(sql, newBird);
            newBird.Id = id;

            // ADO way:
            // connection.Open(); //opens the connection

            // var cmd = connection.CreateCommand(); //set up a command against the connection
            // cmd.CommandText = @"insert into Birds(Type,Color,Size,Name)
            //                 values(@Type, @Color, @Size, @Name)"; //express what command says

            // cmd.Parameters.AddWithValue("Type", newBird.Type); //map values to above @s (avoid sql injection)
            // cmd.Parameters.AddWithValue("Color", newBird.Color);
            // cmd.Parameters.AddWithValue("Size", newBird.Size);
            // cmd.Parameters.AddWithValue("Name", newBird.Name);

            //execute the query, but don't are about the results, just number of rows
            //var numberOfRowsAffected = cmd.ExecuteNonQuery(); //this cmd.ExecuteNonQuery() returns number of rows affected. bit of a misnomer. essentially it's not returning query results

            //execute the query and only get the id of the new row
            // var newId = (Guid) cmd.ExecuteScalar();

            // newBird.Id = newId;
            //_birds.Add(newBird);
        }

        internal Bird GetById(Guid birdId)
        {
            //one-to-many relationships with two separate queries
            // Dapper way:
            using var db = new SqlConnection(_connectionString);
            var birdsql = @"Select * From Birds where id = @id";
            var bird = db.QuerySingleOrDefault<Bird>(birdsql, new { id = birdId }); //new { type = property}
            if (bird == null) return null;
            var accessorysql = @"Select *
                                from Birds
                                where id = @id";
            var accessories = db.Query<BirdAccessory>(accessorysql, new { birdId });
            bird.Accessories = accessories;
            return bird;

            // ADO way:
            //using var connection = new SqlConnection(_connectionString);
            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText = @"Select * 
            //                        From Birds
            //                        where Id = @id"; //@id is 'parameterization', better than string interpolation because it protects against SQL injection attacks

            //parameterization prevents sql injection (little bobby tables)
            //command.Parameters.AddWithValue("id", birdId);

            //execute reader is for when we care about getting all the results of our query
            //var reader = command.ExecuteReader();

            //if (reader.Read())
            //{
            //    return MapFromReader(reader);
            //}

            //return default; -- if no match, return null;
            //return null; //same as above but easier to understand/read

            //return _birds.FirstOrDefault(bird => bird.Id == birdId);
        }

        Bird MapFromReader(SqlDataReader reader)
        {
            var bird = new Bird();
            bird.Id = reader.GetGuid(0);
            bird.Size = reader["Size"].ToString();
            bird.Type = (BirdType)reader["Type"];
            bird.Color = reader["Color"].ToString();
            bird.Name = reader["Name"].ToString();

            return bird;
        }
    }
}
