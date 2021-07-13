using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Fursion_CSharpTools.AsyncJob;
using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools.Tools;

namespace Fursion_CSharpTools.Net.Public
{
    /// <summary>
    /// 数据处理作业实体，适用于服务端
    /// </summary>
    public struct DataProcessJod : IDisposable, IJobTask
    {
        public bool State;
        public DataPacket DataPacket;
        public void CallBack(object obj)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public void Execute(object obj)
        {
            throw new NotImplementedException();
        }
    }
    public struct DataPacket
    {
        private bool inuse;
        public bool InUse { get { return inuse; } set { inuse = value; } }
        private byte[] data;
        public byte[] Data { get { return data; } set { data = value; } }
        private Connect connect;
        public Connect Connect { get { return connect; } set { connect = value; } }
        public DataPacket(byte[] _data, Connect _connect)
        {
            data = _data; connect = _connect; inuse = true;
        }
    }
    public class DataProcessing : Singleton<DataProcessing>
    {
        Queue<DataPacket> DataPacketQueue = new Queue<DataPacket>();
    }

}
