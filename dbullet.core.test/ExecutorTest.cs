//-----------------------------------------------------------------------
// <copyright file="ExecutorTest.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using dbullet.core.attribute;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

using dbullet.core.tools;

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
    	var types = new[] { typeof(NotBullet), typeof(TestBullet1), typeof(TestBullet2WithouAttrs), typeof(NotBulletWithAttrs) };
    	var actual = Executor.GetBulletsInAssembly(new TestAssembly(types));
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
    	var types = new[]
    		{
    			typeof(TestBullet2WithouAttrs), 
					typeof(NotBulletWithAttrs), 
					typeof(TestBullet3), 
					typeof(TestBullet1),
    			typeof(TestBullet2), 
					typeof(NotBullet)
    		};
			var actual = Executor.GetBulletsInAssembly(new TestAssembly(types));
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
    	var types = new[]
    		{
    			typeof(TestBullet2WithouAttrs), 
					typeof(NotBulletWithAttrs), 
					typeof(TestBullet3), 
					typeof(TestBullet1),
    			typeof(TestBullet2), 
					typeof(NotBullet)
    		};
			var actual = Executor.GetBulletsInAssembly(new TestAssembly(types), true);
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
    	var types = new[] { typeof(TestBullet1) };

			var strategy = new Mock<ISysDatabaseStrategy>();
    	strategy.Setup(x => x.GetLastVersion(It.IsAny<Assembly>())).Returns(0);

			ObjectFactory.Initialize(x =>
			{
			  x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy.Object);
			  x.ForSingletonOf<IDatabaseStrategy>().Use(new Mock<IDatabaseStrategy>().Object);
			});

			var assembly = new TestAssembly(types);

			Executor.Execute(assembly);
			Assert.IsTrue(TestBullet1.IsUpdateInvoked);
    }

  	/// <summary>
    /// Тесты должны вызываться в последовательности
    /// </summary>
    [TestMethod]
    public void ExecuteBulletSequence()
  	{
			var types = new[] { typeof(TestBullet1), typeof(TestBullet2), typeof(TestBullet3) };
			int currentVersion = 1;
  		var strategy = new Mock<ISysDatabaseStrategy>();
  		strategy.Setup(x => x.GetLastVersion(It.IsAny<Assembly>())).Returns(currentVersion);
			ObjectFactory.Initialize(x => x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy.Object));
			Executor.Execute(new TestAssembly(types));
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
  		var types = new[] { typeof(TestBullet1), typeof(TestBullet2), typeof(TestBullet3) };
			int currentVersion = 3;

			var strategy = new Mock<ISysDatabaseStrategy>();
			strategy.Setup(x => x.GetLastVersion(It.IsAny<Assembly>())).Returns(() => currentVersion);
  		strategy
				.Setup(x => x.SetCurrentVersion(It.IsAny<Assembly>(), It.IsAny<int>()))
				.Callback((Assembly x, int y) => currentVersion = y);
			strategy
				.Setup(x => x.RemoveVersionInfo(It.IsAny<Assembly>(), It.IsAny<int>()))
				.Callback((Assembly x, int y) => currentVersion = y - 1);

			ObjectFactory.Initialize(x => x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy.Object));

			Executor.ExecuteBack(new TestAssembly(types), 1);
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
  		var types = new[] { typeof(TestBullet1), typeof(TestBullet2), typeof(TestBullet3) };

			int currentVersion = 0;
			var strategy = new Mock<ISysDatabaseStrategy>();
			strategy.Setup(x => x.GetLastVersion(It.IsAny<Assembly>())).Returns(() => currentVersion);
			strategy
				.Setup(x => x.SetCurrentVersion(It.IsAny<Assembly>(), It.IsAny<int>()))
				.Callback((Assembly x, int y) => currentVersion = y);
			ObjectFactory.Initialize(x => x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy.Object));
			Executor.Execute(new TestAssembly(types), 2);
			Assert.AreEqual(2, currentVersion);
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
  		var types = new[] { typeof(TestBullet1), typeof(TestBullet1) };
  		int currentVersion = 0;
			var strategy = new Mock<ISysDatabaseStrategy>();
			strategy.Setup(x => x.GetLastVersion(It.IsAny<Assembly>())).Returns(() => currentVersion);
			strategy
				.Setup(x => x.SetCurrentVersion(It.IsAny<Assembly>(), It.IsAny<int>()))
				.Callback((Assembly x, int y) => currentVersion = y);
			ObjectFactory.Initialize(x => x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy.Object));
			Executor.Execute(new TestAssembly(types));
			Assert.AreEqual(1, currentVersion);
    }

  	/// <summary>
    /// При не прохождении любого скрипта из булета, должен быть выполнен откат
    /// </summary>
    [TestMethod]
    public void ExecuteWithErrors()
  	{
  		var types = new[] { typeof(ErrorBullet) };
			int currentVersion = 1;
			var strategy = new Mock<ISysDatabaseStrategy>();
			strategy.Setup(x => x.GetLastVersion(It.IsAny<Assembly>())).Returns(() => currentVersion);
			strategy
				.Setup(x => x.SetCurrentVersion(It.IsAny<Assembly>(), It.IsAny<int>()))
				.Callback((Assembly x, int y) => currentVersion = y);
			ObjectFactory.Initialize(x =>
			{
			  x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy.Object);
			  x.ForSingletonOf<IDatabaseStrategy>().Use(new Mock<IDatabaseStrategy>().Object);
			});
			Executor.Execute(new TestAssembly(types));
			Assert.AreEqual(1, currentVersion);
			Assert.IsTrue(ErrorBullet.IsDowngradeInvoked);
    }

  	/// <summary>
    /// При возникновении ошибки, не должно проходит обновление дальше
    /// </summary>
    [TestMethod]
    public void ExecuteWithErrorsNotContinue()
  	{
  		var types = new[] { typeof(ErrorBullet), typeof(TestBullet3) };
			int currentVersion = 1;
			var strategy = new Mock<ISysDatabaseStrategy>();
			strategy.Setup(x => x.GetLastVersion(It.IsAny<Assembly>())).Returns(() => currentVersion);
			strategy
				.Setup(x => x.SetCurrentVersion(It.IsAny<Assembly>(), It.IsAny<int>()))
				.Callback((Assembly x, int y) => currentVersion = y);
			ObjectFactory.Initialize(x =>
			{
				x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy.Object);
				x.ForSingletonOf<IDatabaseStrategy>().Use(new Mock<IDatabaseStrategy>().Object);
			});
			Executor.Execute(new TestAssembly(types));
			Assert.AreEqual(1, currentVersion);
			Assert.IsFalse(TestBullet3.IsUpdateInvoked);
    }

  	/// <summary>
    /// При возникновении ошибки в ходе downgrade должны проходить остальные шаги
    /// </summary>
    [TestMethod]
    public void ExecuteWithErrorsDowngradeWithErrors()
  	{
  		var types = new[] { typeof(ErrorStepBullet) };
			int currentVersion = 0;
			var strategy = new Mock<ISysDatabaseStrategy>();
			strategy.Setup(x => x.GetLastVersion(It.IsAny<Assembly>())).Returns(() => currentVersion);
			strategy
				.Setup(x => x.SetCurrentVersion(It.IsAny<Assembly>(), It.IsAny<int>()))
				.Callback((Assembly x, int y) => currentVersion = y);
			
  		var mssqlStrategy = new Mock<IDatabaseStrategy>();
  		mssqlStrategy.Setup(x => x.AddColumn(It.IsAny<Table>(), It.IsAny<Column>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.CreateForeignKey(It.IsAny<ForeignKey>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.CreateIndex(It.IsAny<Index>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.CreateTable(It.IsAny<Table>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.DropForeignKey(It.IsAny<ForeignKey>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.DropIndex(It.IsAny<Index>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.DropTable(It.IsAny<string>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.InsertRows(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<object[]>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.IsTableExist(It.IsAny<string>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.IsColumnExists(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.LoadCsv(It.IsAny<string>(), It.IsAny<StreamReader>(), It.IsAny<Dictionary<string, Func<string, object>>>(), It.IsAny<CsvQuotesType>(), It.IsAny<bool>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.DropColumn(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.DeleteRows(It.IsAny<string>(), It.IsAny<object[]>())).Throws(new Exception()).Verifiable();
			mssqlStrategy.Setup(x => x.UnloadCsv(It.IsAny<string>(), It.IsAny<StreamReader>(), It.IsAny<string>(), It.IsAny<Func<string, object>>(), It.IsAny<CsvQuotesType>())).Throws(new Exception()).Verifiable();
			
			ObjectFactory.Initialize(x =>
			{
			  x.ForSingletonOf<ISysDatabaseStrategy>().Use(strategy.Object);
				x.ForSingletonOf<IDatabaseStrategy>().Use(mssqlStrategy.Object);
			});

			Executor.Execute(new TestAssembly(types));
			mssqlStrategy.VerifyAll();
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
        IsTableExist("test");
        IsColumnExists("test", "test");
        InsertRows("test", true);
        DeleteRows("test");
        UnloadCsv("test", null, "test");
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
  	/// Assembly for test
  	/// </summary>
  	public class TestAssembly : Assembly
  	{
  		/// <summary>
  		/// Test types
  		/// </summary>
  		private readonly Type[] types;

  		/// <summary>
  		/// Constructor
  		/// </summary>
  		/// <param name="types">Types, wich returns method GetTypes</param>
  		public TestAssembly(Type[] types)
  		{
  			this.types = types;
  		}

  		/// <summary>
  		/// Gets the types defined in this assembly.
  		/// </summary>
  		/// <returns>
  		/// An array of type <see cref="T:System.Type"/> containing objects for all the types defined in this assembly.
  		/// </returns>
  		/// <exception cref="T:System.Reflection.ReflectionTypeLoadException">The assembly contains one or more types that cannot be loaded. The array returned by the <see cref="P:System.Reflection.ReflectionTypeLoadException.Types"/> property of this exception contains a <see cref="T:System.Type"/> object for each type that was loaded and null for each type that could not be loaded, while the <see cref="P:System.Reflection.ReflectionTypeLoadException.LoaderExceptions"/> property contains an exception for each type that could not be loaded.</exception>
  		public override Type[] GetTypes()
  		{
  			return this.types;
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
