//-----------------------------------------------------------------------
// <copyright file="ForeignKey.cs" company="SofTrust" author="SKiryshin">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using dbullet.core.dbo;

namespace dbullet.core.lambda
{
	/// <summary>
	/// Внешний ключ
	/// </summary>
	/// <typeparam name="T1">Исходная таблица</typeparam>
	/// <typeparam name="T2">Ссылаемая таблица</typeparam>
	public class ForeignKey<T1, T2> : ForeignKey
	{
		/// <summary>
		/// Фиксированное имя
		/// </summary>
		private bool fixedName = false;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="deleteAction">Действие при удалении</param>
		public ForeignKey(ForeignAction deleteAction = ForeignAction.SetNull) :
			base(typeof(T1).Name, string.Empty, typeof(T2).Name, string.Empty, deleteAction)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название ключа</param>
		/// <param name="deleteAction">Действие при удалении</param>
		public ForeignKey(string name, ForeignAction deleteAction = ForeignAction.SetNull) :
			base(name, typeof(T1).Name, string.Empty, typeof(T2).Name, string.Empty, deleteAction)
		{
			fixedName = true;
		}

		/// <summary>
		/// Связывает таблицы
		/// </summary>
		/// <typeparam name="P">Тип поля связки</typeparam>
		/// <param name="srcColumn">Исходная таблица</param>
		/// <param name="refColumn">Ссылаемая таблица</param>
		/// <returns>Внешний ключ</returns>
		public ForeignKey<T1, T2> Link<P>(Expression<Func<T1, P>> srcColumn, Expression<Func<T2, P>> refColumn)
		{
			var srcMember = srcColumn.Body as MemberExpression;
			var refMember = refColumn.Body as MemberExpression;
			SrcColumnName = srcMember.Member.Name;
			RefColumnName = refMember.Member.Name;

			if (!fixedName)
			{
				Name = string.Format("FK_{0}_{1}_{2}", SrcTableName, RefTableName, SrcColumnName);
			}

			return this;
		}
	}
}