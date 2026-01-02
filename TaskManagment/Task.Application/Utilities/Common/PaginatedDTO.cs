using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment.Application.Utilities.Common
{
    public class PaginatedDTO<T>
    {
        public List<T> Elements { get; set; } = [];
        public int TotalAMountOfRecords { get; set; }
    }
}
