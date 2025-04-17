using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SOTS.Projectiles.Slime;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;

namespace SOTS.NPCs.Critters
{
	public class LunaMoth : ModNPC
    {
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture + "Glow").Value;
            Rectangle frame = NPC.frame;
            if (frame.Y < 0)
            {
                frame.Y = 0;
            }
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2 / Main.npcFrameCount[Type]);
            Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY);
            //Main.spriteBatch.Draw(texture, drawPos, null, drawColor * ((255 - NPC.alpha) / 255f), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
            Color color = new Color(110, 110, 110, 0);
            for (int k = 0; k < 4; k++)
            {
                Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 2 + k * 90));
                Main.EntitySpriteDraw(texture, drawPos + offset, frame, color, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            Main.EntitySpriteDraw(Terraria.GameContent.TextureAssets.Npc[Type].Value, drawPos, frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public virtual Color GoreColor => new Color(86, 226, 100, 0);
        public virtual int CatchItem => ModContent.ItemType<LunaMothItem>();
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            Main.npcCatchable[Type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.NormalGoldCritterBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Butterfly);
            NPC.aiStyle = NPCAIStyleID.Butterfly;
            AnimationType = NPCID.Butterfly;
            NPC.width = 26;
            NPC.height = 34;
            NPC.catchItem = CatchItem;
            NPC.lavaImmune = true;
            NPC.ai[2] = 1; //In order to not screw up butterfly AI
        }
        public override bool PreAI()
        {
            NPC.scale = NPC.ai[3] = 0.85f; //In order to make them slightly smaller
            NPC.ai[2] = 1; //In order to not screw up butterfly AI
            NPC.catchItem = CatchItem;
            Player player = Main.player[NPC.target];
            Vector2 toPlayer = player.Center - NPC.Center;
            if (toPlayer.Length() < 240)
            {
                NPC.velocity *= 1.0085f;
                NPC.velocity.X -= toPlayer.SNormalize().X * 0.05f;
            }
            if(Main.rand.NextBool(5))
            {
                PixelDust.Spawn(NPC.Center - new Vector2(16, 16), 32, 32, -NPC.velocity * Main.rand.NextFloat(0.1f) + Main.rand.NextVector2Circular(0.5f, 0.5f), GoreColor * 0.5f, 3);
            }
            return true;
        }
        public override void FindFrame(int frameHeight)
        {
            //frameHeight = 14;
            //if(MathF.Abs(NPC.velocity.X) > 0)
                //NPC.frameCounter++;
            //if(NPC.frameCounter > 20)
            //{
                //NPC.frame.Y += frameHeight;
                //if (NPC.frame.Y >= 2 * frameHeight)
                    //NPC.frame.Y = 0;
                //NPC.frameCounter = 0;
            //}
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.TintableDust, 2 * hit.HitDirection, -2f, 0, GoreColor);
                    if (Main.rand.NextBool(2))
                    {
                        dust.noGravity = true;
                        dust.scale = 1.2f * NPC.scale;
                    }
                    else
                    {
                        dust.scale = 0.8f * NPC.scale;
                    }
                }
            }
        }
    }
    public class LunaMothItem : ModItem
    {
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Critters/LunaMothItemGlow").Value;
            Color color = new Color(110, 110, 110, 0);
            for (int k = 0; k < 4; k++)
            {
                Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 2 + k * 90));
                Main.spriteBatch.Draw(texture, position + offset, frame, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Critters/LunaMothItemGlow").Value;
            Color color = new Color(110, 110, 110, 0);
            Vector2 drawOrigin = texture.Size() / 2;
            for (int k = 0; k < 4; k++)
            {
                Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 2 + k * 90));
                Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + offset, null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public virtual int BaitPower => 30;
        public virtual int NPC => ModContent.NPCType<LunaMoth>();
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(20);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Frog);
            Item.Size = new Vector2(42, 36);
            Item.makeNPC = NPC;
            Item.value = Item.sellPrice(0, 0, BaitPower, 0); 
            Item.rare = ItemRarityID.Blue;
            Item.bait = BaitPower;
            Item.scale = 0.9f;
        }
        public override void AddRecipes()
        {

        }
    }
}