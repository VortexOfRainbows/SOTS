using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Xml.Schema;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Steamworks;
using SOTS.Void;
using static SOTS.SOTS;

namespace SOTS.NPCs.ArtificialDebuffs
{
    public class DebuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int PlatinumCurse = 0;
        public int HarvestCurse = 0;
        public void SendClientChanges(Player player, NPC npc)
        {
            // Send a Mod Packet with the changes.
            var packet = mod.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncGlobalNPC);
            packet.Write((byte)player.whoAmI);
            packet.Write(npc.whoAmI);
            packet.Write(HarvestCurse);
            packet.Write(PlatinumCurse);
            packet.Send();
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            int height = 18;
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
                pos.Y -= height;
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
                height += 24;
            }
            if (HarvestCurse > 0)
            {
                drawColor = Color.White;
                Color color = new Color(VoidPlayer.soulLootingColor.R, VoidPlayer.soulLootingColor.G, VoidPlayer.soulLootingColor.B, 0);
                Texture2D texture = mod.GetTexture("NPCs/ArtificialDebuffs/Harvesting");
                int size = 0;
                for (int plat = HarvestCurse; plat > 0; plat /= 10)
                {
                    size++;
                }
                Vector2 pos = new Vector2(npc.Center.X, npc.position.Y);
                pos.X += size * ((texture.Width / 11f) - 2) / 2f;
                pos.X += 4;
                pos.Y -= height;
                Vector2 origin = new Vector2(texture.Width / 22, texture.Height / 2);
                Rectangle frame;
                for (int plat = HarvestCurse; plat > 0; plat /= 10)
                {
                    int currentNum = plat % 10;
                    frame = new Rectangle(1 + ((1 + currentNum) * (texture.Width / 11)), 1, texture.Width / 11 - 1, texture.Height - 1);
                    for (int i = 0; i < 6; i++)
                    {
                        float x = Main.rand.Next(-10, 11) * 0.3f;
                        float y = Main.rand.Next(-10, 11) * 0.3f;
                        Main.spriteBatch.Draw(texture, pos - Main.screenPosition + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                    }
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, VoidPlayer.soulLootingColor, 0f, origin, 1f, SpriteEffects.None, 0f);
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
                Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, VoidPlayer.soulLootingColor, 0f, origin, 1f, SpriteEffects.None, 0f);
            }
            base.PostDraw(npc, spriteBatch, drawColor);
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            base.OnHitByItem(npc, player, item, damage, knockback, crit);
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (projectile.type == mod.ProjectileType("HarvestLock"))
            {
                Player player = Main.player[projectile.owner];
                VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
                int amt = HarvestCost(npc);
                if (!npc.immortal)
                {
                    var index = CombatText.NewText(npc.Hitbox, VoidPlayer.soulLootingColor.MultiplyRGB(Color.White), -amt);
                    if (Main.netMode == 2 && index != 100)
                    {
                        var combatText = Main.combatText[index];
                        NetMessage.SendData(81, -1, -1, null, (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, (float)-amt, 0, 0, 0);
                    }
                    HarvestCurse++;
                    voidPlayer.lootingSouls -= amt;
                    if (Main.myPlayer == player.whoAmI && Main.netMode == 1)
                        SendClientChanges(player, npc);
                }
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.immortal)
            {
                return;
            }
            if (item.type == mod.ItemType("PlatinumScythe") || item.type == mod.ItemType("SectionChiefsScythe"))
            {
                if (PlatinumCurse < 10)
                    PlatinumCurse++;
                if (Main.myPlayer == player.whoAmI && Main.netMode == 1)
                    SendClientChanges(player, npc);
            }
            base.ModifyHitByItem(npc, player, item, ref damage, ref knockback, ref crit);
        }
        public static int HarvestCost(NPC npc)
        {
            int amt = 10;
            if (npc.boss)
                amt += 90;
            if (npc.rarity == 1)
                amt += 40;
            if (npc.rarity == 2)
                amt += 30;
            if (npc.rarity == 3)
                amt += 20;
            if (npc.rarity == 4)
                amt += 10;
            if (npc.rarity == 5)
                amt += 10;
            return amt;
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
                if (!proj.friendly && proj.active && proj.type == mod.ProjectileType("PlatinumDart") && (int)proj.ai[1] == npc.whoAmI && proj.timeLeft < 8998)
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
                npc.lifeRegen -= PlatinumCurse * 8;
                damage = PlatinumCurse;
            }
            base.UpdateLifeRegen(npc, ref damage);
        }
        public override void NPCLoot(NPC npc)
        {
            if (npc.SpawnedFromStatue || npc.friendly || npc.lifeMax <= 5)
                return;
            if(HarvestCurse > 0)
            {
                HarvestCurse--;
                npc.NPCLoot();
            }
            else
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && npc.playerInteraction[i])
                    {
                        VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
                        if (voidPlayer.soulsOnKill > 0)
                        {
                            float numberProjectiles = voidPlayer.soulsOnKill * HarvestCost(npc) * 0.1f;
                            for (int j = 0; j < numberProjectiles; j++)
                            {
                                Vector2 perturbedSpeed = new Vector2(-4.5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(180)));
                                Projectile proj = Projectile.NewProjectileDirect(npc.Center, perturbedSpeed, mod.ProjectileType("SoulofLooting"), 0, 0, Main.myPlayer, player.whoAmI, 0);
                                proj.netUpdate = true;
                            }
                        }
                    }
                }
            }
            base.NPCLoot(npc);
        }
    }
}