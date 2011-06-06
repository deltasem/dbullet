//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Reflection;
using dbullet.core.engine;

namespace dbullet.executor
{
	/// <summary>
	/// Стартовый клсаа
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Стартовый метод
		/// </summary>
		/// <param name="args">Параметры</param>
		public static void Main(string[] args)
		{
			Assembly asm = Assembly.LoadFile(args[0]);
			if (args[3] == "downgrade")
			{
				Executor.ExecuteBack(asm, args[1], (SupportedStrategy)Enum.Parse(typeof(SupportedStrategy), args[2]), int.Parse(args[4]));
			}
			else
			{
				Executor.Execute(asm, args[1], (SupportedStrategy)Enum.Parse(typeof(SupportedStrategy), args[2]));
			}
		}
	}
}
