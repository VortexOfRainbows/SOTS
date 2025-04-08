using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Mounts
{
	public class UnholyGrailMount : ModMount
	{
		public override void SetStaticDefaults()
		{
			MountData.spawnDust = DustID.GoldCoin;
			MountData.buff = ModContent.BuffType<Buffs.UnholyGrailBuff>();
			MountData.heightBoost = 0;
			MountData.runSpeed = 2f;
			MountData.dashSpeed = 2f;
			MountData.flightTimeMax = 0;
			MountData.fatigueMax = 0;
			MountData.jumpHeight = 7;
			MountData.acceleration = 0.01f;
			MountData.jumpSpeed = 2f;
			MountData.totalFrames = 1;
			MountData.usesHover = false;
			MountData.playerYOffsets = [10];
			MountData.xOffset = 0;
			MountData.bodyFrame = 6;
			MountData.yOffset = -7;
			MountData.playerHeadOffset = 0;
			MountData.fallDamage = 0;
			MountData.constantJump = true;
			if (Main.netMode != NetmodeID.Server)
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
		}
        public override void UpdateEffects(Player player)
		{
			SetStaticDefaults();
			player.maxFallSpeed *= 2;
			player.sitting.isSitting = true;
			player.statDefense += 12;
			if (player.velocity.Y == 0)
			{
				MountData.acceleration = 0.0001f;
                player.velocity.X *= 0.5f;
            }
			else
				MountData.acceleration = 0.2f;
        }
    }
}