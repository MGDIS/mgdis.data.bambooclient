#region License
// MGDIS.Data.BambooClient, an ADO.NET provider for Bamboo object prevalence engine
// Copyright (C) 2013 MGDIS
// For details see https://github.com/MGDIS/mgdis.data.bambooclient.

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

// This is free software, and you are welcome to redistribute it
// under certain conditions; for details see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MGDIS.Data.BambooClient.Extraction;

namespace MGDIS.Data.BambooClient
{
    public class BambooDataReader : IDataReader
    {
        private List<string> FieldsList { get; set; }

        private IEnumerator<object> Enumerator = null;

        public BambooDataReader(string CommandText, BambooConnection Connection)
        {
            IExpressionGenerator gen = new IronyExpressionGenerator(CommandText, Connection.Mapping);
            FieldsList = gen.SelectClause;

            IEnumerable<object> LinqRequest = Connection.Mapping.GetFieldInfo(gen.FromTable).GetValue(Connection.Engine.PrevalentSystem) as IEnumerable<object>;
            LinqRequest = LinqRequest.Select(gen.SelectFunction);
            if (gen.WhereFunction != null) LinqRequest = LinqRequest.Where(gen.WhereFunction);
            Enumerator = LinqRequest.GetEnumerator();
        }

        public bool Read()
        {
            return Enumerator.MoveNext();
        }

        public int FieldCount
        {
            get { return FieldsList.Count; }
        }

        public string GetName(int i)
        {
            return FieldsList[i];
        }

        public string GetString(int i)
        {
            object[] currentDataLine = Enumerator.Current as object[];
            return Convert.ToString(currentDataLine[i]);
        }

        private bool _Closed = false;

        public void Close()
        {
            _Closed = true;
        }

        public bool IsClosed
        {
            get { return _Closed; }
        }

        public void Dispose()
        {
            Close();
        }

        #region Not implemented IDataReader methods

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
