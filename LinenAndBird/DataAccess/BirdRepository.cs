using LinenAndBird.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;

namespace LinenAndBird.DataAccess
{
    public class BirdRepository
    {
        // Taking the string whiich is used for each method and declaring it as a variable for easier use and if we ever have to change it, we only have to change it in one place //
        const string _connectionString = ("Server=localhost;Database=LinenAndBird;Trusted_Connection=True;");


        internal IEnumerable<Bird> GetAll()
        {
            // connections are like the tunnel between our app and the database //
            // USING // This closes the open connection when the logic in the "GetAll()" method is executed //
            using var db = new SqlConnection(_connectionString);

            //Query<T> is for getting results from the database and putting them into a C# type //
            // On this connection I would like to execute this query, whatever results come back, I would like you to take those results and mash them into this Bird dhaped thing in C# and here is the query that is going to return those things. 
            // This one line does everything that is commented out beforee return birds!!!! //
            // This one line says the same thing as setting up a Command, setting it up as Reader, looping over it, mapping it into a bird shaped thing and adding it to the list //
            // Dapper opens and closes the connection for you //
            // It looks at the results that came back from SQL, and see's all the fields and looks for in the Type class, in this case Bird, for matching Properties and it moves those fields into those mathcing properties for you. Given they match, Dapper will figure it our and create instances with this Type with property filled out stuff //
             var birds = db.Query<Bird>(@"Select *
                                          From Birds");

            return birds;


            // connections aren't open by default, we've gotta do that ourself
            //connection.Open();

            //// SQL Commands // This tells Sql what we want to do //
            //var command = connection.CreateCommand();
            //command.CommandText = @"Select *
            //                        From Birds";

            //// Executing that command against out connection //
            //// Execute Reader // Used for when we care about getting all the results of our query //
            //var reader = command.ExecuteReader();


            //// Data readers only get one row from the results at a time, so we have to write a while loop to turn the reader into a bird or whatever it is we need in C# // The goal is to loop over all the results in the database and turn the info from the database and turn it into an object in C# // 

            //var birds = new List<Bird>();

            //while (reader.Read())
            //{
            //    //// Mapping data from the relational maodel to the objec model //
            //    //var bird = new Bird();
            //    //// Can do it either way, the ones with the breacekts are preffered //
            //    //bird.Id = reader.GetGuid(0); // Gets the top right row of the results //
            //    //bird.Size = reader["Size"].ToString(); // Explicit Caste // Converts the object to string //
            //    //bird.Type = (BirdType)reader["Type"]; // Explicit Caste // Tells C# that I know that this doesn't look like a BirdType, but it is and believe me. // It will blow up the program if the result in the database was stored as Null //
            //    //bird.Color = reader["Color"].ToString();
            //    //bird.Name = reader["Name"].ToString();


            //    //// Each bird goes int the "birds" list to store for later //
            //    //birds.Add(bird);
            //    //// Created an instance of the bird class. Set all the properties to the data for the row that we are iterating over, and then storing it in the "bird" variable //

            //    var bird = MapFromReader(reader); // Refactored code above //

            //    birds.Add(bird);

            //}

            //return birds;

        }

        internal Bird Update(Guid id, Bird bird)
        {
            using var db = new SqlConnection(_connectionString);

            // DAPPER WITH PERAMETERZATION //

            var sql = @"update Birds
                        Set Color = @color,
	                    Name = @name,
	                    Type = @type,
	                    Size = @size 
                     output inserted.*
                     Where id = @id";

            // Setting the id that was passed in to the bird.Id value //
            bird.Id = id;
            // Bc we already have the Object "Bird", all we have to pass is the sql and the property name(s). //
            var updatedBird = db.QuerySingleOrDefault<Bird>(sql, bird);

            return updatedBird;

            // All this does everything from like 80 to 129 //
            // DAPPER WITH PERAMETERZATION //

            //connection.Open();

            //var cmd = connection.CreateCommand();
            //cmd.CommandText = @"update Birds
            //                    Set Color = @color,
            //                 Name = @name,
            //                 Type = @type,
            //                 Size = @size 
            //                  output inserted.*
            //                  Where id = @id";
            //// bird comes from http request in the controller
            //cmd.Parameters.AddWithValue("Type", bird.Type);
            //cmd.Parameters.AddWithValue("Color", bird.Color);
            //cmd.Parameters.AddWithValue("Size", bird.Size);
            //cmd.Parameters.AddWithValue("Name", bird.Name);
            //cmd.Parameters.AddWithValue("id", id);

            //// execution of the sql
            //var reader = cmd.ExecuteReader();

            //// working with the results
            //if (reader.Read())
            //{

            //    return MapFromReader(reader);
            //}

            //return null;
        }

        internal void Remove(Guid id)
        {
            using var db = new SqlConnection(_connectionString);

            // DAPPER DELETE //
            
            // We aren't returning anything in this example //
            var sql  = @"Delete 
                         From Birds
                         Where Id = @id";

            db.Execute(sql, new { id = id });
            // OR // 
            // db.Execute(sql, new { id }); //

            // DAPPER DELETE //


            //connection.Open();

            //var cmd = connection.CreateCommand();
            //cmd.CommandText = @"Delete 
            //                    From Birds
            //                    Where Id = @id";

            //cmd.Parameters.AddWithValue("id", id);

            //cmd.ExecuteNonQuery();

        }


        internal void Add(Bird newBird)
        {

            using var db = new SqlConnection(_connectionString);
                    
            // DAPPER WITH PERAMETERZAION // ONLY WANT THE ID BACK IN THIS EXAMPLE //

            // When you execute scaler I would like a Guid back. Here's the query and then take the Bird object and whatever properties and values are on this new bird instance, map then to the parameters in my SQL query. This only works bc our bird class have all of those properties //
            var sql = @"insert into birds(Type, Color, Size, Name)
                        output inserted.Id
                        values (@Type,@Color,@Size,@Name)";

            // We only want the id //
            var id = db.ExecuteScalar<Guid>(sql, newBird);

            newBird.Id = id;
            // // DAPPER WITH PERAMETERZAION // ONLY WANT THE ID BACK IN THIS EXAMPLE //


            //connection.Open();

            //// SQL Commands // This tells Sql what we want to do //
            //var cmd = connection.CreateCommand();
            //cmd.CommandText = @"insert into birds(Type, Color, Size, Name)
            //                    output inserted.Id
            //                    values (@Type,@Color,@Size,@Name)";

            //cmd.Parameters.AddWithValue("Type", newBird.Type);
            //cmd.Parameters.AddWithValue("Color", newBird.Color);
            //cmd.Parameters.AddWithValue("Size", newBird.Size);
            //cmd.Parameters.AddWithValue("Name", newBird.Name);

            //// Execute the query, but don't care about the results, just the number of rows // 
            ////var numberOfRowsAffected = cmd.ExecuteNonQuery();

            //// execute the query and only get the id of the new row // 
            //var newId = (Guid) cmd.ExecuteScalar(); // EXECUTE SCALER to get the top left row. The guid of the new bird //

            //newBird.Id = newId; // Sets the new bird's Id to the newId variable we created above. //

            // This adds the bird to the database and updates the ID to whatever the database's Id was //
        }




        internal Bird GetById(Guid birdId)
        {
            // connections are like the tunnel between our app and the database //
            // USING // This closes the open connection when the logic in the "GetAll()" method is executed //
            using var db = new SqlConnection(_connectionString);
            // connections aren't open by default, we've gotta do that ourself
            //connection.Open();

            // DAPPER WITH PERAMETERZATION //
            var sql = @"Select *
                        From Birds
                        where id = @id";

            // passed in your Sql command, creating a new object(annonymous type), but real types can be used as well, that the property id is equal to the birdId value of that property //
            // This one line has replaced everything to line 213 //
            var bird = db.QueryFirst<Bird>(sql, new { id = birdId });
            // OR //
            //var bird = db.QuerySingle<Bird>(sql, new { id  = birdId });

            return bird;
            // DAPPER WITH PERAMETERZATION //

            //// SQL Commands // This tells Sql what we want to do //
            //// This Time, with Peramiterzation //
            //// DO THIS // 
            //// PERAMITERZATION //
            //var command = connection.CreateCommand();
            //command.CommandText = @"Select *
            //                        From Birds
            //                        where id = @id";

            //// Perameterization prevents sqp injection (little bobby tables) // 
            //command.Parameters.AddWithValue("id", birdId);

            //// Executing that command against out connection //
            //// Execute Reader // Used for when we care about getting all the results of our query //
            //var reader = command.ExecuteReader();

            //// Only return one row, so we don't need a loop. Instead we can use an If statement //
            //if (reader.Read())
            //{
            //    //var bird = new Bird();
            //    //bird.Id = reader.GetGuid(0);
            //    //bird.Size = reader["Size"].ToString();
            //    //bird.Type = (BirdType)reader["Type"];
            //    //bird.Color = reader["Color"].ToString();
            //    //bird.Name = reader["Name"].ToString();

            //    //return bird;
            //    //// If there are tows to read, then return that bird we just read //

            //    // working witht he results
            //    return MapFromReader(reader); // Refactored code aboy
            //}

            //// If there are no matching rows to read, then return null //
            //return null;
        }

        // WHEN WE STARTED USING DAPPER, WE DON'T NEED THIS MAP READER FUNCTION ANYMORE //
        // Refactoring the Mapping into a private method //
        //Bird MapFromReader(SqlDataReader reader)
        //{
        //    var bird = new Bird();
        //    bird.Id = reader.GetGuid(0);
        //    bird.Size = reader["Size"].ToString();
        //    bird.Type = (BirdType)reader["Type"];
        //    bird.Color = reader["Color"].ToString();
        //    bird.Name = reader["Name"].ToString();

        //    return bird;
        //    // If there are tows to read, then return that bird we just read //
        //}

    }
}// guid inheritance: object -> valuetype -> guid
 // A GUID is a 128-bit integer (16 bytes) that can be used across all computers and networks wherever a unique identifier is required.
 // very low probability of being duplicated.