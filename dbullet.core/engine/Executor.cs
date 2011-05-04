using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dbullet.core.attribute;

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
		}

		/// <summary>
		/// ���������� ������ ������� �� ������
		/// </summary>
		/// <param name="assembly">������ � ��������</param>
		/// <returns>������������� ������ �������</returns>
		internal static IEnumerable<Type> GetBulletsInAssembly(Assembly assembly)
		{
			return assembly.GetTypes().Where(p => typeof(Bullet).IsAssignableFrom(p) && p.IsDefined(typeof(BulletNumberAttribute), true));
		}
	}
}
