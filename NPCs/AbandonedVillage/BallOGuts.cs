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

namespace SOTS.NPCs.AbandonedVillage
{
    public class BallOGuts : ModNPC  
    {
        private float addedStretch = 0f;
        private float stretchRecoil = 0f;

        private bool hasCollidedWithWall = false;

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
            NPC.lifeMax = 100;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.width = 44;
			NPC.height = 46;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0.5f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.value = Item.buyPrice(0, 0, 2, 0);
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
            if (stretchRecoil > 0)
			{
				stretchRecoil -= 0.1f;
			}
			else
			{
				stretchRecoil = 0;
			}

			addedStretch = -stretchRecoil;

            //only run screenshake code if the player is close enough
            //probably should add screenshake here eventually
            if (player.Distance(NPC.Center) < 250f)
            {
                //collide with walls if traveling at maximum speed
                if ((NPC.velocity.X >= 6 || NPC.velocity.X <= -6) && player.velocity.Y == 0 && Collision.SolidCollision(NPC.Center, NPC.width, NPC.height))
                {
                    hasCollidedWithWall = false;
                }

                //collide with walls and play a sound
                if (!hasCollidedWithWall && (NPC.oldVelocity.X >= 5 || NPC.oldVelocity.X <= -5) && NPC.collideX)
                {
                    SoundEngine.PlaySound(SoundID.Item177 with { Volume = SoundID.Item177.Volume * 0.35f }, NPC.Center);
                    stretchRecoil = 0.8f;

                    //set timer to slow down the npc after hitting a wall
                    NPC.localAI[0] = 60;

                    //set velocity to zero
                    NPC.velocity = Vector2.Zero;

                    hasCollidedWithWall = true;
                }
            }

            if (NPC.localAI[0] > 0)
            {
                NPC.localAI[0]--;

                NPC.velocity.X *= 0.2f;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            if (NPC.life <= 0)
            {
                for (int i = 1; i <= 6; i++)
                {
                    Vector2 circular = new Vector2(0, -16).RotatedBy(MathHelper.ToRadians(i * 60));
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular - new Vector2(9, 9), circular * 0.15f, ModGores.GoreType("Gores/Ball/BallOGutsGore" + i), .9f);
                }
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FamishedDustCrimson>(), hit.HitDirection, -1f, NPC.alpha);
                }
            }
            else
            {
                int num = 0;
                while (num < hit.Damage / (float)NPC.lifeMax * 60)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FamishedDustCrimson>(), hit.HitDirection, -1f, NPC.alpha);
                    num++;
                }
            }
        }
    }
}