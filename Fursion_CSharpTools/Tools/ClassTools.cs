using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Fursion_CSharpTools.Net.Public;

namespace Fursion_CSharpTools
{
    /// <summary>
    /// 套接字委托，用以承接在收到数据后的自定义处理函数
    /// </summary>
    /// <param name="bs"></param>
    public delegate void SocketCallBack(byte[] bs);
    /// <summary>
    /// 连接类委托事件
    /// </summary>
    /// <param name="bs"></param>
    public delegate void ConnnctAction(byte[] bs);

    public delegate void ProcessingAction(byte[] bs,Connect connect);
    /// <summary>
    /// 工具类
    /// </summary>
    public static class CSharpTools
    {
        /// <summary>
        /// 将Byte数组输出在控制台
        /// </summary>
        /// <param name="bs"></param>
        public static void PrintByteArray(this byte[] bs)
        {
            string str = "";
            foreach (var item in bs)
            {
                str += item.ToString();
                str += "  ";
            }
            Console.WriteLine(str);
        }
        /// <summary>
        /// Key校验
        /// </summary>
        /// <param name="Key_Standard"></param>
        /// <param name="Key_Input"></param>
        /// <returns></returns>
        public static bool Check_Key(string Key_Standard, string Key_Input)
        {
            if (Key_Standard == Key_Input)
                return true;
            return false;
        }
        public static void GetMethodDo(string ClassName,string MethodName)
        {
            Type type = Type.GetType(ClassName);
            object obj = System.Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(MethodName, new Type[] { });
            object[] parameters = null;
            method.Invoke(obj,parameters);
        }
        public static void GetMethodDo<T>(string MethodName) where T:new()
        {
            T obj = new T();
            Type type = obj.GetType();
            MethodInfo method = type.GetMethod(MethodName, new Type[] { });
            object[] parameters = null;
            method.Invoke(obj, parameters);
        }
        
    }

    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        public static T instance;
        public static T GetInstance()
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }

}
