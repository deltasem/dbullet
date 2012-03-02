//-----------------------------------------------------------------------
// <copyright file="CreateForeignKeyTest.cs" company="delta" author="delta">
//     Copyright (c) 2012. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using dbullet.core.dbo;
using dbullet.core.dbs;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace dbullet.core.test.AllStrategy
{
	/// <summary>
	/// ����� �������� ������� ������
	/// </summary>
	[TestFixture]
	public abstract class CreateForeignKeyTest : TestBase
	{
		/// <summary>
		/// �������� �������� �����
		/// </summary>
		protected abstract string RegularCreateFKCommand { get; }

		/// <summary>
		/// �������� �������� ����� ��� ����������������
		/// </summary>
		protected abstract string DefaultNameCommand { get; }

		/// <summary>
		/// �������� �������� ����� ��������� ��������
		/// </summary>
		protected abstract string CascadeCommand { get; }

		/// <summary>
		/// �������� �������� ����� ��� ��������
		/// </summary>
		protected abstract string NoActionCommand { get; }

		/// <summary>
		/// �������� �������� �����
		/// </summary>
		[Test]
		public void RegularCreateFK()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			command.VerifySet(x => x.CommandText = RegularCreateFKCommand);
		}

		/// <summary>
		/// �������� �������� ����� ��� ����������������
		/// </summary>
		[Test]
		public void DefaultName()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2"));
			command.VerifySet(x => x.CommandText = DefaultNameCommand);
		}

		/// <summary>
		/// �������� �������� ����� ��� ����������������
		/// </summary>
		[Test]
		public void Cascade()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.Cascade));
			command.VerifySet(x => x.CommandText = CascadeCommand);
		}

		/// <summary>
		/// �������� �������� ����� ��� ����������������
		/// </summary>
		[Test]
		public void NoAction()
		{
			strategy = ObjectFactory.GetInstance<IDatabaseStrategy>();
			command.SetupSet(x => x.CommandText = It.IsAny<string>()).Verifiable();
			strategy.CreateForeignKey(new ForeignKey("FK_TEST", "TABLE1", "ID_TABLE1", "TABLE2", "ID_TABLE2", ForeignAction.NoAction));
			command.VerifySet(x => x.CommandText = NoActionCommand);
		}
	}
}