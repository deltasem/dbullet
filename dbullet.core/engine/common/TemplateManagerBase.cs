//-----------------------------------------------------------------------
// <copyright file="TemplateManagerBase.cs" author="delta" company="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace dbullet.core.engine.common
{
	/// <summary>
	/// Базовый менеджер темплейтов
	/// </summary>
	public abstract class TemplateManagerBase : ITemplateManager
	{
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
		/// Получить шаблон из ресурса
		/// </summary>
		/// <param name="resourceName">Название ресурса</param>
		/// <returns>Шаблон</returns>
		protected abstract string GetTemplateFromResource(string resourceName);
	}
}