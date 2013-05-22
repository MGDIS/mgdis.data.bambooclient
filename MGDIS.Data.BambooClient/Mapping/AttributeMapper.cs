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
using System.Reflection;
using Bamboo.Prevalence;

namespace MGDIS.Data.BambooClient.Mapping
{
    internal class AttributeMapper : IMapper
    {
        private Dictionary<string, Tuple<FieldInfo, Dictionary<string, PropertyInfo>>> Mapping = null;

        public AttributeMapper(PrevalenceEngine Engine)
        {
            Mapping = new Dictionary<string, Tuple<FieldInfo, Dictionary<string, PropertyInfo>>>();
            foreach (FieldInfo Field in Engine.PrevalentSystem.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                object[] TableAttributes = Field.GetCustomAttributes(typeof(SQLTableAttribute), false);
                if (TableAttributes.Length == 0) continue;
                string TableName = (TableAttributes[0] as SQLTableAttribute).TableName;

                if (!Field.FieldType.IsGenericType || Field.FieldType.GetGenericTypeDefinition() != typeof(List<>))
                    throw new NotImplementedException("This version of the Bamboo ADO.NET provider only supports List<T> for SQL table associations");

                Type TargetType = Field.FieldType.GetGenericArguments()[0];
                Dictionary<string, PropertyInfo> ColumnAssociations = new Dictionary<string, PropertyInfo>();
                foreach (PropertyInfo InnerMember in TargetType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    object[] ColumnAttributes = InnerMember.GetCustomAttributes(typeof(SQLColumnAttribute), false);
                    if (ColumnAttributes.Length == 0) continue;
                    ColumnAssociations.Add((ColumnAttributes[0] as SQLColumnAttribute).ColumnName, InnerMember);
                }

                Mapping.Add(TableName, new Tuple<FieldInfo, Dictionary<string, PropertyInfo>>(Field, ColumnAssociations));
            }
        }

        public IEnumerable<string> GetFields(string Table)
        {
            foreach (var couple in Mapping[Table].Item2)
                yield return couple.Key;
        }

        public FieldInfo GetFieldInfo(string Table)
        {
            return Mapping[Table].Item1;
        }

        public PropertyInfo GetPropertyInfo(string Table, string Column)
        {
            return Mapping[Table].Item2[Column];
        }
    }
}
