//-----------------------------------------------------------------------
// <copyright file="LoadCsvTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using dbullet.core.engine;
using dbullet.core.test.tools;
using NUnit.Framework;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	using dbullet.core.tools;

	/// <summary>
	/// ����� �������� CSV
	/// </summary>
	[TestFixture]
	public class LoadCsvTest
	{
		/// <summary>
		/// ������ ����
		/// </summary>
		[Test]
		public void LoadCsvEmptyStream()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE", 
					new StreamReader(new MemoryStream()),
					new Dictionary<string, Func<string, object>>()));
		}

		/// <summary>
		/// ������ ���� ������ ��������
		/// </summary>
		[Test]
		public void InvalidHeader()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE", 
					new StreamReader(new MemoryStream(Encoding.Default.GetBytes("\r\n100500,hello"))), 
					new Dictionary<string, Func<string, object>>()));
		}

		/// <summary>
		/// ����������� ������
		/// </summary>
		[Test]
		public void LoadCsvEmptyData()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID"))),
				new Dictionary<string, Func<string, object>>());
			Assert.IsTrue(string.IsNullOrEmpty(connection.LastCommandText));
		}

		/// <summary>
		/// �������� ����� ������
		/// </summary>
		[Test]
		public void LoadCsvOneRow()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID\r\n100500"))),
				new Dictionary<string, Func<string, object>>());
			Assert.AreEqual("insert into TESTTABLE (ID) values(@ID);", connection.LastCommandText);
		}

		/// <summary>
		/// ������ ���� ������ ��������
		/// </summary>
		[Test]
		public void ParametrMustBeCreatedCount()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID,PASS\r\n100500,hello"))),
				new Dictionary<string, Func<string, object>>());
			Assert.AreEqual(2, connection.DbDataParametrs.Count);
		}

		/// <summary>
		/// ������ ���� ������ �������� � �������� ������
		/// </summary>
		[Test]
		public void ParametrMustBeCreatedName()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500hello"))),
				new Dictionary<string, Func<string, object>>());
			Assert.AreEqual("@COLUMN_NAME", connection.DbDataParametrs[0].ParameterName);
		}

		/// <summary>
		/// ������ ���� ������ �������� � �������� ���������
		/// </summary>
		[Test]
		public void ParametrMustBeCreatedValue()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500hello"))),
				new Dictionary<string, Func<string, object>>());
			Assert.AreEqual("100500hello", connection.DbDataParametrs[0].Value);
		}

		/// <summary>
		/// ������ ���� ������ �������� � �������� ���������
		/// </summary>
		[Test]
		public void DataDoesntMatchHeader()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE",
					new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500,hello"))),
					new Dictionary<string, Func<string, object>>()));
		}

		/// <summary>
		/// ������ ����������� ����� ����������
		/// </summary>
		[Test]
		public void ModulatorInvoked()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500"))),
				new Dictionary<string, Func<string, object>> { { "COLUMN_NAME", p => "500100" } });
			Assert.AreEqual("500100", connection.DbDataParametrs[0].Value);
		}

		/// <summary>
		/// ������ ����������� ���� identity
		/// </summary>
		[Test]
		public void LoadCsvShouldUseIdenityInsert()
		{
			var connection = new TestConnection();
			var strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID\r\n100500"))),
				new Dictionary<string, Func<string, object>>(), 
				CsvQuotesType.DoubleQuotes, 
				true);
			Assert.AreEqual("set identity_insert [TESTTABLE] on;", connection.AllCommands[0]);
			Assert.AreEqual("insert into TESTTABLE (ID) values(@ID);", connection.AllCommands[1]);
			Assert.AreEqual("set identity_insert [TESTTABLE] off;", connection.AllCommands[2]);
		}
	}
}