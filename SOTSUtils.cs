using ReLogic.Utilities;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace SOTS
{
	public static class SOTSUtils
	{
		public static SlotId PlaySound(int type, float posX, float posY, int style, float volume, float pitch)
        {
			SoundStyle style = new SoundStyle();
			return Terraria.Audio.SoundEngine.PlaySound(style, Vector2(posX, posY));
        }
		public static string GetPath<T>()
		{
			return GetPath(typeof(T));
		}
		public static string GetPath<T>(string extra)
		{
			return GetPath(typeof(T), extra);
		}
		public static string GetPath(this object o)
		{
			return GetPath(o.GetType());
		}
		public static string GetPath(this object o, string extra)
		{
			return GetPath(o.GetType(), extra);
		}
		public static string GetPath(Type t)
		{
			return t.Namespace.Replace('.', '/') + "/" + t.Name;
		}
		public static string GetPath(Type t, string extra)
		{
			return GetPath(t) + extra;
		}
	}
}