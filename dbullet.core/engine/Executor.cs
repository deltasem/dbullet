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
using NLog;
using StructureMap;

namespace dbullet.core.engine
{
	/// <summary>
	/// Выполнитель
	/// </summary>
	public class Executor
	{
		/// <summary>
		/// Логгер
		/// </summary>
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Инициализация
		/// </summary>
		/// <param name="connectionString">Строка подключения</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		[Obsolete("Используйте перегруженый метод")]
		public static void Initialize(string connectionString, SupportedStrategy strategy)
		{
			Initialize(connectionString, strategy, typeof(Executor).Assembly);
		}

		/// <summary>
		/// Инициализация
		/// </summary>
		/// <param name="connectionString">Строка подключения</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		/// <param name="assembly">Сборка, содержащая булеты</param>
		public static void Initialize(string connectionString, SupportedStrategy strategy, Assembly assembly)
		{
			InitConnections(assembly, strategy, connectionString);
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
			Type[] types = assembly.GetTypes();

			foreach (var bulletType in GetSortedBullets(types))
			{
				var currentVersion = ObjectFactory.GetInstance<ISysDatabaseStrategy>().GetLastVersion(assembly.GetName().Name);
				var bulletVersion = ((BulletNumberAttribute)bulletType.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision;
				if (bulletVersion > currentVersion && bulletVersion <= stopVersion)
				{
					var bullet = (Bullet)Activator.CreateInstance(bulletType);
					try
					{
						bullet.Update();
						ObjectFactory.GetInstance<ISysDatabaseStrategy>().SetCurrentVersion(bulletVersion, assembly.GetName().Name);
					}
					catch (Exception ex)
					{
						logger.Error(ex.Message);
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
			Type[] types = assembly.GetTypes();

			foreach (var bulletType in GetSortedBullets(types, true))
			{
				var currentVersion = ObjectFactory.GetInstance<ISysDatabaseStrategy>().GetLastVersion(assembly.GetName().Name);
				var bulletVersion = ((BulletNumberAttribute)bulletType.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision;
				if (currentVersion == bulletVersion && bulletVersion > stopVersion)
				{
					var bullet = (Bullet)Activator.CreateInstance(bulletType);
					bullet.Downgrade();
					ObjectFactory.GetInstance<ISysDatabaseStrategy>().RemoveVersionInfo(bulletVersion, assembly.GetName().Name);
				}
			}
		}

		/// <summary>
		/// Возвращает список булетов из сборки
		/// </summary>
		/// <param name="types">Тип объекта</param>
		/// <param name="revert">В обратном порядке</param>
		/// <returns>Упорядоченный список булетов</returns>
		internal static IEnumerable<Type> GetSortedBullets(Type[] types, bool revert = false)
		{
			return types
				.Where(p => typeof(Bullet).IsAssignableFrom(p) && p.IsDefined(typeof(BulletNumberAttribute), true))
				.OrderBy(p => ((BulletNumberAttribute)p.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision * (revert ? -1 : 1));
		}

		/// <summary>
		/// Инициализация соединений с БД
		/// </summary>
		/// <param name="assembly">Сборка с булетами</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		/// <param name="connectionString">Строка подключения</param>
		private static void InitConnections(Assembly assembly, SupportedStrategy strategy, string connectionString)
		{
			if (strategy != SupportedStrategy.Mssql2008)
			{
				throw new NotSupportedException("Only MS SQL supported");
			}

			ObjectFactory.Initialize(x => InitializeStructureMap(x, connectionString));
			ObjectFactory.GetInstance<ISysDatabaseStrategy>().InitDatabase(assembly.GetName().Name);
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
