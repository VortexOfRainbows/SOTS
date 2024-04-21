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
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
        }
		public override void SetDefaults()
		{
            NPC.lifeMax = 500;
			NPC.damage = 40;
			NPC.defense = 0;  
            NPC.knockBackResist = 0f;
            NPC.width = 52;
            NPC.height = 52;
            NPC.value = 0;
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
		}
        public override bool PreKill()
		{
			return false;
		}
		public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance * bossAdjustment);
			NPC.damage = (int)(NPC.damage * 0.75f);
		}
		public override void AI()
		{	
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
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
				NPC.ai[0]++;
				if(NPC.ai[0] >= 30)
				{
					NPC.ai[0] = 0;
					NPC.rotation += MathHelper.ToRadians(9);
					NPC.ai[2] += 9;
					if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
						int damage = NPC.GetBaseDamage();
						for (int i = 0; i < 4; i++)
						{
							Vector2 rotateVelocity = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[2] + 90 * i));
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, rotateVelocity, ModContent.ProjectileType<PolarBullet>(), damage, 0, Main.myPlayer, 0f, 0f);
						}
					}
				}
			}
		}
	}
}





















