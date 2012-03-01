//-----------------------------------------------------------------------
// <copyright file="GetDefaultValueTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using NUnit.Framework;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// ����� GetDefaultValue
	/// </summary>
	[TestFixture]
	public abstract class GetDefaultValueTest : TestBase
	{
		/// <summary>
		/// ������-��������
		/// </summary>
		[Test]
		public abstract void ValueDefault();

		/// <summary>
		/// ������-������� �����
		/// </summary>
		[Test]
		public abstract void StandartDefaultDate();

		/// <summary>
		/// ������-����� GUID
		/// </summary>
		[Test]
		public abstract void StandartDefaultGuid();
	}
}