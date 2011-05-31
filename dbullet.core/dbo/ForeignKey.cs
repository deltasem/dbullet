//-----------------------------------------------------------------------
// <copyright file="ForeignKey.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace dbullet.core.dbo
{
	/// <summary>
	/// ������� ����
	/// </summary>
	public class ForeignKey : DatabaseObjectBase
	{
		/// <summary>
		/// �������� �������
		/// </summary>
		private readonly string srcTableName;

		/// <summary>
		/// ������� �������� ������� ��� �����
		/// </summary>
		private readonly string srcColumnName;

		/// <summary>
		/// ��������� �������
		/// </summary>
		private readonly string refTableName;

		/// <summary>
		/// ������� ��������� ������� ��� �����
		/// </summary>
		private readonly string refColumnName;

		/// <summary>
		/// �������� ��� ��������
		/// </summary>
		private readonly ForeignAction deleteAction;

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="srcTableName">�������� �������</param>
		/// <param name="srcColumnName">������� �������� ������� ��� �����</param>
		/// <param name="refTableName">��������� �������</param>
		/// <param name="refColumnName">������� ��������� ������� ��� �����</param>
		/// <param name="deleteAction">�������� ��� ��������</param>
		public ForeignKey(string srcTableName, string srcColumnName, string refTableName, string refColumnName, ForeignAction deleteAction = ForeignAction.SetNull)
			: this(string.Format("FK_{0}_{1}", srcTableName, refTableName), srcTableName, srcColumnName, refTableName, refColumnName, deleteAction)
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="name">�������� �����</param>
		/// <param name="srcTableName">�������� �������</param>
		/// <param name="srcColumnName">������� �������� ������� ��� �����</param>
		/// <param name="refTableName">��������� �������</param>
		/// <param name="refColumnName">������� ��������� ������� ��� �����</param>
		/// <param name="deleteAction">�������� ��� ��������</param>
		public ForeignKey(string name, string srcTableName, string srcColumnName, string refTableName, string refColumnName, ForeignAction deleteAction = ForeignAction.SetNull)
			: base(name)
		{
			this.srcTableName = srcTableName;
			this.srcColumnName = srcColumnName;
			this.refTableName = refTableName;
			this.refColumnName = refColumnName;
			this.deleteAction = deleteAction;
		}

		/// <summary>
		/// �������� �������
		/// </summary>
		public string SrcTableName
		{
			get { return srcTableName; }
		}

		/// <summary>
		/// ������� �������� ������� ��� �����
		/// </summary>
		public string SrcColumnName
		{
			get { return srcColumnName; }
		}

		/// <summary>
		/// ��������� �������
		/// </summary>
		public string RefTableName
		{
			get { return refTableName; }
		}

		/// <summary>
		/// ������� ��������� ������� ��� �����
		/// </summary>
		public string RefColumnName
		{
			get { return refColumnName; }
		}

		/// <summary>
		/// �������� ��� ��������
		/// </summary>
		public ForeignAction DeleteAction
		{
			get { return deleteAction; }
		}
	}
}