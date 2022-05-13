using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace SOTS.Mounts
{
	public class IceShield : ModMount
	{
		public override void SetStaticDefaults()
		{	
			MountData.spawnDust = 229;
			MountData.buff = ModContent.BuffType<Buffs.IceShield>();
			MountData.heightBoost = 2;
			MountData.fallDamage = 0.5f;
			MountData.runSpeed = 14f;
			MountData.dashSpeed = 14f;
			MountData.flightTimeMax = 999999999;
			MountData.fatigueMax = 999999999;
			MountData.jumpHeight = 40;
			MountData.acceleration = 0.19f;
			MountData.jumpSpeed = 6f;
			MountData.totalFrames = 4;
			MountData.usesHover = true;
			int[] array = new int[MountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 4;
			}
			MountData.playerYOffsets = array;
			MountData.xOffset = 0;
			MountData.bodyFrame = 0;
			MountData.yOffset = 0;
			MountData.playerHeadOffset = 0;
			MountData.standingFrameCount = 4;
			MountData.standingFrameDelay = 6;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 4;
			MountData.runningFrameDelay = 25;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 4;
			MountData.flyingFrameDelay = 6;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 4;
			MountData.inAirFrameDelay = 6;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 4;
			MountData.idleFrameDelay = 6;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
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
