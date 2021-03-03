using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineElection.Services
{
    public class JSONService<T>


    {

        public  string ToJSON(T t)
        {
            return JsonSerializer.Serialize(t);
        }

        public  T FromJSON(string str)
        {
            T t = default(T);
            try
            {
                if(JsonSerializer.Deserialize(str,typeof( T)) is T)
                {
                    t = (T)JsonSerializer.Deserialize(str, typeof(T));
                }
            }
            catch(InvalidCastException)
            {

            }
            return t;
        }
    }
}
