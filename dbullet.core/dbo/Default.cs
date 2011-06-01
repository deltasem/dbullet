//-----------------------------------------------------------------------
// <copyright file="Default.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Дефалт для поля
	/// </summary>
	public class Default : Constraint
	{
		/// <summary>
		/// Значение
		/// </summary>
		private readonly string value;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="value">Значение</param>
		public Default(string value) : base(string.Empty)
		{
			this.value = value;
		}

		/// <summary>
		/// Значение
		/// </summary>
		public string Value
		{
			get { return value; }
		}
	}
}