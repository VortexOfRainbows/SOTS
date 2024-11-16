using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using SOTS.Items.Banners;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.NPCs.AbandonedVillage
{
    public class BallOGuts : ModNPC  
    {
        protected float addedStretch = 0f;
        protected float stretchRecoil = 0f;
        protected bool hasCollidedWithWall = false;
        private static Asset<Texture2D> NPCTexture;
        private static Asset<Texture2D> PieceOfBallTexture;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hasCollidedWithWall);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hasCollidedWithWall = reader.ReadBoolean();
        }
        public override void SetDefaults()
		{
            NPC.lifeMax = 45;
            NPC.damage = 24;
            NPC.defense = 4;
            NPC.width = 44;
			NPC.height = 46;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0.4f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath11;
            NPC.aiStyle = 26;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<BallOGutsBanner>();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPCTexture ??= ModContent.Request<Texture2D>(Texture);
            PieceOfBallTexture ??= ModContent.Request<Texture2D>("SOTS/NPCs/AbandonedVillage/BallOGutsPieces");

			float stretch = 0f;

			stretch = Math.Abs(stretch) - addedStretch;
			
			//limit how much it can stretch
			if (stretch > 0.5f)
			{
				stretch = 0.5f;
			}

			//limit how much it can squish
			if (stretch < -0.5f)
			{
				stretch = -0.5f;
			}

			Vector2 scaleStretch = new Vector2(1f + stretch, 1f - stretch);

            Vector2 drawPosition = new Vector2(NPC.Center.X, NPC.Center.Y) - Main.screenPosition + new Vector2(0, NPC.gfxOffY + 4);

            //draw npc manually for stretching
            spriteBatch.Draw(NPCTexture.Value, drawPosition, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2f, scaleStretch, SpriteEffects.None, 0f);

            //theres probably a better way to do this but i didnt feel like spending 6 hours on it
            for (int numFrame = 0; numFrame < 6; numFrame++)
            {
                float Pulsing = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.5f / 2.5f * 6f)) / 2f + 0.5f;

                //I LOVE RANDOM NUMBERS
                if (numFrame == 0 || numFrame == 2 || numFrame == 4)
                {
                    Pulsing = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.5f / 2.5f * 6f)) / 2f + 0.5f;
                }
                else
                {
                    Pulsing = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly % 2.5f / 2.5f * 6f)) / 2f + 0.5f;
                }

                Pulsing = MathHelper.Clamp(Pulsing, 0f, 1f);

                spriteBatch.Draw(PieceOfBallTexture.Value, drawPosition, new Rectangle(0, numFrame * NPC.height, NPC.width, NPC.height), 
                drawColor, NPC.rotation, NPC.frame.Size() / 2f, new Vector2(scaleStretch.X + Pulsing / 12, scaleStretch.Y + Pulsing / 12), SpriteEffects.None, 0f);
            }

			return false;
		}
        
        public override void AI()
		{
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

			NPC.spriteDirection = NPC.direction;

            NPC.rotation += 0.05f * (float)NPC.direction + (NPC.velocity.X / 40);

            //stretch stuff
            stretchRecoil = MathF.Max(stretchRecoil - 0.04f, 0);
			addedStretch = -stretchRecoil;

            if ((NPC.velocity.X >= 5.5f || NPC.velocity.X <= -5.5f) && Collision.SolidCollision(NPC.Center, NPC.width, NPC.height))
                hasCollidedWithWall = false;

            //collide with walls and play a sound
            if (!hasCollidedWithWall && (NPC.oldVelocity.X >= 5 || NPC.oldVelocity.X <= -5) && NPC.collideX)
            {
                SoundEngine.PlaySound(SoundID.Item177 with { Volume = SoundID.Item177.Volume * 0.35f }, NPC.Center);
                stretchRecoil += 0.4f;

                //set timer to slow down the npc after hitting a wall
                NPC.localAI[0] = 60;

                //set velocity to zero
                NPC.velocity = Vector2.Zero;

                hasCollidedWithWall = true;
            }

            if (NPC.localAI[0] > 0)
            {
                NPC.localAI[0]--;
                NPC.velocity.X *= 0.2f;
            }
            NPC.velocity *= 0.982f; //It will have faster turning and slower top speed compared to unicorn and angry tumbler

            if(MathF.Abs(NPC.velocity.X) > 0 && NPC.collideY)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y + NPC.height) - new Vector2(4, 4), NPC.width, 0, NPC.type == ModContent.NPCType<BallOGuts>() ? ModContent.DustType<FamishedDustCrimson>() : ModContent.DustType<FamishedDustCorruption>());
                dust.scale = dust.scale * 0.45f + 1.1f;
                dust.noGravity = true;
                dust.velocity = dust.velocity * 0.04f - NPC.velocity * Main.rand.NextFloat(0.25f);
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if(Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.SinglePlayer)
            {
                float chance = hit.Damage / 14f;
                float amt = Main.rand.NextFloat(1);
                int reduceOdds = 1 + NPC.CountNPCS(ModContent.NPCType<BallOGutsPile>()) / 4;
                while(chance > amt && Main.rand.NextBool(reduceOdds))
                {
                    chance -= 0.5f;
                    chance *= 0.4f;
                    NPC npc = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<BallOGutsPile>(), 0, Main.rand.NextFloat(120));
                    npc.netUpdate = true;
                }
            }
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            if (NPC.life <= 0)
            {
                for (int i = 1; i <= 6; i++)
                {
                    Vector2 circular = new Vector2(0, -16).RotatedBy(MathHelper.ToRadians(i * 60));
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular - new Vector2(9, 9), circular * 0.15f, ModGores.GoreType("Gores/Ball/BallOGutsGore" + i), .9f);
                }
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FamishedDustCrimson>(), hit.HitDirection, -1f, NPC.alpha, Scale: 1.25f);
                }
            }
            else
            {
                int num = 0;
                while (num < hit.Damage / (float)NPC.lifeMax * 60)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FamishedDustCrimson>(), hit.HitDirection, -1f, NPC.alpha, Scale: 1.25f);
                    num++;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEvil>(), 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldKey>(), 100));
        }
    }
    public class BallOGutsPile : ModNPC
    {
        private bool RunOnce = true;
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(NPC.frame.Y);
        public override void ReceiveExtraAI(BinaryReader reader) => NPC.frame.Y = reader.ReadInt32();
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = NPC.lifeMax / 2;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 10;
            NPC.damage = 12;
            NPC.knockBackResist = NPC.defense = 0;
            NPC.width = 14;
            NPC.height = 12;
            NPC.value = NPC.npcSlots = NPC.alpha = 0;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.dontTakeDamage = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 3f - 2);
            Vector2 drawPos = NPC.Center - screenPos;
            float sin = .1f + .1f * MathF.Sin(MathHelper.ToRadians(NPC.ai[0] * 3));
            spriteBatch.Draw(texture, drawPos + new Vector2(0, NPC.height - 2), NPC.frame, drawColor, NPC.rotation, drawOrigin, NPC.scale * new Vector2(1 - sin, 1 + sin), SpriteEffects.None, 0f);
            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !NPC.dontTakeDamage;
        }
        public override bool CanHitNPC(NPC target)
        {
            return !NPC.dontTakeDamage;
        }
        public override bool PreAI()
        {
            NPC.ai[0]++;
            if (RunOnce)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.velocity += Main.rand.NextVector2CircularEdge(4, 4) - new Vector2(0, 1);
                    if(NPC.velocity.Y > 0)
                        NPC.velocity.Y = -NPC.velocity.Y;
                    NPC.frame.Y = Main.rand.Next(3) * 14;
                    NPC.netUpdate = true;
                }
			    SOTSUtils.PlaySound(SoundID.NPCDeath1, NPC.Center, 0.9f, -0.25f);
                NPC.dontTakeDamage = true;
                RunOnce = false;
            }
            NPC.TargetClosest(true);
            return true;
        }
        public override void AI()
        {
            NPC.velocity.X *= 0.95f;
            NPC.velocity.Y += 0.15f;
            NPC.rotation = NPC.velocity.X * 0.2f;
            if(NPC.velocity.LengthSquared() > 1)
            {
                Dust dust = Dust.NewDustDirect(NPC.Center - new Vector2(4) - NPC.velocity, 0, 0, ModContent.DustType<FamishedDustCrimson>());
                dust.scale = dust.scale * 0.1f + 1.0f;
                dust.noGravity = true;
                dust.velocity = dust.velocity * 0.3f + NPC.velocity * Main.rand.NextFloat(0.4f);
            }
            else
                NPC.dontTakeDamage = false;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            if (NPC.life > 0)
            {
                int num = 0;
                while (num < hit.Damage / NPC.lifeMax * 10.0)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FamishedDustCrimson>(), 2 * hit.HitDirection, -2f, 0, default, 1.2f);
                    num++;
                }
            }
            else
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FamishedDustCrimson>(), 2 * hit.HitDirection, -2f, 0, default, 1.2f);
                }
            }
        }
        public override void UpdateLifeRegen(ref int damage)
        {
            if (NPC.ai[0] > 150)
                NPC.lifeRegen -= 2;
        }
        public override bool PreKill()
        {
            return false;
        }
    }
}