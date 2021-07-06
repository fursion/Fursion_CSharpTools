using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fursion_CSharpTools.AsyncJob
{
    public interface IJobTask
    {
        void CallBack(object obj);
        void Execute(object obj);

    }
    public interface IJobTaskGet<T>
    {
        void CallBack(object obj);
        T Execute(object obj);

    }
    public class TaskCore
    {
        public void RunTask(Task task)
        {
            task.Start();
            IJobTask jobTask = (IJobTask)task.AsyncState;
        }
        public static void Run(IJobTask jobTask)
        {
            Task task = new Task(jobTask.Execute, jobTask);
            task.ContinueWith(jobTask.CallBack);
            task.Start();
        }
        /// <summary>
        /// 运行一个带返回值的任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobTask"></param>
        /// <returns></returns>
        public async static Task<T> Run<T>(IJobTaskGet<T> jobTask)
        {
            Task<T> task = new Task<T>(jobTask.Execute, jobTask);
            task.Start();
            await task.ContinueWith(jobTask.CallBack);
            return task.Result;
        }
    }
}
