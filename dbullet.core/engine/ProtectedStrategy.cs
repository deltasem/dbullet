//-----------------------------------------------------------------------
// <copyright file="ProtectedStrategy.cs" company="delta" created="18.09.2011">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.tools;
using NLog;

namespace dbullet.core.engine
{
	/// <summary>
	/// Делегирует вызов методов, но перехватывает исключения
	/// </summary>
	public class ProtectedStrategy : IDatabaseStrategy
	{
		/// <summary>
		/// Стратегия
		/// </summary>
		private readonly IDatabaseStrategy strategy;

		/// <summary>
		/// Логгер
		/// </summary>
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		///  Конструктор
		/// </summary>
		/// <param name="strategy">Стратегия</param>
		public ProtectedStrategy(IDatabaseStrategy strategy)
		{
			this.strategy = strategy;
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		public void AddColumn(Table table, Column column)
		{
			try
			{
				strategy.AddColumn(table, column);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката AddColumn", e);
			}
		}

		/// <summary>
		/// Создать внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void CreateForeignKey(ForeignKey foreignKey)
		{
			try
			{
				strategy.CreateForeignKey(foreignKey);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката CreateForeignKey", e);
			}
		}

		/// <summary>
		/// Создаёт индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void CreateIndex(Index index)
		{
			try
			{
				strategy.CreateIndex(index);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката CreateIndex", e);
			}
		}

		/// <summary>
		/// Создаёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		public void CreateTable(Table table)
		{
			try
			{
				strategy.CreateTable(table);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката CreateTable", e);
			}
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			try
			{
				strategy.DropForeignKey(foreignKey);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката DropForeignKey", e);
			}
		}

		/// <summary>
		/// Удаляет индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void DropIndex(Index index)
		{
			try
			{
				strategy.DropIndex(index);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката DropIndex", e);
			}
		}

		/// <summary>
		/// Удаляет таблицу
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		public void DropTable(string tableName)
		{
			try
			{
				strategy.DropTable(tableName);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката DropTable", e);
			}
		}

		/// <summary>
		/// Добавляет записи в таблицу
		/// </summary>
		/// <param name="table">Таблицы</param>
		/// <param name="rows">Записи</param>
		public void InsertRows(string table, params object[] rows)
		{
			try
			{
				strategy.InsertRows(table, rows);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката InsertRows", e);
			}
		}

		/// <summary>
		/// Существует ли таблица
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <returns>true - если существует, иначе false</returns>
		public bool IsTableExist(string tableName)
		{
			try
			{
				return strategy.IsTableExist(tableName);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката IsTableExist", e);
			}

			return false;
		}

		/// <summary>
		/// Загружает поток в базу. Данные в формате CSV
		/// </summary>
		/// <param name="tableName">Таблица для загрузки</param>
		/// <param name="stream">Входной поток</param>
		/// <param name="modulator">Преобразования</param>
		/// <param name="csvQuotesType">Тип кавычек CSV</param>
		public void LoadCsv(string tableName, StreamReader stream, Dictionary<string, Func<string, object>> modulator, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes)
		{
			try
			{
				strategy.LoadCsv(tableName, stream, modulator, csvQuotesType);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката LoadCsv", e);
			}
		}

		/// <summary>
		/// Удаляет колонку из таблицы
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		public void DropColumn(string table, string column)
		{
			try
			{
				strategy.DropColumn(table, column);
			}
			catch (Exception e)
			{
				logger.WarnException("Ошибка выполнения отката DropColumn", e);
			}
		}
	}
}