using LinenAndBird.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        readonly string _connectionString;

        public HatRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("LinenAndBird");
        }

        internal Hat GetById(Guid hatId)
        {
            // Create Connection //
            using var db = new SqlConnection(_connectionString);

            // Sql Command and Execution //
            var hat = db.QueryFirstOrDefault<Hat>(@"Select *
                                                    From Hats
                                                    where Id = @id",
                                                    new { id = hatId });

            /* for paramerters this is what dapper is doing internally, basically:
            *
            *For each property on the paramerter object
            *
            *Add a parameter with value to the sql command
            *
            *and for each 
            *
            *execute the command 
            *
            */

            return hat;

            //return _hats.FirstOrDefault(hat => hat.Id == hatId);
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
