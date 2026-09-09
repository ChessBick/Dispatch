using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatch.Application.Dtos
{
    public class ChangeStatusRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
