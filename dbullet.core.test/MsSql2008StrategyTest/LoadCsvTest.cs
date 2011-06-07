//-----------------------------------------------------------------------
// <copyright file="LoadCsvTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.IO;
using System.IO.Moles;
using System.Text;
using dbullet.core.engine;
using dbullet.core.test.tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// ����� �������� CSV
	/// </summary>
	[TestClass]
	public class LoadCsvTest
	{
		/// <summary>
		/// ������ ����
		/// </summary>
		[TestMethod]
		public void LoadCsvEmptyStream()
		{
			TestConnection connection = new TestConnection();
			MsSql2008Strategy strategy = new MsSql2008Strategy(connection);
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE", 
					new StreamReader(new MemoryStream()),
					new Dictionary<string, System.Func<string, string>>()));
		}

		/// <summary>
		/// ������ ���� ������ ��������
		/// </summary>
		[TestMethod]
		public void InvalidHeader()
		{
			TestConnection connection = new TestConnection();
			MsSql2008Strategy strategy = new MsSql2008Strategy(connection);
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE", 
					new StreamReader(new MemoryStream(Encoding.Default.GetBytes("\r\n100500,hello"))), 
					new Dictionary<string, System.Func<string, string>>()));
		}

		/// <summary>
		/// ����������� ������
		/// </summary>
		[TestMethod]
		public void LoadCsvEmptyData()
		{
			TestConnection connection = new TestConnection();
			MsSql2008Strategy strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID"))),
				new Dictionary<string, System.Func<string, string>>());
			Assert.IsTrue(string.IsNullOrEmpty(connection.LastCommandText));
		}

		/// <summary>
		/// �������� ����� ������
		/// </summary>
		[TestMethod]
		public void LoadCsvOneRow()
		{
			TestConnection connection = new TestConnection();
			MsSql2008Strategy strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID\r\n100500"))),
				new Dictionary<string, System.Func<string, string>>());
			Assert.AreEqual("insert into TESTTABLE (ID) values(@ID);", connection.LastCommandText);
		}

		/// <summary>
		/// ������ ���� ������ ��������
		/// </summary>
		[TestMethod]
		public void ParametrMustBeCreatedCount()
		{
			TestConnection connection = new TestConnection();
			MsSql2008Strategy strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("ID,PASS\r\n100500,hello"))),
				new Dictionary<string, System.Func<string, string>>());
			Assert.AreEqual(2, connection.DbDataParametrs.Count);
		}

		/// <summary>
		/// ������ ���� ������ �������� � �������� ������
		/// </summary>
		[TestMethod]
		public void ParametrMustBeCreatedName()
		{
			TestConnection connection = new TestConnection();
			MsSql2008Strategy strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500hello"))),
				new Dictionary<string, System.Func<string, string>>());
			Assert.AreEqual("@COLUMN_NAME", connection.DbDataParametrs[0].ParameterName);
		}

		/// <summary>
		/// ������ ���� ������ �������� � �������� ���������
		/// </summary>
		[TestMethod]
		public void ParametrMustBeCreatedValue()
		{
			TestConnection connection = new TestConnection();
			MsSql2008Strategy strategy = new MsSql2008Strategy(connection);
			strategy.LoadCsv(
				"TESTTABLE",
				new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500hello"))),
				new Dictionary<string, System.Func<string, string>>());
			Assert.AreEqual("100500hello", connection.DbDataParametrs[0].Value);
		}

		/// <summary>
		/// ������ ���� ������ �������� � �������� ���������
		/// </summary>
		[TestMethod]
		public void DataDoesntMatchHeader()
		{
			TestConnection connection = new TestConnection();
			MsSql2008Strategy strategy = new MsSql2008Strategy(connection);
			AssertHelpers.Throws<InvalidDataException>(
				() => strategy.LoadCsv(
					"TESTTABLE",
					new StreamReader(new MemoryStream(Encoding.Default.GetBytes("COLUMN_NAME\r\n100500,hello"))),
					new Dictionary<string, System.Func<string, string>>()));
		}
	}
}