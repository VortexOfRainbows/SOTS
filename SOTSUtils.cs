using ReLogic.Utilities;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using SOTS.Common.GlobalNPCs;

namespace SOTS
{
	public static class SOTSUtils
	{
		public static SlotId PlaySound(SoundStyle style, Vector2 position, float volume = 1f, float pitch = 0f, float pitchVariance = 0f)
		{
			style.Volume = volume;
			style.Pitch = pitch;
			style.PitchVariance = pitchVariance;
			return SoundEngine.PlaySound(style, position);
		}
		public static SlotId PlaySound(SoundStyle style, float posX, float posY, float volume = 1f, float pitch = 0f)
		{
			return PlaySound(style, new Vector2(posX, posY), volume, pitch);
        }
		public static int GetBaseDamage(this NPC npc)
        {
			return SOTSNPCs.GetBaseDamage(npc);
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