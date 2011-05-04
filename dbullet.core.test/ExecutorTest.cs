using System;
using System.Collections.Generic;
using System.Reflection.Moles;
using dbullet.core.attribute;
using dbullet.core.engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dbullet.core.test
{
	/// <summary>
	/// Тесты выполнителя
	/// </summary>
	[TestClass]
	public class ExecutorTest
	{
		/// <summary>
		/// A test for GetBulletsInAssembly
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void GetBulletsInAssemblyTest()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(NotBullet),
					typeof(TestBullet1),
					typeof(TestBullet2WithouAttrs),
					typeof(NotBulletWithAttrs)
				}
			};
			var actual = Executor.GetBulletsInAssembly(assembly);
			var enumerator = actual.GetEnumerator();
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(typeof(TestBullet1), enumerator.Current);
			Assert.IsFalse(enumerator.MoveNext());
		}

		#region тестовые булеты
		/// <summary>
		/// Нормальный булет
		/// </summary>
		[BulletNumber(1)]
		public abstract class TestBullet1 : Bullet
		{
		}

		/// <summary>
		/// Булет без атрибута
		/// </summary>
		public abstract class TestBullet2WithouAttrs : Bullet
		{
		}

		/// <summary>
		/// Вообще не булет
		/// </summary>
		public class NotBullet
		{
		}

		/// <summary>
		/// Не булет, но с атрибутом
		/// </summary>
		[BulletNumber(1)]
		public class NotBulletWithAttrs
		{
		}
		#endregion
	}
}
