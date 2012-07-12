//-----------------------------------------------------------------------
// <copyright file="FileSysStrategy.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Text;
using dbullet.core.dbs;

namespace dbullet.core.engine.File
{
	/// <summary>
	/// Стратегия в файл
	/// </summary>
	public class FileSysStrategy : ISysDatabaseStrategy
	{
		/// <summary>
		/// Имплементация стратегии
		/// </summary>
		private readonly ISysDatabaseStrategy sysStrategyImpl;

		/// <summary>
		/// Соединение
		/// </summary>
		private readonly FileConnection fileConnection;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="sysStrategyImpl">Стратегия</param>
		/// <param name="fileConnection">Соединение</param>
		public FileSysStrategy(ISysDatabaseStrategy sysStrategyImpl, FileConnection fileConnection)
		{
			this.sysStrategyImpl = sysStrategyImpl;
			this.fileConnection = fileConnection;
		}

		/// <summary>
		/// Инициализация базы данных
		/// Добавление, если нет системной таблицы
		/// </summary>
		/// <param name="name">Имя</param>
		public void InitDatabase(string name)
		{
			// sysStrategyImpl.InitDatabase(name);
		}

		/// <summary>
		/// Возвращает последнюю версию базы
		/// </summary>
		/// <param name="name">Имя</param>
		/// <returns>Версия базы</returns>
		public int GetLastVersion(string name)
		{
			return 0;
		}

		/// <summary>
		/// Установка текущей версии
		/// </summary>
		/// <param name="version">Версия</param>
		/// <param name="name">Имя</param>
		public void SetCurrentVersion(int version, string name)
		{
			string fileName = string.Format("{2}\\{0}.v{1}.sql", name, version, fileConnection.ConnectionString);

			System.IO.File.WriteAllText(fileName, "begin\r\n");
			System.IO.File.AppendAllText(fileName, fileConnection.Output.ToString());
			System.IO.File.AppendAllText(fileName, string.Format("begin\r\n insert into dbullet(Version, Assembly) values({0}, '{1}');\r\nend;\r\n", version, name));
			System.IO.File.AppendAllText(fileName, "end;");
			fileConnection.Output = new StringBuilder();
		}

		/// <summary>
		/// Удаление информации об указанной версии
		/// </summary>
		/// <param name="version">Версия</param>
		/// <param name="name">Имя</param>
		public void RemoveVersionInfo(int version, string name)
		{
			sysStrategyImpl.RemoveVersionInfo(version, name);
		}
	}
}