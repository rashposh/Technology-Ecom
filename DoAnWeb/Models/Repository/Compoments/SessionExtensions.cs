﻿using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace DoAnWeb.Models.Repository.Compoments
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}
