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
        static List<Bird> _birds = new List<Bird>
        {
            new Bird
            {
                Id = Guid.NewGuid(),
                Name = "Jimmy",
                Color = "Red",
                Size = "Small",
                Type = BirdType.Dead,
                Accessories = new List<string> { "Beanie", "Gold wing tips" }
            }
        };

        internal IEnumerable<Bird> GetAll()
        {
            //connections are like the tunnel between our app and the database
            using var connection = new SqlConnection("Server=localhost;Database=LinenAndBird;Trusted_Connection=True;");
            //connections aren't open by default, we've gotta do that ourself
            connection.Open();

            //this is what tells sql what we want to do
            var command = connection.CreateCommand();
            command.CommandText = @"Select *
                                    From Birds";

            //execute reader is for when we care about getting all the results of our query
            var reader = command.ExecuteReader();

            var birds = new List<Bird>();

            //data readers are weird, only get one row from the results at a time
            while (reader.Read())
            {
                //Mapping data from the relational model to the object model
                var bird = new Bird();
                bird.Id = reader.GetGuid(0);
                bird.Size = reader["Size"].ToString();

                //Enum.TryParse<BirdType>(reader["Type"].ToString(),out var birdType);
                //bird.Type = birdType;

                bird.Type = (BirdType)reader["Type"];
                bird.Color = reader["Color"].ToString();
                bird.Name = reader["Name"].ToString();

                //each bird goes in the list to return later
                birds.Add(bird);
            }

            return birds;
        }

        internal void Add(Bird newBird)
        {
            newBird.Id = Guid.NewGuid();

            _birds.Add(newBird);
        }

        internal Bird GetById(Guid birdId)
        {
            //connections are like the tunnel between our app and the database
            using var connection = new SqlConnection("Server=localhost;Database=LinenAndBird;Trusted_Connection=True;");
            //connections aren't open by default, we've gotta do that ourself
            connection.Open();

            //this is what tells sql what we want to do
            var command = connection.CreateCommand();
            command.CommandText = @"Select *
                                    From Birds
                                    where id = @id";

            //parameterization prevents sql injection (little bobby tables)
            command.Parameters.AddWithValue("id", birdId);

            //execute reader is for when we care about getting all the results of our query
            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var bird = new Bird();
                bird.Id = reader.GetGuid(0);
                bird.Size = reader["Size"].ToString();
                bird.Type = (BirdType)reader["Type"];
                bird.Color = reader["Color"].ToString();
                bird.Name = reader["Name"].ToString();

                return bird;
            }

            return null;

            //return _birds.FirstOrDefault(bird => bird.Id == birdId);
        }
    }
}
// guid inheritance: object -> valuetype -> guid
// A GUID is a 128-bit integer (16 bytes) that can be used across all computers and networks wherever a unique identifier is required.
// very low probability of being duplicated.