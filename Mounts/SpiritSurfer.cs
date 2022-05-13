using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Dusts;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace SOTS.Mounts
{
	public class SpiritSurfer : ModMount
	{
		public override void SetStaticDefaults()
		{	
			MountData.spawnDust = ModContent.DustType<LostSoulDust>();
			MountData.buff = ModContent.BuffType<Buffs.SpiritSurfer>();
			MountData.heightBoost = 0;
			MountData.runSpeed = 16f;
			MountData.dashSpeed = 16f;
			MountData.flightTimeMax = 999999999;
			MountData.fatigueMax = 999999999;
			MountData.jumpHeight = 40;
			MountData.acceleration = 0.19f;
			MountData.jumpSpeed = 6f;
			MountData.totalFrames = 1;
			MountData.usesHover = true;
			int[] array = new int[MountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 2;
			}
			MountData.playerYOffsets = array;
			MountData.xOffset = 0;
			MountData.bodyFrame = 6;
			MountData.yOffset = 6;
			MountData.playerHeadOffset = 0;
			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}
		float variation = 0; 
        public override void UpdateEffects(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.flatVoidRegen -= 6f;
			Vector2 curve = new Vector2(24,0).RotatedBy(MathHelper.ToRadians(variation));
			if(player.velocity.X > 0.1)
			{
				variation += 4;
				int dust = Dust.NewDust(new Vector2(player.Center.X - 30, player.Center.Y + curve.Y + 21), 4, 4, ModContent.DustType<LostSoulDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.1f;
				dust = Dust.NewDust(new Vector2(player.Center.X - 30, player.Center.Y - curve.Y + 21), 4, 4, ModContent.DustType<LostSoulDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.1f;
			}
			if(player.velocity.X < -0.1)
			{
				variation += 4;
				int dust = Dust.NewDust(new Vector2(player.Center.X + 20, player.Center.Y + curve.Y + 21), 4, 4, ModContent.DustType<LostSoulDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.1f;
				dust = Dust.NewDust(new Vector2(player.Center.X + 20, player.Center.Y - curve.Y + 21), 4, 4, ModContent.DustType<LostSoulDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.1f;
			}
			player.armorEffectDrawOutlines = true;
		}
	}
}