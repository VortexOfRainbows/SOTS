using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Gizmos
{
	public class EarthenGizmo : ModNPC
	{
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }
        public override void SetDefaults()
		{
            NPC.aiStyle = -1; 
            NPC.lifeMax = 60;   
            NPC.damage = 20; 
            NPC.defense = 10;  
            NPC.knockBackResist = .6f;
            NPC.width = 36;
            NPC.height = 60; 
            NPC.value = 500;
            NPC.npcSlots = 0.5f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
			//Banner = NPC.type;
			//BannerItem = ItemType<SittingMushroomBanner>();
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center - new Vector2(0, 32);
			bool canSeePlayerShort = Collision.CanHitLine(NPC.Center + new Vector2(0, 32), 0, 0, player.position, player.width, player.height);
            bool canSeePlayerTall = Collision.CanHitLine(NPC.Center - new Vector2(0, 64), 0, 0, player.position, player.width, player.height);
			if(!canSeePlayerShort && !canSeePlayerTall)
            {
                NPC.ai[0] = 1;
            }
			if(canSeePlayerShort)
            {
                NPC.ai[0] = -1;
            }
			if(NPC.ai[0] == 1)
            {
				NPC.noTileCollide = true;
				NPC.behindTiles = true;
				NPC.noGravity = true;
				NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
				NPC.velocity += toPlayer.SNormalize() * 0.5f;
                NPC.velocity *= 0.85f;
            }
			else
            {
                NPC.noTileCollide = false;
                NPC.behindTiles = false;
                NPC.noGravity = false;
                NPC.velocity.X += MathF.Sign(toPlayer.X) * 0.07f;
                NPC.velocity.X *= 0.93f;
                NPC.velocity.Y += 0.04f; //Add additional gravity
                NPC.rotation = 0;
            }

			NPC.spriteDirection = -NPC.direction;
		}
		int frame = 0;
		public override void FindFrame(int frameHeight)
        {
            float frameSpeed = 10f;
            frame = frameHeight;
			NPC.frameCounter += 0.5f + MathF.Sqrt(MathF.Abs(NPC.velocity.X));
			if (NPC.frameCounter >= frameSpeed) 
			{
				NPC.frameCounter -= frameSpeed;
				NPC.frame.Y += frame;
				if(NPC.frame.Y >= 4 * frame)
				{
					NPC.frame.Y = 0;
				}
			}
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			//npcLoot.Add(ItemDropRule.Common(ItemType<SporeSprayer>(), 100));
			//npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<FragmentOfEarth>(), 2, 1));
			//LeadingConditionRule onFire = new LeadingConditionRule(new Common.ItemDropConditions.OnFireCondition());
			//onFire.OnSuccess(ItemDropRule.Common(ItemType<CookedMushroom>(), 1, 3, 4));
			//onFire.OnFailedConditions(ItemDropRule.Common(ItemType<CookedMushroom>(), 10, 3, 4)
			//	.OnFailedRoll(ItemDropRule.Common(ItemID.GlowingMushroom, 1, 7, 12)));
		}
        public override void OnKill()
		{
			//for (int k = 0; k < 7; k++)
			//{
			//	Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
			//	int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
			//	if (Main.netMode != NetmodeID.MultiplayerClient)
			//		Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center.X, NPC.Center.Y, circularLocation.X, circularLocation.Y, ProjectileType<Projectiles.SporeCloud>(), damage2, 2f, Main.myPlayer);
			//}
        }
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			//if (NPC.life > 0)
			//{
			//	int num = 0;
			//	while ((double)num < hit.Damage / (double)NPC.lifeMax * 20.0)
			//	{
			//		Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GlowingMushroom, (float)(2 * hit.HitDirection), -2f, 250, new Color(100, 100, 100, 250), 0.8f);
			//		num++;
			//	}
			//}
			//else
			//{
			//	for (int k = 0; k < 10; k++)
			//	{
			//		Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GlowingMushroom, (float)(2 * hit.HitDirection), -2f, 250, new Color(100, 100, 100, 250), 0.8f);
			//	}
			//	SOTSUtils.PlaySound(SoundID.Item34, NPC.Center);
			//	Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/SittingMushroomGore1"), 1f);
			//}
		}
	}
}





















