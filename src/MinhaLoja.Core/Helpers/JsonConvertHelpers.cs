using Newtonsoft.Json;

namespace MinhaLoja
{
    public static partial class Helpers
    {
        //Serialização de entidades ou objetos que as contêm
        public static string SerializeEntities(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}
