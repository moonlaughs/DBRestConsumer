using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestCoinService.Model;

namespace DBRestConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private static string connectionString = ConnectionString.connectionString;

        // GET: api/Bids
        [HttpGet]
        public IEnumerable<Bid> Get()
        {
            string selectionString = "SELECT * FROM Bid;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(selectionString, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Bid> bids = new List<Bid>();
                        while (reader.Read())
                        {
                            Bid bid = ReadBid(reader);
                            bids.Add(bid);
                        }
                        return bids;
                    }
                }
            }
        }

        private Bid ReadBid(SqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string item = reader.GetString(1);
            double price = reader.GetDouble(2);
            string name = reader.GetString(3);

            Bid bid = new Bid()
            {
                Id = id,
                Item = item,
                Price = price,
                Name = name
            };

            return bid;
        }

        // GET: api/Bids/5
        [Route("{id}")]
        public Bid Get(int id)
        {
            string selectionString = "SELECT * FROM Bid WHERE id = @id;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(selectionString, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return ReadBid(reader);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        // POST: api/Bids
        [HttpPost]
        public int Post([FromBody] Bid value)
        {
            string postString = "INSERT INTO Bid (item, price, [name]) VALUES (@item, @price, @name);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(postString, conn))
                {
                    command.Parameters.AddWithValue("@item", value.Item);
                    command.Parameters.AddWithValue("@price", value.Price);
                    command.Parameters.AddWithValue("@name", value.Name);

                    int rowAffected = command.ExecuteNonQuery();
                    return rowAffected;
                }
            }
        }

        // PUT: api/Bids/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Bid value)
        {
            string putString = "UPDATE Bid SET item = @item, price = @price, [name] = @name WHERE id = @id;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(putString, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@item", value.Item);
                    command.Parameters.AddWithValue("@price", value.Price);
                    command.Parameters.AddWithValue("@name", value.Name);

                    int rowAffected = command.ExecuteNonQuery();
                    return rowAffected;
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            string deleteString = "DELETE FROM Bid WHERE id = @id;";

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(deleteString, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rowAffected = command.ExecuteNonQuery();
                    return rowAffected;
                }
            }
        }
    }
}
