using System;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools
{
    public delegate void NetAction();  
    public interface IAction
    {
        void Do(byte[] bs);
    }
    public class ActionHand
    {
        
    }
}
