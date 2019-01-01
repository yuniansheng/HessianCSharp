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

namespace hessiancsharp.io
{
    /// <summary>
    /// This class conatains hessian protocol constants.
    /// </summary>
    public class CHessianProtocolConstants
    {
        #region CONSTANTS

        public const char PROT_CALL_START = 'c';
        public const char PROT_METHOD = 'm';
        public const char PROT_REPLY_START = 'r';
        public const char PROT_HEADER = 'H';
        public const string MESSAGE_WRONG_REPLY_START = "expected hessian reply"; //$NON-NLS-1$
        public const char PROT_REPLY_FAULT = 'f';
        public const string MESSAGE_WRONG_REPLY_END = "expected end of reply"; //$NON-NLS-1$
        public const char PROT_REPLY_END = 'z';
        public const char PROT_NULL = 'N';
        public const char PROT_BOOLEAN_TRUE = 'T';
        public const char PROT_BOOLEAN_FALSE = 'F';
        public const char PROT_INTEGER_TYPE = 'I';
        public const char PROT_STRING_FINAL = 'S';
        public const char PROT_STRING_INITIAL = 's';
        public const char PROT_XML_FINAL = 'X';
        public const char PROT_XML_INITIAL = 'x';
        public const char PROT_LONG_TYPE = 'L';
        public const char PROT_DOUBLE_TYPE = 'D';
        public const char PROT_MAP_TYPE = 'M';
        public const int END_OF_DATA = -2;
        public const char PROT_DATE_TYPE = 'd';
        public const char PROT_REF_TYPE = 'R';
        public const char PROT_BINARY_START = 'b';
        public const char PROT_BINARY_END = 'B';
        public const char PROT_LIST_TYPE = 'V';
        public const char PROT_TYPE = 't';
        public const char PROT_LENGTH = 'l';

        #endregion CONSTANTS

        #region Hessian2Constants

        public const int BC_BINARY = 'B'; // const chunk
        public const int BC_BINARY_CHUNK = 'A'; // non-const chunk
        public const int BC_BINARY_DIRECT = 0x20; // 1-byte length binary
        public const int BINARY_DIRECT_MAX = 0x0f;
        public const int BC_BINARY_SHORT = 0x34; // 2-byte length binary
        public const int BINARY_SHORT_MAX = 0x3ff; // 0-1023 binary

        public const int BC_CLASS_DEF = 'C'; // object/class definition

        public const int BC_DATE = 0x4a; // 64-bit millisecond UTC date
        public const int BC_DATE_MINUTE = 0x4b; // 32-bit minute UTC date

        public const int BC_DOUBLE = 'D'; // IEEE 64-bit double

        public const int BC_DOUBLE_ZERO = 0x5b;
        public const int BC_DOUBLE_ONE = 0x5c;
        public const int BC_DOUBLE_BYTE = 0x5d;
        public const int BC_DOUBLE_SHORT = 0x5e;
        public const int BC_DOUBLE_MILL = 0x5f;

        public const int BC_FALSE = 'F'; // boolean false

        public const int BC_INT = 'I'; // 32-bit int

        public const int INT_DIRECT_MIN = -0x10;
        public const int INT_DIRECT_MAX = 0x2f;
        public const int BC_INT_ZERO = 0x90;

        public const int INT_BYTE_MIN = -0x800;
        public const int INT_BYTE_MAX = 0x7ff;
        public const int BC_INT_BYTE_ZERO = 0xc8;

        public const int BC_END = 'Z';

        public const int INT_SHORT_MIN = -0x40000;
        public const int INT_SHORT_MAX = 0x3ffff;
        public const int BC_INT_SHORT_ZERO = 0xd4;

        public const int BC_LIST_VARIABLE = 0x55;
        public const int BC_LIST_FIXED = 'V';
        public const int BC_LIST_VARIABLE_UNTYPED = 0x57;
        public const int BC_LIST_FIXED_UNTYPED = 0x58;

        public const int BC_LIST_DIRECT = 0x70;
        public const int BC_LIST_DIRECT_UNTYPED = 0x78;
        public const int LIST_DIRECT_MAX = 0x7;

        public const int BC_LONG = 'L'; // 64-bit signed integer
        public const long LONG_DIRECT_MIN = -0x08;
        public const long LONG_DIRECT_MAX = 0x0f;
        public const int BC_LONG_ZERO = 0xe0;

        public const long LONG_BYTE_MIN = -0x800;
        public const long LONG_BYTE_MAX = 0x7ff;
        public const int BC_LONG_BYTE_ZERO = 0xf8;

        public const int LONG_SHORT_MIN = -0x40000;
        public const int LONG_SHORT_MAX = 0x3ffff;
        public const int BC_LONG_SHORT_ZERO = 0x3c;

        public const int BC_LONG_INT = 0x59;

        public const int BC_MAP = 'M';
        public const int BC_MAP_UNTYPED = 'H';

        public const int BC_NULL = 'N';

        public const int BC_OBJECT = 'O';
        public const int BC_OBJECT_DEF = 'C';

        public const int BC_OBJECT_DIRECT = 0x60;
        public const int OBJECT_DIRECT_MAX = 0x0f;

        public const int BC_REF = 0x51;

        public const int BC_STRING = 'S'; // const string
        public const int BC_STRING_CHUNK = 'R'; // non-const string

        public const int BC_STRING_DIRECT = 0x00;
        public const int STRING_DIRECT_MAX = 0x1f;
        public const int BC_STRING_SHORT = 0x30;
        public const int STRING_SHORT_MAX = 0x3ff;

        public const int BC_TRUE = 'T';

        public const int P_PACKET_CHUNK = 0x4f;
        public const int P_PACKET = 'P';

        public const int P_PACKET_DIRECT = 0x80;
        public const int PACKET_DIRECT_MAX = 0x7f;

        public const int P_PACKET_SHORT = 0x70;
        public const int PACKET_SHORT_MAX = 0xfff;

        #endregion
    }
}