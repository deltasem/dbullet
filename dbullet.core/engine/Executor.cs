using System.Reflection;

namespace dbullet.core.engine
{
	/// <summary>
	/// Запускатор
	/// </summary>
	public class Executor
	{
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
		}
	}
}
