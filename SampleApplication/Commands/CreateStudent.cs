﻿#region License
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
using Bamboo.Prevalence;
using SampleApplication.Entities;

namespace SampleApplication.Commands
{
    /// <summary>
    /// Bamboo command to create a student entity in the persistence set
    /// </summary>
    [Serializable]
    class CreateStudent : ICommand
    {
        private Student _Instance = null;

        public CreateStudent(Student Instance)
        {
            this._Instance = Instance;
        }

        public object Execute(object system)
        {
            (system as PersistenceSet).Students.Add(_Instance);
            return _Instance;
        }

        public object Execute()
        {
            return PersistenceEngine.Engine.ExecuteCommand(this);
        }
    }
}
