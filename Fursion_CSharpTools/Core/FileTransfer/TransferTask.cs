using System;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Core.FileTransfer
{
    public enum TransferTaskType
    {
        FileUpload,
        FileDownload
    }
    /// <summary>
    /// 传输作业实体
    /// </summary>
    public class TransferTask
    {
        public TransferTaskType TaskType { get; set; }
    }
}
