mgdis.data.bambooclient
-----------------------

ADO.NET Provider for Bamboo Prevalence Engine

Object prevalence is an old concept that has recently reborn with the NoSQL approaches.
As the drawbacks of tabulated storage are more and more recognized, the NoSQL movement
proposes new way to persist data in a format that is closer to the way it is used in memory.
Sometimes, these solutions even store the data in memory, at least temporarily.

Object prevalence goes to the extreme of the concept, by establishing the in-memory
object-oriented model as the persistence model. Thus, there is no ORM and no conversion
whatsoever when using a prevalence engine. The persistence to disk, which is necessary
to spare the data of a power failure for example, is done by the engine, in a way that
is transparent to the programmer. The developer only manages commands and queries
on the model, which makes object prevalence a good choice for CQRS architectures.

One of the limits of object prevalence (and generally-speaking NoSQL solutions) is that
it forces to abandon the SQL legacy requests. Sometimes, this accounts for a fair amount
of intelligence and can slow down, or even hinder, the migration to object prevalence,
despite the huge advantages in performance as well as code-writing ease and robustness.

The goal of this project is to bridge the gap and allow for a progressive migration
by providing an ADO.NET provider for Bamboo.Prevalence, which allows to access prevalent
data by using legacy SQL, once a mapping is established between the data in memory and
the names of the fields and tables in the SQL requests.


License
-------

MGDIS.Data.BambooClient, an ADO.NET provider for Bamboo object prevalence engine
Copyright (C) 2013 MGDIS
For details see https://github.com/MGDIS/mgdis.data.bambooclient.

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <http://www.gnu.org/licenses/>.

This is free software, and you are welcome to redistribute it
under certain conditions; for details see <http://www.gnu.org/licenses/>.


Layout of the solution
----------------------

The solution provided is made of three projets :
- MGDIS.Data.BambooClient : the ADO.NET provider itself
- MGDIS.Data.BambooClient.Tests : the associated unit tests (not yet migrated)
- Sample application : a simple WPF application demonstrating use of the provider

The provider is itself composed of several parts :
- The root contains the four classes that are necessary to implement the ADO.NET interface
- The Extraction directory groups all classes used to decompose an SQL request
- The Mapping directory contains different ways to map the SQL fields and tables to the prevalent engine

The global functioning of the provider is as follows :
- A connection is created, which starts the mapping process
- A command is created with the connection attached
- A reader is executed, which transforms the SQL tree into lambda expressions
- The projection and restriction expressions are compiled and turned into a Linq query on the prevalent list
- The enumerator is exposed through the provider


Current limitations
-------------------

No support for DataSet and DataAdapter : only DataReader can be used

Only one mapper is provided, based on attributes placed on List-types fields and properties

Only one method is proposed for SQL extraction

The support classes are not injected, even if the code is ready for that, with strict interfacing

Only string-based properties are supported

The sample application does not support the association between entities

Only SELECT, FROM and WHERE in certain cases are supported


Proposed features / Work In Progress
------------------------------------

A convention-based mapper could be useful for POCO processes

An XML-based file mapper would allow for configuration without recompiling

Taking into account the full SQL grammer


External libraries
------------------

https://github.com/bamboo/Bamboo.Prevalence (MIT License)

https://irony.codeplex.com (MIT License)


Authors
-------

Jean-Philippe Gouigoux (Software architect - MGDIS - http://gouigoux.com/blog-fr/?tag=bamboo)

Damien Gaillard (Intern - MGDIS - 2012)

Amine Benhila (Intern - MGDIS - 2011)

Marie-Charlotte Ynesta (Student - 2010)


Thanks
------

Rodrigo B. de Oliveira (https://github.com/bamboo)

Klaud Wuestefeld (http://sourceforge.net/projects/prevayler/)

Carlos Eduardo Villela (http://www.ibm.com/developerworks/library/wa-objprev/)
