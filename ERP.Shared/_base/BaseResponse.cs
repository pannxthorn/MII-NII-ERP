using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Shared._base.BaseResponse
{
    public class BaseResponse<T>
    {
        public string TrackingCode { get; set; }
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        public BaseResponse()
        {
            TrackingCode = Guid.NewGuid().ToString();
        }
    }
}
