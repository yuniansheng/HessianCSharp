﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.xxl.job.core.rpc.codec
{
    public class RpcRequest
    {
        public string requestId;
        public string serverAddress;
        public long createMillisTime;
        public string accessToken;

        public string className;
        public string methodName;
        public object[] parameterTypes;
        public object[] parameters;

        public string version;
    }
}
