using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.User.ResponseModel
{
    public class ResponseModel<T>
    {
        public bool success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }
}
