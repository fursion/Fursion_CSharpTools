using System;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Core.FileTransfer
{
    public static class TransferManager
    {
        /// <summary>
        /// 运行中的Transfer service列表
        /// </summary>
        public static List<TransferService> TransferTaskList = new List<TransferService>();
        public static void AddNewTransferTask(TransferService transferService)
        {

        }
        public static void RemoveTransferTask(TransferService transfer)
        {
            if(TransferTaskList.Contains(transfer))
            {
                TransferTaskList.Remove(transfer);
            }
        }
        public static void ShowNowServicelist()
        {
            if (TransferTaskList.Count != 0)
            {

            }
            Console.WriteLine("No Transfer Task");
        }
        public static void CreateTransferService()
        {

        }
    }
}
