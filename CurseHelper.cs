using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using SOTS.Projectiles.Pyramid;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SOTS
{
    public static class CurseHelper
    {
		public static void DrawPlayerFoam(SpriteBatch spriteBatch, Player player)
		{
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			List<CurseFoam> foamList = modPlayer.foamParticleList1;
			List<int> slots = new List<int>();
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				bool validType = proj.type == ModContent.ProjectileType<GasBlast>() || proj.type == ModContent.ProjectileType<Projectiles.Minions.CursedBlade>();
				if (validType && proj.active && proj.owner == player.whoAmI)
				{
					slots.Add(i);
					List<CurseFoam> list = getFoamList(player, proj);
					DrawFoam(list, 2, spriteBatch);
				}
			}
			DrawFoam(foamList, 2, spriteBatch);
			for (int i = 0; i < slots.Count; i++)
			{
				Projectile proj = Main.projectile[slots[i]];
				List<CurseFoam> list = getFoamList(player, proj);
				DrawFoam(list, 1, spriteBatch);
			}
			DrawFoam(foamList, 1, spriteBatch);
			for (int i = 0; i < slots.Count; i++)
			{
				Projectile proj = Main.projectile[slots[i]];
				List<CurseFoam> list = getFoamList(player, proj);
				DrawFoam(list, 0, spriteBatch);
			}
			DrawFoam(foamList, 0, spriteBatch);
		}
		public static List<CurseFoam> getFoamList(Player player, Projectile proj)
		{
			if (proj.type == ModContent.ProjectileType<GasBlast>() && proj.active && proj.owner == player.whoAmI)
			{
				GasBlast ring = proj.modProjectile as GasBlast;
				return ring.foamParticleList1;
			}
			if (proj.type == ModContent.ProjectileType<Projectiles.Minions.CursedBlade>() && proj.active && proj.owner == player.whoAmI)
			{
				Projectiles.Minions.CursedBlade ring = proj.modProjectile as Projectiles.Minions.CursedBlade;
				return ring.foamParticleList1;
			}
			return null;
		}
		public static void DrawFoam(List<CurseFoam> dustList, int layer, SpriteBatch spriteBatch)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Assets/PlayerCurseFoam");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 6);
			for (int i = 0; i < dustList.Count; i++)
			{
				int shade = 255 - (int)(dustList[i].counter * 4f);
				if (shade < 0)
					shade = 0;
				Color color = new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation);
				color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
				float reduction = shade / 255f;
				if (layer == 2)
				{
					Color first = new Color((int)(111 * reduction), (int)(80 * reduction), (int)(154 * reduction));
					Color second = new Color((int)(76 * reduction), (int)(58 * reduction), (int)(101 * reduction));
					color = Color.Lerp(first, second, 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 2)));
				}
				Vector2 drawPos = dustList[i].position - Main.screenPosition;
				Rectangle frame = new Rectangle(0, texture.Height / 3 * layer, texture.Width, texture.Width);
				float scale = layer == 0 ? 1.5f : 2.0f;
				spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * scale, SpriteEffects.None, 0);
			}
		}
	}
}