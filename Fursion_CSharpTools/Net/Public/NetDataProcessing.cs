using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools.Tools;

namespace Fursion_CSharpTools.Net.Public
{
    public delegate T DataProcessJodAction<T>(DataProcessJod jod);
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
    public struct DataProcessJod : IDisposable, IJobAsync
    {
        public DataProcessJodAction<int> action;
        public AsyncCallback AsyncCallback;
        public bool State;
        public byte[] Data;
        public Connect Connect;

        public IAsyncResult BeginExecute(AsyncCallback asyncCallback, object ob)
        {
            return action.BeginInvoke(this, asyncCallback, ob);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public void EndExecute(IAsyncResult result)
        {
            action.EndInvoke(result);
        }
    }
    public interface IJobAsync
    {
        IAsyncResult BeginExecute(AsyncCallback asyncCallback, object @object);
        void EndExecute(IAsyncResult result);
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
        /// <summary>
        /// 取出一份数据处理作业
        /// </summary>
        /// <returns>作业实体</returns>
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
            DataProcessing.GetInstance().ExecuteJobAsync();
        }
        public async Task ExecuteJobAsync()
        {
            while (true)
            {
                if (DataJobQueue.Count <= 0)
                    continue;
                DataProcessJod job = GetTobJob();
                if (job.State)
                {
                    job.action = JOBACTION;
                    job.AsyncCallback = Callback;
                    Task task = Task.Run(() => {job.action.Invoke(job); });
                    var followuptask = task.ContinueWith(Callback);
                    await followuptask;
                }

            }
        }
        public int JOBACTION(DataProcessJod jod)
        {

            return 0;
        }
        public void Callback(IAsyncResult result)
        {
            DataProcessJod jod = (DataProcessJod)result.AsyncState;
            var value = jod.action.EndInvoke(result);          
            Console.WriteLine(value);
        }
    }

}
