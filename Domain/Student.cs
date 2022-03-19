using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Student : Person
    {
        public int EnrollmentYear { get; set; }
        public int EnrollmentNumber { get; set; }
    }
}
