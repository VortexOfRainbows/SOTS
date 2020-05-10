using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace SOTS.Mounts
{
	public class IceShield : ModMountData
	{
		public override void SetDefaults()
		{	
			mountData.spawnDust = 229;
			mountData.buff = mod.BuffType("IceShield");
			mountData.heightBoost = 2;
			mountData.fallDamage = 0.5f;
			mountData.runSpeed = 14f;
			mountData.dashSpeed = 14f;
			mountData.flightTimeMax = 999999999;
			mountData.fatigueMax = 999999999;
			mountData.jumpHeight = 40;
			mountData.acceleration = 0.19f;
			mountData.jumpSpeed = 6f;
			mountData.totalFrames = 4;
			mountData.usesHover = true;
			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 4;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			mountData.bodyFrame = 0;
			mountData.yOffset = 0;
			mountData.playerHeadOffset = 0;
			mountData.standingFrameCount = 4;
			mountData.standingFrameDelay = 6;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 25;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 4;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 4;
			mountData.inAirFrameDelay = 6;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 6;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}
		public override void UpdateEffects(Player player)
        {
			player.statDefense += 8;
               Rectangle rect = player.getRect();
               //Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, 229);
			
			player.armorEffectDrawOutlines = true;
        }
	}
}
