using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools.Tools;

namespace Fursion_CSharpTools.Net.Public
{
    public delegate void DataProcessJodAction(DataProcessJod jod);
    /// <summary>
    /// 
    /// </summary>
    class NetDataProcessing
    {
        public static void DecryptNetBytes(byte[] NetBytes, Connect connect)//解密网络数据
        {
            var DBytes = NetBytes.AesEncode("def key");

        }
        public static void DecryptNetBytes(byte[] NetBytes, Connect connect, ConnnctAction connnctAction)
        {

        }
    }
    /// <summary>
    /// 数据处理作业实体，适用于服务端
    /// </summary>
    public struct DataProcessJod : IFuJob, IDisposable
    {
        public DataProcessJodAction action;
        public bool State;
        public byte[] Data;
        public Connect Connect;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Execute(DataProcessJodAction action)
        {
            action?.Invoke(this);
        }
        public void Execute(object ob)
        {
            action?.Invoke(this);
        }
    }
    public interface IFuJob
    {
        void Execute(DataProcessJodAction action);

    }

    public class DataProcessing : Singleton<DataProcessing>
    {
        private Queue<DataProcessJod> DataJobQueue;
        public DataProcessing()
        {
            DataJobQueue = new Queue<DataProcessJod>();
            ThreadPool.QueueUserWorkItem(ThreadAction);

        }
        public bool AddData(DataProcessJod DataJob)
        {
            try
            {
                DataJobQueue.Enqueue(DataJob);
                return true;
            }
            catch (Exception e)
            {
                FDebug.Log(e.Message);
                return false;
            }

        }
        public DataProcessJod GetTobJob()
        {
            try
            {
                var job = DataJobQueue.Dequeue();
                return job;
            }
            catch (Exception e)
            {
                FDebug.Log(e.Message);
                return new DataProcessJod() { State = false };
            }
        }
        public static void ThreadAction(object self)
        {
            DataProcessing.GetInstance().ExecuteJob();
        }
        public void ExecuteJob()
        {
            while (true)
            {
                if (DataJobQueue.Count <= 0)
                    continue;
                var job = GetTobJob();
                if (job.State)
                {
                    job.action = (o) => { FDebug.Log(BitConverter.ToString(o.Data)); };
                    ThreadPool.QueueUserWorkItem(job.Execute);
                    job.Dispose();
                    continue;
                }
            }

        }
    }

}
