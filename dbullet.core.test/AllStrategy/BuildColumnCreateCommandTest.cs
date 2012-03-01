using NUnit.Framework;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// Тесты BuildColumnCreateCommand
	/// </summary>
	[TestFixture]
	public abstract class BuildColumnCreateCommandTest
	{
		/// <summary>
		/// Если строка без размера - сгенерить ошибкуx
		/// </summary>
		[Test]
		public abstract void StringWithoutSize();

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[Test]
		public abstract void StringDatatype();

		/// <summary>
		/// Обычная колонка-число
		/// </summary>
		[Test]
		public abstract void NumericDatatype();

		/// <summary>
		/// Обычная колонка целое число
		/// </summary>
		[Test]
		public abstract void IntDataType();

		/// <summary>
		/// Обычная колонка булевый тип
		/// </summary>
		[Test]
		public abstract void BooleanDataType();

		/// <summary>
		/// Обычная колонка дата
		/// </summary>
		[Test]
		public abstract void DateDataType();

		/// <summary>
		/// Обычная колонка дата
		/// </summary>
		[Test]
		public abstract void DateTimeDataType();

		/// <summary>
		/// Обычная колонка GUID
		/// </summary>
		[Test]
		public abstract void GuidDataType();

		/// <summary>
		/// Обычная колонка GUID
		/// </summary>
		[Test]
		public abstract void XmlDataType();

		/// <summary>
		/// Binary
		/// </summary>
		[Test]
		public abstract void BinaryDataTypeWithoutSize();

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[Test]
		public abstract void BinaryDatatype();

		/// <summary>
		/// Обычная колонка-строка
		/// </summary>
		[Test]
		public abstract void VarBinaryDatatype();
	}
}