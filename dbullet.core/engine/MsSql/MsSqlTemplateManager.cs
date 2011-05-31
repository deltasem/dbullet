//-----------------------------------------------------------------------
// <copyright file="MsSqlTemplateManager.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
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
				case DbType.AnsiString:
				case DbType.Binary:
				case DbType.Byte:
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

		/// <summary>
		/// Возвращает шаблон для создания таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetCreateTableTemplate()
		{
			return @"create table @Model.Name (
@for (int i = 0; i < Model.Columns.Count; i++){
@dbullet.core.engine.MsSql.MsSqlTemplateManager.BuildColumnCreateCommand(Model.Columns[i])
@(i != Model.Columns.Count - 1 ? "", "" : """")
}
@{var pk = Model.Columns.FirstOrDefault(p => p.Constraint != null);}
@if (pk != null){
@:, constraint @pk.Constraint.Name primary key clustered(@pk.Name asc) with (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
}
) on [@Model.PartitionName]
";
		}

		/// <summary>
		/// Возвращает шаблон для проверки существования таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetIsTableExistTemplate()
		{
			return "select count(*) from sysobjects where id = object_id(N'@Model.Name') and OBJECTPROPERTY(id, N'IsTable') = 1";
		}

		/// <summary>
		/// Возвращает шаблон для создания индекса
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetCreateIndexTemplate()
		{
			return @"create @(Model.IsUnique ? ""unique "" : string.Empty)
@(Model.IndexType == dbullet.core.dbo.IndexType.Clustered ? ""clustered"" : ""nonclustered"") 
index @Model.Name on @Model.Table.Name (
@for (int i = 0; i < @Model.Columns.Count; i++){
@Model.Columns[i].Name @(Model.Columns[i].Direction == dbullet.core.dbo.Direction.Ascending ? "" asc"" : "" desc"")
@(i != Model.Columns.Count - 1 ? "", "" : """")
}
) 
whth (STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
ON [@Model.PartitionName]";
		}

		/// <summary>
		/// Возвращает шаблон для удаления таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		public string GetDropTableTemplate()
		{
			return "drop table @Model.Name";
		}
	}
}