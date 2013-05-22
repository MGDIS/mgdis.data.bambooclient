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
using MGDIS.Data.BambooClient.Mapping;

namespace SampleApplication.Entities
{
    [Serializable]
    public class Teacher
    {
        [SQLColumn("id")]
        public string Id { get; private set; }

        [SQLColumn("fname")]
        public string FirstName { get; private set; }

        [SQLColumn("lname")]
        public string LastName { get; private set; }

        [SQLColumn("phone")]
        public string PhoneExtension { get; private set; }

        public Teacher(string Id, string FirstName, string LastName, string PhoneExtension)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PhoneExtension = PhoneExtension;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} (ext. {2})", FirstName, LastName, PhoneExtension);
        }
    }
}
