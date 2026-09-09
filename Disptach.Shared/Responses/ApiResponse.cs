using System;
using System.Collections.Generic;
using System.Text;

namespace Disptach.Shared.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public string? Message { get; set; }
    }
}
