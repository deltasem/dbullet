//-----------------------------------------------------------------------
// <copyright file="GetDefaultValueTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using NUnit.Framework;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты GetDefaultValue
	/// </summary>
	[TestFixture]
	public abstract class GetDefaultValueTest : TestBase
	{
		/// <summary>
		/// Дефалт-значение
		/// </summary>
		[Test]
		public abstract void ValueDefault();

		/// <summary>
		/// Дефалт-текущее время
		/// </summary>
		[Test]
		public abstract void StandartDefaultDate();

		/// <summary>
		/// Дефалт-новый GUID
		/// </summary>
		[Test]
		public abstract void StandartDefaultGuid();
	}
}