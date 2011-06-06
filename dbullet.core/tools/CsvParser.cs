//-----------------------------------------------------------------------
// <copyright file="CsvParser.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace dbullet.core.tools
{
	/// <summary>
	/// Парсер CSV строки
	/// </summary>
	public static class CsvParser
	{
		/// <summary>
		/// Регулярное выражение для разбора CSV
		/// </summary>
		private static Regex parserRegExp = new Regex(@"""[^""\r\n]*""|'[^'\r\n]*'|[^,\r\n]*", RegexOptions.CultureInvariant | RegexOptions.Compiled);

		/// <summary>
		/// Разбирает CSV строку
		/// </summary>
		/// <param name="text">Текст в формате CSV</param>
		/// <returns>Массив значений</returns>
		public static string[] Parse(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException();
			}

			List<string> str = new List<string>();

			Match matchResults = parserRegExp.Match(text);
			while (matchResults.Success)
			{
				str.Add(matchResults.Value);
				matchResults = matchResults.NextMatch();
			}

			return str.ToArray();
		}
	}
}