using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.DTOs
{
    public class BaseQueryDTO<T> where T : class
    {
        public int TotalCount { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; } = 1;
        public List<T> Data { get; set; } = [];
    }
}