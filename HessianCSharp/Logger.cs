using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace hessiancsharp
{
    public static class Logger
    {
        public static ILoggerFactory LoggerFactory { get; set; }

        static Logger()
        {
            LoggerFactory = new NullLoggerFactory();
        }

        public static void SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            LoggerFactory = loggerFactory;
        }

        public static ILogger GetLogger(string name)
        {
            return LoggerFactory.CreateLogger(name);
        }

        public static ILogger GetLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }
    }
}
