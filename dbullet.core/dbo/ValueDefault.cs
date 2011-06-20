//-----------------------------------------------------------------------
// <copyright file="ValueDefault.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Дефалт-значение
	/// </summary>
	public class ValueDefault : Default
	{
		/// <summary>
		/// Значение
		/// </summary>
		private readonly string value;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название дефалта</param>
		/// <param name="value">Значение</param>
		public ValueDefault(string name, string value)
			: base(name)
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