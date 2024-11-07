using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10451547CLVD7112Exam.Common
{
    public class AppSettings
    {
        public ConnectionStrings? ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string? HealthCheckDbConnection { get; set; }
    }
}

