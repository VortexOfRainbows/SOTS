using ReLogic.Utilities;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using SOTS.Common.GlobalNPCs;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SOTS
{
	public static class SOTSUtils
	{
		public static Texture2D inventoryBoxTexture => Terraria.GameContent.TextureAssets.InventoryBack.Value;
		public static SlotId PlaySound(SoundStyle style, Vector2 position, float volume = 1f, float pitch = 0f, float pitchVariance = 0f)
		{
			style = style.WithVolumeScale(volume);
			style = style.WithPitchOffset(pitch);
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
		/// <summary>
		/// Sets the cost to duplicate the item in journey mode
		/// <para> 1 - Weapons, tools, armor, accessories, furniture, one-time consumables (Cat Liscense) </para>
		/// <para> 2 - Quest fish, tombstones, herb bag, can of worms </para>
		/// <para> 3 - Treasure bags, boss-summons, dyes, fish, gold critters </para>
		/// <para> 5 - critters, mechanisms, fruits, rare crafting materials (unicorn horn) </para>
		/// <para> 10 - crates, life/mana crystal, life fruit, food/drink </para>
		/// <para> 15 - gems </para>
		/// <para> 20 - potions </para>
		/// <para> 25 - bars, herbs, seeds, crafting materials </para>
		/// <para> 30 - mana/healing potions </para>
		/// <para> 50 - acorns, fallen stars, beams </para>
		/// <para> 99 - ammo, explosives </para>
		/// <para> 100 - ores, blocks, torches, ropes, empty bullets, coins </para>
		/// <para> 200 - platforms, silt/slush etc. </para>
		/// <para> 400 - Background walls </para>
		/// <para> N/A - Etherian Mana and unobtainable items </para>
		/// </summary>
		public static void SetResearchCost(this ModItem modItem, int amt)
		{
			modItem.Item.ResearchUnlockCount = amt;
		}
		/// <summary>
		/// Flips the void bar rectangles horizontally. Only runs if alternative void bar style is on
		/// </summary>
		public static Rectangle FlipHorizontal(this Rectangle rect, Vector2 flipPos)
		{
			if(SOTS.Config.alternateVoidBarDirection)
			{
				int XAxis = (int)flipPos.X;
				int relativeToXAxis = rect.X - XAxis;
				int overXAxis = relativeToXAxis + rect.Width;
				rect.Location = new Point((int)XAxis - overXAxis, rect.Location.Y);
			}
			return rect;
		}
		public static VoidPlayer VoidPlayer(this Player player)
        {
			return Void.VoidPlayer.ModPlayer(player);
		}
		public static SOTSPlayer SOTSPlayer(this Player player)
		{
			return global::SOTS.SOTSPlayer.ModPlayer(player);
		}
        /*public static void SetResearchCostAutomatically(this ModItem modItem)
		{
			Item item = modItem.Item;
			item.SetDefaults(modItem.Type);
			int amt = 1;
			SetResearchCost(modItem.Type, amt);
		}*/
        public static void RemoveBySwap<T>(this List<T> list, int index)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
    }
}