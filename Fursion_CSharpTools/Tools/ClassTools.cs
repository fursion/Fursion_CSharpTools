using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
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
    /// <summary>
    /// 委托=>处理远端连接发来的信息
    /// </summary>
    /// <param name="bs"></param>
    /// <param name="connect"></param>
    public delegate void ProcessingAction(byte[] bs,Connect connect);
    /// <summary>
    /// 工具类
    /// </summary>
    public static class CSharpTools
    {
        /// <summary>
        /// 打印·Byte数组
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
        /// <summary>
        /// 通过给定的方法名和类名寻找并执行方法
        /// </summary>
        /// <param name="ClassName">类型名</param>
        /// <param name="MethodName">方法名</param>
        public static void GetMethodDo(string ClassName,string MethodName)
        {
            Type type = Type.GetType(ClassName);
            object obj = System.Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod(MethodName, new Type[] { });
            object[] parameters = null;
            method.Invoke(obj,parameters);
        }
        /// <summary>
        /// 在指定的类型中通过方法名找到方法并执行
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="MethodName">方法名</param>
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
        /// <summary>
        /// 静态实例
        /// </summary>
        public static T instance;
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
    /// <summary>
    /// 信息接口
    /// </summary>
    public abstract class INfo
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }

}
