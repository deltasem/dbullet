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
	/// ��������� ��� MsSql
	/// </summary>
	public class MsSqlTemplateManager : ITemplateManager
	{
		#region BuildColumnCreateCommand
		/// <summary>
		/// ������ ����� �������� �������
		/// </summary>
		/// <param name="column">�������</param>
		/// <returns>����� �������� �������</returns>
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
		#endregion

		/// <summary>
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		public string GetCreateTableTemplate()
		{
			return GetTemplateFromResource("CreateTable.template");
		}

		/// <summary>
		/// ���������� ������ ��� �������� ������������� �������
		/// </summary>
		/// <returns>������</returns>
		public string GetIsTableExistTemplate()
		{
			return GetTemplateFromResource("IsTableExist.template");
		}

		/// <summary>
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		public string GetCreateIndexTemplate()
		{
			return GetTemplateFromResource("CreateIndex.template");
		}

		/// <summary>
		/// ���������� ������ ��� �������� �������
		/// </summary>
		/// <returns>������</returns>
		public string GetDropTableTemplate()
		{
			return GetTemplateFromResource("DropTable.template");
		}

		/// <summary>
		/// �������� ������ �� �������
		/// </summary>
		/// <param name="resourceName">�������� �������</param>
		/// <returns>������</returns>
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