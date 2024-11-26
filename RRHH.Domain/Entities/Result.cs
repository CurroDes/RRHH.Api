using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Domain.Entities
{
    public class Result
    {
        public string Text { get; set; }
        public string Error { get; set; }
        public object GenericObject { get; set; }
    }
}
