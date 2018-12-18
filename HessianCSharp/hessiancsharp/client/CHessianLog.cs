using System;
using System.Collections.Generic;
using System.Text;

namespace hessiancsharp.client
{
    public class CHessianLog
    {
        private const int MAX_ENTRIES = 200;
        private static bool LOGGING_ENABLED = false;
        private static bool BREADCRUMB_ENABLED = false;
        private static List<String> BREADCRUMBS = null;

        public static bool LoggingEnabled
        {
            get { return LOGGING_ENABLED; }
            set { LOGGING_ENABLED = value; }
        }

        public static bool BreadcrumbEnabled
        {
            get { return BREADCRUMB_ENABLED; }
            set { BREADCRUMB_ENABLED = value; }
        }

        private static List<CHessianLogEntry> LOG_ENTRIES = new List<CHessianLogEntry>();

        public static void AddLogEntry(string methodName, DateTime start, DateTime finish, long bytesIn, long bytesOut)
        {
            if (LoggingEnabled)
            {
                if (LOG_ENTRIES.Count >= MAX_ENTRIES)
                    LOG_ENTRIES.RemoveAt(0);
                LOG_ENTRIES.Add(new CHessianLogEntry(methodName, start, finish, bytesIn, bytesOut));
            }
        }

        public static List<CHessianLogEntry> GetLog()
        {
            return LOG_ENTRIES;
        }

        public static void AddBreadcrumb(Object bc)
        {
            if (BreadcrumbEnabled)
            {
                if (BREADCRUMBS == null)
                    BREADCRUMBS = new List<String>();
                String bcs = bc != null ? bc.ToString() : "<NULL>"; //$NON-NLS-1$
                BREADCRUMBS.Add(bcs);
            }
        }

        public static void ResetBreadcrumb()
        {
            if (BreadcrumbEnabled && BREADCRUMBS != null)
                BREADCRUMBS.Clear();
        }

        public static String CurrentBreadcrumbs
        {
            get
            {
                if (BreadcrumbEnabled && BREADCRUMBS != null)
                {
                    StringBuilder path = new StringBuilder();
                    foreach (String bc in BREADCRUMBS)
                    {
                        if (path.Length > 0)
                            path.Append(" > "); //$NON-NLS-1$
                        path.Append(bc);
                    }
                    return path.ToString();
                }
                else
                    return null;
            }
        }

        public static void RemoveBreadcrumb()
        {
            if (BreadcrumbEnabled)
            {
                if (BREADCRUMBS != null && BREADCRUMBS.Count > 0)
                    BREADCRUMBS.RemoveAt(BREADCRUMBS.Count - 1);
            }
        }
    }

    public class CHessianLogEntry
    {
        public CHessianLogEntry(string name, DateTime start, DateTime stop, long bytesIn, long bytesOut)
        {
            MethodName = name;
            ExecutionStart = start;
            ExecutionFinish = stop;
            BytesIn = bytesIn;
            BytesOut = bytesOut;
        }

        private string methodName;

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

        private DateTime executionStart;

        public DateTime ExecutionStart
        {
            get { return executionStart; }
            set { executionStart = value; }
        }

        private DateTime executionFinish;

        public DateTime ExecutionFinish
        {
            get { return executionFinish; }
            set { executionFinish = value; }
        }

        public int ExecutionDuration
        {
            get
            {
                TimeSpan duration = ExecutionFinish - ExecutionStart;
                return (int)duration.TotalMilliseconds;
            }
        }

        private long bytesIn;

        public long BytesIn
        {
            get { return bytesIn; }
            set { bytesIn = value; }
        }

        private long bytesOut;

        public long BytesOut
        {
            get { return bytesOut; }
            set { bytesOut = value; }
        }
    }
}