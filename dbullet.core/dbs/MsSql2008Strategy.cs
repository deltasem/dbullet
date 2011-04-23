using System.Data.SqlClient;
using System.Text;
using dbullet.core.dbo;
using dbullet.core.exception;

namespace dbullet.core.dbs
{
	using System;
	using System.Data;

	/// <summary>
	/// Стратегия работы с базой MS SQL 2008
	/// </summary>
	internal class MsSql2008Strategy : IDatabaseStrategy
	{
		/// <summary>
		/// Подключение к базе
		/// </summary>
		private readonly SqlConnection connection;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="connection">Соединение с БД</param>
		public MsSql2008Strategy(SqlConnection connection)
		{
			this.connection = connection;
		}

		#region BuildColumnCreateCommand
		/// <summary>
		/// Создаёт текст создания столбца
		/// </summary>
		/// <param name="column">Столбец</param>
		/// <returns>Текст создания столбца</returns>
		public static string BuildColumnCreateCommand(Column column)
		{
			if (column.ColumnType.DbType == DbType.String && column.ColumnType.Length == 0)
			{
				throw new ArgumentException("String must have length");
			}

			StringBuilder sb = new StringBuilder(column.Name);
			switch (column.ColumnType.DbType)
			{
				case DbType.Decimal:
					sb.Append(" decimal");
					break;
				case DbType.String:
					sb.Append(" nvarchar");
					break;
				case DbType.Int32:
					sb.Append(" int ");
					break;
				case DbType.AnsiString:
				case DbType.Binary:
				case DbType.Byte:
				case DbType.Boolean:
				case DbType.Currency:
				case DbType.Date:
				case DbType.DateTime:
				case DbType.Double:
				case DbType.Guid:
				case DbType.Int16:
				case DbType.Int64:
				case DbType.Object:
				case DbType.SByte:
				case DbType.Single:
				case DbType.Time:
				case DbType.UInt16:
				case DbType.UInt32:
				case DbType.UInt64:
				case DbType.VarNumeric:
				case DbType.AnsiStringFixedLength:
				case DbType.StringFixedLength:
				case DbType.Xml:
				case DbType.DateTime2:
				case DbType.DateTimeOffset:
				default:
					throw new UnsuportedDbTypeException();
			}

			if (column.ColumnType.Length != 0 && column.ColumnType.Scale == 0)
			{
				sb.AppendFormat("({0}) ", column.ColumnType.Length);
			}
			else if (column.ColumnType.Length != 0 && column.ColumnType.Scale != 0)
			{
				sb.AppendFormat("({0}, {1}) ", column.ColumnType.Length, column.ColumnType.Scale);
			}

			sb.Append(column.Nullable ? "null" : "not null");

			return sb.ToString();
		}
		#endregion

		#region CreateTable
		/// <summary>
		/// Создаёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		public void CreateTable(Table table)
		{
			if (table.Columns == null || table.Columns.Count == 0)
			{
				throw new CollumnExpectedException();
			}

			this.connection.Open();
			using (var cmd = new SqlCommand(string.Empty, this.connection))
			{
				var sb = new StringBuilder();
				sb.AppendFormat("create table {0} ", table.Name);
				sb.Append("(");
				for (int i = 0; i < table.Columns.Count; i++)
				{
					var column = table.Columns[i];
					sb.Append(BuildColumnCreateCommand(column));
					if (i != table.Columns.Count - 1)
					{
						sb.Append(", ");
					}
				}

				sb.AppendFormat(") on [{0}]", table.Partition.Name);
				cmd.CommandText = sb.ToString();
				cmd.ExecuteNonQuery();
			}

			this.connection.Close();
		}
		#endregion
	}
}
