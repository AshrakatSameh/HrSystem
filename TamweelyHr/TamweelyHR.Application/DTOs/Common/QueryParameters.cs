using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamweelyHR.Application.DTOs.Common
{
    public class QueryParameters
    {
        private int _pageSize = 10;
        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        //Search term
        public string? SearchTerm { get; set; }
        //prop to sort by
        public string? SortBy { get; set; }
        //Sort direction
        public bool SortDescending { get; set; }
    }
}
