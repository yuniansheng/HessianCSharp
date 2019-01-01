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
******************************************************************************************************
*/

#region NAMESPACES

using System;

#endregion NAMESPACES

namespace hessiancsharp.io
{
    /// <summary>
    /// Deserializing an object.
    /// </summary>
    public abstract class AbstractDeserializer : CSerializationConstants
    {
        #region PUBLIC_METHODS

        public virtual bool IsReadResolve()
        {
            return false;
        }

        /// <summary>
        /// Reads object
        /// </summary>
        /// <param name="abstractHessianInput">HessianInput-Instance</param>
        /// <returns>Object that was read</returns>
        /// <exception cref="CHessianException"/>
        public virtual object ReadObject(AbstractHessianInput abstractHessianInput)
        {
            throw new CHessianException(this.GetType().ToString());
        }

        public virtual object ReadObject(AbstractHessianInput abstractHessianInput, string[] fieldNames)
        {
            return ReadObject(abstractHessianInput, (object[])fieldNames);
        }

        public virtual object ReadObject(AbstractHessianInput abstractHessianInput, object[] fields)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates an empty array for the deserializers field entries.
        /// </summary>
        /// <param name="len">number of fields to be read</param>
        /// <returns>empty array of the proper field type.</returns>
        public virtual object[] CreateFields(int len)
        {
            return new string[len];
        }

        /// <summary>
        /// Returns the deserializer's field reader for the given name.
        /// </summary>
        /// <param name="name">the field name</param>
        /// <returns>the deserializer's internal field reader</returns>
        public virtual object CreateField(String name)
        {
            return name;
        }

        /// <summary>
        /// Returns the type of the reader
        /// </summary>
        /// <returns>type of the reader</returns>
        public virtual Type GetOwnType()
        {
            return typeof(object);
        }

        /// <summary>
        /// Reads the map
        /// </summary>
        /// <param name="abstractHessianInput">HessianInput-Instance to read from</param>
        /// <returns>Read map</returns>
        /// <exception cref="CHessianException"/>
        public virtual object ReadMap(AbstractHessianInput abstractHessianInput)
        {
            throw new CHessianException(this.GetType().ToString());
        }

        /// <summary>
        /// Reads the list
        /// </summary>
        /// <param name="abstractHessianInput">HessianInput-Instance to read from</param>
        /// <returns>Read list</returns>
        /// <exception cref="CHessianException"/>
        public virtual object ReadList(AbstractHessianInput abstractHessianInput, int intLength)
        {
            throw new CHessianException(this.GetType().ToString());
        }

        /// <summary>
        /// Reads the length list
        /// </summary>
        public virtual object ReadLengthList(AbstractHessianInput abstractHessianInput, int intLength)
        {
            throw new CHessianException(this.GetType().ToString());
        }

        #endregion PUBLIC_METHODS
    }
}