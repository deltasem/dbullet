//-----------------------------------------------------------------------
// <copyright file="MsSqlTemplateManager.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using System.IO;
using System.Text;
using dbullet.core.dbo;
using dbullet.core.exception;

namespace dbullet.core.engine.MsSql
{
	/// <summary>
	/// Темплейты для MsSql
	/// </summary>
	public class MsSqlTemplateManager : ITemplateManager
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
			if (column.Identity)
			{
				sb.Append(" identity(1, 1)");
			}

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
		/// Возвращает шаблон для создания таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetCreateTableTemplate()
		{
			return GetTemplateFromResource("CreateTable.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для проверки существования таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetIsTableExistTemplate()
		{
			return GetTemplateFromResource("IsTableExist.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для проверки существования столбца
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetIsColumnExistTemplate()
		{
			return GetTemplateFromResource("IsColumnExist.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для создания индекса
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetCreateIndexTemplate()
		{
			return GetTemplateFromResource("CreateIndex.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для удаления таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetDropTableTemplate()
		{
			return GetTemplateFromResource("DropTable.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для удаления индекса
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetDropIndexTemplate()
		{
			return GetTemplateFromResource("DropIndex.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для создения внешнего ключа
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetCreateForeignKeyTemplate()
		{
			return GetTemplateFromResource("CreateForeignKey.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для удаления внешнего ключа
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetDropForeignKeyTemplate()
		{
			return GetTemplateFromResource("DropForeignKey.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для вставки записей
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetInsertRowsTemplate()
		{
			return GetTemplateFromResource("InsertRows.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для вставки записей потоком
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetInsertRowsStreamTemplate()
		{
			return GetTemplateFromResource("InsertRowsStream.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон для добавления колонки
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetAddColumnTemplate()
		{
			return GetTemplateFromResource("AddColumn.cshtml");
		}

		/// <summary>
		/// Возвращает шаблон удаления колонки
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetDropColumnTemplate()
		{
			return GetTemplateFromResource("DropColumn.cshtml");
		}

		/// <summary>
		/// Returns template for delete row script
		/// </summary>
		/// <returns>Delete row template</returns>
		public string GetDeleteRowsTemplate()
		{
			return GetTemplateFromResource("DeleteRows.cshtml");
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

			var sb = new StringBuilder(string.Format("[{0}]", column.Name));
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
				case DbType.Boolean:
					sb.Append(" bit ");
					break;
				case DbType.Date:
					sb.Append(" date ");
					break;
				case DbType.DateTime:
					sb.Append(" datetime ");
					break;
				case DbType.Guid:
					sb.Append(" uniqueidentifier ");
					break;
				case DbType.Xml:
					sb.Append(" xml ");
					break;
				case DbType.Binary:
					sb.Append(" binary");
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
			var sb = new StringBuilder(string.Format("[{0}]", column.Name));
			switch (column.ColumnType.SqlDbType)
			{
				case SqlDbType.VarBinary:
					sb.Append(" varbinary(MAX) ");
					break;
				default:
					throw new UnsuportedDbTypeException(column.ColumnType.DbType.GetValueOrDefault());
			}

			return sb;
		}

		/// <summary>
		/// Получить шаблон из ресурса
		/// </summary>
		/// <param name="resourceName">Название ресурса</param>
		/// <returns>Шаблон</returns>
		private string GetTemplateFromResource(string resourceName)
		{
			using (var resource = GetType().Assembly.GetManifestResourceStream("dbullet.core.engine.MsSql." + resourceName))
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
	}
}