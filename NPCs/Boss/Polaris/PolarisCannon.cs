using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss.Polaris
{
	public class PolarisCannon : ModNPC
	{	
		bool runOnce = true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polar Cannon");
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =0;
			NPC.lifeMax = 1200;   
            NPC.damage = 30;
			NPC.defense = 24;
			NPC.knockBackResist = 0f;
            NPC.width = 48;
            NPC.height = 28;
            NPC.value = 0;
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[BuffID.Ichor] = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.gfxOffY = 0;
		}
		public override bool PreNPCLoot()
		{
			return false;
		}
		float WidthOffset = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, Color.White * ((255 - NPC.alpha) / 255f), NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Boss/Polaris/PolarisCannonPump").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f + WidthOffset * NPC.spriteDirection, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, Color.White * ((255 - NPC.alpha) / 255f), NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale); 
			NPC.damage = (int)(NPC.damage * 0.75f); 
		}
		public override void AI()
		{	
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
			if(runOnce)
			{
				int myID = (int)NPC.ai[0];
				NPC.rotation = MathHelper.ToRadians(90 * myID);
				NPC.ai[3] = 90 * myID;
				runOnce = false;
			}
			NPC.ai[0]++;
			int damage = NPC.damage / 2;
			damage *= 2;
			if (Main.expertMode)
			{
				damage = (int)(damage / Main.expertDamage);
			}
			Vector2 rotateVelocity = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[3]));
			if(NPC.scale >= 0.9f)
			{
				if (NPC.ai[0] % 35 == 0 && NPC.ai[0] <= 330)
				{
					if (Main.netMode != 1)
					{
						Projectile.NewProjectile(NPC.Center, rotateVelocity, ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
						if (Main.expertMode && NPC.ai[0] % 70 == 0)
							for (int i = -1; i < 2; i += 2)
								Projectile.NewProjectile(NPC.Center, rotateVelocity.RotatedBy(MathHelper.ToRadians(20f * i)), ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
					}
					WidthOffset = 13;
				}
				if (NPC.ai[0] >= 360 && NPC.ai[0] <= 400)
				{
					NPC.ai[3] += 3f;
					if(NPC.ai[0] % 3 == 0)
					{
						if (Main.netMode != 1)
						{
							float speed = 0.5f;
							if (NPC.ai[0] >= 380)
								speed = 1.25f;
							Projectile.NewProjectile(NPC.Center, rotateVelocity * speed, ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
						}
						WidthOffset = 13;
					}
				}
			}
			if(WidthOffset > 0)
			{
				WidthOffset *= 0.96f;
				WidthOffset -= 0.24f;
			}
			else
            {
				WidthOffset = 0;
            }
			if(NPC.ai[0] >= 420)
				NPC.ai[0] = 0;
			NPC polaris = Main.npc[(int)NPC.ai[1]];
			if (!polaris.active || polaris.type != ModContent.NPCType<Polaris>())
			{
				NPC.scale -= 0.008f;
				NPC.rotation += 0.3f;
				if (NPC.scale < 0)
					NPC.active = false;
			}
			else
			{
				float dist = 128;
				rotateVelocity = new Vector2(dist, 0).RotatedBy(MathHelper.ToRadians(++NPC.ai[3]));
				NPC.rotation = rotateVelocity.ToRotation() + (float)MathHelper.Pi;
				if(rotateVelocity.X > 0)
                {
					NPC.rotation -= MathHelper.Pi;
					NPC.spriteDirection = 1;
                }
				else
                {
					NPC.spriteDirection = -1;
                }
				NPC.Center = polaris.Center + rotateVelocity;
			}
		}
	}
}