//-----------------------------------------------------------------------
// <copyright file="TestDataParametrCollection.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace dbullet.core.test.tools
{
	/// <summary>
	/// Коллекция параметров
	/// </summary>
	public class TestDataParametrCollection : List<TestParametr>, IDataParameterCollection
	{
		/// <summary>
		/// Gets or sets the parameter at the specified index.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Object"/> at the specified index.
		/// </returns>
		/// <param name="parameterName">The name of the parameter to retrieve. </param><filterpriority>2</filterpriority>
		public object this[string parameterName]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether a parameter in the collection has the specified name.
		/// </summary>
		/// <returns>
		/// true if the collection contains the parameter; otherwise, false.
		/// </returns>
		/// <param name="parameterName">The name of the parameter. </param><filterpriority>2</filterpriority>
		public bool Contains(string parameterName)
		{
			return this.FirstOrDefault(p => p.ParameterName == parameterName) != null;
		}

		/// <summary>
		/// Gets the location of the <see cref="T:System.Data.IDataParameter"/> within the collection.
		/// </summary>
		/// <returns>
		/// The zero-based location of the <see cref="T:System.Data.IDataParameter"/> within the collection.
		/// </returns>
		/// <param name="parameterName">The name of the parameter. </param><filterpriority>2</filterpriority>
		public int IndexOf(string parameterName)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this[i].ParameterName == parameterName)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Removes the <see cref="T:System.Data.IDataParameter"/> from the collection.
		/// </summary>
		/// <param name="parameterName">The name of the parameter. </param><filterpriority>2</filterpriority>
		public void RemoveAt(string parameterName)
		{
		}
	}
}