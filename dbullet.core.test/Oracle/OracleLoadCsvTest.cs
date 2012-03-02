//-----------------------------------------------------------------------
// <copyright file="OracleLoadCsvTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using dbullet.core.dbs;
using dbullet.core.engine.Oracle;
using dbullet.core.test.AllStrategy;
using dbullet.core.tools;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.Oracle
{
	/// <summary>
	/// Тесты загрузки CSV
	/// </summary>
	[TestFixture]
	public class OracleLoadCsvTest : LoadCsvTest
	{
		/// <summary>
		/// Комманда загрузки одной записи
		/// </summary>
		protected override string LoadCsvOneRowCommand
		{
			get { return "insert into \"TESTTABLE\" (\"ID\") values(:1)"; }
		}

		/// <summary>
		/// Имя параметра
		/// </summary>
		protected override string ParametrMustBeCreatedNameParametrName
		{
			get { return ":1"; }
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

		/// <summary>
		/// Должен учитываться флаг identity
		/// </summary>
		[Test]
		public override void LoadCsvShouldUseIdenityInsert()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			var dbParams = new Mock<IDbDataParameter>();
			command.Setup(x => x.CreateParameter()).Returns(dbParams.Object);
			command.Setup(x => x.Parameters.Add(It.IsAny<object>())); 
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID\r\n100500"))),
				new Dictionary<string, Func<string, object>>(), 
				CsvQuotesType.DoubleQuotes, 
				true);
			command.VerifySet(x => x.CommandText = "insert into \"TESTTABLE\" (\"ID\") values(:1)");
			command.Verify(x => x.ExecuteNonQuery(), Times.Once());
		}
	}
}