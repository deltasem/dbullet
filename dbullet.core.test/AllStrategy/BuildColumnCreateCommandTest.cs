using NUnit.Framework;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// ����� BuildColumnCreateCommand
	/// </summary>
	[TestFixture]
	public abstract class BuildColumnCreateCommandTest
	{
		/// <summary>
		/// ���� ������ ��� ������� - ��������� ������x
		/// </summary>
		[Test]
		public abstract void StringWithoutSize();

		/// <summary>
		/// ������� �������-������
		/// </summary>
		[Test]
		public abstract void StringDatatype();

		/// <summary>
		/// ������� �������-�����
		/// </summary>
		[Test]
		public abstract void NumericDatatype();

		/// <summary>
		/// ������� ������� ����� �����
		/// </summary>
		[Test]
		public abstract void IntDataType();

		/// <summary>
		/// ������� ������� ������� ���
		/// </summary>
		[Test]
		public abstract void BooleanDataType();

		/// <summary>
		/// ������� ������� ����
		/// </summary>
		[Test]
		public abstract void DateDataType();

		/// <summary>
		/// ������� ������� ����
		/// </summary>
		[Test]
		public abstract void DateTimeDataType();

		/// <summary>
		/// ������� ������� GUID
		/// </summary>
		[Test]
		public abstract void GuidDataType();

		/// <summary>
		/// ������� ������� GUID
		/// </summary>
		[Test]
		public abstract void XmlDataType();

		/// <summary>
		/// Binary
		/// </summary>
		[Test]
		public abstract void BinaryDataTypeWithoutSize();

		/// <summary>
		/// ������� �������-������
		/// </summary>
		[Test]
		public abstract void BinaryDatatype();

		/// <summary>
		/// ������� �������-������
		/// </summary>
		[Test]
		public abstract void VarBinaryDatatype();
	}
}