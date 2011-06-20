//-----------------------------------------------------------------------
// <copyright file="GetDefaultValueTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using dbullet.core.engine.MsSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test.MsSql2008StrategyTest
{
	/// <summary>
	/// Тесты получения значения дефалта
	/// </summary>
	[TestClass]
	public class GetDefaultValueTest
	{
		/// <summary>
		/// Дефалт-значение
		/// </summary>
		[TestMethod]
		public void ValueDefault()
		{
			var result = MsSqlTemplateManager.GetDefaultValue(new ValueDefault(string.Empty, "Value default"));
			Assert.AreEqual("Value default", result);
		}

		/// <summary>
		/// Дефалт-текущее время
		/// </summary>
		[TestMethod]
		public void StandartDefaultDate()
		{
			var result = MsSqlTemplateManager.GetDefaultValue(new StandartDefault(string.Empty, StandartDefaultType.date));
			Assert.AreEqual("getdate()", result);
		}
	
		/// <summary>
		/// Дефалт-новый GUID
		/// </summary>
		[TestMethod]
		public void StandartDefaultGuid()
		{
			var result = MsSqlTemplateManager.GetDefaultValue(new StandartDefault(string.Empty, StandartDefaultType.guid));
			Assert.AreEqual("newid()", result);
		}
	}
}