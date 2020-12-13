using System;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools
{
    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T>where T:new()
    {
        public T instance;
        public T GetInstance()
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }

}
