using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Mounts
{
	public class SpiritSurfer : ModMountData
	{
		public override void SetDefaults()
		{	
			mountData.spawnDust = mod.DustType("LostSoulDust");
			mountData.buff = mod.BuffType("SpiritSurfer");
			mountData.heightBoost = 0;
			mountData.runSpeed = 20f;
			mountData.dashSpeed = 20f;
			mountData.flightTimeMax = 999999999;
			mountData.fatigueMax = 999999999;
			mountData.jumpHeight = 40;
			mountData.acceleration = 0.19f;
			mountData.jumpSpeed = 6f;
			mountData.totalFrames = 1;
			mountData.usesHover = true;
			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 2;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			mountData.bodyFrame = 6;
			mountData.yOffset = 6;
			mountData.playerHeadOffset = 0;
			
			if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}
		float variation = 0;
		public override void UpdateEffects(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen -= 4f;
					Vector2 curve = new Vector2(24,0).RotatedBy(MathHelper.ToRadians(variation));
					if(player.velocity.X > 0.1)
					{
						variation += 4;
						int dust = Dust.NewDust(new Vector2(player.Center.X - 30, player.Center.Y + curve.Y + 21), 4, 4, mod.DustType("LostSoulDust"));
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.1f;
						dust = Dust.NewDust(new Vector2(player.Center.X - 30, player.Center.Y - curve.Y + 21), 4, 4, mod.DustType("LostSoulDust"));
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.1f;
					}
					if(player.velocity.X < -0.1)
					{
						variation += 4;
						int dust = Dust.NewDust(new Vector2(player.Center.X + 20, player.Center.Y + curve.Y + 21), 4, 4, mod.DustType("LostSoulDust"));
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.1f;
						dust = Dust.NewDust(new Vector2(player.Center.X + 20, player.Center.Y - curve.Y + 21), 4, 4, mod.DustType("LostSoulDust"));
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.1f;
					}
					
					
					player.armorEffectDrawOutlines = true;
		}
	}
}