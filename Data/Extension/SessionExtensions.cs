using Newtonsoft.Json;

namespace TestTask.Data.Extensions;

public static class SessionExtensions
{
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObject<T>(this ISession session, string key) where T : class
    {
        var value = session.GetString(key);
        return value == null ? null! : JsonConvert.DeserializeObject<T>(value)!;
    }
}
