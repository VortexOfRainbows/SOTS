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
			npc.aiStyle = 0;
			npc.lifeMax = 1000;   
            npc.damage = 30;
			npc.defense = 24;
			npc.knockBackResist = 0f;
            npc.width = 48;
            npc.height = 28;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netAlways = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.buffImmune[BuffID.Ichor] = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.gfxOffY = 0;
		}
		public override bool PreNPCLoot()
		{
			return false;
		}
		float WidthOffset = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White * ((255 - npc.alpha) / 255f), npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Texture2D texture = mod.GetTexture("NPCs/Boss/Polaris/PolarisCannonPump");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f + WidthOffset * npc.spriteDirection, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White * ((255 - npc.alpha) / 255f), npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale); 
			npc.damage = (int)(npc.damage * 0.75f); 
		}
		public override void AI()
		{	
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			if(runOnce)
			{
				int myID = (int)npc.ai[0];
				npc.rotation = MathHelper.ToRadians(90 * myID);
				npc.ai[3] = 90 * myID;
				runOnce = false;
			}
			npc.ai[0]++;
			int damage = npc.damage / 2;
			damage *= 2;
			if (Main.expertMode)
			{
				damage = (int)(damage / Main.expertDamage);
			}
			Vector2 rotateVelocity = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(npc.ai[3]));
			if(npc.scale >= 0.9f)
			{
				if (npc.ai[0] % 35 == 0 && npc.ai[0] <= 330)
				{
					if (Main.netMode != 1)
					{
						Projectile.NewProjectile(npc.Center, rotateVelocity, ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
						if (Main.expertMode && npc.ai[0] % 70 == 0)
							for (int i = -1; i < 2; i += 2)
								Projectile.NewProjectile(npc.Center, rotateVelocity.RotatedBy(MathHelper.ToRadians(20f * i)), ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
					}
					WidthOffset = 13;
				}
				if (npc.ai[0] >= 360 && npc.ai[0] <= 400)
				{
					npc.ai[3] += 3f;
					if(npc.ai[0] % 3 == 0)
					{
						if (Main.netMode != 1)
						{
							float speed = 0.5f;
							if (npc.ai[0] >= 380)
								speed = 1.25f;
							Projectile.NewProjectile(npc.Center, rotateVelocity * speed, ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
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
			if(npc.ai[0] >= 420)
				npc.ai[0] = 0;
			NPC polaris = Main.npc[(int)npc.ai[1]];
			if (!polaris.active || polaris.type != ModContent.NPCType<Polaris>())
			{
				npc.scale -= 0.008f;
				npc.rotation += 0.3f;
				if (npc.scale < 0)
					npc.active = false;
			}
			else
			{
				float dist = 128;
				rotateVelocity = new Vector2(dist, 0).RotatedBy(MathHelper.ToRadians(++npc.ai[3]));
				npc.rotation = rotateVelocity.ToRotation() + (float)MathHelper.Pi;
				if(rotateVelocity.X > 0)
                {
					npc.rotation -= MathHelper.Pi;
					npc.spriteDirection = 1;
                }
				else
                {
					npc.spriteDirection = -1;
                }
				npc.Center = polaris.Center + rotateVelocity;
			}
		}
	}
}