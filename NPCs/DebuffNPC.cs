using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Xml.Schema;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Steamworks;

namespace SOTS.NPCs.ArtificialDebuffs
{
    public class DebuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        int PlatinumCurse = 0;
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if(PlatinumCurse > 0)
            {
                drawColor = Color.White;
                Color color = new Color(100, 100, 255, 0);
                Texture2D texture = mod.GetTexture("NPCs/ArtificialDebuffs/PlatinumCurse");
                int size = 0;
                for(int plat = PlatinumCurse; plat > 0; plat /= 10)
                {
                    size++;
                }
                Vector2 pos = new Vector2(npc.Center.X, npc.position.Y);
                pos.X += size * ((texture.Width / 11f) - 2 ) / 2f;
                pos.X += 4;
                pos.Y -= 18;
                Vector2 origin = new Vector2(texture.Width / 22, texture.Height/2);
                Rectangle frame;
                for (int plat = PlatinumCurse; plat > 0; plat /= 10)
                {
                    int currentNum = plat % 10;
                    frame = new Rectangle(1 + ((1 + currentNum) * (texture.Width / 11)), 1, texture.Width / 11 - 1, texture.Height - 1);
                    for (int i = 0; i < 6; i++)
                    {
                        float x = Main.rand.Next(-10, 11) * 0.3f;
                        float y = Main.rand.Next(-10, 11) * 0.3f;
                        Main.spriteBatch.Draw(texture, pos - Main.screenPosition + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                    }
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                    pos.X -= (texture.Width / 11f) - 2;
                }
                pos.X -= 4;
                frame = new Rectangle(0, 0, texture.Width / 11, texture.Height);
                for (int i = 0; i < 6; i++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.3f;
                    float y = Main.rand.Next(-10, 11) * 0.3f;
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
            }
            base.PostDraw(npc, spriteBatch, drawColor);
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (npc.immortal)
            {
                return;
            }
            if (item.type == mod.ItemType("PlatinumScythe") || item.type == mod.ItemType("SectionChiefsScythe"))
            {
                if (PlatinumCurse < 10)
                    PlatinumCurse++;
            }
            base.OnHitByItem(npc, player, item, damage, knockback, crit);
        }
        public override void PostAI(NPC npc)
        {
            for(int i = 0; i < PlatinumCurse; i++)
            {
                if(Main.rand.NextBool(20 + i))
                {
                    Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5f), npc.width, npc.height, mod.DustType("CopyDust4"), 0, -2, 200, new Color(), 1f);
                    dust.velocity *= 0.4f;
                    dust.color = new Color(100, 100, 255, 120);
                    dust.noGravity = true;
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.5f;
                }
            }
            float impaledDarts = 0;
            for(int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.friendly && proj.active && proj.type == mod.ProjectileType("PlatinumDart") && proj.ai[1] == npc.whoAmI && proj.timeLeft < 8998)
                {
                    impaledDarts++;
                }
            }
            float mult = 0.125f;
            if(npc.boss == true)
            {
                mult = 0.05f;
            }
            float negativeVeloMult = 1 - 1 / (1 + mult * impaledDarts);
            npc.position -= npc.velocity * negativeVeloMult;
            base.PostAI(npc);
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if(PlatinumCurse > 0)
            {
                npc.lifeRegen -= PlatinumCurse * 6;
            }
            base.UpdateLifeRegen(npc, ref damage);
        }
        public override void NPCLoot(NPC npc)
        {
            base.NPCLoot(npc);
        }
    }
}