//-----------------------------------------------------------------------
// <copyright file="CsvParserTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace dbullet.core.test.CsvParser
{
	/// <summary>
	/// Тесты парсера CSV
	/// </summary>
	[TestFixture]
	public class CsvParserTest
	{
		/// <summary>
		/// Обычная строка CSV
		/// </summary>
		[Test]
		public void EmptyStringCsv()
		{
			AssertHelpers.Throws<ArgumentNullException>(() => core.tools.CsvParser.Parse(string.Empty));
		}

		/// <summary>
		/// Разделитель - запятая
		/// </summary>
		[Test]
		public void CommaSeparator()
		{
			string[] res = core.tools.CsvParser.Parse("value1,value2");
			Assert.AreEqual("value1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Разделитель - точка с запятой
		/// </summary>
		[Test]
		public void SemicolonSeparator()
		{
			string[] res = core.tools.CsvParser.Parse("value1;value2");
			Assert.AreEqual("value1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Запятая внутри значения
		/// </summary>
		[Test]
		public void CommaInValue()
		{
			string[] res = core.tools.CsvParser.Parse("\"val,ue1\",value2");
			Assert.AreEqual("val,ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);			
		}

		/// <summary>
		/// Точка с запятой внутри значения
		/// </summary>
		[Test]
		public void SemicolonInValue()
		{
			string[] res = core.tools.CsvParser.Parse("\"val;ue1\",value2");
			Assert.AreEqual("val;ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Запятая внутри значения
		/// </summary>
		[Test]
		public void CommaInValueSingleQuote()
		{
			string[] res = core.tools.CsvParser.Parse("'val,ue1',value2", core.tools.CsvQuotesType.SingleQuotes);
			Assert.AreEqual("val,ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Точка с запятой внутри значения
		/// </summary>
		[Test]
		public void SemicolonInValueSingleQuote()
		{
			string[] res = core.tools.CsvParser.Parse("'val;ue1',value2", core.tools.CsvQuotesType.SingleQuotes);
			Assert.AreEqual("val;ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Двойные ковычки внутри
		/// </summary>
		[Test]
		public void QuoteInQuotes()
		{
			string[] res = core.tools.CsvParser.Parse("\"val\"\"ue1\",value2");
			Assert.AreEqual("val\"ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// ковычки внутри
		/// </summary>
		[Test]
		public void SingleQuoteInSingleQuotes()
		{
			string[] res = core.tools.CsvParser.Parse("'val''ue1',value2", core.tools.CsvQuotesType.SingleQuotes);
			Assert.AreEqual("val'ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Одинарные ковычки внутри двойных
		/// </summary>
		[Test]
		public void SingleQuoteInQuotes()
		{
			string[] res = core.tools.CsvParser.Parse("\"val''ue1\",value2");
			Assert.AreEqual("val''ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Двойные ковычки внутри одинарных
		/// </summary>
		[Test]
		public void QuoteInSingleQuotes()
		{
			string[] res = core.tools.CsvParser.Parse("'val\"\"ue1',value2", core.tools.CsvQuotesType.SingleQuotes);
			Assert.AreEqual("val\"\"ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Пустое поле
		/// </summary>
		[Test]
		public void EmptyField()
		{
			string[] res = core.tools.CsvParser.Parse("hello;;world");
			Assert.AreEqual(3, res.Length);
			Assert.AreEqual(string.Empty, res[1]);
		}
	}
}