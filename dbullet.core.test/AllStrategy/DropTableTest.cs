//-----------------------------------------------------------------------
// <copyright file="DropTableTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using dbullet.core.dbs;
using dbullet.core.exception;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// ����� DropTable
	/// </summary>
	[TestFixture]
	public abstract class DropTableTest : TestBase
	{
		/// <summary>
		/// �������� �������
		/// </summary>
		protected abstract string RegularDropTableCommand { get; }

		/// <summary>
		/// �������� ������� ��� ��������
		/// </summary>
		[Test]
		public void EmptyTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			Assert.Throws<TableExpectedException>(() => strategy.DropTable(string.Empty));
		}

		/// <summary>
		/// �������� �������
		/// </summary>
		[Test]
		public void RegularDropTable()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			strategy.DropTable("TableForDrop");
			command.VerifySet(x => x.CommandText = RegularDropTableCommand);
		}
	}
}