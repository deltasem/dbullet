//-----------------------------------------------------------------------
// <copyright file="UnsuportedDbTypeException.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;

namespace dbullet.core.exception
{
	/// <summary>
	/// Не поддерживаемый тип
	/// </summary>
	public class UnsuportedDbTypeException : ApplicationException
	{
		/// <summary>
		/// Не поддерживаемый тип
		/// </summary>
		/// <param name="type">Тип</param>
		public UnsuportedDbTypeException(DbType type)
			: base(string.Format("Тип столбца [{0}] на данный момент не поддерживается\nЕсли он Вам нужен - сообщите об этом: https://github.com/deltasem/dbullet/issues", type.ToString()))
		{
		}

		/// <summary>
		/// Не поддерживаемый тип
		/// </summary>
		/// <param name="type">Тип</param>
		public UnsuportedDbTypeException(SqlDbType type)
			: base(string.Format("Тип столбца [{0}] на данный момент не поддерживается\nЕсли он Вам нужен - сообщите об этом: https://github.com/deltasem/dbullet/issues", type.ToString()))
		{
		}
	}
}
