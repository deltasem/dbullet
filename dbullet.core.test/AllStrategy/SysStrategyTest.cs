//-----------------------------------------------------------------------
// <copyright file="SysStrategyTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;
using dbullet.core.dbo;
using dbullet.core.dbs;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты системной стратегии
	/// </summary>
	[TestFixture]
	public abstract class SysStrategyTest : TestBase
	{
		/// <summary>
		/// Проверка возврата последней версии
		/// </summary>
		[Test]
		public void GetLastVersionTest()
		{
			sysStrategy = ObjectFactory.GetInstance<ISysDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(100500);
			Assembly assembly = this.GetType().Assembly;
			var actual = sysStrategy.GetLastVersion(assembly.GetName().Name);
			Assert.AreEqual(100500, actual);
		}

		/// <summary>
		/// Последняя версия должна учитывать сборку
		/// </summary>
		[Test]
		public void GetLastVersionShouldUseAssembly()
		{
			sysStrategy = ObjectFactory.GetInstance<ISysDatabaseStrategy>();
			int currentVersion = -1;
			command.SetupSet(x => x.CommandText = It.IsAny<string>())
				.Callback<string>(x => currentVersion = x.Contains(string.Format("'{0}'", GetType().Assembly.GetName().Name)) ? 10 : 0);
			command.Setup(x => x.ExecuteScalar()).Returns(() => currentVersion);
			Assembly assembly = this.GetType().Assembly;
			var actual = sysStrategy.GetLastVersion(assembly.GetName().Name);
			Assert.AreEqual(10, actual);
			Assembly assembly1 = typeof(int).Assembly;
			actual = sysStrategy.GetLastVersion(assembly1.GetName().Name);
			Assert.AreEqual(0, actual);
		}

		/// <summary>
		/// Когда еще нет ниодной версии
		/// </summary>
		[Test]
		public void GetLastVersionNull()
		{
			sysStrategy = ObjectFactory.GetInstance<ISysDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(DBNull.Value);
			Assembly assembly = this.GetType().Assembly;
			var actual = sysStrategy.GetLastVersion(assembly.GetName().Name);
			Assert.AreEqual(0, actual);
		}

		/// <summary>
		/// Проверка инициализации БД
		/// </summary>
		[Test]
		public void InitDatabaseNewDatabaseTest()
		{
			var databasesysStrategy = new Mock<IDatabaseStrategy>(); 
			ObjectFactory.Inject(databasesysStrategy.Object);
			sysStrategy = ObjectFactory.GetInstance<ISysDatabaseStrategy>();
			databasesysStrategy.Setup(x => x.IsTableExist("dbullet")).Returns(false);
			Assembly assembly = this.GetType().Assembly;
			sysStrategy.InitDatabase(assembly.GetName().Name);
			databasesysStrategy.Verify(x => x.CreateTable(It.Is<Table>(y => y.Name == "dbullet")), Times.Once());
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[Test]
		public void SetCurrentVersionTest()
		{
			sysStrategy = ObjectFactory.GetInstance<ISysDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);
			Assembly assembly = this.GetType().Assembly;
			sysStrategy.SetCurrentVersion(18, assembly.GetName().Name);
			command.VerifySet(x => x.CommandText = string.Format("insert into dbullet(Version, Assembly) values(18, '{0}')", GetType().Assembly.GetName().Name), Times.Once());
			command.Verify(x => x.ExecuteScalar(), Times.Once());
		}

		/// <summary>
		/// Тест установки текущей версии
		/// </summary>
		[Test]
		public void RemoveVersionInfo()
		{
			sysStrategy = ObjectFactory.GetInstance<ISysDatabaseStrategy>();
			command.Setup(x => x.ExecuteScalar()).Returns(1);

			Assembly assembly = this.GetType().Assembly;
			sysStrategy.RemoveVersionInfo(18, assembly.GetName().Name);

			command.VerifySet(x => x.CommandText = string.Format("delete from dbullet where version = 18 and Assembly = '{0}'", GetType().Assembly.GetName().Name), Times.Once());
			command.Verify(x => x.ExecuteScalar(), Times.Once());
		}
	}
}