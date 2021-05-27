using System;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Net.Server
{
    public interface IUser_Role_ToDo
    {
         void Implement();
    }
    class User<T> where T : UserInfo,IUser_Role_ToDo
    {
        public string ApplicationID { get; set; }
        public void Loadinginfo()
        {

        }
        public T Role;

        protected void Implement()
        {

        }
    }
}
