using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fursion_CSharpTools.Core
{
    public abstract class BaseComponent : IAsyncDisposable
    {
        public bool Disposable = false;
        public abstract ValueTask DisposeAsync();
    }
}
