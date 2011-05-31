//-----------------------------------------------------------------------
// <copyright file="ITemplateManager.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.engine
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
		/// Возвращает шаблон для создания индекса
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetCreateIndexTemplate();

		/// <summary>
		/// Возвращает шаблон для удаления таблицы
		/// </summary>
		/// <returns>Шаблон</returns>
		string GetDropTableTemplate();
	}
}