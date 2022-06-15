using ReLogic.Utilities;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using SOTS.Common.GlobalNPCs;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

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
		public static void SetDamageBasedOnOriginalDamage(this Projectile projectile, Player player)
		{
			projectile.damage = SOTSPlayer.ApplyDamageClassModWithGeneric(player, projectile.DamageType, projectile.originalDamage);
		}
		public static void SetDamageBasedOnOriginalDamage(this Projectile projectile, int ownerID)
		{
			if(ownerID >= 0 && ownerID <= 255)
				projectile.damage = SOTSPlayer.ApplyDamageClassModWithGeneric(Main.player[ownerID], projectile.DamageType, projectile.originalDamage);
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
		public static void SetResearchCost(this ModItem modItem, int amt)
		{
			SetResearchCost(modItem.Type, amt);
		}
		public static void SetResearchCost(int type, int amt)
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[type] = amt;
		}
		/*public static void SetResearchCostAutomatically(this ModItem modItem)
		{
			Item item = modItem.Item;
			item.SetDefaults(modItem.Type);
			int amt = 1;
			SetResearchCost(modItem.Type, amt);
		}*/
	}
}