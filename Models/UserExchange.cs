using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserExchange
    {
        public List<ExchangeInstance> ExchangeList { get; set; }
        public User User { get; set; }

        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Object Id { get; set; }

    }
}
