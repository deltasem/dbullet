//-----------------------------------------------------------------------
// <copyright file="Executor.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using dbullet.core.attribute;
using dbullet.core.dbs;

namespace dbullet.core.engine
{
	/// <summary>
	/// Выполнитель
	/// </summary>
	public class Executor
	{
		/// <summary>
		/// Системная стратегия
		/// </summary>
		private static ISysDatabaseStrategy systemStrategy;

		/// <summary>
		/// Стратегия работы с БД
		/// </summary>
		private static IDatabaseStrategy databaseStrategy;

		/// <summary>
		/// Стратегия работы с БД
		/// </summary>
		public static IDatabaseStrategy DatabaseStrategy
		{
			get { return databaseStrategy; }
		}

		/// <summary>
		/// Системная стратегия
		/// </summary>
		internal static ISysDatabaseStrategy SystemStrategy
		{
			get { return systemStrategy; }
		}

		/// <summary>
		/// Выполнить обновление
		/// </summary>
		/// <param name="assemblyName">Название сборки, содержащей булеты</param>
		/// <param name="connectionString">Строка подключения</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		public static void Execute(string assemblyName, string connectionString, SupportedStrategy strategy)
		{
			var asm = Assembly.Load(assemblyName);
			Execute(asm, connectionString, strategy);
		}

		/// <summary>
		/// Выполнить обновление
		/// </summary>
		/// <param name="assembly">Сборка, содержащая булеты</param>
		/// <param name="connectionString">Строка подключения</param>
		/// <param name="strategy">Стратегия работы с БД</param>
		public static void Execute(Assembly assembly, string connectionString, SupportedStrategy strategy)
		{
			if (strategy != SupportedStrategy.Mssql2008)
			{
				throw new NotSupportedException("Only MS SQL supported");
			}

			systemStrategy = new MsSql2008SysStrategy(new SqlConnection(connectionString));
			databaseStrategy = new MsSql2008Strategy(new SqlConnection(connectionString));
			systemStrategy.InitDatabase();

			foreach (var bulletType in GetBulletsInAssembly(assembly))
			{
				var currentVersion = systemStrategy.GetLastVersion();
				var bulletVersion = ((BulletNumberAttribute)bulletType.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision;
				if (bulletVersion > currentVersion)
				{
					var bullet = (Bullet)Activator.CreateInstance(bulletType);
					try
					{
						bullet.Update();
						systemStrategy.SetCurrentVersion(bulletVersion);
					}
					catch(Exception)
					{
						bullet.Downgrade();
						break;
					}
				}
			}
		}

		/// <summary>
		/// Возвращает список булетов из сборки
		/// </summary>
		/// <param name="assembly">Сборка с булетами</param>
		/// <returns>Упорядоченный список булетов</returns>
		internal static IEnumerable<Type> GetBulletsInAssembly(Assembly assembly)
		{
			return assembly.GetTypes()
				.Where(p => typeof(Bullet).IsAssignableFrom(p) && p.IsDefined(typeof(BulletNumberAttribute), true))
				.OrderBy(p => ((BulletNumberAttribute)p.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision);
		}
	}
}
