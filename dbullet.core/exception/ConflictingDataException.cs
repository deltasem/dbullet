//-----------------------------------------------------------------------
// <copyright file="ConflictingDataException.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace dbullet.core.exception
{
	/// <summary>
	/// Конфликтующие данные
	/// </summary>
	public class ConflictingDataException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">A message that describes the error. </param>
		public ConflictingDataException(string message) : base(message)
		{
		}
	}
}