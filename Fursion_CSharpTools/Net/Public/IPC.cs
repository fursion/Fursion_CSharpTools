using System;
using System.Collections.Generic;
using System.Text;
using Fursion_CSharpTools;
using System.Threading;
using ProtocolTools;
/******************************
IPC 数据处理中心
Developer: fursion
Email:fursion@fursion.cn 
*******************************/
namespace Fursion_CSharpTools.Net.Public
{
    /// <summary>
    /// 数据处理中心
    /// </summary>
    public class IPC : Singleton<IPC>
    {
        /// <summary>
        /// 数据缓存池
        /// </summary>
        public static List<Tuple<byte[],Connect>> IPC_DATA_POOL;
        Thread PossingThread;
        public IPC()
        {
            IPC_DATA_POOL = new List<Tuple<byte[], Connect>>();
            ThreadPool.QueueUserWorkItem(Possing_Action);
        }
        public int InComing_DATA(byte[] BS,Connect connect)
        {
            try
            {
                IPC_DATA_POOL.Add(new Tuple<byte[], Connect>(BS,connect));
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public int Remove_DATA(byte[] bs,Connect connect)
        {
            try
            {
                IPC_DATA_POOL.Remove(new Tuple<byte[], Connect>(bs,connect));
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }         
        }
        public void Possing_Action(object state)
        {
            while (true)
            {
                if (IPC_DATA_POOL.Count > 0)
                {
                    try
                    {
                        CSharpTools.PrintByteArray(IPC_DATA_POOL[0].Item1);
                        byte[] b = { 0, 1, 2, 3 };
                        IPC_DATA_POOL[0].Item2.Send(b);
                        IPC_DATA_POOL.RemoveAt(0);
                    }
                    catch
                    {

                    }
                }
                
            }          
        }
    }
}
