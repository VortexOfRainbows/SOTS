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
using System;
using SOTS.Projectiles.BiomeChest;
using SOTS.Buffs;
using SOTS.Projectiles.Minions;
using SOTS.Items;
using SOTS.Dusts;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Pyramid;
using SOTS.Projectiles.Otherworld;
using SOTS.Items.OreItems;
using SOTS.Items.Otherworld.FromChests;
using System.Linq;
using SOTS.NPCs.Constructs;

namespace SOTS.NPCs.ArtificialDebuffs
{
    public class DebuffNPC : GlobalNPC
    {
        public static int[] miniBosses;
        public static void LoadArrays()
        {
            miniBosses = new int[] { NPCID.Mothron, NPCID.IceQueen, NPCID.SantaNK1, NPCID.Everscream, NPCID.MourningWood, NPCID.Pumpking, NPCID.GoblinSummoner, NPCID.MartianSaucerCore, NPCID.LunarTowerSolar, NPCID.LunarTowerNebula, NPCID.LunarTowerStardust, NPCID.LunarTowerVortex };
        }
        public override bool InstancePerEntity => true;
        public int PlatinumCurse = 0;
        public int HarvestCurse = 0;
        public int DestableCurse = 0;
        public int BleedingCurse = 0;
        //public bool hasJustSpawned = true;
        public override bool PreAI(NPC npc)
        {
            //hasJustSpawned = false;
            return base.PreAI(npc);
        }
        public void SendClientChanges(Player player, NPC npc)
        {
            // Send a Mod Packet with the changes.
            var packet = mod.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncGlobalNPC);
            packet.Write((byte)player.whoAmI);
            packet.Write(npc.whoAmI);
            packet.Write(HarvestCurse);
            packet.Write(PlatinumCurse);
            packet.Write(DestableCurse);
            packet.Write(BleedingCurse);
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
                    frame = new Rectangle(1 + ((1 + currentNum) * (texture.Width / 11)), 1, texture.Width / 11 - 2, texture.Height - 2);
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
                pos.Y -= 1;
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
                    frame = new Rectangle(1 + ((1 + currentNum) * (texture.Width / 11)), 1, texture.Width / 11 - 2, texture.Height - 2);
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
                pos.Y -= 1;
                for (int i = 0; i < 6; i++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.3f;
                    float y = Main.rand.Next(-10, 11) * 0.3f;
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, VoidPlayer.soulLootingColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                height += 24;
            }
            if (DestableCurse > 0)
            {
                drawColor = Color.White;
                Color color = new Color(VoidPlayer.destabilizeColor.R, VoidPlayer.destabilizeColor.G, VoidPlayer.destabilizeColor.B, 0);
                Texture2D texture = mod.GetTexture("NPCs/ArtificialDebuffs/Destabilized");
                int size = 0;
                for (int plat = DestableCurse; plat > 0; plat /= 10)
                {
                    size++;
                }
                Vector2 pos = new Vector2(npc.Center.X, npc.position.Y);
                pos.X += size * ((texture.Width / 11f) - 2) / 2f;
                pos.X += 4;
                pos.Y -= height;
                Vector2 origin = new Vector2(texture.Width / 22, texture.Height / 2);
                Rectangle frame;
                for (int plat = DestableCurse; plat > 0; plat /= 10)
                {
                    int currentNum = plat % 10;
                    frame = new Rectangle(1 + ((1 + currentNum) * (texture.Width / 11)), 1, texture.Width / 11 - 2, texture.Height - 2);
                    for (int i = 0; i < 6; i++)
                    {
                        float x = Main.rand.Next(-10, 11) * 0.3f;
                        float y = Main.rand.Next(-10, 11) * 0.3f;
                        Main.spriteBatch.Draw(texture, pos - Main.screenPosition + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                    }
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, VoidPlayer.destabilizeColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                    pos.X -= (texture.Width / 11f) - 2;
                }
                pos.X -= 4;
                frame = new Rectangle(0, 0, texture.Width / 11, texture.Height);
                pos.Y -= 1;
                for (int i = 0; i < 6; i++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.3f;
                    float y = Main.rand.Next(-10, 11) * 0.3f;
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, VoidPlayer.destabilizeColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                height += 24;
            }
            if (BleedingCurse > 0)
            {
                drawColor = new Color(255, 0, 0);
                Color color = new Color(255, 50, 50, 0);
                Texture2D texture = mod.GetTexture("NPCs/ArtificialDebuffs/Bleeding");
                int size = 0;
                for (int plat = BleedingCurse; plat > 0; plat /= 10)
                {
                    size++;
                }
                Vector2 pos = new Vector2(npc.Center.X, npc.position.Y);
                pos.X += size * ((texture.Width / 11f) - 2) / 2f;
                pos.X += 4;
                pos.Y -= height;
                Vector2 origin = new Vector2(texture.Width / 22, texture.Height / 2);
                Rectangle frame;
                for (int plat = BleedingCurse; plat > 0; plat /= 10)
                {
                    int currentNum = plat % 10;
                    frame = new Rectangle(1 + ((1 + currentNum) * (texture.Width / 11)), 1, texture.Width / 11 - 2, texture.Height - 2);
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
                pos.Y -= 1;
                for (int i = 0; i < 6; i++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.3f;
                    float y = Main.rand.Next(-10, 11) * 0.3f;
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture, pos - Main.screenPosition, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                height += 24;
            }
            base.PostDraw(npc, spriteBatch, drawColor);
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            base.OnHitByItem(npc, player, item, damage, knockback, crit);
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (projectile.type == ModContent.ProjectileType<HarvestLock>())
            {
                Player player = Main.player[projectile.owner];
                VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
                int amt = HarvestCost(npc);
                if (!npc.immortal)
                {
                    var index = CombatText.NewText(npc.Hitbox, VoidPlayer.soulLootingColor.MultiplyRGB(Color.White), -amt);
                    if (Main.netMode == NetmodeID.Server && index != 100)
                    {
                        var combatText = Main.combatText[index];
                        NetMessage.SendData(MessageID.CombatTextInt, -1, -1, null, (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, (float)-amt, 0, 0, 0);
                    }
                    HarvestCurse++;
                    voidPlayer.lootingSouls -= amt;
                    if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                        SendClientChanges(player, npc);
                }
            }
        }
        bool hitByRay = false;
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            if (npc.immortal)
            {
                return;
            }
            if(Main.rand.NextFloat(100f) < 5f * DestableCurse)
            {
                if (!crit)
                    crit = true;
                else
                    damage *= 2;
            }
            if (projectile.type == ModContent.ProjectileType<CodeVolley>() || projectile.type == ModContent.ProjectileType<CodeBurst>())
            {
                if(projectile.type == ModContent.ProjectileType<CodeVolley>())
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.7f, 1 + DestableCurse * 0.45f) && DestableCurse < 20)
                        DestableCurse++;
                    if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                        SendClientChanges(player, npc);
                }
                if (projectile.type == ModContent.ProjectileType<CodeBurst>() && projectile.ai[1] != -1)
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.3f, 1 + DestableCurse * 0.45f) && DestableCurse < 20)
                        DestableCurse++;
                    if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                        SendClientChanges(player, npc);
                }

                if (projectile.type == ModContent.ProjectileType<CodeBurst>() && projectile.ai[1] == -1)
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.25f, 1 + DestableCurse * 0.5f) && DestableCurse < 20)
                        DestableCurse++;
                    if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                        SendClientChanges(player, npc);
                }
            }
            if (projectile.type == ModContent.ProjectileType<DestabilizingBeam>() && !hitByRay)
            {
                hitByRay = true;
                DestableCurse += 4;
                if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                    SendClientChanges(player, npc);
            }
            base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.immortal)
            {
                return;
            }
            if (Main.rand.NextFloat(100f) < 5f * DestableCurse)
            {
                if (!crit)
                    crit = true;
                else
                    damage *= 2;
            }
            if (item.type == ModContent.ItemType<PlatinumScythe>() || item.type == ModContent.ItemType<SectionChiefsScythe>())
            {
                if (PlatinumCurse < 10)
                    PlatinumCurse++;
                if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
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
            if (npc.rarity == 5 || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>())
                amt += 10;
            if (miniBosses.Contains(npc.type))
            {
                amt += 50;
            }
            return amt;
        }
        bool pinkied = false;
        public override void PostAI(NPC npc)
        {
            if(npc.immortal)
            {
                return;
            }
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
            for (int i = 0; i < DestableCurse; i++)
            {
                if (Main.rand.NextBool(20 + i * 2))
                {
                    Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5f), npc.width, npc.height, mod.DustType("CodeDust"));
                    dust.velocity *= 0.75f;
                    dust.noGravity = true;
                    dust.scale *= 2.25f;
                }
            }
            float impaledDarts = 0;
            float flowered = 0;
            pinkied = false;
            bool hooked = false;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.friendly && proj.active && proj.type == ModContent.ProjectileType<Projectiles.Minions.FluxSlimeBall>())
                {
                    Projectiles.Minions.FluxSlimeBall slimeBall = proj.modProjectile as Projectiles.Minions.FluxSlimeBall;
                    if (slimeBall != null)
                    {
                        if (slimeBall.targetID == npc.whoAmI && slimeBall.hasHit)
                        {
                            pinkied = true;
                            Projectile owner = Main.projectile[(int)proj.ai[0]];
                            if(owner.type == ModContent.ProjectileType<PetPutridPinkyCrystal>())
                            {
                                Vector2 toOwner = new Vector2(owner.Center.X, owner.position.Y - 8) - new Vector2(npc.Center.X, npc.position.Y + npc.height);
                                float dist = toOwner.Length();
                                toOwner = toOwner.SafeNormalize(Vector2.Zero);
                                float mult = dist * 0.0015f * (npc.boss ? 0.01f : 1);
                                toOwner *= 0.25f + mult;
                                npc.position.X += toOwner.X;
                            }
                        }
                    }
                }
                if (proj.friendly && proj.active && proj.type == ModContent.ProjectileType<Projectiles.Doomhook>())
                {
                    Projectiles.Doomhook hook = proj.modProjectile as Projectiles.Doomhook;
                    if (hook != null)
                    {
                        if (hook.targetID == npc.whoAmI && hook.hasHit && !hook.letGo)
                        {
                            hooked = true;
                            Projectile owner = Main.projectile[(int)proj.ai[0]];
                            if (owner.type == ModContent.ProjectileType<Projectiles.DoomstickHoldOut>())
                            {
                                if(!npc.boss)
                                {
                                    Vector2 toOwner = owner.Center - npc.Center;
                                    float dist = toOwner.Length();
                                    toOwner = toOwner.SafeNormalize(Vector2.Zero);
                                    float mult = dist * 0.0075f;
                                    toOwner *= 4.8f + mult;
                                    npc.position += toOwner;
                                }
                                else
                                {
                                    Player player = Main.player[proj.owner];
                                    Vector2 toNPC = npc.Center - player.Center;
                                    float dist = toNPC.Length();
                                    toNPC = toNPC.SafeNormalize(Vector2.Zero);
                                    float mult = dist * 0.0045f;
                                    toNPC.X *= 5.6f + mult * 1.2f;
                                    toNPC.Y *= 4.2f + mult;
                                    player.velocity += toNPC * 0.1f;
                                }
                            }
                        }
                    }
                }
                if (!proj.friendly && proj.active && proj.type == ModContent.ProjectileType<Projectiles.Ores.PlatinumDart>() && (int)proj.ai[1] == npc.whoAmI && proj.timeLeft < 8998)
                {
                    impaledDarts++;
                }
                if (!proj.friendly && proj.active && proj.type == ModContent.ProjectileType<Rebar>() && (int)proj.ai[1] == npc.whoAmI && proj.timeLeft < 8998)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Vector2 rotate = new Vector2(18, 0).RotatedBy(proj.rotation);
                        Dust dust = Dust.NewDustDirect(proj.Center + rotate - new Vector2(5f), 0, 0, DustID.Blood);
                        dust.velocity *= 0.10f;
                        dust.noGravity = false;
                        dust.scale *= 1.75f;
                    }
                    if ((int)proj.ai[0] == npc.whoAmI)
                    {
                        Player player = Main.player[proj.owner];
                        if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                            SendClientChanges(player, npc);
                        BleedingCurse++;
                        proj.ai[0] = -1;
                    }
                }
                if (!proj.friendly && proj.active && proj.type == ModContent.ProjectileType<FloweringBud>() && proj.timeLeft < 8998)
                {
                    FloweringBud flower = (FloweringBud)proj.modProjectile;
                    if(flower.effected[npc.whoAmI] && !npc.immortal && npc.type != ModContent.NPCType<BloomingHook>() && npc.realLife == -1)
                    {
                        flowered++;
                        if (flowered <= 1)
                        {
                            Player player = Main.player[proj.owner];
                            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
                            if(modPlayer.halfLifeRegen < 3)
                                modPlayer.halfLifeRegen += 3;
                            modPlayer.halfLifeRegen++;
                            if (npc.boss)
                                modPlayer.halfLifeRegen++;
                        }
                        if (flower.enemyIndex == npc.whoAmI)
                        {
                            flowered++;
                        }
                        else
                        {
                            Vector2 toFlower = new Vector2(proj.Center.X, proj.position.Y - 8) - new Vector2(npc.Center.X, npc.position.Y + npc.height);
                            float dist = toFlower.Length();
                            toFlower = toFlower.SafeNormalize(Vector2.Zero);
                            float mult = (dist * 0.025f) * (npc.boss ? 0.01f : 1);
                            toFlower *= 0.5f + mult;
                            npc.position += toFlower;
                        }
                    }
                }
            }
            if (flowered >= 1)
            {
                isFlowered = true;
            }
            else
                isFlowered = false;
            float dartMult = 0.125f;
            if(npc.boss == true)
            {
                dartMult = 0.05f;
            }
            float flowerMult = 0.5f;
            if (npc.boss == true)
            {
                flowerMult = 0.04f;
            }
            float dartVeloMult = 1 / (1 + dartMult * impaledDarts);
            float flowerVeloMult = 1 / (1 + flowerMult * flowered);
            float finalSlowdown = 1f;
            if(npc.HasBuff(ModContent.BuffType<WebbedNPC>()))
            {
                if(!npc.boss)
                    finalSlowdown *= 0.2f;
                else
                    finalSlowdown *= 0.875f;
            }
            if (pinkied)
            {
                if (!npc.boss)
                    finalSlowdown *= 0.625f;
                else
                    finalSlowdown *= 0.95f;
            }
            if(hooked)
            {
                if (!npc.boss)
                    finalSlowdown *= 0.4f;
                else
                    finalSlowdown *= 0.975f;
            }
            npc.position -= npc.velocity * (1 - dartVeloMult * flowerVeloMult * finalSlowdown);
            base.PostAI(npc);
        }
        bool isFlowered = false;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if(PlatinumCurse > 0)
            {
                npc.lifeRegen -= PlatinumCurse * 8;
                damage = PlatinumCurse;
            }
            if (BleedingCurse > 0)
            {
                npc.lifeRegen -= BleedingCurse * 10;
                damage += BleedingCurse / 2;
            }
            if(isFlowered)
            {
                npc.lifeRegen -= 8;
            }
            if(npc.HasBuff(ModContent.BuffType<Infected>()))
            {
                npc.lifeRegen -= 24;
                damage += 1;
            }
            if (npc.HasBuff(ModContent.BuffType<PharaohsCurse>()))
            {
                npc.lifeRegen -= 20;
                damage += 1;
                if(Main.rand.NextBool(3))
                {
                    Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
                    Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5), npc.width, npc.height, ModContent.DustType<CurseDust>());
                    dust.velocity *= 1.25f;
                    dust.velocity += 1f * circular.SafeNormalize(Vector2.Zero);
                    dust.scale = 1.35f;
                    dust.noGravity = true;
                    dust.color = new Color(150, 100, 130, 0);
                    dust.alpha = 70;
                }
            }
            if (pinkied)
            {
                npc.lifeRegen -= 12;
            }
            base.UpdateLifeRegen(npc, ref damage);
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.friendly && proj.active && proj.type == mod.ProjectileType("FloweringBud") && proj.timeLeft < 8998)
                {
                    FloweringBud flower = (FloweringBud)proj.modProjectile;
                    if (flower.effected[npc.whoAmI] && npc.type != ModContent.NPCType<BloomingHook>() && npc.realLife == -1)
                    {
                        Texture2D texture2 = mod.GetTexture("Projectiles/BiomeChest/TangleGrowthVine");
                        float scale = proj.scale;
                        scale *= 0.7f;
                        Vector2 drawPos;
                        Vector2 betweenPositions = npc.Center - proj.Center;
                        Color color = Color.White;
                        float max = betweenPositions.Length() / (texture2.Width * scale);
                        for (int k = 0; k < max; k++)
                        {
                            drawPos = npc.Center + -betweenPositions * (k / max) - Main.screenPosition;
                            Main.spriteBatch.Draw(texture2, drawPos, null, color, betweenPositions.ToRotation(), new Vector2(texture2.Width / 2, texture2.Height / 2), scale, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
            if(npc.HasBuff(ModContent.BuffType<Infected>()) && !npc.immortal)
            {
                Texture2D texture = Main.projectileTexture[ModContent.ProjectileType<Pathogen>()];
                Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                Color color;
                float dimensions = (float)Math.Sqrt(npc.width * npc.height);
                int max = (int)(dimensions / 6 + 3) * 10;
                for (int k = 0; k < max; k++)
                {
                    float total = 3600f / max;
                    float counter = (float)(Main.GlobalTime * 60);
                    float length = dimensions;
                    Vector2 lengthMod = new Vector2(length / 24f, 0).RotatedBy(MathHelper.ToRadians(counter * 5));
                    Vector2 circularLength = new Vector2(length / 16f + lengthMod.X, 0).RotatedBy(MathHelper.ToRadians(k * total + counter * 2));
                    Vector2 circularPos = new Vector2(length, 0).RotatedBy(MathHelper.ToRadians(k * total * 0.1f));
                    Vector2 bonus = new Vector2(circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * total * 0.1f));
                    color = new Color(250, 250, 250, 0);
                    circularPos += bonus;
                    Vector2 drawPos = npc.Center + circularPos - Main.screenPosition;
                    color = npc.GetAlpha(color);
                    Main.spriteBatch.Draw(texture, drawPos, null, color, npc.rotation, drawOrigin, 0.33f, SpriteEffects.None, 0f);
                }
            }
            return base.PreDraw(npc, spriteBatch, drawColor);
        }
        public override void HitEffect(NPC npc, int hitDirection, double damageTaken)
        {
            if (npc.HasBuff(ModContent.BuffType<Infected>()) && npc.life <= 0)
            {
                int index = npc.FindBuffIndex(ModContent.BuffType<Infected>());
                int time = npc.buffTime[index];
                int damage = time / 60;
                Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 14, 0.6f);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<Pathogen>(), damage, 0, Main.myPlayer, -1);
                    }
                }
            }
            if (npc.HasBuff(ModContent.BuffType<PharaohsCurse>()) && npc.life <= 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 circular = new Vector2(4.5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<CurseGhost>(), (int)(npc.lifeMax * 0.1f) + 10, 0, Main.myPlayer, -1);
                }
            }
            base.HitEffect(npc, hitDirection, damageTaken);
        }
        public override void NPCLoot(NPC npc)
        {
            if (npc.SpawnedFromStatue || npc.friendly || npc.lifeMax <= 5)
                return;
            if(HarvestCurse > 0)
            {
                npc.extraValue = 0;
                HarvestCurse--;
                npc.NPCLoot();
            }
            else
            {
                int packCount = 0;
                int baguetteCount = 0;
                for(int i = 0; i < Main.maxItems; i++)
                {
                    Item item = Main.item[i];
                    if(item.type == ModContent.ItemType<HealPack>() || item.type == ModContent.ItemType<ManaPack>())
                    {
                        if(item.active)
                            packCount++;
                    }
                    if (item.type == ModContent.ItemType<BaguetteCrumb>())
                    {
                        if (item.active)
                            baguetteCount++;
                    }
                }
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
                        if(SOTSPlayer.ModPlayer(player).doomDrops && packCount < 40)
                        {
                            int rand = Main.rand.Next(4);
                            if (player.statLifeMax2 > player.statLife)
                                for(int j = 0; j < rand; j++)
                                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<HealPack>(), 1);
                            rand = 3 - rand;
                            if (player.statManaMax2 > player.statMana)
                                for (int j = 0; j < rand; j++)
                                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ManaPack>(), 1);
                        }
                        if (SOTSPlayer.ModPlayer(player).baguetteDrops && baguetteCount < 40)
                        {
                            int rand = Main.rand.Next(2);
                            if(rand >= 1)
                                rand += Main.rand.Next(3) / 2;
                            if (rand >= 2)
                                rand += Main.rand.Next(4) / 3;
                            if (rand >= 3)
                                rand += Main.rand.Next(5) / 4;
                            for (int j = 0; j < rand; j++)
                                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BaguetteCrumb>(), 1);
                        }
                    }
                }
            }
            base.NPCLoot(npc);
        }
    }
}