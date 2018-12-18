/*
*****************************************************************************************************
* HessianCharp - The .Net implementation of the Hessian Binary Web Service Protocol (www.caucho.com)
* Copyright (C) 2004-2005  by D. Minich, V. Byelyenkiy, A. Voltmann
* http://www.hessiancsharp.org
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*
* You can find the GNU Lesser General Public here
* http://www.gnu.org/licenses/lgpl.html
* or in the license.txt file in your source directory.
******************************************************************************************************
* You can find all contact information on http://www.hessiancsharp.com
******************************************************************************************************
*
*
******************************************************************************************************
* Last change: 2005-12-16
* By Dimitri Minich
* Exception handling
******************************************************************************************************
*/

#region NAMESPACES

using hessiancsharp.io;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

#endregion NAMESPACES

namespace hessiancsharp.server
{
    /// <summary>
    /// Proxy class for Hessian services.
    /// </summary>
    public class CHessianSkeleton
    {
        #region CLASS_FIELDS

        //Api Classe
        private Type m_typApi;

        private Object m_Service;
        private IDictionary m_dictMethod = new Hashtable();

        #endregion CLASS_FIELDS

        /// <summary>
        /// Create a new hessian skeleton.
        ///
        ///param service the underlying service object.
        ///param apiClass the API interface
        /// </summary>
        public CHessianSkeleton(Type api, Object service)
        {
            m_typApi = api;
            m_Service = service;

            foreach (MethodInfo metInfo in m_typApi.GetMethods())
            {
                ParameterInfo[] parInfo = metInfo.GetParameters();

                if (!m_dictMethod.Contains(metInfo.Name))
                {
                    m_dictMethod.Add(metInfo.Name, metInfo);
                }

                String mangledName = metInfo.Name + "__" + parInfo.Length; //$NON-NLS-1$
                if (!m_dictMethod.Contains(mangledName))
                {
                    m_dictMethod.Add(mangledName, metInfo);
                }

                String mangledName2 = MangleName(metInfo, false);
                if (!m_dictMethod.Contains(mangledName2))
                {
                    m_dictMethod.Add(mangledName2, metInfo);
                }
            }
        }

        /// <summary>
        /// Invoke the object with the request from the input stream.
        /// </summary>
        /// <param name="inHessian">the Hessian input stream</param>
        /// <param name="outHessian">the Hessian output stream</param>
        public void invoke(AbstractHessianInput inHessian, AbstractHessianOutput outHessian)
        {
            inHessian.StartCall();
            MethodInfo methodInf = getMethodInfo(inHessian.Method);

            //If the method doesn't exist
            if (methodInf == null)
            {
                outHessian.StartReply();
                outHessian.WriteFault("NoSuchMethodException",  //$NON-NLS-1$
                    "The service has no method named: " + inHessian.Method, //$NON-NLS-1$
                    null);
                outHessian.CompleteReply();
                return;
            }

            ParameterInfo[] paramInfo = methodInf.GetParameters();
            Object[] valuesParam = new Object[paramInfo.Length];

            for (int i = 0; i < paramInfo.Length; i++)
            {
                valuesParam[i] = inHessian.ReadObject(paramInfo[i].ParameterType);
            }
            inHessian.CompleteCall();

            Object result = null;

            try
            {
                result = methodInf.Invoke(m_Service, valuesParam);
            }
            catch (Exception e)
            {
                //TODO: Exception besser behandeln

                if (e.GetType() == typeof(System.Reflection.TargetInvocationException))
                {
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                }
                outHessian.StartReply();
                outHessian.WriteFault("ServiceException", e.Message, e); //$NON-NLS-1$

                outHessian.CompleteReply();
                return;
            }
            outHessian.StartReply();

            outHessian.WriteObject(result);

            outHessian.CompleteReply();
        }

        /// <summary>
        /// Returns the method by the mangled name.
        /// </summary>
        /// <param name="mangledName">the name passed by the protocol</param>
        /// <returns>MethodInfo of the method</returns>
        protected MethodInfo getMethodInfo(String mangledName)
        {
            return (MethodInfo)m_dictMethod[mangledName];
        }

        /// <summary>
        /// Creates a unique mangled method name based on the method name and
        /// the method parameters.
        /// </summary>
        /// <param name="methodInfo">he method to mangle</param>
        /// <param name="isFull">if true, mangle the full classname</param>
        /// <returns>return a mangled string.</returns>
        private String MangleName(MethodInfo methodInfo, bool isFull)
        {
            StringBuilder sbTemp = new StringBuilder();

            sbTemp.Append(methodInfo.Name);
            ParameterInfo[] paramsInf = methodInfo.GetParameters();
            foreach (ParameterInfo p in paramsInf)
            {
                sbTemp.Append('_');
                MangleClass(sbTemp, p.ParameterType, isFull);
            }

            return sbTemp.ToString();
        }

        /// <summary>
        /// Mangles a classname.
        /// </summary>
        /// <param name="sb">StringBuilder for writing in</param>
        /// <param name="paramType">Type of methodparameter</param>
        ///  <param name="isFull">if true, mangle the full classname</param>
        private void MangleClass(StringBuilder sb, Type paramType, bool isFull)
        {
            String nameTemp = paramType.ToString();

            if (nameTemp.Equals("bool") || nameTemp.Equals("System.Boolean")) //$NON-NLS-1$ $NON-NLS-2$
            {
                sb.Append("boolean"); //$NON-NLS-1$
            }
            else if (nameTemp.Equals("int") || nameTemp.Equals("System.Int32") ||				 //$NON-NLS-1$ $NON-NLS-2$
                nameTemp.Equals("short") || nameTemp.Equals("System.Int16") || //$NON-NLS-1$ $NON-NLS-2$
                nameTemp.Equals("byte") || nameTemp.Equals("System.Byte"))  //$NON-NLS-1$ $NON-NLS-2$
            {
                sb.Append("int"); //$NON-NLS-1$
            }
            else if (nameTemp.Equals("long") || nameTemp.Equals("System.Int64")) //$NON-NLS-1$ $NON-NLS-2$
            {
                sb.Append("long"); //$NON-NLS-1$
            }
            else if (nameTemp.Equals("float") || nameTemp.Equals("System.Single") || //$NON-NLS-1$ $NON-NLS-2$
                nameTemp.Equals("double") || nameTemp.Equals("System.Double"))  //$NON-NLS-1$ $NON-NLS-2$
            {
                sb.Append("double"); //$NON-NLS-1$
            }
            else if (nameTemp.Equals("System.String") ||				 //$NON-NLS-1$
                nameTemp.Equals("char") || nameTemp.Equals("System.Char"))				 //$NON-NLS-1$ $NON-NLS-2$
            {
                sb.Append("string"); //$NON-NLS-1$
            }
            else if (nameTemp.Equals("System.DateTime"))				 //$NON-NLS-1$
            {
                sb.Append("date"); //$NON-NLS-1$
            }
            else if (paramType.IsAssignableFrom(typeof(Stream)) || nameTemp.Equals("[B"))				 //$NON-NLS-1$
            {
                sb.Append("binary");				 //$NON-NLS-1$
            }
            else if (paramType.IsArray)
            {
                sb.Append("["); //$NON-NLS-1$
                MangleClass(sb, paramType.GetElementType(), isFull); ;
            }
            else if (isFull)
            {
                sb.Append(nameTemp);
            }
            else
            {
                int p = nameTemp.LastIndexOf('.');
                if (p > 0)
                    sb.Append(nameTemp.Substring(p + 1));
                else
                    sb.Append(nameTemp);
            }
            //TODO:XML
        }
    }
}