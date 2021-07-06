using System;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Tools
{
    class DisposeBase
    {
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposeing)
        {
            if (disposed)
                return;
            if (disposeing)
            {

            }
            disposed = true;
        }
    }
}
