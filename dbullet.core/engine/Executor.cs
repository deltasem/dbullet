using System.Reflection;

namespace dbullet.core.engine
{
	/// <summary>
	/// ����������
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
	}
}
