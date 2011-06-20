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
			if (column.ColumnType.DbType == DbType.String && column.ColumnType.Length == 0)
			{
				throw new ArgumentException("String must have length");
			}

			var sb = new StringBuilder(column.Name);
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
				case DbType.AnsiString:
				case DbType.Binary:
				case DbType.Byte:
				case DbType.Currency:
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