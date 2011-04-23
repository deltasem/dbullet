namespace dbullet.core.test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	/// <summary>
	/// Клас для ассертов
	/// </summary>
	public static class AssertHelpers
	{
		/// <summary>
		/// Метод должен генерить исключения и заданное сообщение
		/// </summary>
		/// <typeparam name="T">Тип исключения</typeparam>
		/// <param name="action">Метод</param>
		/// <param name="expectedMessage">Сообщение</param>
		public static void Throws<T>(Action action, string expectedMessage) where T : Exception
		{
			try
			{
				action.Invoke();
				Assert.Fail("Exception of type {0} should be thrown.", typeof(T));
			}
			catch (T exc)
			{
				Assert.AreEqual(expectedMessage, exc.Message);
			}
		}

		/// <summary>
		/// Ожидается исключение
		/// </summary>
		/// <typeparam name="T">Тип исключения</typeparam>
		/// <param name="action">Метод</param>
		public static void Throws<T>(Action action) where T : Exception
		{
			try
			{
				action.Invoke();
				Assert.Fail("Exception of type {0} should be thrown.", typeof(T));
			}
			catch (T)
			{
			}
		}
	}
}
