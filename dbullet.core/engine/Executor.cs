//-----------------------------------------------------------------------
// <copyright file="Executor.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using dbullet.core.attribute;
using dbullet.core.dbs;
using StructureMap;

namespace dbullet.core.engine
{
	/// <summary>
	/// Выполнитель
	/// </summary>
	public class Executor
	{
		/// <summary>
		/// Инициализация
		/// </summary>
		/// <param name="connectionString">Строка подключения</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		public static void Initialize(string connectionString, SupportedStrategy strategy)
		{
			InitConnections(strategy, connectionString);
		}

		/// <summary>
		/// Выполнить обновление
		/// </summary>
		/// <param name="assemblyName">Название сборки, содержащей булеты</param>
		/// <param name="stopVersion">Последняя версия</param>
		public static void Execute(string assemblyName, int stopVersion = int.MaxValue)
		{
			var asm = Assembly.Load(assemblyName);
			Execute(asm, stopVersion);
		}

		/// <summary>
		/// Выполнить обновление
		/// </summary>
		/// <param name="assembly">Сборка, содержащая булеты</param>
		/// <param name="stopVersion">Последняя версия</param>
		public static void Execute(Assembly assembly, int stopVersion = int.MaxValue)
		{
			foreach (var bulletType in GetBulletsInAssembly(assembly))
			{
				var currentVersion = ObjectFactory.GetInstance<ISysDatabaseStrategy>().GetLastVersion(assembly);
				var bulletVersion = ((BulletNumberAttribute)bulletType.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision;
				if (bulletVersion > currentVersion && bulletVersion <= stopVersion)
				{
					var bullet = (Bullet)Activator.CreateInstance(bulletType);
					try
					{
						bullet.Update();
						ObjectFactory.GetInstance<ISysDatabaseStrategy>().SetCurrentVersion(bulletVersion);
					}
					catch (Exception)
					{
						var strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
						try
						{
							ObjectFactory.Configure(x => x.ForSingletonOf<IDatabaseStrategy>().Use(new ProtectedStrategy(strategy)));
							bullet.Downgrade();
						}
						finally
						{
							ObjectFactory.Configure(x => x.ForSingletonOf<IDatabaseStrategy>().Use(strategy));
						}

						break;
					}
				}
			}
		}

		/// <summary>
		/// Откатиться назад
		/// </summary>
		/// <param name="assemblyName">Название сборки, содержащей булеты</param>
		/// <param name="stopVersion">Последняя версия</param>		
		public static void ExecuteBack(string assemblyName, int stopVersion)
		{
			var asm = Assembly.Load(assemblyName);
			ExecuteBack(asm, stopVersion);
		}

		/// <summary>
		/// Выполнить обновление
		/// </summary>
		/// <param name="assembly">Сборка, содержащая булеты</param>
		/// <param name="stopVersion">Последняя версия</param>
		public static void ExecuteBack(Assembly assembly, int stopVersion)
		{
			foreach (var bulletType in GetBulletsInAssembly(assembly, true))
			{
				var currentVersion = ObjectFactory.GetInstance<ISysDatabaseStrategy>().GetLastVersion(assembly);
				var bulletVersion = ((BulletNumberAttribute)bulletType.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision;
				if (currentVersion == bulletVersion && bulletVersion > stopVersion)
				{
					var bullet = (Bullet)Activator.CreateInstance(bulletType);
					bullet.Downgrade();
					ObjectFactory.GetInstance<ISysDatabaseStrategy>().RemoveVersionInfo(bulletVersion);
				}
			}
		}

		/// <summary>
		/// Возвращает список булетов из сборки
		/// </summary>
		/// <param name="assembly">Сборка с булетами</param>
		/// <param name="revert">В обратном порядке</param>
		/// <returns>Упорядоченный список булетов</returns>
		internal static IEnumerable<Type> GetBulletsInAssembly(Assembly assembly, bool revert = false)
		{
			return assembly.GetTypes()
				.Where(p => typeof(Bullet).IsAssignableFrom(p) && p.IsDefined(typeof(BulletNumberAttribute), true))
				.OrderBy(p => ((BulletNumberAttribute)p.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision * (revert ? -1 : 1));
		}

		/// <summary>
		/// Инициализация соединений с БД
		/// </summary>
		/// <param name="strategy">Стратегия работы с БД</param>
		/// <param name="connectionString">Строка подключения</param>
		private static void InitConnections(SupportedStrategy strategy, string connectionString)
		{
			if (strategy != SupportedStrategy.Mssql2008)
			{
				throw new NotSupportedException("Only MS SQL supported");
			}

			ObjectFactory.Initialize(x => InitializeStructureMap(x, connectionString));
			ObjectFactory.GetInstance<ISysDatabaseStrategy>().InitDatabase();
		}

		/// <summary>
		/// Инициализация IoC контейнера
		/// </summary>
		/// <param name="x">Инициализатор</param>
		/// <param name="connectionString">Строка подключения</param>
		private static void InitializeStructureMap(IInitializationExpression x, string connectionString)
		{
			x.ForSingletonOf<IDbConnection>().Use(new SqlConnection(connectionString));
			x.ForSingletonOf<IDatabaseStrategy>().Use<MsSql2008Strategy>();
			x.ForSingletonOf<ISysDatabaseStrategy>().Use<MsSql2008SysStrategy>();
		}
	}
}
