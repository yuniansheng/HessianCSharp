/*
*****************************************************************************************************
* HessianCharp - The .Net implementation of the Hessian Binary Web Service Protocol (www.caucho.com)
* Copyright (C) 2004-2005  by D. Minich, V. Byelyenkiy, A. Voltmann
* http://www.hessiancsharp.com
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
* Last change: 2005-08-14
* By Andre Voltmann
* Licence added.
* 2006-02-23 Support for deserializing to Generic list types
******************************************************************************************************
*/

#region NAMESPACES

using hessiancsharp.client;
using System;
using System.Collections;
using System.Reflection;

#endregion NAMESPACES

namespace hessiancsharp.io
{
    /// <summary>
    /// Deserializing of the Lists
    /// </summary>
    public class CCollectionDeserializer : AbstractDeserializer
    {
        #region CLASS_FIELDS

        /// <summary>
        /// Type of the list instances
        /// </summary>
        private System.Type m_type = null;

        #endregion CLASS_FIELDS

        #region PROPERTIES

        /// <summary>
        /// Returns type of the list instances
        /// </summary>
        public System.Type Type
        {
            get
            {
                return m_type;
            }
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type of the list instances</param>
        public CCollectionDeserializer(System.Type type)
        {
            m_type = type;
        }

        #endregion CONSTRUCTORS

        #region PUBLIC_METHODS

        /// <summary>
        /// Reads list.
        /// </summary>
        /// <param name="abstractHessianInput">HessianInput - Instance</param>
        /// <param name="intListLength">Length of the list</param>
        /// <returns>Return value is always an ArrayList - Instance,
        /// apart from StringCollection - Instances</returns>
        public override System.Object ReadList(AbstractHessianInput abstractHessianInput, int intListLength)
        {
            if (m_type != null && IsGenericList(m_type))
                return ReadGenericList(abstractHessianInput);
            else
                if (m_type != null && IsGenericHashSet(m_type))
                    return ReadGenericHashSet(abstractHessianInput);
                else
                    return ReadUntypedList(abstractHessianInput);
        }

        public static bool IsGenericList(Type type)
        {
            if (!type.IsGenericType)
                return false;
            Type listType = typeof(System.Collections.Generic.List<>);
            Type genTD = type.GetGenericTypeDefinition();
            return (listType.IsAssignableFrom(genTD));
        }

        public static bool IsGenericHashSet(Type type)
        {
            if (!type.IsGenericType)
                return false;
            Type listType = typeof(System.Collections.Generic.HashSet<>);
            Type genTD = type.GetGenericTypeDefinition();
            return (listType.IsAssignableFrom(genTD));
        }

        private Object ReadGenericList(AbstractHessianInput abstractHessianInput)
        {
            Type[] args = m_type.GetGenericArguments();
            Type itemType = args[0];
            Type listType = typeof(System.Collections.Generic.List<>).MakeGenericType(itemType);

            return readCollection(abstractHessianInput, listType, itemType);
        }

        private Object ReadGenericHashSet(AbstractHessianInput abstractHessianInput)
        {
            Type[] args = m_type.GetGenericArguments();
            Type itemType = args[0];
            Type setType = typeof(System.Collections.Generic.HashSet<>).MakeGenericType(itemType);

            return readCollection(abstractHessianInput, setType, itemType);
        }

        private Object readCollection(AbstractHessianInput abstractHessianInput, Type collectionType, Type itemType)
        {
            object list = Activator.CreateInstance(collectionType);
            abstractHessianInput.AddRef(list);

            int i = 0;
            while (!abstractHessianInput.IsEnd())
            {
                CHessianLog.AddBreadcrumb(i++);
                object item = abstractHessianInput.ReadObject(itemType);
                CHessianLog.RemoveBreadcrumb();
                collectionType.InvokeMember("Add", BindingFlags.InvokeMethod, null, list, new object[] { item }); //$NON-NLS-1$
            }
            abstractHessianInput.ReadEnd();
            return list;
        }

        private Object ReadUntypedList(AbstractHessianInput abstractHessianInput)
        {
            IList listResult = new ArrayList();
            abstractHessianInput.AddRef(listResult);
            int i = 0;
            while (!abstractHessianInput.IsEnd())
            {
                CHessianLog.AddBreadcrumb(i++);
                listResult.Add(abstractHessianInput.ReadObject());
                CHessianLog.RemoveBreadcrumb();
            }
            abstractHessianInput.ReadEnd();
            return listResult;
        }

        /// <summary>
        /// Reads objects as list
        /// <see cref="ReadList(AbstractHessianInput,int)"/>
        /// </summary>
        /// <param name="abstractHessianInput">HessianInput - Instance</param>
        /// <returns>List instance</returns>
        public override object ReadObject(AbstractHessianInput abstractHessianInput)
        {
            int intCode = abstractHessianInput.ReadListStart();
            switch (intCode)
            {
                case CHessianInput.PROT_NULL:
                    return null;

                case CHessianInput.PROT_REF_TYPE:
                    return abstractHessianInput.ReadRef();
            }
            String strType = abstractHessianInput.ReadType();
            int intLength = abstractHessianInput.ReadLength();
            return ReadList(abstractHessianInput, intLength);
        }

        #endregion PUBLIC_METHODS
    }
}