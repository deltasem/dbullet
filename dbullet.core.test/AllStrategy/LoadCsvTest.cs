//-----------------------------------------------------------------------
// <copyright file="LoadCsvTest.cs" company="SofTrust" author="SKiryshin">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using dbullet.core.dbs;
using dbullet.core.tools;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты загрузки CSV
	/// </summary>
	[TestFixture]
	public abstract class LoadCsvTest : TestBase
	{
		/// <summary>
		/// Пустой файл
		/// </summary>
		[Test]
		public void LoadCsvEmptyStream()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE", 
					new StreamReader(new MemoryStream()),
					new Dictionary<string, Func<string, object>>()));
		}

		/// <summary>
		/// Должен быть создан параметр
		/// </summary>
		[Test]
		public void InvalidHeader()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE", 
					new StreamReader(new MemoryStream(Encoding.Default.GetBytes("\r\n100500,hello"))), 
					new Dictionary<string, Func<string, object>>()));
		}

		/// <summary>
		/// Отсутствуют данные
		/// </summary>
		[Test]
		public void LoadCsvEmptyData()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();

			var dbParams = new Mock<IDbDataParameter>();
			command.Setup(x => x.CreateParameter()).Returns(dbParams.Object);
			command.Setup(x => x.Parameters.Add(It.IsAny<object>())).Verifiable();

			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID"))),
				new Dictionary<string, Func<string, object>>());
			command.Verify(x => x.ExecuteNonQuery(), Times.Never());
		}

		/// <summary>
		/// Загрузка одной записи
		/// </summary>
		[Test]
		public void LoadCsvOneRow()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();

			var dbParams = new Mock<IDbDataParameter>();
			command.Setup(x => x.CreateParameter()).Returns(dbParams.Object);
			command.Setup(x => x.Parameters.Add(It.IsAny<object>())).Verifiable();

			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID\r\n100500"))),
				new Dictionary<string, Func<string, object>>());
			command.VerifySet(x => x.CommandText = "insert into TESTTABLE (ID) values(@ID);");
		}

		/// <summary>
		/// Должен быть создан параметр
		/// </summary>
		[Test]
		public void ParametrMustBeCreatedCount()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			var dbParams = new Mock<IDbDataParameter>();
			command.Setup(x => x.CreateParameter()).Returns(dbParams.Object);
			command.Setup(x => x.Parameters.Add(It.IsAny<object>())).Verifiable();
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID,PASS\r\n100500,hello"))),
				new Dictionary<string, Func<string, object>>());
			command.Verify(x => x.Parameters.Add(It.IsAny<object>()), Times.Exactly(2));
		}

		/// <summary>
		/// Должен быть создан параметр с указаным именем
		/// </summary>
		[Test]
		public void ParametrMustBeCreatedName()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			var dbParams = new Mock<IDbDataParameter>();
			command.Setup(x => x.CreateParameter()).Returns(dbParams.Object);
			command.Setup(x => x.Parameters.Add(It.IsAny<object>()));
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500hello"))),
				new Dictionary<string, Func<string, object>>());
			dbParams.VerifySet(x => x.ParameterName = "@COLUMN_NAME");
		}

		/// <summary>
		/// Должен быть создан параметр с указаным значением
		/// </summary>
		[Test]
		public void ParametrMustBeCreatedValue()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			var dbParams = new Mock<IDbDataParameter>();
			command.Setup(x => x.CreateParameter()).Returns(dbParams.Object);
			command.Setup(x => x.Parameters.Add(It.IsAny<object>())); 
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500hello"))),
				new Dictionary<string, Func<string, object>>());
			dbParams.VerifySet(x => x.Value = "100500hello");
		}

		/// <summary>
		/// Должен быть создан параметр с указаным значением
		/// </summary>
		[Test]
		public void DataDoesntMatchHeader()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			var dbParams = new Mock<IDbDataParameter>();
			command.Setup(x => x.CreateParameter()).Returns(dbParams.Object);
			command.Setup(x => x.Parameters.Add(It.IsAny<object>())); 
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE",
					new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500,hello"))),
					new Dictionary<string, Func<string, object>>()));
		}

		/// <summary>
		/// Должен происходить вызов модулятора
		/// </summary>
		[Test]
		public void ModulatorInvoked()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			var dbParams = new Mock<IDbDataParameter>();
			command.Setup(x => x.CreateParameter()).Returns(dbParams.Object);
			command.Setup(x => x.Parameters.Add(It.IsAny<object>())); 
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500"))),
				new Dictionary<string, Func<string, object>> { { "COLUMN_NAME", p => "500100" } });
			dbParams.VerifySet(x => x.Value = "500100");
		}

		/// <summary>
		/// Должен учитываться флаг identity
		/// </summary>
		[Test]
		public void LoadCsvShouldUseIdenityInsert()
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
			command.VerifySet(x => x.CommandText = "set identity_insert [TESTTABLE] on;");
			command.VerifySet(x => x.CommandText = "insert into TESTTABLE (ID) values(@ID);");
			command.VerifySet(x => x.CommandText = "set identity_insert [TESTTABLE] off;");
		}
	}
}