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
using Bamboo.Prevalence;
using MGDIS.Data.BambooClient.Mapping;

namespace MGDIS.Data.BambooClient
{
    public class BambooConnection : IDbConnection
    {
        internal PrevalenceEngine Engine = null;

        private ConnectionState _ConnectionState = ConnectionState.Closed;

        internal IMapper Mapping = null;

        public BambooConnection(PrevalenceEngine Engine)
        {
            this.Engine = Engine;
        }

        public void Open()
        {
            if (Mapping == null) Mapping = new AttributeMapper(Engine);
            _ConnectionState = ConnectionState.Open;
        }

        public void Close()
        {
            _ConnectionState = ConnectionState.Closed;
        }

        public ConnectionState State
        {
            get { return _ConnectionState; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            return new BambooCommand(null, this);
        }

        #region Not implemented IDbConnection methods

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public string ConnectionString
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

        public int ConnectionTimeout
        {
            get { throw new NotImplementedException(); }
        }

        public string Database
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
