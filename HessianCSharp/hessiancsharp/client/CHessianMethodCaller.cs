/*****************************************************************************************************
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
* Last change:
* 2005-08-14 Licence added  (Andre Voltmann)
* 2005-12-16 Session Cookie added (Dimitri Minich)
* 2007-12-08 Proxy support (Matthias Wuttke)
* 2007-12-14 GZIP compression (Matthias Wuttke)
* 2008-01-20 Keep-Alive retry (Matthias Wuttke)
*
******************************************************************************************************
*/

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;

#if COMPACT_FRAMEWORK
            // do CF stuff
#else

using System.Web;
using System.Web.SessionState;

#endif

using hessiancsharp.io;
using System.Text;
using System.IO.Compression;

namespace hessiancsharp.client
{
    /// <summary>
    /// Zusammenfassung für CHessianMethodCaller.
    /// </summary>
    public class CHessianMethodCaller
    {
        #region Constants

        public const string CUSTOM_HEADER_KEY = "__CUSTOM_HEADERS"; //$NON-NLS-1$
        public const bool USE_GZIP_COMPRESSION = true;

        #endregion Constants

        #region CLASS_FIELDS

        /// <summary>
        /// Instance of the proxy factory
        /// </summary>
        private CHessianProxyFactory m_CHessianProxyFactory;

        /// <summary>
        /// Uri for connection to the hessian service
        /// </summary>
        private Uri m_uriHessianServiceUri;

        private NetworkCredential m_credentials = null;
        private WebProxy m_proxy = null; // null = system default

        private IDictionary m_additionalCustomHeaders = null;

        /// <summary>
        /// Use default credentials. Set to true to use Kerberos.
        /// (Authenticate using Negotiate.)
        /// </summary>
        private Boolean m_defaultCredentials = false;

        #endregion CLASS_FIELDS

        #region PROPERTIES

        /// <summary>
        /// Returns the connection uri to the hessian service.
        /// </summary>
        public virtual Uri URI
        {
            get { return m_uriHessianServiceUri; }
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hessianProxyFactory">HessianProxyFactory - Instance</param>
        /// <param name="uri">Server-Proxy uri</param>
        public CHessianMethodCaller(CHessianProxyFactory hessianProxyFactory, Uri uri)
        {
            this.m_CHessianProxyFactory = hessianProxyFactory;
            this.m_uriHessianServiceUri = uri;
        }

        public CHessianMethodCaller(CHessianProxyFactory hessianProxyFactory, Uri uri, string username, string password)
        {
            this.m_CHessianProxyFactory = hessianProxyFactory;
            this.m_uriHessianServiceUri = uri;
            this.m_credentials = new System.Net.NetworkCredential(username, password);
        }

        public CHessianMethodCaller(CHessianProxyFactory hessianProxyFactory, Uri uri, string username, string password, WebProxy proxy)
        {
            this.m_CHessianProxyFactory = hessianProxyFactory;
            this.m_uriHessianServiceUri = uri;
            this.m_credentials = new System.Net.NetworkCredential(username, password);
            this.m_proxy = proxy;
        }

        public CHessianMethodCaller(CHessianProxyFactory hessianProxyFactory, Uri uri, string username, string password, WebProxy proxy, IDictionary customHeaders,
            bool defaultCredentials = false)
        {
            this.m_CHessianProxyFactory = hessianProxyFactory;
            this.m_uriHessianServiceUri = uri;
            this.m_credentials = new System.Net.NetworkCredential(username, password);
            this.m_proxy = proxy;
            this.m_additionalCustomHeaders = customHeaders;
            this.m_defaultCredentials = defaultCredentials;
        }

        #endregion CONSTRUCTORS

        #region PUBLIC_METHODS

        /// <summary>
        /// This method wrapps an instance call to the hessian
        /// requests, sends it to the hessian service and translates the reply of this call to the C# - data type
        /// </summary>
        /// <param name="methodInfo">The method to call</param>
        /// <param name="arrMethodArgs">The arguments to the method call</param>
        /// <returns>Invocation result</returns>

        public object DoHessianMethodCall(object[] arrMethodArgs, MethodInfo methodInfo)
        {
            Stream sInStream = null, sOutStream = null;
            try
            {
                int totalBytesRead;
                DateTime start = DateTime.Now;

                byte[] request = GetRequestBytes(arrMethodArgs, methodInfo);

                object result;
                try
                {
                    WebRequest webRequest = SendRequest(request, out sOutStream);
                    result = ReadReply(webRequest, methodInfo, out sInStream, out totalBytesRead);
                }
                catch (Exception e)
                {
                    /*
                    SocketException se = e.InnerException as SocketException;
                    WebException we = e as WebException;
                    if ((se != null && se.SocketErrorCode == SocketError.ConnectionAborted)
                        || (we != null && we.Status == WebExceptionStatus.KeepAliveFailure))
                     */
                    if (e is WebException)
                    {
                        WebException we = (WebException)e;
                        if (we.Status == WebExceptionStatus.ProtocolError)
                        {
                            HttpWebResponse hwr = we.Response as HttpWebResponse;
                            if (hwr != null && hwr.StatusCode == HttpStatusCode.Unauthorized)
                                // falsches Passwort
                                throw e;
                        }
                    }

                    if (!(e is CHessianException))
                    {
                        try
                        {
                            // retry once (Keep-Alive connection closed?)
                            WebRequest webRequest = SendRequest(request, out sOutStream);
                            result = ReadReply(webRequest, methodInfo, out sInStream, out totalBytesRead);
                        }
                        catch (Exception)
                        {
                            // retry again (last time)
                            WebRequest webRequest = SendRequest(request, out sOutStream);
                            result = ReadReply(webRequest, methodInfo, out sInStream, out totalBytesRead);
                        }
                    }
                    else
                        throw e; // rethrow
                }

                CHessianLog.AddLogEntry(methodInfo.Name, start, DateTime.Now, totalBytesRead, request.Length);
                return result;
            }
            catch (CHessianException he)
            {
                if (he.FaultWrapper)
                    // wrapper for a received exception
                    throw he.InnerException;
                else
                    throw he; // rethrow
            }
            finally
            {
                if (sInStream != null)
                    sInStream.Close();
                if (sOutStream != null)
                    sOutStream.Close();
            }
        }

        /// <summary>
        /// Reads and decodes the reply.
        /// </summary>
        /// <param name="webRequest"></param>
        /// <returns></returns>
        private object ReadReply(WebRequest webRequest, MethodInfo methodInfo, out Stream sInStream, out int totalBytesRead)
        {
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            if (webResponse.StatusCode != HttpStatusCode.OK)
                ReadAndThrowHttpFault(webResponse);
            sInStream = webResponse.GetResponseStream();

            if (webResponse.ContentEncoding.ToLower().Contains("gzip")) //$NON-NLS-1$
                sInStream = new GZipStream(sInStream, CompressionMode.Decompress);
            else if (webResponse.ContentEncoding.ToLower().Contains("deflate")) //$NON-NLS-1$
                sInStream = new DeflateStream(sInStream, CompressionMode.Decompress);

#if COMPACT_FRAMEWORK
            AbstractHessianInput hessianInput = this.GetHessianInput(sInStream);
#else
            BufferedStream bStream = new BufferedStream(sInStream, 4096);
            AbstractHessianInput hessianInput = this.GetHessianInput(bStream);
#endif
            CHessianLog.ResetBreadcrumb();
            object result = hessianInput.ReadReply(methodInfo.ReturnType);
            totalBytesRead = ((CHessianInput)hessianInput).GetTotalBytesRead();
            return result;
        }

        /// <summary>
        /// Sends the web request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private WebRequest SendRequest(byte[] request, out Stream sOutStream)
        {
            WebRequest webRequest = PrepareWebRequest(request.Length);
            sOutStream = webRequest.GetRequestStream();
            sOutStream.Write(request, 0, request.Length);
            sOutStream.Flush();
            return webRequest;
        }

        /// <summary>
        /// Translates the method call to a request byte array.
        /// </summary>
        /// <param name="arrMethodArgs"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        private byte[] GetRequestBytes(object[] arrMethodArgs, MethodInfo methodInfo)
        {
            Type[] argumentTypes = GetArgTypes(arrMethodArgs);
            MemoryStream memoryStream = new MemoryStream(4096);
            CHessianOutput cHessianOutput = this.GetHessianOutput(memoryStream);
            string strMethodName = methodInfo.Name;
            if (m_CHessianProxyFactory.IsOverloadEnabled)
            {
                if (arrMethodArgs != null)
                    strMethodName = strMethodName + "__" + arrMethodArgs.Length; //$NON-NLS-1$
                else
                    strMethodName = strMethodName + "__0"; //$NON-NLS-1$
            }
            cHessianOutput.Call(strMethodName, arrMethodArgs);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Reads a HTTP fault and throws a CHessianException.
        /// </summary>
        /// <param name="webResponse"></param>
        private void ReadAndThrowHttpFault(WebResponse webResponse)
        {
            StringBuilder sb = new StringBuilder();
            int chTemp;

            Stream sInStream = webResponse.GetResponseStream();
            if (sInStream != null)
            {
                while ((chTemp = sInStream.ReadByte()) >= 0)
                    sb.Append((char)chTemp);
                sInStream.Close();
            }

            throw new CHessianException(sb.ToString());
        }

        /// <summary>
        /// Prepares a WebRequest object for communication
        /// with the Hessian server.
        /// </summary>
        /// <returns></returns>
        private WebRequest PrepareWebRequest(long contentLength)
        {
            WebRequest webRequest = OpenConnection(m_uriHessianServiceUri);

#if COMPACT_FRAMEWORK
#else
            //webRequest.Headers
            HttpWebRequest req = webRequest as HttpWebRequest;

            System.Net.ServicePointManager.Expect100Continue = false;

            // mw: gzip compression
            if (USE_GZIP_COMPRESSION)
                req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate"); //$NON-NLS-1$

            // mw: custom headers
            if (m_additionalCustomHeaders != null)
                AddAdditionalCustomHeadersToRequest(webRequest);

            // mw: cookies
            if (COOKIE_CONTAINER == null)
                COOKIE_CONTAINER = new CookieContainer();
            req.CookieContainer = COOKIE_CONTAINER;

            //Preserve cookies to allow for session affinity between remote server and client
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["SessionCookie"] == null) //$NON-NLS-1$
                    HttpContext.Current.Session.Add("SessionCookie", new CookieContainer()); //$NON-NLS-1$
                req.CookieContainer = (CookieContainer)HttpContext.Current.Session["SessionCookie"]; //$NON-NLS-1$
                AddCustomHeadersToRequest(webRequest, HttpContext.Current.Session);
            }
#endif

            webRequest.Proxy = m_proxy;
            webRequest.ContentType = "application/x-hessian"; //$NON-NLS-1$
            if (contentLength > -1)
                webRequest.ContentLength = contentLength;
            webRequest.Method = "POST"; //$NON-NLS-1$

            return webRequest;
        }

        private static CookieContainer COOKIE_CONTAINER;

        /// <summary>
        /// Returns array with types of the instance from
        /// the argument array
        /// </summary>
        /// <param name="arrArgs">Any array</param>
        /// <returns>Array with types of the instance from
        /// the argument array</returns>
        public static Type[] GetArgTypes(object[] arrArgs)
        {
            if (null == arrArgs)
            {
                return new Type[0];
            }

            Type[] result = new Type[arrArgs.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                if (arrArgs[i] == null)
                {
                    result[i] = null;
                }
                else
                {
                    result[i] = arrArgs[i].GetType();
                }
            }

            return result;
        }

        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS

        /// <summary>
        /// Creates the URI connection.
        /// </summary>
        /// <param name="uri">Uri for connection</param>
        /// <returns>Request instance</returns>
        private WebRequest OpenConnection(Uri uri)
        {
            WebRequest request = WebRequest.Create(uri);
            if (this.m_defaultCredentials)
            {
                request.Credentials = CredentialCache.DefaultCredentials;
                request.PreAuthenticate = true;
            }
            else if (this.m_credentials != null)
            {
                // mw: PreAuthenticate tut nicht, ist aber bei Kerberos wichtig
                //request.Credentials = this.m_credentials;
                //request.PreAuthenticate = true;
                SetBasicAuthHeader(request);
            }
            return request;
        }

        private void SetBasicAuthHeader(WebRequest request)
        {
            string authInfo = m_credentials.UserName + ":" + m_credentials.Password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo)); // ISO
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authInfo);
        }

        /// <summary>
        /// Instantiation of the hessian input (not cached)
        /// </summary>
        /// <param name="stream">Stream for HessianInput-Instantiation</param>
        /// <returns>New HessianInput - Instance</returns>
        private AbstractHessianInput GetHessianInput(Stream stream)
        {
            return new CHessianInput(stream);
        }

        /// <summary>
        /// Instantiation of the hessian output (not cached)
        /// </summary>
        /// <param name="stream">Strean for HessianOutput - Instantiation</param>
        /// <returns>New HessianOutput - Instance</returns>
        private CHessianOutput GetHessianOutput(Stream stream)
        {
            CHessianOutput cHessianOut = new CHessianOutput(stream);
            return cHessianOut;
        }

#if COMPACT_FRAMEWORK
            // do CF stuff
#else

        private void AddCustomHeadersToRequest(WebRequest request, HttpSessionState session)
        {
            if (session[CUSTOM_HEADER_KEY] != null)
            {
                IDictionary headers = session[CUSTOM_HEADER_KEY] as IDictionary;
                foreach (DictionaryEntry entry in headers)
                {
                    request.Headers.Add("X-" + entry.Key, entry.Value.ToString()); //$NON-NLS-1$
                }
            }
        }

        // mw
        private void AddAdditionalCustomHeadersToRequest(WebRequest request)
        {
            foreach (DictionaryEntry entry in m_additionalCustomHeaders)
                request.Headers.Add(entry.Key.ToString(), entry.Value.ToString()); //$NON-NLS-1$
        }

#endif

        #endregion PRIVATE_METHODS
    }
}