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
	/// �����������
	/// </summary>
	public class Executor
	{
		/// <summary>
		/// ��������� ����������
		/// </summary>
		/// <param name="assemblyName">�������� ������, ���������� ������</param>
		/// <param name="connectionString">������ �����������</param>
		/// <param name="strategy">��������� ������ � ��</param>
		public static void Execute(string assemblyName, string connectionString, SupportedStrategy strategy)
		{
			var asm = Assembly.Load(assemblyName);
			Execute(asm, connectionString, strategy);
		}

		/// <summary>
		/// ��������� ����������
		/// </summary>
		/// <param name="assembly">������, ���������� ������</param>
		/// <param name="connectionString">������ �����������</param>
		/// <param name="strategy">��������� ������ � ��</param>
		public static void Execute(Assembly assembly, string connectionString, SupportedStrategy strategy)
		{
			if (strategy != SupportedStrategy.Mssql2008)
			{
				throw new NotSupportedException("Only MS SQL supported");
			}

			// todo: ������� � �������
			ISysDatabaseStrategy systemStrategy = new MsSql2008SysStrategy(new SqlConnection(connectionString));

			foreach (var bulletType in GetBulletsInAssembly(assembly))
			{
				var currentVersion = systemStrategy.GetLastVersion();
				var bulletVersion = ((BulletNumberAttribute)bulletType.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision;
				if (bulletVersion > currentVersion)
				{
					var bullet = (Bullet)Activator.CreateInstance(bulletType);
					bullet.Update();
					systemStrategy.SetCurrentVersion(bulletVersion);
				}
			}
		}

		/// <summary>
		/// ���������� ������ ������� �� ������
		/// </summary>
		/// <param name="assembly">������ � ��������</param>
		/// <returns>������������� ������ �������</returns>
		internal static IEnumerable<Type> GetBulletsInAssembly(Assembly assembly)
		{
			return assembly.GetTypes()
				.Where(p => typeof(Bullet).IsAssignableFrom(p) && p.IsDefined(typeof(BulletNumberAttribute), true))
				.OrderBy(p => ((BulletNumberAttribute)p.GetCustomAttributes(typeof(BulletNumberAttribute), false)[0]).Revision);
		}
	}
}
