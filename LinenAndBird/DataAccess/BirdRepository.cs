using LinenAndBird.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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
            using var connection = new SqlConnection(_connectionString);
            // connections aren't open by default, we've gotta do that ourself
            connection.Open();

            // SQL Commands // This tells Sql what we want to do //
            var command = connection.CreateCommand();
            command.CommandText = @"Select *
                                    From Birds";

            // Executing that command against out connection //
            // Execute Reader // Used for when we care about getting all the results of our query //
            var reader = command.ExecuteReader();


            // Data readers only get one row from the results at a time, so we have to write a while loop to turn the reader into a bird or whatever it is we need in C# // The goal is to loop over all the results in the database and turn the info from the database and turn it into an object in C# // 

            var birds = new List<Bird>();

            while (reader.Read())
            {
                //// Mapping data from the relational maodel to the objec model //
                //var bird = new Bird();
                //// Can do it either way, the ones with the breacekts are preffered //
                //bird.Id = reader.GetGuid(0); // Gets the top right row of the results //
                //bird.Size = reader["Size"].ToString(); // Explicit Caste // Converts the object to string //
                //bird.Type = (BirdType)reader["Type"]; // Explicit Caste // Tells C# that I know that this doesn't look like a BirdType, but it is and believe me. // It will blow up the program if the result in the database was stored as Null //
                //bird.Color = reader["Color"].ToString();
                //bird.Name = reader["Name"].ToString();


                //// Each bird goes int the "birds" list to store for later //
                //birds.Add(bird);
                //// Created an instance of the bird class. Set all the properties to the data for the row that we are iterating over, and then storing it in the "bird" variable //

                var bird = MapFromReader(reader); // Refactored code above //

                birds.Add(bird);

            }

            return birds;

        }

        internal Bird Update(Guid id, Bird bird)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"update Birds
                                Set Color = @color,
	                            Name = @name,
	                            Type = @type,
	                            Size = @size 
                                output inserted.*
                                Where id = @id";
            // bird comes from http request in the controller
            cmd.Parameters.AddWithValue("Type", bird.Type);
            cmd.Parameters.AddWithValue("Color", bird.Color);
            cmd.Parameters.AddWithValue("Size", bird.Size);
            cmd.Parameters.AddWithValue("Name", bird.Name);
            cmd.Parameters.AddWithValue("id", id);

            // execution of the sql
            var reader = cmd.ExecuteReader();

            // working with the results
            if (reader.Read())
            {

                return MapFromReader(reader);
            }

            return null;
        }

        internal void Remove(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"Delete 
                                From Birds
                                Where Id = @id";

            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();

        }


        internal void Add(Bird newBird)
        {

            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            // SQL Commands // This tells Sql what we want to do //
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"insert into birds(Type, Color, Size, Name)
                                output inserted.Id
                                values (@Type,@Color,@Size,@Name)";

            cmd.Parameters.AddWithValue("Type", newBird.Type);
            cmd.Parameters.AddWithValue("Color", newBird.Color);
            cmd.Parameters.AddWithValue("Size", newBird.Size);
            cmd.Parameters.AddWithValue("Name", newBird.Name);

            // Execute the query, but don't care about the results, just the number of rows // 
            //var numberOfRowsAffected = cmd.ExecuteNonQuery();

            // execute the query and only get the id of the new row // 
            var newId = (Guid) cmd.ExecuteScalar(); // EXECUTE SCALER to get the top left row. The guid of the new bird //

            newBird.Id = newId; // Sets the new bird's Id to the newId variable we created above. //

            // This adds the bird to the database and updates the ID to whatever the database's Id was //
        }




        internal Bird GetById(Guid birdId)
        {
            // connections are like the tunnel between our app and the database //
            // USING // This closes the open connection when the logic in the "GetAll()" method is executed //
            using var connection = new SqlConnection(_connectionString);
            // connections aren't open by default, we've gotta do that ourself
            connection.Open();

            // SQL Commands // This tells Sql what we want to do //
            // This Time, with Peramiterzation //
            // DO THIS // 
            // PERAMITERZATION //
            var command = connection.CreateCommand();
            command.CommandText = @"Select *
                                    From Birds
                                    where id = @id";

            // Perameterization prevents sqp injection (little bobby tables) // 
            command.Parameters.AddWithValue("id", birdId);

            // Executing that command against out connection //
            // Execute Reader // Used for when we care about getting all the results of our query //
            var reader = command.ExecuteReader();

            // Only return one row, so we don't need a loop. Instead we can use an If statement //
            if (reader.Read())
            {
                //var bird = new Bird();
                //bird.Id = reader.GetGuid(0);
                //bird.Size = reader["Size"].ToString();
                //bird.Type = (BirdType)reader["Type"];
                //bird.Color = reader["Color"].ToString();
                //bird.Name = reader["Name"].ToString();

                //return bird;
                //// If there are tows to read, then return that bird we just read //
                
                // working witht he results
                return MapFromReader(reader); // Refactored code aboy
            }

            // If there are no matching rows to read, then return null //
            return null;
        }


        // Refactoring the Mapping into a private method //
        Bird MapFromReader(SqlDataReader reader)
        {
            var bird = new Bird();
            bird.Id = reader.GetGuid(0);
            bird.Size = reader["Size"].ToString();
            bird.Type = (BirdType)reader["Type"];
            bird.Color = reader["Color"].ToString();
            bird.Name = reader["Name"].ToString();

            return bird;
            // If there are tows to read, then return that bird we just read //
        }

    }
}// guid inheritance: object -> valuetype -> guid
 // A GUID is a 128-bit integer (16 bytes) that can be used across all computers and networks wherever a unique identifier is required.
 // very low probability of being duplicated.