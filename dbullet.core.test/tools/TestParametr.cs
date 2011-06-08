//-----------------------------------------------------------------------
// <copyright file="TestParametr.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Data;

namespace dbullet.core.test.tools
{
	/// <summary>
	/// Тестовый параметр SQL запроса
	/// </summary>
	public class TestParametr : IDbDataParameter
	{
		/// <summary>
		/// Gets or sets the <see cref="T:System.Data.DbType"/> of the parameter.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Data.DbType"/> values. The default is <see cref="F:System.Data.DbType.String"/>.
		/// </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property was not set to a valid <see cref="T:System.Data.DbType"/>. </exception><filterpriority>2</filterpriority>
		public DbType DbType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Data.ParameterDirection"/> values. The default is Input.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">The property was not set to one of the valid <see cref="T:System.Data.ParameterDirection"/> values. </exception><filterpriority>2</filterpriority>
		public ParameterDirection Direction { get; set; }

		/// <summary>
		/// Gets a value indicating whether the parameter accepts null values.
		/// </summary>
		/// <returns>
		/// true if null values are accepted; otherwise, false. The default is false.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public bool IsNullable { get; private set; }

		/// <summary>
		/// Gets or sets the name of the <see cref="T:System.Data.IDataParameter"/>.
		/// </summary>
		/// <returns>
		/// The name of the <see cref="T:System.Data.IDataParameter"/>. The default is an empty string.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public string ParameterName { get; set; }

		/// <summary>
		/// Gets or sets the name of the source column that is mapped to the <see cref="T:System.Data.DataSet"/> and used for loading or returning the <see cref="P:System.Data.IDataParameter.Value"/>.
		/// </summary>
		/// <returns>
		/// The name of the source column that is mapped to the <see cref="T:System.Data.DataSet"/>. The default is an empty string.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public string SourceColumn { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="T:System.Data.DataRowVersion"/> to use when loading <see cref="P:System.Data.IDataParameter.Value"/>.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Data.DataRowVersion"/> values. The default is Current.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">The property was not set one of the <see cref="T:System.Data.DataRowVersion"/> values. </exception><filterpriority>2</filterpriority>
		public DataRowVersion SourceVersion { get; set; }

		/// <summary>
		/// Gets or sets the value of the parameter.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Object"/> that is the value of the parameter. The default value is null.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public object Value { get; set; }

		/// <summary>
		/// Indicates the precision of numeric parameters.
		/// </summary>
		/// <returns>
		/// The maximum number of digits used to represent the Value property of a data provider Parameter object. The default value is 0, which indicates that a data provider sets the precision for Value.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public byte Precision { get; set; }

		/// <summary>
		/// Indicates the scale of numeric parameters.
		/// </summary>
		/// <returns>
		/// The number of decimal places to which <see cref="T:System.Data.OleDb.OleDbParameter.Value"/> is resolved. The default is 0.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public byte Scale { get; set; }

		/// <summary>
		/// The size of the parameter.
		/// </summary>
		/// <returns>
		/// The maximum size, in bytes, of the data within the column. The default value is inferred from the the parameter value.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public int Size { get; set; }
	}
}