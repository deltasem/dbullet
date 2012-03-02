//-----------------------------------------------------------------------
// <copyright file="OracleDropColumnTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbs;
using dbullet.core.engine.Oracle;
using dbullet.core.test.AllStrategy;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Тесты удаления столбцов
	/// </summary>
	[TestFixture]
	public class OracleDropColumnTest : DropColumnTest
	{
		/// <summary>
		/// Удаление столбца
		/// </summary>
		protected override string RegularDropColumnCommand
		{
			get { return "alter table \"TESTTABLE\" drop column \"TESTCOLUMN\""; }
		}

		/// <summary>
		/// Инициализация
		/// </summary>
		[SetUp]
		public void SelfTestInitialize()
		{
			ObjectFactory.Initialize(x => x.For<IDatabaseStrategy>().Use<OracleStrategy>());
			ObjectFactory.Inject(connection.Object);
		}
	}
}