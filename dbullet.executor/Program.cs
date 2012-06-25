//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="delta">
//     Copyright (c) 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using dbullet.core.engine;
using NLog;
using NLog.Targets;

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
			bool silent = false;
			bool update = true;
			if (args.Length > 4 && args[4] == "-f")
			{
				silent = true;
			}

			var colored = new ColoredConsoleTarget();
			colored.Layout = "${message} ${exception:format=ToString,StackTrace}";
			NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(colored);
			Assembly asm = Assembly.LoadFile(args[0]);
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;

			if (!silent)
			{
				logger.Info("Press 1 to upgrade or press 2 to downgrade");
				var input = Console.ReadKey();
				Console.WriteLine();
				if (input.KeyChar == '2')
				{
					update = false;
				}
				else if (input.KeyChar == '1')
				{
					update = true;
				}
			}

			Executor.Initialize(args[1], (SupportedStrategy)Enum.Parse(typeof(SupportedStrategy), args[2]), asm);

			if (!update)
			{
				logger.Info("Старт отката");
				Executor.ExecuteBack(asm, int.Parse(args[3]));
			}
			else
			{
				var regex = new Regex(@"Data Source=[^;]+", RegexOptions.IgnoreCase);
				logger.Info("Поиск обновлений в {0} для {1}", Path.GetFileName(args[0]), regex.Match(args[1]).Value);
				int stopVersion = int.MaxValue;
				if (args.Length > 2)
				{
					int.TryParse(args[3], out stopVersion);
				}

				Executor.Execute(asm, stopVersion);
				logger.Info("Обновление завершено");
			}

			if (!silent)
			{
				logger.Warn("Press any key to exit");
				Console.ReadKey();
			}
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
