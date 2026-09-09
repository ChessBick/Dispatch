using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatch.Application.Dtos
{
    public class ListWorkOrdersRequest
    {
        public string? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
