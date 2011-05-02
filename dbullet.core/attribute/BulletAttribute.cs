using System;

namespace dbullet.core.attribute
{
	/// <summary>
	/// Все булеты должны быть помечены этим аттрибутом
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class BulletAttribute : Attribute
	{
		/// <summary>
		/// Ревизия булета
		/// </summary>
		private readonly int revision;

		/// <summary>
		/// Этим аттрибутом нужно помечать булеты
		/// </summary>
		/// <param name="revision">Ревизия булета</param>
		public BulletAttribute(int revision)
		{
			this.revision = revision;
		}

		/// <summary>
		/// Ревизия булета
		/// </summary>
		public int Revision
		{
			get { return revision; }
		}
	}
}
