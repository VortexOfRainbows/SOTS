using ReLogic.Utilities;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using SOTS.Common.GlobalNPCs;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.ModPlayers;
using System.Collections.Generic;

namespace SOTS
{
	public static class SOTSUtils
	{
		public static Texture2D WhitePixel => ModContent.Request<Texture2D>("SOTS/Assets/WhitePixel").Value;
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
        public static ConduitPlayer ConduitPlayer(this Player player)
        {
            return Common.ModPlayers.ConduitPlayer.ModPlayer(player);
        }
        public static void RemoveBySwap<T>(this List<T> list, int index)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
		/// <summary>
		/// Shorthand for Vector2.SafeNormalize(Vector2.Zero)
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Vector2 SNormalize(this Vector2 vector)
		{
			return vector.SafeNormalize(Vector2.Zero);
		}
		/// <summary>
		/// Same as Math.Sign(float), but returns 1 instead of 0
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static int SignNoZero(float num)
		{
			int sign = Math.Sign(num);
			if (sign == 0)
				sign = 1;
            return sign;
		}
		/// <summary>
		/// Linearly interpolates an angle, taking the shortest path, where a normal lerp would not suffice. For example, lerping (-160, 150) degrees would take a route from -160 to -210 or 200 to 150, instead of -160 to 150.
		/// </summary>
		/// <returns></returns>
		public static float AngularLerp(float angle1, float angle2, float amount)
		{
			angle1 = MathHelper.WrapAngle(angle1);
			angle2 = MathHelper.WrapAngle(angle2);
			bool doNormalRoute = MathF.Abs(angle1 - angle2) < MathHelper.Pi;
			if(doNormalRoute)
			{
                return MathHelper.WrapAngle(MathHelper.Lerp(angle1, angle2, amount));
            }
            else //Doing the backward route
            {
				if (angle1 > angle2)
                {
                    angle2 += MathHelper.TwoPi;
                }
                else
                {
                    angle1 += MathHelper.TwoPi;
                }
                return MathHelper.WrapAngle(MathHelper.Lerp(angle1, angle2, amount));
            }
		}
		public static Color ToColor(this Vector3 v3)
		{
			return new Color(v3.X, v3.Y, v3.Z);
		}
    }
}