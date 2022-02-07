using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Exchange
    {
        public string Name { get; set; }
        public List<Strategy> AvailableStrategies { get; set; }
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Object Id { get; set; }
    }
}
