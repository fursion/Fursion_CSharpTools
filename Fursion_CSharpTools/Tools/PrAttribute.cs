using System;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Tools
{
    public class TestAttribute : Attribute
    {
        private string developer;
        private string lastReview;
        private string message;
        public TestAttribute(string dev, string lastv)
        {
            this.developer = dev;
            this.lastReview = lastv;
        }
        public string Developer { get { return developer; } }
        public string LastReview { get { return lastReview; } }
        public string Message { get { return message; } set { message = value; } }
    }

}
