//-----------------------------------------------------------------------
// <copyright file="ITemplateManager.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.engine.common
{
	/// <summary>
	/// Менеджер темплейтов
	/// </summary>
	public interface ITemplateManager
	{
		/// <summary>
		/// Возвращает шаблон для создания таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetCreateTableTemplate();

		/// <summary>
		/// Возвращает шаблон для проверки существования таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetIsTableExistTemplate();

		/// <summary>
		/// Возвращает шаблон для проверки существования столбца
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetIsColumnExistTemplate();

		/// <summary>
		/// Возвращает шаблон для создания индекса
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetCreateIndexTemplate();

		/// <summary>
		/// Возвращает шаблон для удаления таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetDropTableTemplate();

		/// <summary>
		/// Возвращает шаблон для удаления индекса
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetDropIndexTemplate();

		/// <summary>
		/// Возвращает шаблон для создения внешнего ключа
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetCreateForeignKeyTemplate();

		/// <summary>
		/// Возвращает шаблон для удаления внешнего ключа
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetDropForeignKeyTemplate();

		/// <summary>
		/// Возвращает шаблон для вставки записей
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetInsertRowsTemplate();

		/// <summary>
		/// Возвращает шаблон для вставки записей потоком
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetInsertRowsStreamTemplate();

		/// <summary>
		/// Возвращает шаблон для добавления колонки
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetAddColumnTemplate();

		/// <summary>
		/// Возвращает шаблон удаления колонки
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetDropColumnTemplate();

		/// <summary>
		/// Returns template for delete row script
		/// </summary>
		/// <returns>Delete row template</returns>
		string GetDeleteRowsTemplate();
	}
}