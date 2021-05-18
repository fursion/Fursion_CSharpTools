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
        public ProcessingAction ProcessingAction;
        Thread PossingThread;
        public IPC()
        {
            IPC_DATA_POOL = new List<Tuple<byte[], Connect>>();
            ThreadPool.QueueUserWorkItem(Possing_Action);
        }
        /// <summary>
        /// 压入数据到数据池
        /// </summary>
        /// <param name="BS"></param>
        /// <param name="connect"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 从数据池移除数据
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="connect"></param>
        /// <returns></returns>
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
                        ProcessingAction?.BeginInvoke(IPC_DATA_POOL[0].Item1, IPC_DATA_POOL[0].Item2, null, null);
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
