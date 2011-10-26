//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Reflection;
using dbullet.core.engine;
using NLog;

namespace dbullet.executor
{
	/// <summary>
	/// Стартовый клсаа
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Logger
		/// </summary>
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Стартовый метод
		/// </summary>
		/// <param name="args">Параметры</param>
		public static void Main(string[] args)
		{
			NLog.Config.SimpleConfigurator.ConfigureForConsoleLogging();
			Assembly asm = Assembly.LoadFile(args[0]);
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
			Console.WriteLine("Press 1 to upgrade or press 2 to downgrade");
			var input = Console.ReadKey();

			Executor.Initialize(args[1], (SupportedStrategy)Enum.Parse(typeof(SupportedStrategy), args[2]), asm);

			if (input.KeyChar == '2')
			{
				Executor.ExecuteBack(asm, int.Parse(args[3]));
			}
			else if (input.KeyChar == '1')
			{
				Executor.Execute(asm);
			}

			Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}

		/// <summary>
		/// Резёлв сборки
		/// </summary>
		/// <param name="sender">Сендер</param>
		/// <param name="args">Аргументы</param>
		/// <returns>Сборка</returns>
		private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
		{
			var path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(args.RequestingAssembly.Location), args.Name.Split(',')[0] + ".dll");
			return Assembly.LoadFile(path);			
		}
	}
}
