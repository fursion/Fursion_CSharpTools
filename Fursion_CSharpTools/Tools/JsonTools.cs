using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fursion_CSharpTools.Tools
{
    /// <summary>
    /// Json工具类
    /// </summary>
    public static class JsonTools
    {
        /// <summary>
        /// 反序列化json字符串
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="JsonString">json字符串</param>
        /// <returns></returns>
        public static T JsonDeserializer<T>(string JsonString) where T : new()
        {
            T TT = new T();
            try
            {
                TT = JsonConvert.DeserializeObject<T>(JsonString);
            }
            catch (JsonSerializationException e)
            {
                FDebug.Log(e.Message);
            }
            return TT;
        }

    }
}
