//-----------------------------------------------------------------------
// <copyright file="OracleTemplateManager.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using dbullet.core.dbo;
using dbullet.core.engine.common;
using dbullet.core.exception;

namespace dbullet.core.engine.Oracle
{
	/// <summary>
	/// Менеджер оракловых темплейтов
	/// </summary>
	public class OracleTemplateManager : TemplateManagerBase
	{
		#region BuildColumnCreateCommand
		/// <summary>
		/// Создаёт текст создания столбца
		/// </summary>
		/// <param name="column">Столбец</param>
		/// <returns>Текст создания столбца</returns>
		public static string BuildColumnCreateCommand(Column column)
		{
			StringBuilder sb = null;
			if (column.ColumnType.DbType.HasValue)
			{
				sb = GetDbTypeString(column);
			}
			else
			{
				sb = GetSqlDbTypeString(column);
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

		/// <summary>
		/// Возвращает значение дефалта
		/// </summary>
		/// <param name="def">Дефалт</param>
		/// <returns>Значение дефалта</returns>
		public static string GetDefaultValue(Default def)
		{
			string result = string.Empty;
			ValueDefault vd = def as ValueDefault;
			if (vd != null)
			{
				result = vd.Value;
			}
			else
			{
				StandartDefault sd = def as StandartDefault;
				if (sd != null)
				{
					switch (sd.DefaultType)
					{
						case StandartDefaultType.date:
							result = "getdate()";
							break;
						case StandartDefaultType.guid:
							result = "newid()";
							break;
						default:
							result = string.Empty;
							break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Получить шаблон из ресурса
		/// </summary>
		/// <param name="resourceName">Название ресурса</param>
		/// <returns>Шаблон</returns>
		protected override string GetTemplateFromResource(string resourceName)
		{
			using (var resource = GetType().Assembly.GetManifestResourceStream("dbullet.core.engine.Oracle." + resourceName))
			{
				if (resource == null)
				{
					throw new TemplateNotFoundException();
				}

				using (StreamReader sr = new StreamReader(resource))
				{
					return sr.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Получает строку из перечисления DbType
		/// </summary>
		/// <param name="column">Колонка</param>
		/// <returns>Строка</returns>
		private static StringBuilder GetDbTypeString(Column column)
		{
			if (column.ColumnType.DbType == DbType.String && column.ColumnType.Length == 0)
			{
				throw new ArgumentException("String must have length");
			}

			if (column.ColumnType.DbType == DbType.Binary && column.ColumnType.Length == 0)
			{
				throw new ArgumentException("Binary must have length");
			}

			var sb = new StringBuilder(string.Format("\"{0}\"", column.Name));
			switch (column.ColumnType.DbType)
			{
				case DbType.Decimal:
					sb.Append(" number");
					break;
				case DbType.String:
					sb.Append(" varchar2");
					break;
				case DbType.Int32:
					sb.Append(" int ");
					break;
				case DbType.Boolean:
					sb.Append(" char(1) ");
					break;
				case DbType.Date:
					sb.Append(" date ");
					break;
				case DbType.DateTime:
					sb.Append(" date ");
					break;
				case DbType.Guid:
					sb.Append(" raw(16) ");
					break;
				case DbType.Xml:
					sb.Append(" blob ");
					break;
				case DbType.Binary:
					sb.Append(" raw");
					break;
				default:
					throw new UnsuportedDbTypeException(column.ColumnType.DbType.GetValueOrDefault());
			}

			return sb;
		}

		/// <summary>
		/// Получает строку из перечисления DbType
		/// </summary>
		/// <param name="column">Колонка</param>
		/// <returns>Строка</returns>
		private static StringBuilder GetSqlDbTypeString(Column column)
		{
			var sb = new StringBuilder(string.Format("\"{0}\"", column.Name));
			switch (column.ColumnType.SqlDbType)
			{
				case SqlDbType.VarBinary:
					sb.Append(" blob ");
					break;
				default:
					throw new UnsuportedDbTypeException(column.ColumnType.DbType.GetValueOrDefault());
			}

			return sb;
		}
	}
}