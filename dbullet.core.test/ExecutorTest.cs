//-----------------------------------------------------------------------
// <copyright file="ExecutorTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Moles;
using dbullet.core.attribute;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.dbs.Moles;
using dbullet.core.engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

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
			ErrorBullet.IsUpdateInvoked = false;
			TestBullet1.IsDowngradeInvoked = false;
			TestBullet2.IsDowngradeInvoked = false;
			TestBullet3.IsDowngradeInvoked = false;
			ErrorBullet.IsDowngradeInvoked = false;
		}

		/// <summary>
		/// A test for GetBulletsInAssembly
		/// </summary>
		[TestMethod]
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
					typeof(NotBullet)
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
		/// Булеты должны быть упорядоченными
		/// </summary>
		[TestMethod]
		public void GetBulletsInAssemblyOrderedRevert()
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
					typeof(NotBullet)
				}
			};
			var actual = Executor.GetBulletsInAssembly(assembly, true);
			var enumerator = actual.GetEnumerator();
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(typeof(TestBullet3), enumerator.Current);
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(typeof(TestBullet2), enumerator.Current);
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(typeof(TestBullet1), enumerator.Current);
			Assert.IsFalse(enumerator.MoveNext());
		}

		/// <summary>
		/// A test for Execute
		/// </summary>
		[TestMethod]
		public void ExecuteSimple()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(TestBullet1)
				}
			};
			var strategy = new SISysDatabaseStrategy
			{
				GetLastVersionAssembly = x => 0
			};
			ObjectFactory.Initialize(x =>
			{
				x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy);
				x.ForSingletonOf<IDatabaseStrategy>().Use(new SIDatabaseStrategy());
			});
			Executor.Execute(assembly);
			Assert.IsTrue(TestBullet1.IsUpdateInvoked);
		}

		/// <summary>
		/// Тесты должны вызываться в последовательности
		/// </summary>
		[TestMethod]
		public void ExecuteBulletSequence()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(TestBullet1),
					typeof(TestBullet2),
					typeof(TestBullet3)
				}
			};
			int[] currentVersion = { 1 };
			var strategy = new SISysDatabaseStrategy
			{
				GetLastVersionAssembly = x => currentVersion[0],
				SetCurrentVersionAssemblyInt32 = (x, y) => currentVersion[0] = y,
			};
			ObjectFactory.Initialize(x => x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy));
			Executor.Execute(assembly);
			Assert.IsFalse(TestBullet1.IsUpdateInvoked);
			Assert.IsTrue(TestBullet2.IsUpdateInvoked);
			Assert.IsTrue(TestBullet3.IsUpdateInvoked);
		}

		/// <summary>
		/// Тесты должны вызываться в последовательности
		/// </summary>
		[TestMethod]
		public void ExecuteBackBulletSequence()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(TestBullet1),
					typeof(TestBullet2),
					typeof(TestBullet3)
				}
			};
			int[] currentVersion = { 3 };

			var strategy = new SISysDatabaseStrategy
			{
				GetLastVersionAssembly = x => currentVersion[0],
				SetCurrentVersionAssemblyInt32 = (x, y) => currentVersion[0] = y,
				RemoveVersionInfoInt32 = (j) => currentVersion[0] = j - 1
			};
			ObjectFactory.Initialize(x => x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy));

			Executor.ExecuteBack(assembly, 1);
			Assert.IsFalse(TestBullet1.IsDowngradeInvoked);
			Assert.IsTrue(TestBullet2.IsDowngradeInvoked);
			Assert.IsTrue(TestBullet3.IsDowngradeInvoked);
		}

		/// <summary>
		/// Выполнение до определенной версии
		/// </summary>
		[TestMethod]
		public void ExecuteBulletToVersion()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(TestBullet1),
					typeof(TestBullet2),
					typeof(TestBullet3)
				}
			};
			int[] currentVersion = { 0 };
			var strategy = new SISysDatabaseStrategy
			{
				GetLastVersionAssembly = x => currentVersion[0],
				SetCurrentVersionAssemblyInt32 = (x, y) => currentVersion[0] = y,
			};
			ObjectFactory.Initialize(x => x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy));
			Executor.Execute(assembly, 2);
			Assert.AreEqual(2, currentVersion[0]);
			Assert.IsTrue(TestBullet1.IsUpdateInvoked);
			Assert.IsTrue(TestBullet2.IsUpdateInvoked);
			Assert.IsFalse(TestBullet3.IsUpdateInvoked);
		}

		/// <summary>
		/// Версия должна записываться в базу
		/// </summary>
		[TestMethod]
		public void ExecteVersionWriting()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(TestBullet1),
					typeof(TestBullet1),
				}
			};
			int[] currentVersion = { 0 };
			var strategy = new SISysDatabaseStrategy
			{
				GetLastVersionAssembly = x => currentVersion[0],
				SetCurrentVersionAssemblyInt32 = (x, y) => currentVersion[0] = y
			};
			ObjectFactory.Initialize(x => x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy));
			Executor.Execute(assembly);
			Assert.AreEqual(1, currentVersion[0]);
		}

		/// <summary>
		/// При не прохождении любого скрипта из булета, должен быть выполнен откат
		/// </summary>
		[TestMethod]
		public void ExecuteWithErrors()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(ErrorBullet),
				}
			};
			int[] currentVersion = { 1 };
			var strategy = new SISysDatabaseStrategy
			{
				GetLastVersionAssembly = x => currentVersion[0],
				SetCurrentVersionAssemblyInt32 = (x, y) => currentVersion[0] = y,
			};
			ObjectFactory.Initialize(x =>
			{
				x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy);
				x.ForSingletonOf<IDatabaseStrategy>().Use(new SIDatabaseStrategy());
			});
			Executor.Execute(assembly);
			Assert.AreEqual(1, currentVersion[0]);
			Assert.IsTrue(ErrorBullet.IsDowngradeInvoked);
		}

		/// <summary>
		/// При возникновении ошибки, не должно проходит обновление дальше
		/// </summary>
		[TestMethod]
		public void ExecuteWithErrorsNotContinue()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[]
				{
					typeof(ErrorBullet),
					typeof(TestBullet3)
				}
			};
			int[] currentVersion = { 1 };
			var strategy = new SISysDatabaseStrategy
			{
				GetLastVersionAssembly = x => currentVersion[0],
				SetCurrentVersionAssemblyInt32 = (x, y) => currentVersion[0] = y,
			};
			ObjectFactory.Initialize(x =>
			{
				x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy);
				x.ForSingletonOf<IDatabaseStrategy>().Use(new SIDatabaseStrategy());
			});
			Executor.Execute(assembly);
			Assert.AreEqual(1, currentVersion[0]);
			Assert.IsFalse(TestBullet3.IsUpdateInvoked);
		}

		/// <summary>
		/// При возникновении ошибки в ходе downgrade должны проходить остальные шаги
		/// </summary>
		[TestMethod]
		public void ExecuteWithErrorsDowngradeWithErrors()
		{
			var assembly = new SAssembly
			{
				GetTypes01 = () => new[] 
				{ 
					typeof(ErrorStepBullet) 
				}
			};
			int[] currentVersion = { 0 };
			var strategy = new SISysDatabaseStrategy 
			{
				GetLastVersionAssembly = x => currentVersion[0],
				SetCurrentVersionAssemblyInt32 = (x, y) => currentVersion[0] = y
			};
			var results = new bool[11];
			var mssqlStrategy = new SIDatabaseStrategy 
			{
				DropTableString = x => { results[0] = true; throw new Exception(); },
				DropColumnStringString = (x, y) => { results[1] = true; throw new Exception(); },
				DropForeignKeyForeignKey = x => { results[2] = true; throw new Exception(); },
				DropIndexIndex = x => { results[3] = true; throw new Exception(); },
				LoadCsvStringStreamReaderDictionaryOfStringFuncOfStringObjectCsvQuotesType = (x, y, z, t) => { results[4] = true; throw new Exception(); },
				AddColumnTableColumn = (x, y) => { results[5] = true; throw new Exception(); },
				CreateForeignKeyForeignKey = x => { results[6] = true; throw new Exception(); },
				CreateIndexIndex = x => { results[7] = true; throw new Exception(); },
				CreateTableTable = x => { results[8] = true; throw new Exception(); },
				InsertRowsStringObjectArray = (x, y) => { results[9] = true; throw new Exception(); },
				IsTableExistString = x => { results[10] = true; throw new Exception(); }
			};
			ObjectFactory.Initialize(x =>
			{
				x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy);
				x.ForSingletonOf<IDatabaseStrategy>().Use(mssqlStrategy);
			});

			Executor.Execute(assembly);
			var result = results.All(x => x);
			Assert.IsTrue(result);
		}

		#region тестовые булеты
		/// <summary>
		/// Ошибки в момент довнгрейда
		/// </summary>
		[BulletNumber(1)]
		public class ErrorStepBullet : Bullet
		{
			/// <summary>
			/// Обновление
			/// </summary>
			public override void Update()
			{
				// todo: поменять на исключение, которое генерится при ошибке запроса
				throw new Exception();
			}

			/// <summary>
			/// Отмена обновления
			/// </summary>
			public override void Downgrade()
			{
				DropTable("test");
				DropColumn("test", "test");
				DropForeignKey(new ForeignKey("test", "test", "test", "test"));
				DropIndex(new Index("test", "test"));
				LoadCsv("test", new StreamReader(new MemoryStream()), null);
				AddColumn("test", "test", DbType.Int32);
				CreateForeignKey(new ForeignKey("test", "test", "test", "test"));
				CreateIndex(new Index("test", "test"));
				CreateTable(new Table("test"));
				InsertRows("test");
				IsTableExist("test");
			}
		}

		/// <summary>
		/// Нормальный булет
		/// </summary>
		[BulletNumber(2)]
		public class ErrorBullet : Bullet
		{
			/// <summary>
			/// Был ли вызов
			/// </summary>
			public static bool IsUpdateInvoked { get; set; }

			/// <summary>
			/// Был ли проведен откат
			/// </summary>
			public static bool IsDowngradeInvoked { get; set; }

			/// <summary>
			/// Обновление
			/// </summary>
			public override void Update()
			{
				// todo: поменять на исключение, которое генерится при ошибке запроса
				throw new Exception("fake exception");
			}

			/// <summary>
			/// Отмена обновления
			/// </summary>
			public override void Downgrade()
			{
				IsDowngradeInvoked = true;
			}
		}

		/// <summary>
		/// Нормальный булет
		/// </summary>
		[BulletNumber(1)]
		public class TestBullet1 : Bullet
		{
			/// <summary>
			/// Был ли проведен откат
			/// </summary>
			public static bool IsDowngradeInvoked { get; set; }

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
				IsDowngradeInvoked = true;
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
			/// Был ли проведен откат
			/// </summary>
			public static bool IsDowngradeInvoked { get; set; }

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
				IsDowngradeInvoked = true;
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
			/// Был ли проведен откат
			/// </summary>
			public static bool IsDowngradeInvoked { get; set; }

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
				IsDowngradeInvoked = true;
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
