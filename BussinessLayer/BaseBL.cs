using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class BaseBL<TDAL> where TDAL : class, new()
    {
        public TDAL Methods;
        public BaseBL()
        {

            Methods = Activator.CreateInstance<TDAL>();
        }
    }
}
