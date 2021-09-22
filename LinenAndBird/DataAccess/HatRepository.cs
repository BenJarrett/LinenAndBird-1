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
    public class HatRepository
    {
        static List<Hat> _hats = new List<Hat>
            {
                new Hat
                {
                    Id = Guid.NewGuid(),
                    Color = "Blue",
                    Designer = "Charlie",
                    Style = HatStyle.OpenBack
                },
                new Hat
                {
                    Id = Guid.NewGuid(),
                    Color = "Black",
                    Designer = "Nathan",
                    Style = HatStyle.WideBrim
                },
                new Hat
                {
                    Id = Guid.NewGuid(),
                    Color = "Magenta",
                    Designer = "Charlie",
                    Style = HatStyle.Normal
                }
            };

        string _connectionString;

        public HatRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("LinenAndBird");
        }

        internal Hat GetById(Guid id)
        {
            //create connection 
            using var db = new SqlConnection(_connectionString);

            var hat = db.QueryFirstOrDefault<Hat>("Select * from Hats where Id = @id", new { id });

            /*for parameters this is what dapper is doing internally, basically:
             *
             *for each property on the parameter object
             *
             * add a parameter with value to the sql command
             *
             *end for each
             *
             * execute the command
             *
             */

            return hat;
        }

        internal List<Hat> GetAll()
        {
            return _hats;
        }

        internal IEnumerable<Hat> GetByStyle(HatStyle style)
        {
            return _hats.Where(hat => hat.Style == style);
        }

        internal void Add(Hat newHat)
        {
            newHat.Id = Guid.NewGuid();

            _hats.Add(newHat);
        }
    }
}
