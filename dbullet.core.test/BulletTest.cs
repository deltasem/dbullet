//-----------------------------------------------------------------------
// <copyright file="BulletTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using dbullet.core.dbo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test
{
	/// <summary>
	/// Тесты Bullet
	/// </summary>
	[TestClass()]
	public class BulletTest
	{
		/// <summary>
		/// Контекст
		/// </summary>
		public TestContext TestContext { get; set; }

		/// <summary>
		/// A test for CreateTable
		/// </summary>
		[TestMethod()]
		[DeploymentItem("dbullet.core.dll")]
		public void CreateTableTest()
		{
			Bullet_Accessor target = new Bullet_Accessor(); 
			Table table = null; 
			target.CreateTable(table);
		}
	}
}