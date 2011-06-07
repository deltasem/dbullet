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
		private static readonly Regex parserDoubleQuotes = new Regex(@"(?:^|,|;)(\""(?:[^\""]+|\""\"")*\""|[^,;]*)", RegexOptions.CultureInvariant | RegexOptions.Compiled);

		/// <summary>
		/// Регулярное выражение для разбора CSV
		/// </summary>
		private static readonly Regex parserSingleQuotes = new Regex(@"(?:^|,|;)('(?:[^']+|'')*'|[^,;]*)", RegexOptions.CultureInvariant | RegexOptions.Compiled);

		/// <summary>
		/// Разбирает CSV строку
		/// </summary>
		/// <param name="text">Текст в формате CSV</param>
		/// <param name="quotesType">Типы ковычек</param>
		/// <returns>Массив значений</returns>
		public static string[] Parse(string text, CsvQutestType quotesType = CsvQutestType.DoubleQuotes)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException();
			}

			List<string> str = new List<string>();

			Match matchResults;
			switch (quotesType)
			{
				case CsvQutestType.DoubleQuotes:
					matchResults = parserDoubleQuotes.Match(text);
					break;
				case CsvQutestType.SingleQuotes:
					matchResults = parserSingleQuotes.Match(text);
					break;
				default:
					throw new ArgumentOutOfRangeException("quotesType");
			}

			while (matchResults.Success)
			{
				if (!string.IsNullOrEmpty(matchResults.Value))
				{
					if (quotesType == CsvQutestType.DoubleQuotes && matchResults.Groups[1].Value.StartsWith("\""))
					{
						str.Add(matchResults.Groups[1].Value.Substring(1, matchResults.Groups[1].Value.Length - 2).Replace("\"\"", "\""));
					}
					else if (quotesType == CsvQutestType.SingleQuotes && matchResults.Groups[1].Value.StartsWith("'"))
					{
						str.Add(matchResults.Groups[1].Value.Substring(1, matchResults.Groups[1].Value.Length - 2).Replace("''", "'"));
					}
					else
					{
						str.Add(matchResults.Groups[1].Value);
					}
				}

				matchResults = matchResults.NextMatch();
			}

			return str.ToArray();
		}
	}
}