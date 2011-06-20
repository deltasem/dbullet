//-----------------------------------------------------------------------
// <copyright file="StandartDefault.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// Системные дефалты
	/// </summary>
	public class StandartDefault : Default
	{
		/// <summary>
		/// Тип дефалта
		/// </summary>
		private readonly StandartDefaultType defaultType;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Название дефалта</param>
		/// <param name="defaultType">Тип дефалта</param>
		public StandartDefault(string name, StandartDefaultType defaultType) : base(name)
		{
			this.defaultType = defaultType;
		}

		/// <summary>
		/// Тип дефалта
		/// </summary>
		public StandartDefaultType DefaultType
		{
			get { return defaultType; }
		}
	}
}