using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Models
{
    public class RegisterCardActivityData
    {
        public CreateAccountRequest CreateAccountRequest { get; set; }
        public dynamic OtherData { get; set; }
    }
}
