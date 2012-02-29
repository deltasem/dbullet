//-----------------------------------------------------------------------
// <copyright file="OracleTemplateManager.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;
using dbullet.core.engine.common;
using dbullet.core.exception;

namespace dbullet.core.engine.Oracle
{
	/// <summary>
	/// Менеджер оракловых темплейтов
	/// </summary>
	public class OracleTemplateManager : TemplateManagerBase
	{
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
	}
}