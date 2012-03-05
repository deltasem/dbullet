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
using StructureMap;

namespace dbullet.core
{
	/// <summary>
	/// Операции, которые можно произвести над БД
	/// </summary>
	public abstract class Bullet : IDatabaseStrategy
	{
		/// <summary>
		/// Стратегия
		/// </summary>
		public SupportedStrategy Strategy
		{
			get { return SupportedStrategy.Any; }
		}

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
			ObjectFactory.GetInstance<IDatabaseStrategy>().CreateTable(table);
		}

		/// <summary>
		/// Добавляет записи в таблицу
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="identity">true - отключать идентити спецификацию</param>
		/// <param name="rows">Список записей</param>
		public void InsertRows(string table, bool identity = false, params object[] rows)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().InsertRows(table, identity, rows);
		}

		/// <summary>
		/// Существует ли таблица
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <returns>true - если существует, иначе false</returns>
		public bool IsTableExist(string tableName)
		{
			return ObjectFactory.GetInstance<IDatabaseStrategy>().IsTableExist(tableName);
		}

		/// <summary>
		/// Существует ли заданная колонка в таблице
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		/// <param name="columnName">Название колонки</param>
		/// <returns>true - если существует, иначе false</returns>
		public bool IsColumnExists(string tableName, string columnName)
		{
			return ObjectFactory.GetInstance<IDatabaseStrategy>().IsColumnExists(tableName, columnName);
		}

		/// <summary>
		/// Создаёт индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void CreateIndex(Index index)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().CreateIndex(index);
		}

		/// <summary>
		/// Удаляет таблицу
		/// </summary>
		/// <param name="tableName">Название таблицы</param>
		public void DropTable(string tableName)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().DropTable(tableName);
		}

		/// <summary>
		/// Удаляет таблицы
		/// </summary>
		/// <typeparam name="T">ДТО</typeparam>
		public void DropTable<T>()
		{
			DropTable(typeof(T).Name);
		}
		
		/// <summary>
		/// Удаляет индекс
		/// </summary>
		/// <param name="index">Индеес</param>
		public void DropIndex(Index index)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().DropIndex(index);
		}

		/// <summary>
		/// Добавляет колонку
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		public void AddColumn(Table table, Column column)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().AddColumn(table, column);
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
		/// <param name="dbType">Тип колонки</param>
		public void AddColumn(string table, string column, SqlDbType dbType)
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
		/// <param name="dbType">Тип колонки</param>
		/// <param name="defaultValue">Дефалт</param>
		public void AddColumn(string table, string column, SqlDbType dbType, string defaultValue)
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
		/// <param name="dbType">Тип колонки</param>
		/// <param name="standartDefaultType">Дефалт</param>
		public void AddColumn(string table, string column, SqlDbType dbType, StandartDefaultType standartDefaultType)
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
			ObjectFactory.GetInstance<IDatabaseStrategy>().CreateForeignKey(foreignKey);
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="foreignKey">Внешний ключ</param>
		public void DropForeignKey(ForeignKey foreignKey)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().DropForeignKey(foreignKey);
		}

		/// <summary>
		/// Удалить внешний ключ
		/// </summary>
		/// <param name="name">Имя ключа</param>
		/// <param name="tableName">Таблица с ключем</param>
		public void DropForeignKey(string name, string tableName)
		{
			DropForeignKey(new ForeignKey(name, tableName, string.Empty, string.Empty, string.Empty));
		}

		/// <summary>
		/// Загружает поток в базу. Данные в формате CSV
		/// </summary>
		/// <param name="tableName">Таблица для загрузки</param>
		/// <param name="stream">Входной поток</param>
		/// <param name="modulator">Преобразования</param>
		/// <param name="csvQuotesType">Тип кавычек CSV</param>
		/// <param name="identity">true - отключать идентити спецификацию</param>
		public void LoadCsv(string tableName, StreamReader stream, Dictionary<string, Func<string, object>> modulator, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes, bool identity = false)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().LoadCsv(tableName, stream, modulator, csvQuotesType, identity);
		}

		/// <summary>
		/// Удаляет колонку из таблицы
		/// </summary>
		/// <param name="table">Таблица</param>
		/// <param name="column">Колонка</param>
		public void DropColumn(string table, string column)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().DropColumn(table, column);
		}

		/// <summary>
		/// Remove rows from table
		/// </summary>
		/// <param name="table">Table</param>
		/// <param name="eq">Equality conditions</param>
		/// <example>DeleteRows("sometable", new { ID = 1 }, new { ID = 2 })</example>
		public void DeleteRows(string table, params object[] eq)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().DeleteRows(table, eq);
		}

		/// <summary>
		/// Remove from the table each row, using the equality condition
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="stream">Stream with CSV data</param>
		/// <param name="keyColumn">Equality column in CSV file</param>
		/// <param name="modulator">Converting keyColumn value to some types</param>
		/// <param name="csvQuotesType">Quotes type</param>
		/// <example>UnloadCsv("someTable", stream, "ID", x => (int)x)</example>
		public void UnloadCsv(string table, StreamReader stream, string keyColumn, Func<string, object> modulator = null, CsvQuotesType csvQuotesType = CsvQuotesType.DoubleQuotes)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().UnloadCsv(table, stream, keyColumn, modulator, csvQuotesType);
		}

		/// <summary>
		/// Выполняет скрипт
		/// </summary>
		/// <param name="strategy">Стратегия</param>
		/// <param name="query">Запрос</param>
		public void ExecuteQuery(SupportedStrategy strategy, string query)
		{
			ObjectFactory.GetInstance<IDatabaseStrategy>().ExecuteQuery(strategy, query);
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
