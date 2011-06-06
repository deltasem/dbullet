//-----------------------------------------------------------------------
// <copyright file="CsvParserTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.CsvParser
{
	/// <summary>
	/// Тесты парсера CSV
	/// </summary>
	[TestClass]
	public class CsvParserTest
	{
		/// <summary>
		/// Обычная строка CSV
		/// </summary>
		[TestMethod]
		public void EmptyStringCsv()
		{
			AssertHelpers.Throws<ArgumentNullException>(() => dbullet.core.tools.CsvParser.Parse(string.Empty));
		}

		/// <summary>
		/// Разделитель - запятая
		/// </summary>
		[TestMethod]
		public void CommaSeparator()
		{
			string[] res = dbullet.core.tools.CsvParser.Parse("value1,value2");
			Assert.AreEqual("value1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Разделитель - точка с запятой
		/// </summary>
		[TestMethod]
		public void SemicolonSeparator()
		{
			string[] res = dbullet.core.tools.CsvParser.Parse("value1;value2");
			Assert.AreEqual("value1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);
		}

		/// <summary>
		/// Запятая внутри значения
		/// </summary>
		[TestMethod]
		public void CommaInValue()
		{
			string[] res = dbullet.core.tools.CsvParser.Parse("\"val,ue1\",value2");
			Assert.AreEqual("val,ue1", res[0]);
			Assert.AreEqual("value2", res[1]);
			Assert.AreEqual(2, res.Length);			
		}
	}
}