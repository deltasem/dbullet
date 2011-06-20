//-----------------------------------------------------------------------
// <copyright file="Bullet.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using dbullet.core.dbo;
using dbullet.core.dbs;
using dbullet.core.engine;
using dbullet.core.tools;

namespace dbullet.core
{
	/// <summary>
	/// Операции, которые можно произвести над БД
	/// </summary>
	public abstract class Bullet : IDatabaseStrategy
	{
		/// <summary>
		/// Парсер даты
		/// </summary>
		/// <param name="d">дата строковая</param>
		/// <returns>дата</returns>
		public static object DateModulator(string d)
		{
			try
			{
				return DateTime.Parse(d);
			}
			catch (Exception)
			{
				return DBNull.Value;
			}
		}

		/// <summary>
		/// Обновление
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// Отмена обновления
		/// </summary>
		public abstract void Downgrade();

		/// <summary>
		/// Создаёт таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		public void CreateTable(Table table)
		{
			Executor.DatabaseStrategy.CreateTable(table);
		}

		/// <summary>
		/// Существует ли таблица
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <returns>true - если существует, иначе false</returns>
		public bool IsTableExist(string tableName)
		{
			return Executor.DatabaseStrategy.IsTableExist(tableName);
		}

		/// <summary>
		/// Создаёт индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void CreateIndex(Index index)
		{
			Executor.DatabaseStrategy.CreateIndex(index);
		}

		/// <summary>
		/// Удаляет таблицу
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		public void DropTable(string tableName)
		{
			Executor.DatabaseStrategy.DropTable(tableName);
		}

		/// <summary>
		/// Удаляет индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void DropIndex(Index index)
		{
			Executor.DatabaseStrategy.DropIndex(index);
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		public void AddColumn(Table table, Column column)
		{
			Executor.DatabaseStrategy.AddColumn(table, column);
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		/// <param name="dbType">Тип колонки</param>
		public void AddColumn(string table, string column, DbType dbType)
		{
			AddColumn(new Table(table), new Column(column, dbType));
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		/// <param name="columnType">Тип колонки</param>
		public void AddColumn(string table, string column, ColumnType columnType)
		{
			AddColumn(new Table(table), new Column(column, columnType));
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		/// <param name="dbType">Тип колонки</param>
		/// <param name="defaultValue">Дефалт</param>
		public void AddColumn(string table, string column, DbType dbType, string defaultValue)
		{
			AddColumn(new Table(table), new Column(column, dbType) { Constraint = new ValueDefault(string.Format("DF_{0}_{1}", table, column).ToUpper(), defaultValue) });
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		/// <param name="columnType">Тип колонки</param>
		/// <param name="defaultValue">Дефалт</param>
		public void AddColumn(string table, string column, ColumnType columnType, string defaultValue)
		{
			AddColumn(new Table(table), new Column(column, columnType) { Constraint = new ValueDefault(string.Format("DF_{0}_{1}", table, column).ToUpper(), defaultValue) });
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		/// <param name="dbType">Тип колонки</param>
		/// <param name="standartDefaultType">Дефалт</param>
		public void AddColumn(string table, string column, DbType dbType, StandartDefaultType standartDefaultType)
		{
			AddColumn(new Table(table), new Column(column, dbType) { Constraint = new StandartDefault(string.Format("DF_{0}_{1}", table, column).ToUpper(), standartDefaultType) });
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		/// <param name="columnType">Тип колонки</param>
		/// <param name="standartDefaultType">Дефалт</param>
		public void AddColumn(string table, string column, ColumnType columnType, StandartDefaultType standartDefaultType)
		{
			AddColumn(new Table(table), new Column(column, columnType) { Constraint = new StandartDefault(string.Format("DF_{0}_{1}", table, column).ToUpper(), standartDefaultType) });
		}

		/// <summary>
		/// Создать внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void CreateForeignKey(ForeignKey foreignKey)
		{
			Executor.DatabaseStrategy.CreateForeignKey(foreignKey);
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			Executor.DatabaseStrategy.DropForeignKey(foreignKey);
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="name">Имя ключа</param>
		/// <param name="tableName">Таблица с ключем</param>
		public void DropForeignKey(string name, string tableName)
		{
			DropForeignKey(new ForeignKey(name, tableName, String.Empty, String.Empty, String.Empty));
		}

		/// <summary>
		/// Добавляет записи в таблицу
		/// </summary>
		/// <param name="table">Таблицы</param>
		/// <param name="rows">Записи</param>
		public void InsertRows(string table, params object[] rows)
		{
			Executor.DatabaseStrategy.InsertRows(table, rows);
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
			Executor.DatabaseStrategy.LoadCsv(tableName, stream, modulator, csvQuotesType);
		}

		/// <summary>
		/// Загружает поток в базу. Данные в формате CSV
		/// </summary>
		/// <param name="tableName">Таблица для загрузки</param>
		/// <param name="resource">Имя ресурса</param>
		/// <param name="modulator">Преобразования</param>
		/// <param name="csvQuotesType">Тип кавычек CSV</param>
		public void LoadCsv(string tableName, string resource, Dictionary<string, Func<string, object>> modulator, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes)
		{
			using (var stream = GetType().Assembly.GetManifestResourceStream(resource))
			{
				LoadCsv(tableName, new StreamReader(stream, Encoding.Default), modulator, csvQuotesType);
			}
		}
	}
}
