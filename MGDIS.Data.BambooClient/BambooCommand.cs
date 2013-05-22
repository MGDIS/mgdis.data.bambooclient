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
using System.Data;

namespace MGDIS.Data.BambooClient
{
    public class BambooCommand : IDbCommand
    {
        private BambooConnection _Connection = null;

        private string _CommandText = null;

        public string CommandText
        {
            get { return _CommandText; }
            set { _CommandText = value; }
        }

        public CommandType CommandType
        {
            get { return CommandType.Text; }
            set { throw new NotImplementedException(); }
        }

        public BambooCommand(string CommandText, BambooConnection Connection)
        {
            _CommandText = CommandText;
            _Connection = Connection;
        }

        public IDataReader ExecuteReader()
        {
            return new BambooDataReader(_CommandText, _Connection);
        }

        public object ExecuteScalar()
        {
            using (IDataReader Reader = ExecuteReader())
            {
                if (Reader.Read())
                    return Reader[0];
            }
            return null;
        }

        public IDbConnection Connection
        {
            get
            {
                return _Connection;
            }
            set
            {
                BambooConnection Connection = value as BambooConnection;
                if (Connection == null)
                    throw new ArgumentException("Value could not be cast to a BambooConnection");
                _Connection = Connection;
            }
        }

        #region Not implemented IDbCommand methods

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public int CommandTimeout
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbDataParameter CreateParameter()
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public IDataParameterCollection Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction Transaction
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
