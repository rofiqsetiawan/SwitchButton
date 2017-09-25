// Created by Rofiq Setiawan (rofiqsetiawan@gmail.com)

using Android.Graphics;

namespace Lib.KingJA.SwitchButton
{
	internal static class ExtensionMethod
	{
		/// <summary>Convert int to Android Color</summary>
		/// <returns>The Android color.</returns>
		/// <param name="color">Color.</param>
		public static Color ToColor(this int color)
		{
			var alpha = (byte)(color >> 24);
			var red = (byte)(color >> 16);
			var green = (byte)(color >> 8);
			var blue = (byte)(color >> 0);
			return Color.Argb(alpha, red, green, blue);
		}
	}
}
