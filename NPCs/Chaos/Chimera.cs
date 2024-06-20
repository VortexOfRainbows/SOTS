using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using SOTS.Items.Slime;
using SOTS.Items.Void;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Pyramid;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Chaos
{
	public class Chimera : ModNPC
	{
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
        }
        public override void SetDefaults()
		{
			NPC.aiStyle = NPCAIStyleID.Unicorn;
            NPC.lifeMax = 1000;  
            NPC.damage = 70; 
            NPC.defense = 36;  
            NPC.knockBackResist = 0.04f;
            NPC.width = 88;
            NPC.height = 66;
            NPC.value = 1800;
            NPC.npcSlots = .5f;
            NPC.HitSound = SoundID.NPCHit12;
			NPC.DeathSound = SoundID.NPCDeath18;
			NPC.rarity = 5;
			AnimationType = NPCID.None;
            Banner = NPC.type;
			BannerItem = ItemType<ChimeraBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)Request<Texture2D>("SOTS/NPCs/Chaos/ChimeraGoatHead");
            Texture2D textureG = (Texture2D)Request<Texture2D>("SOTS/NPCs/Chaos/ChimeraGoatHeadGlow");
            int frameY = GoatFrameY;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 16);
			Rectangle frame = new Rectangle(0, frameY * texture.Height / 8, texture.Width, texture.Height / 8);
            Vector2 drawPos = NPC.Center - new Vector2(16 * NPC.spriteDirection, 20 - NPC.gfxOffY) - screenPos;
            spriteBatch.Draw(texture, drawPos, frame, NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : 0, 0f);
            spriteBatch.Draw(textureG, drawPos, frame, ColorHelpers.ChaosPink, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : 0, 0f);
            return true;
		}
		public override bool PreAI()
        {
            NPC.TargetClosest(false);
            NPC.velocity.X *= 0.99f;
			return true;
		}
        public override void FindFrame(int frameHeight)
        {
			int frameSpeed = 10;
			NPC.frameCounter += 1 + MathF.Sqrt(Math.Abs(NPC.velocity.X));
			if(NPC.frameCounter > frameSpeed)
			{
				NPC.frameCounter -= frameSpeed;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type])
				{
					NPC.frame.Y = 0;
				}
            }
            if (NPC.velocity.Y != 0)
            {
                NPC.frame.Y = frameHeight * 4;
            }
        }
		private bool runOnce = true;
        private int counter = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(counter);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			counter = reader.ReadInt32();
		}
		private int GoatFrameY = 0;
		public override void AI()
		{
			if (MathF.Sign(NPC.velocity.X) == MathF.Sign(NPC.oldVelocity.X))
				NPC.direction = NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
			counter++;
			if (runOnce) 
			{
				runOnce = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
				{
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height, NPCType<ChimeraSnake>(), 0, NPC.whoAmI);
                }
			}
			if(counter > 60)
			{
				counter++;
				if(counter >= 70)
				{
					NPC.netUpdate = true;
					counter -= 10;
					GoatFrameY++;
                    if (GoatFrameY == 4)
                    {
						if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
							Vector2 away = -new Vector2(16 * NPC.direction, 20 - NPC.gfxOffY);
                            Vector2 spawn = NPC.Center + away;
							away.Y *= 0.6f;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, away * 0.5f, ProjectileType<ChimeraFireball>(), damage2, 1f, Main.myPlayer, NPC.target);
                        }
                    }
                }
				if(GoatFrameY >= 8)
				{
					GoatFrameY = 0;
					counter = -90;
				}
			}
			if (counter < 0)
			{
				GoatFrameY = 0;
			}
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < hit.Damage / (double)NPC.lifeMax * 100.0)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, NPC.alpha, Scale: 1.1f);
					dust.color = new Color(140, 108, 37) * 1.7f;


                    short d = DustID.Stone;
                    if (Main.rand.NextBool(2))
                        d = DustID.Blood;
                    dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, d, hit.HitDirection, -1f, NPC.alpha, Scale: 1.1f);
                    dust.color = new Color(138, 137, 138) * 1.3f;
                    num++;
                }
			}
			else
			{
				for (int k = 0; k < 35; k++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Dirt, 2 * hit.HitDirection, -2f, NPC.alpha);
                    dust.color = new Color(140, 108, 37) * 1.7f;


                    short d = DustID.Stone;
                    if (Main.rand.NextBool(2))
                        d = DustID.Blood;
                    dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, d, 2 * hit.HitDirection, -2f, NPC.alpha, Scale: 1.1f);
					dust.color = new Color(138, 137, 138) * 1.3f;
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center + Vector2.UnitX * NPC.width / 3 * NPC.direction - new Vector2(20, 20), NPC.velocity, ModGores.GoreType("Gores/Chimera/ChimeraGore1"), NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center - Vector2.UnitX * NPC.width / 3 * NPC.direction - new Vector2(20, 20), NPC.velocity, ModGores.GoreType("Gores/Chimera/ChimeraGore2"), NPC.scale);
            }
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<Snakeskin>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfChaos>(), 1, 1, 2));
		}
	}
}