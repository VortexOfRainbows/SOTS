using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Utils;
using SOTS.Items.CritBonus;
using Stubble.Core.Tokens;
using System;
using System.CodeDom;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Gizmos
{
	public class EarthenGizmo : ModNPC
	{
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 8);
            Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, -6);
            spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, texture.Width, texture.Height / 4), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
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
            NPC.width = 32; //Has to be smaller than the sprite size to allow jumping over blocks properly
            NPC.height = 40; //Has to be shorter than the sprite height to prevent falling through platforms erroneously 
            NPC.value = 500;
            NPC.npcSlots = 0.5f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
			//Banner = NPC.type;
			//BannerItem = ItemType<SittingMushroomBanner>();
		}
		public int AIMode = 0;
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center - new Vector2(0, 32);
			float toPlayerLen = toPlayer.Length();
			if(toPlayerLen < 2400) //150 blocks radius
            {
                NPC.DiscourageDespawn(600);
            }
            bool canSeePlayerShort = Collision.CanHitLine(NPC.Center + new Vector2(0, 18), 0, 0, player.Center + new Vector2(0, 18), 0, 0);
            bool canSeePlayerTall = Collision.CanHitLine(NPC.Center - new Vector2(0, 40), 0, 0, player.Center - new Vector2(0, 40), 0, 0);
			if(!canSeePlayerShort && !canSeePlayerTall)
            {
				if(AIMode < 1)
                {
                    AIMode++;
                } //Not using NPC.ai array because it is used by the vanilla AI in this case.
            }
			if(canSeePlayerShort)
            {
                if (AIMode > 0)
                    AIMode = -30;
            }
            NPC.ai[2] = 100; // This should prevent the fighter AI from despawning this enemy during day
            if (AIMode >= 1) //Drill mode
            {
                NPC.aiStyle = -1;
                //NPC.behindTiles = true;
                if (AIMode > 65)
                {
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                }
                if (AIMode < 70)
				{
					AIMode++;
					NPC.velocity.X *= 0.98f;
                    if (AIMode == 70)
					{
						NPC.velocity.Y -= 12.5f;
					}
				}
				else 
                {
                    //This behavior needs A LOT more work... Basically, the NPC should stay in the ground longer, instead of trending towards the surface. Until it is close to the player.
                    //A good way to pathfind through blocks might be to try making it so the drill checks blocks in several cardinal directions out from it, finding the line that is the combination of the longest and closest and puts it closest to the player.
                    //--Check cardinals out 20 blocks. If there are no blocks in that direction, consider that the check position. Now go in the direction that brings you closest to the player.
                    //Another things is that this needs to be able to reach the player from below, meaning it might need to jump higher up or be able to drill better when going up.
                    bool closePlayerX = Math.Abs(toPlayer.X) < 160; //20 blocks left/rigth
                    bool insideBlock = Collision.SolidTiles(NPC.Center - new Vector2(4, 8 + NPC.height / 2), 8, 15);
                    bool outsideBlock = !Collision.SolidTiles(NPC.Center + new Vector2(-14, -8), 28, 32);
                    float tooClose = Math.Clamp(MathF.Sqrt(Math.Abs(toPlayer.X) / 480f), 0, 1.5f);
                    float cAI = AIMode - 70f;
                    float progression = cAI / 120f;
					if(insideBlock || cAI > 20)
						AIMode++;
                    if (closePlayerX && insideBlock)
						AIMode++;
                    Vector2 toPlayerN = toPlayer.SNormalize();
					float dipWhenFarAway = Math.Clamp((toPlayer.Y - 320 * toPlayerN.Y) * 0.005f, -1, 1) * progression * 0.15f;
					float bonusVelo = Math.Clamp(2 - Math.Abs(toPlayer.X) / 160f, 1, 2);
					NPC.velocity.Y += toPlayerN.Y * (0.3f * bonusVelo * progression + 0.05f * (cAI > 0 ? 1 : 0)) + dipWhenFarAway;
					if(cAI > 20)
						NPC.velocity.X += MathF.Sqrt(MathF.Abs(toPlayerN.X)) * MathF.Sign(toPlayerN.X) * 0.5f * tooClose;
                    NPC.velocity *= 0.875f;
                   
					if(!insideBlock || (cAI < 20 && toPlayerN.Y > 0))
					{
						NPC.velocity.Y += 0.5f * Math.Clamp(1 - progression, 0, 1);
                    }
                    if (outsideBlock && cAI > 60)
                    {
                        AIMode = -120;
                    }

                    NPC.rotation = SOTSUtils.AngularLerp(NPC.rotation, NPC.velocity.ToRotation() + MathHelper.PiOver2, 0.25f);
                }
            }
			else //Walking mode (fighter AI)
            {
                NPC.aiStyle = NPCAIStyleID.Fighter;
                NPC.noTileCollide = false;
                NPC.behindTiles = false;
				NPC.noGravity = false;
				NPC.velocity.X += MathF.Sign(toPlayer.X) * 0.066f;
				if (Math.Abs(toPlayer.X) > 32)
					NPC.velocity.X *= 0.94f;
				NPC.velocity.Y += 0.05f; //Add additional gravity
				if (NPC.velocity.Y < 0)
					NPC.velocity.Y *= 0.99f;
				NPC.rotation = 0;
			}
			NPC.spriteDirection = NPC.direction;
		}
		int frame = 0;
		public override void FindFrame(int frameHeight)
        {
            float frameSpeed = 10f;
            frame = frameHeight;
			float baseSpeed = 0.5f * Math.Clamp(1f - AIMode / 70f, 0, 1);
			if(AIMode > 150)
			{
				baseSpeed += 2.2f;
			}
			NPC.frameCounter += baseSpeed + MathF.Sqrt(MathF.Abs(NPC.velocity.X));
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