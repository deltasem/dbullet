//-----------------------------------------------------------------------
// <copyright file="ExecutorTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Reflection.Moles;
using dbullet.core.attribute;
using dbullet.core.engine;
using dbullet.core.engine.Moles;
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
		/// Инициализация тестов
		/// </summary>
		[TestInitialize]
		public void TestInitialize()
		{
			TestBullet1.IsUpdateInvoked = false;
			TestBullet2.IsUpdateInvoked = false;
			TestBullet3.IsUpdateInvoked = false;
		}

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

		/// <summary>
		/// Булеты должны быть упорядоченными
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void GetBulletsInAssemblyOrdered()
		{
			var assembly = new SAssembly 
			{ 
				GetTypes01 = () => new[]
				{
					typeof(TestBullet2WithouAttrs),
					typeof(NotBulletWithAttrs),
					typeof(TestBullet3),
					typeof(TestBullet1),
					typeof(TestBullet2),
					typeof(NotBullet),
				}
			};
			var actual = Executor.GetBulletsInAssembly(assembly);
			var enumerator = actual.GetEnumerator();
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(typeof(TestBullet1), enumerator.Current);
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(typeof(TestBullet2), enumerator.Current);
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(typeof(TestBullet3), enumerator.Current);
			Assert.IsFalse(enumerator.MoveNext());
		}

		/// <summary>
		/// A test for Execute
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void ExecuteSimple()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(TestBullet1)
				}
			};
			MMsSql2008SysStrategy.AllInstances.GetLastVersion = p => 0;
			MMsSql2008SysStrategy.AllInstances.SetCurrentVersionInt32 = (i, j) => { };
			Executor.Execute(assembly, string.Empty, SupportedStrategy.Mssql2008);
			Assert.IsTrue(TestBullet1.IsUpdateInvoked);
		}

		/// <summary>
		/// Тесты должны вызываться в последовательности
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void ExecuteBulletSequence()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(TestBullet1),
					typeof(TestBullet2),
					typeof(TestBullet3),
				}
			};
			int[] currentVersion = { 1 };
			MMsSql2008SysStrategy.AllInstances.GetLastVersion = p => currentVersion[0];
			MMsSql2008SysStrategy.AllInstances.SetCurrentVersionInt32 = (i, j) => currentVersion[0] = j;
			Executor.Execute(assembly, string.Empty, SupportedStrategy.Mssql2008);
			Assert.IsFalse(TestBullet1.IsUpdateInvoked);
			Assert.IsTrue(TestBullet2.IsUpdateInvoked);
			Assert.IsTrue(TestBullet3.IsUpdateInvoked);
		}

		/// <summary>
		/// Версия должна записываться в базу
		/// </summary>
		[TestMethod]
		[HostType("Moles")]
		public void ExecteVersionWriting()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(TestBullet1),
				}
			};
			int[] currentVersion = { 0 };
			MMsSql2008SysStrategy.AllInstances.GetLastVersion = p => currentVersion[0];
			MMsSql2008SysStrategy.AllInstances.SetCurrentVersionInt32 = (i, j) => currentVersion[0] = j;
			Executor.Execute(assembly, string.Empty, SupportedStrategy.Mssql2008);
			Assert.AreEqual(1, currentVersion[0]);
		}

		#region тестовые булеты
		/// <summary>
		/// Нормальный булет
		/// </summary>
		[BulletNumber(1)]
		public class TestBullet1 : Bullet
		{
			/// <summary>
			/// Был ли вызов
			/// </summary>
			public static bool IsUpdateInvoked { get; set; }

			/// <summary>
			/// Обновление
			/// </summary>
			public override void Update()
			{
				IsUpdateInvoked = true;
			}

			/// <summary>
			/// Отмена обновления
			/// </summary>
			public override void Downgrade()
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Нормальный булет
		/// </summary>
		[BulletNumber(2)]
		public class TestBullet2 : Bullet
		{
			/// <summary>
			/// Был ли вызов
			/// </summary>
			public static bool IsUpdateInvoked { get; set; }

			/// <summary>
			/// Обновление
			/// </summary>
			public override void Update()
			{
				IsUpdateInvoked = true;
			}

			/// <summary>
			/// Отмена обновления
			/// </summary>
			public override void Downgrade()
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Нормальный булет
		/// </summary>
		[BulletNumber(3)]
		public class TestBullet3 : Bullet
		{
			/// <summary>
			/// Был ли вызов
			/// </summary>
			public static bool IsUpdateInvoked { get; set; }

			/// <summary>
			/// Обновление
			/// </summary>
			public override void Update()
			{
				IsUpdateInvoked = true;
			}

			/// <summary>
			/// Отмена обновления
			/// </summary>
			public override void Downgrade()
			{
				throw new NotImplementedException();
			}
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
