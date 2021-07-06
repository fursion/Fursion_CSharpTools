using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fursion_CSharpTools.Net.Public
{
    public delegate T JobCallback<T>();
    interface IFSJob
    {
        void JobCallback(Task task, object obj);
        void Execute(object obj);
    }
    struct Job1 : IFSJob
    {
        public void Execute(object obj)
        {
      
        }

        public void JobCallback(Task task,object obj)
        {
            throw new NotImplementedException();
        }
    }

    class FJobCore
    {
        public void RunTask(IFSJob job)
        {
            Task task = new Task(job.Execute, job);
            task.ContinueWith(job.JobCallback, task.AsyncState);
        }
        

    }
}
