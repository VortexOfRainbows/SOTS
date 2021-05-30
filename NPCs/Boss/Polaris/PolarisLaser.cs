using Microsoft.Xna.Framework;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss.Polaris
{
	public class PolarisLaser : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polar Quad-Cannon");
		}
		public override void SetDefaults()
		{
            npc.lifeMax = 500;
			npc.damage = 40;
			npc.defense = 0;  
            npc.knockBackResist = 0f;
            npc.width = 52;
            npc.height = 52;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.buffImmune[44] = true;
		}
		public override bool PreNPCLoot()
		{
			return false;
		}
		public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}
		public override void AI()
		{	
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
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
				npc.ai[0]++;
				if(npc.ai[0] >= 30)
				{
					npc.ai[0] = 0;
					npc.rotation += MathHelper.ToRadians(9);
					npc.ai[2] += 9;
					if (Main.netMode != 1)
                    {
						int damage = npc.damage / 2;
						damage *= 2;
						if (Main.expertMode)
						{
							damage = (int)(damage / Main.expertDamage);
						}
						for (int i = 0; i < 4; i++)
						{
							Vector2 rotateVelocity = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(npc.ai[2] + 90 * i));
							Projectile.NewProjectile(npc.Center, rotateVelocity, ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
						}
					}
				}
			}
		}
	}
}





















