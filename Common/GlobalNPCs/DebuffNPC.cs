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
using SOTS.Projectiles.Evil;
using static Terraria.ModLoader.ModContent;
using SOTS.NPCs;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Advisor;
using SOTS.NPCs.Boss.Polaris;
using SOTS.Items.AbandonedVillage;
using SOTS.NPCs.Boss.Lux;
using SOTS.Items.Tools;
using SOTS.Projectiles.Chaos;
using SOTS.NPCs.Constructs;

namespace SOTS.Common.GlobalNPCs
{
    public class DebuffNPC : GlobalNPC
    {
        public static int[] nerfBeeNPC;
        public static int[] nerfBeeBoss;
        public static int[] nerfBeeProj;
        public static int[] nerfRealityShatter;
        public static int[] vanillaNPCHasVoidDamage;
        public static int[] miniBosses;
        public static int[] intimidating;
        public static int[] vanillaBoss;
        public static int[] spirits;
        public static int[] Constructs;
        public static int[] Zombies;
        public static void LoadArrays()
        {
            vanillaNPCHasVoidDamage = new int[] { NPCID.BigCrimera, NPCID.LittleCrimera, NPCID.HeavySkeleton, NPCID.BigEater, NPCID.LittleEater, NPCID.BlackSlime, NPCID.BabySlime, NPCID.Slimer2, NPCID.Slimeling, NPCID.EaterofSouls, NPCID.DevourerHead, NPCID.EaterofWorldsHead, NPCID.MotherSlime, NPCID.ChaosBall, NPCID.ArmoredSkeleton, NPCID.DarkMummy, NPCID.Wraith, NPCID.Corruptor, NPCID.SeekerHead,
                NPCID.Werewolf, NPCID.Slimer, NPCID.PossessedArmor, NPCID.VampireBat, NPCID.Vampire, NPCID.SwampThing, NPCID.Crimera, NPCID.Reaper, NPCID.BlueArmoredBones, NPCID.BlueArmoredBonesMace, NPCID.BlueArmoredBonesNoPants, NPCID.BlueArmoredBonesSword, NPCID.Necromancer, NPCID.NecromancerArmored, NPCID.DungeonSpirit, NPCID.Ghost, NPCID.MourningWood, NPCID.Splinterling, NPCID.Pumpking, NPCID.Poltergeist,
                NPCID.Everscream, NPCID.IceQueen, NPCID.StardustCellBig, NPCID.StardustCellSmall, NPCID.CultistBoss, NPCID.CultistDragonHead, NPCID.BloodZombie, NPCID.Drippler
            };
            Constructs = new int[] { NPCType<NatureConstruct>(), NPCType<EarthenConstruct>(), NPCType<PermafrostConstruct>(), NPCType<OtherworldlyConstructHead>(), NPCType<TidalConstruct>(), NPCType<InfernoConstruct>(), NPCType<EvilConstruct>(), NPCType<ChaosConstruct>() };
            spirits = new int[] { NPCType<NPCs.Constructs.NatureSpirit>(), NPCType<NPCs.Constructs.EarthenSpirit>(), NPCType<NPCs.Constructs.PermafrostSpirit>(), NPCType<NPCs.Constructs.TidalSpirit>(), NPCType<NPCs.Constructs.EvilSpirit>(), NPCType<NPCs.Constructs.InfernoSpirit>(), NPCType<NPCs.Constructs.ChaosSpirit>(), NPCType<Lux>(), NPCType<FakeLux>() };
            intimidating = new int[] { NPCType<NatureConstruct>(), NPCType<EarthenConstruct>(), NPCType<PermafrostConstruct>(), NPCType<OtherworldlyConstructHead>(), NPCType<TidalConstruct>(), NPCType<EvilConstruct>(), NPCType<InfernoConstruct>(), NPCType<ChaosConstruct>(),
                NPCType<PutridPinkyPhase2>(), NPCType<NPCs.Boss.Curse.PharaohsCurse>(), NPCType<TheAdvisorHead>(), NPCType<Polaris>(), NPCType<SubspaceSerpentHead>()};
            vanillaBoss = new int[] { NPCID.KingSlime, NPCID.EyeofCthulhu, NPCID.EaterofWorldsHead, NPCID.BrainofCthulhu, NPCID.QueenBee, NPCID.SkeletronHead, NPCID.WallofFlesh, NPCID.Spazmatism,
                NPCID.Retinazer, NPCID.TheDestroyer, NPCID.SkeletronPrime, NPCID.Plantera, NPCID.Golem, NPCID.DukeFishron, NPCID.CultistBoss, NPCID.MoonLordCore, NPCID.HallowBoss, NPCID.QueenSlimeBoss};
            miniBosses = new int[] { NPCID.Mothron, NPCID.IceQueen, NPCID.SantaNK1, NPCID.Everscream, NPCID.MourningWood, NPCID.Pumpking, NPCID.GoblinSummoner, NPCID.MartianSaucerCore, NPCID.LunarTowerSolar, NPCID.LunarTowerNebula, NPCID.LunarTowerStardust, NPCID.LunarTowerVortex };
            nerfBeeNPC = new int[] { NPCType<PutridHook>() };
            nerfBeeBoss = new int[] { NPCType<PutridPinkyPhase2>(), NPCType<NPCs.Boss.Curse.PharaohsCurse>(), NPCType<TheAdvisorHead>() };
            nerfBeeProj = new int[] { ProjectileID.Bee, ProjectileID.GiantBee };
            nerfRealityShatter = new int[] { NPCType<SubspaceSerpentBody>(), NPCType<SubspaceSerpentHead>(), NPCType<SubspaceSerpentTail>() };
            Zombies = new int[] { NPCID.Zombie, NPCID.ZombieDoctor, NPCID.ZombieElf, NPCID.ZombieElfBeard,
                NPCID.ZombieElfGirl, NPCID.ZombieEskimo, NPCID.ZombieMushroom, NPCID.ZombieMushroomHat, NPCID.ZombiePixie, NPCID.ZombieRaincoat,
                NPCID.ZombieSuperman, NPCID.ZombieSweater, NPCID.ZombieXmas, NPCID.ArmedZombie, NPCID.ArmedZombieCenx, NPCID.ArmedZombieEskimo,
                NPCID.ArmedZombiePincussion, NPCID.ArmedZombieSlimed, NPCID.ArmedZombieSwamp, NPCID.ArmedZombieTwiggy, NPCID.BaldZombie, NPCID.BigBaldZombie, NPCID.BigFemaleZombie,
                NPCID.BigPincushionZombie, NPCID.BigRainZombie, NPCID.BigSlimedZombie, NPCID.BigSwampZombie, NPCID.BigTwiggyZombie, NPCID.BigZombie,
                NPCID.FemaleZombie, NPCID.PincushionZombie, NPCID.SlimedZombie, NPCID.SmallBaldZombie, NPCID.SmallFemaleZombie, NPCID.SmallPincushionZombie,
                NPCID.SmallRainZombie, NPCID.SmallSlimedZombie, NPCID.SmallSwampZombie, NPCID.SmallTwiggyZombie, NPCID.SmallZombie, NPCID.SwampZombie, NPCID.TwiggyZombie};
        }
        public override bool InstancePerEntity => true;
        public int PlatinumCurse = 0;
        public int HarvestCurse = 0;
        public int DestableCurse = 0;
        public int BleedingCurse = 0;
        public int timeFrozen = 0;
        public bool netUpdateTime = false;
        public bool frozen = false;
        public float aiSpeedCounter = 0;
        public float aiSpeedMultiplier = 1;
        public const float timeBeforeFullFreeze = 30f;
        public float frozenForTime = 0;
        //public bool hasJustSpawned = true;
        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if (spirits.Contains(npc.type) || npc.type == NPCType<NPCs.Constructs.OtherworldlySpirit>())
            {
                int vDamage = 0;
                int debuffTime = 120;
                if(npc.type == NPCType<NPCs.Constructs.NatureSpirit>())
                {
                    debuffTime = 120;
                    vDamage = 5;
                }
                if (npc.type == NPCType<NPCs.Constructs.EarthenSpirit>())
                {
                    debuffTime = 150;
                    vDamage = 8;
                }
                if (npc.type == NPCType<NPCs.Constructs.PermafrostSpirit>() || npc.type == NPCType<NPCs.Constructs.OtherworldlySpirit>())
                {
                    debuffTime = 180;
                    vDamage = 15;
                }
                if (npc.type == NPCType<NPCs.Constructs.TidalSpirit>() || npc.type == NPCType<NPCs.Constructs.ChaosSpirit>())
                {
                    debuffTime = 210;
                    vDamage = 25;
                }
                if (npc.type == NPCType<NPCs.Constructs.EvilSpirit>())
                {
                    debuffTime = 240;
                    vDamage = 30;
                }
                if (npc.type == NPCType<NPCs.Constructs.InfernoSpirit>())
                {
                    debuffTime = 270;
                    vDamage = 40;
                }
                if (npc.type == NPCType<Lux>() || npc.type == NPCType<FakeLux>())
                {
                    debuffTime = 300;
                    vDamage = 50;
                }
                VoidPlayer.VoidBurn(Mod, target, vDamage, debuffTime);
            }
            bool canHappen = (Main.dayTime ? (Main.expertMode ? Main.rand.NextBool(3) : Main.rand.NextBool(5)) : Main.rand.NextBool(2));
            if((npc.type == NPCID.DungeonGuardian || vanillaNPCHasVoidDamage.Contains(npc.type)) && canHappen && target.whoAmI == Main.myPlayer)
            {
                int vDamage = 1 + npc.damage / 6; //void damage is 6th of the normal npc damage
                if(npc.type == NPCID.DungeonGuardian)
                {
                    vDamage = 6969;
                }
                VoidPlayer.VoidDamage(Mod, target, vDamage);
            }
            if(npc.type == NPCType<Teratoma>() || npc.type == NPCType<Maligmor>() || npc.type == NPCType<MaligmorChild>() || npc.type == NPCType<Ghast>() || npc.type == NPCType<BleedingGhast>() || npc.type == NPCType<FlamingGhast>())
            {
                int vDamage = 8;
                if (npc.type == NPCType<Teratoma>() || npc.type == NPCType<BleedingGhast>() || npc.type == NPCType<FlamingGhast>())
                    vDamage = 20;
                VoidPlayer.VoidDamage(Mod, target, vDamage);
            }
            base.OnHitPlayer(npc, target, damage, crit);
        }
        public override bool PreAI(NPC npc)
        {
            if(netUpdateTime)
            {
                SendClientChanges(null, npc, 1); //update frozen Time
                netUpdateTime = false;
            }
            if (intimidating.Contains(npc.type) || spirits.Contains(npc.type) || vanillaBoss.Contains(npc.type))
            {
                bool canIntimidate = true;
                if (npc.type == NPCType<TheAdvisorHead>() && npc.dontTakeDamage)
                    canIntimidate = false;
                if(canIntimidate)
                {
                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        Player player = Main.player[i];
                        if (player.Distance(npc.Center) <= 2000)
                        {
                            player.AddBuff(BuffType<IntimidatingPresence>(), 6, true);
                        }
                    }
                }
            }
            //hasJustSpawned = false;
            return base.PreAI(npc);
        }
        public static void SetTimeFreeze(Player player, NPC npc, int time)
        {
            bool worm = npc.realLife != -1;
            if (worm)
                return;
            DebuffNPC instancedNPC = npc.GetGlobalNPC<DebuffNPC>();
            instancedNPC.timeFrozen = time;
            instancedNPC.frozen = false;
            if ((player == null && Main.netMode == NetmodeID.Server) || Main.netMode != NetmodeID.SinglePlayer)
                instancedNPC.SendClientChanges(player, npc, 1);
        }
        public static bool UpdateWhileFrozen(NPC npc, int i)
        {
            NPC realNPC = npc;
            if (npc.realLife != -1)
                realNPC = Main.npc[npc.realLife];
            DebuffNPC debuffNPC = realNPC.GetGlobalNPC<DebuffNPC>();
            if (debuffNPC.timeFrozen > 0 || debuffNPC.frozen)
            {
                if (npc.immune[Main.myPlayer] > 0)
                    npc.immune[Main.myPlayer]--;
                if (!debuffNPC.frozen)
                {
                    if (debuffNPC.aiSpeedMultiplier > 0)
                    {
                        debuffNPC.aiSpeedMultiplier -= 1 / timeBeforeFullFreeze;
                    }
                    else
                    {
                        debuffNPC.aiSpeedMultiplier = 0;
                        debuffNPC.frozen = true;
                    }
                }
                else
                {
                    debuffNPC.frozenForTime++;
                    if (debuffNPC.timeFrozen > 1)
                    {
                        debuffNPC.timeFrozen--;
                    }
                    else
                    {
                        debuffNPC.aiSpeedMultiplier += 1 / timeBeforeFullFreeze;
                        if (debuffNPC.aiSpeedMultiplier > 1)
                        {
                            debuffNPC.aiSpeedMultiplier = 1;
                            debuffNPC.timeFrozen = 0;
                            debuffNPC.frozen = false;
                        }
                    }
                }
                if (debuffNPC.timeFrozen == 0 && Main.netMode == NetmodeID.Server)
                {
                    debuffNPC.netUpdateTime = true;
                }
                npc.whoAmI = i;
            }
            else
            {
                if (debuffNPC.frozenForTime > 0)
                {
                    debuffNPC.frozenForTime--;
                }
                else
                    debuffNPC.frozenForTime = 0;
                debuffNPC.frozen = false;
            }
            debuffNPC.aiSpeedCounter += debuffNPC.aiSpeedMultiplier;
            if (debuffNPC.aiSpeedCounter >= 1)
            {
                debuffNPC.aiSpeedCounter -= 1;
            }
            else
                return true;
            return false;
        }
        public void SendClientChanges(Player player, NPC npc, int type = 0)
        {
            // Send a Mod Packet with the changes.
            if (type == 0) //should be called by player
            {
                byte playerWhoAmI = (byte)player.whoAmI;
                var packet = Mod.GetPacket();
                packet.Write((byte)SOTSMessageType.SyncGlobalNPC);
                packet.Write(playerWhoAmI);
                packet.Write(npc.whoAmI);
                packet.Write(HarvestCurse);
                packet.Write(PlatinumCurse);
                packet.Write(DestableCurse);
                packet.Write(BleedingCurse);
                packet.Send();
            }
            else if(type == 1) //can be called by server or player
            {
                int playerWhoAmI = player != null ? player.whoAmI : -1;
                var packet = Mod.GetPacket();
                packet.Write((byte)SOTSMessageType.SyncGlobalNPCTime);
                packet.Write(playerWhoAmI);
                packet.Write(npc.whoAmI);
                packet.Write(timeFrozen);
                packet.Write(frozen);
                packet.Send();
            }
        }
        public void DrawTimeFreeze(NPC npc, SpriteBatch spriteBatch)
        {
            float alphaMult = 1 - aiSpeedMultiplier;
            int type = npc.whoAmI % 3 + 1;
            Texture2D ring1 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Common/GlobalNPCs/FreezeSpiral" + type);
            Vector2 ringOrigin = new Vector2(ring1.Width / 2, ring1.Height / 2);
            Vector2 drawPos = new Vector2(npc.Center.X, npc.Center.Y + npc.gfxOffY) + /*npc.VisualPosition*/ - Main.screenPosition;
            Color color = new Color(70, 0, 105, 0);
            float secondsHandMult = Main.GameUpdateCount / 90f;
            float drawDimensions = npc.Size.Length();
            float scale = 0.02f + 1.04f * (drawDimensions / 800f);
            float rotation1 = secondsHandMult * MathHelper.TwoPi;
            int direction = npc.direction;
            if (direction == 0)
                direction = 1;
            for (int i = 0; i < 2; i++)
            {
                spriteBatch.Draw(ring1, drawPos, null, color * alphaMult, rotation1 * direction, ringOrigin, scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                color = new Color(40, 0, 100, 0);
                rotation1 = secondsHandMult * MathHelper.TwoPi * 0.5f;
                scale *= 0.97f;
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int height = 18;
            if(PlatinumCurse > 0)
            {
                drawColor = Color.White;
                Color color = new Color(100, 100, 255, 0);
                Texture2D texture = Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/PlatinumCurse").Value;
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
                    /*for (int i = 0; i < 6; i++)
                    {
                        float x = Main.rand.Next(-10, 11) * 0.3f;
                        float y = Main.rand.Next(-10, 11) * 0.3f;
                        Main.spriteBatch.Draw(texture, pos - screenPos + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                    }*/
                    Main.spriteBatch.Draw(texture, pos - screenPos, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                    pos.X -= (texture.Width / 11f) - 2;
                }
                pos.X -= 4;
                frame = new Rectangle(0, 0, texture.Width / 11, texture.Height);
                pos.Y -= 1;
                /*for (int i = 0; i < 6; i++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.3f;
                    float y = Main.rand.Next(-10, 11) * 0.3f;
                    Main.spriteBatch.Draw(texture, pos - screenPos + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }*/
                Main.spriteBatch.Draw(texture, pos - screenPos, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                height += 24;
            }
            if (HarvestCurse > 0)
            {
                drawColor = Color.White;
                Color color = new Color(VoidPlayer.soulLootingColor.R, VoidPlayer.soulLootingColor.G, VoidPlayer.soulLootingColor.B, 0);
                Texture2D texture = Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/Harvesting").Value;
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
                    /*for (int i = 0; i < 6; i++)
                    {
                        float x = Main.rand.Next(-10, 11) * 0.3f;
                        float y = Main.rand.Next(-10, 11) * 0.3f;
                        Main.spriteBatch.Draw(texture, pos - screenPos + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                    }*/
                    Main.spriteBatch.Draw(texture, pos - screenPos, frame, VoidPlayer.soulLootingColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                    pos.X -= (texture.Width / 11f) - 2;
                }
                pos.X -= 4;
                frame = new Rectangle(0, 0, texture.Width / 11, texture.Height);
                pos.Y -= 1;
                /*for (int i = 0; i < 6; i++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.3f;
                    float y = Main.rand.Next(-10, 11) * 0.3f;
                    Main.spriteBatch.Draw(texture, pos - screenPos + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }*/
                Main.spriteBatch.Draw(texture, pos - screenPos, frame, VoidPlayer.soulLootingColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                height += 24;
            }
            if (DestableCurse > 0)
            {
                drawColor = Color.White;
                Color color = new Color(VoidPlayer.destabilizeColor.R, VoidPlayer.destabilizeColor.G, VoidPlayer.destabilizeColor.B, 0);
                Texture2D texture = Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/Destabilized").Value;
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
                    /*for (int i = 0; i < 6; i++)
                    {
                        float x = Main.rand.Next(-10, 11) * 0.3f;
                        float y = Main.rand.Next(-10, 11) * 0.3f;
                        Main.spriteBatch.Draw(texture, pos - screenPos + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                    }*/
                    Main.spriteBatch.Draw(texture, pos - screenPos, frame, VoidPlayer.destabilizeColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                    pos.X -= (texture.Width / 11f) - 2;
                }
                pos.X -= 4;
                frame = new Rectangle(0, 0, texture.Width / 11, texture.Height);
                pos.Y -= 1;
                /*for (int i = 0; i < 6; i++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.3f;
                    float y = Main.rand.Next(-10, 11) * 0.3f;
                    Main.spriteBatch.Draw(texture, pos - screenPos + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }*/
                Main.spriteBatch.Draw(texture, pos - screenPos, frame, VoidPlayer.destabilizeColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                height += 24;
            }
            if (BleedingCurse > 0)
            {
                drawColor = new Color(255, 0, 0);
                Color color = new Color(255, 50, 50, 0);
                Texture2D texture = Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/Bleeding").Value;
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
                    /*for (int i = 0; i < 6; i++)
                    {
                        float x = Main.rand.Next(-10, 11) * 0.3f;
                        float y = Main.rand.Next(-10, 11) * 0.3f;
                        Main.spriteBatch.Draw(texture, pos - screenPos + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                    }*/
                    Main.spriteBatch.Draw(texture, pos - screenPos, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                    pos.X -= (texture.Width / 11f) - 2;
                }
                pos.X -= 4;
                frame = new Rectangle(0, 0, texture.Width / 11, texture.Height);
                pos.Y -= 1;
                /*for (int i = 0; i < 6; i++)
                {
                    float x = Main.rand.Next(-10, 11) * 0.3f;
                    float y = Main.rand.Next(-10, 11) * 0.3f;
                    Main.spriteBatch.Draw(texture, pos - screenPos + new Vector2(x, y), frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }*/
                Main.spriteBatch.Draw(texture, pos - screenPos, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                height += 24;
            }
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            lastHitWasCrit = crit;
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            lastHitWasCrit = crit;
            if (projectile.type == ProjectileType<HarvestLock>())
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
        bool lastHitWasCrit = false;
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            if (npc.HasBuff(BuffType<Shattered>()) && projectile.CountsAsClass(DamageClass.Melee) && projectile.type != ProjectileType<Projectiles.Evil.AncientSteelHalberd>())
            {
                int ignoreDefense = ((npc.defense + 1) / 2);
                damage += ignoreDefense;
                crit = true;
                npc.DelBuff(npc.FindBuffIndex(BuffType<Shattered>()));
            }
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
            if (projectile.type == ProjectileType<CodeVolley>() || projectile.type == ProjectileType<CodeBurst>())
            {
                if(projectile.type == ProjectileType<CodeVolley>())
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.7f, 1 + DestableCurse * 0.45f) && DestableCurse < 20)
                        DestableCurse++;
                    if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                        SendClientChanges(player, npc);
                }
                if (projectile.type == ProjectileType<CodeBurst>() && projectile.ai[1] != -1)
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.3f, 1 + DestableCurse * 0.45f) && DestableCurse < 20)
                        DestableCurse++;
                    if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                        SendClientChanges(player, npc);
                }

                if (projectile.type == ProjectileType<CodeBurst>() && projectile.ai[1] == -1)
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.25f, 1 + DestableCurse * 0.5f) && DestableCurse < 20)
                        DestableCurse++;
                    if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                        SendClientChanges(player, npc);
                }
            }
            if(projectile.type == ProjectileType<DeathSpiralProj>() || (projectile.type == ProjectileType<BloodSpark>() && crit))
            {
                bool worm = npc.realLife != -1;
                float baseChance = projectile.type == ProjectileType<BloodSpark>() ? 1.1f : 0.2f;
                int baseStacks = 1;
                if (worm)
                {
                    baseStacks = 2;
                    baseChance = 0.1f;
                }
                if (Main.rand.NextFloat(1) < baseChance / (baseStacks + BleedingCurse)) //1 in 10, drops lower ever time
                    BleedingCurse++;
                if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                    SendClientChanges(player, npc);
            }
            if (projectile.type == ProjectileType<DestabilizingBeam>() && !hitByRay)
            {
                hitByRay = true;
                DestableCurse += 4;
                if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                    SendClientChanges(player, npc);
            }
            if (nerfBeeProj.Contains(projectile.type))
            {
                if (nerfBeeBoss.Contains(npc.type))
                    damage = (int)(damage * 0.8f);
                if (nerfBeeNPC.Contains(npc.type))
                    damage = (int)(damage * 0.6f);
            }
            if(projectile.type == ProjectileType<RealityShatter>())
            {
                if (nerfRealityShatter.Contains(npc.type))
                    damage = (int)(damage * 0.3f);
                else if(npc.boss)
                {
                    damage = (int)(damage * 0.8f);
                }
            }
            base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.immortal)
            {
                return;
            }
            if (npc.HasBuff(BuffType<Shattered>()) && item.CountsAsClass(DamageClass.Melee))
            {
                int ignoreDefense = ((npc.defense + 1) / 2);
                damage += ignoreDefense;
                crit = true;
                npc.DelBuff(npc.FindBuffIndex(BuffType<Shattered>()));
            }
            if (Main.rand.NextFloat(100f) < 5f * DestableCurse)
            {
                if (!crit)
                    crit = true;
                else
                    damage *= 2;
            }
            if (item.type == ItemType<PlatinumScythe>() || item.type == ItemType<SectionChiefsScythe>())
            {
                if (PlatinumCurse < 10)
                    PlatinumCurse++;
                if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                    SendClientChanges(player, npc);
            }
            if ((item.type == ItemType<AncientSteelSword>() || item.type == ItemType<AncientSteelGreatPickaxe>() || item.type == ItemType<AncientSteelGreatHamaxe>()) && crit)
            {
                bool worm = npc.realLife != -1;
                float baseChance = 1f;
                int baseStacks = 1;
                if (worm)
                {
                    baseStacks = 2;
                    baseChance = 0.1f;
                }
                if (Main.rand.NextFloat(1) < baseChance / (baseStacks + BleedingCurse * 0.7f)) //1 in 1, drops lower ever time
                    BleedingCurse++;
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
            if (npc.rarity == 5 || npc.type == NPCType<OtherworldlyConstructHead2>())
                amt += 10;
            if (miniBosses.Contains(npc.type))
            {
                amt += 50;
            }
            return amt;
        }
        bool pinkied = false;
        bool shattered = false;
        public override void PostAI(NPC npc)
        {
            if(npc.immortal)
            {
                return;
            }
            int indexShatter = npc.FindBuffIndex(BuffType<Shattered>());
            if (indexShatter >= 0 && npc.buffTime[indexShatter] < 4)
                shattered = false;
            else if(npc.HasBuff(BuffType<Shattered>()))
            {
                if(!shattered)
                {
                    SOTSUtils.PlaySound(SoundID.Item23, (int)npc.Center.X, (int)npc.Center.Y, 1.0f, -0.3f);
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 circular = new Vector2(0, 3).RotatedBy(MathHelper.ToRadians(i * 10));
                        Dust dust = Dust.NewDustDirect(npc.Center - new Vector2(4, 4) + circular.SafeNormalize(Vector2.Zero) * (npc.Size.Length()) * 0.5f, 0, 0, DustID.Silver, 0, 0, 100, Scale: Main.rand.NextFloat(1.2f, 1.6f));
                        dust.velocity += circular * Main.rand.NextFloat(0.8f, 1.2f);
                        dust.velocity *= 0.2f;
                        dust.noGravity = true;
                        dust.color = Color.LightGray;
                    }
                    shattered = true;
                }
            }
            else if(shattered)
            {
                SOTSUtils.PlaySound(SoundID.Tink, (int)npc.Center.X, (int)npc.Center.Y, 1f, -0.6f);
                for (int i = 0; i < 12; i++)
                {
                    Vector2 circular = new Vector2(0, 3).RotatedBy(MathHelper.ToRadians(i * 30 + Main.rand.NextFloat(-8f, 8f)));
                    Dust dust = Dust.NewDustDirect(npc.Center - new Vector2(4, 4) + circular.SafeNormalize(Vector2.Zero) * (npc.Size.Length()) * 0.3f, 0, 0, DustID.Silver, 0, 0, 100, Scale: Main.rand.NextFloat(1.2f, 1.6f));
                    dust.velocity += circular * Main.rand.NextFloat(0.1f, 1f);
                    dust.velocity *= 0.2f;
                    dust.noGravity = true;
                    dust.color = Color.LightGray;
                }
                shattered = false;
            }
            for(int i = 0; i < PlatinumCurse; i++)
            {
                if(Main.rand.NextBool(20 + i))
                {
                    Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5f), npc.width, npc.height, ModContent.DustType<CopyDust4>(), 0, -2, 200, new Color(), 1f);
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
                    Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5f), npc.width, npc.height, ModContent.DustType<CopyDust4>());
                    dust.velocity *= 0.75f;
                    dust.noGravity = true;
                    dust.scale *= 2.25f;
                }
            }
            float impaledDarts = 0;
            float flowered = 0;
            pinkied = false;
            bool hooked = false;
            bool darkArmed = false;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.friendly && proj.active && proj.type == ProjectileType<Projectiles.Minions.FluxSlimeBall>())
                {
                    Projectiles.Minions.FluxSlimeBall slimeBall = proj.ModProjectile as Projectiles.Minions.FluxSlimeBall;
                    if (slimeBall != null)
                    {
                        if (slimeBall.targetID == npc.whoAmI && slimeBall.hasHit)
                        {
                            pinkied = true;
                            Projectile owner = Main.projectile[(int)proj.ai[0]];
                            if(owner.type == ProjectileType<PetPutridPinkyCrystal>())
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
                if (proj.friendly && proj.active && proj.type == ProjectileType<Projectiles.Doomhook>())
                {
                    Projectiles.Doomhook hook = proj.ModProjectile as Projectiles.Doomhook;
                    if (hook != null)
                    {
                        if (hook.targetID == npc.whoAmI && hook.hasHit && !hook.letGo)
                        {
                            hooked = true;
                            Projectile owner = Main.projectile[(int)proj.ai[0]];
                            if (owner.type == ProjectileType<Projectiles.DoomstickHoldOut>())
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
                if (!proj.friendly && proj.active && proj.type == ProjectileType<Projectiles.Ores.PlatinumDart>() && (int)proj.ai[1] == npc.whoAmI && proj.timeLeft < 8998)
                {
                    impaledDarts++;
                }
                if (!proj.friendly && proj.active && proj.type == ProjectileType<Rebar>() && (int)proj.ai[1] == npc.whoAmI && proj.timeLeft < 8998)
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
                if (!proj.friendly && proj.active && (proj.type == ProjectileType<FloweringBud>() || proj.type == ProjectileType<EvilGrowth>()) && proj.timeLeft < 8998)
                {
                    bool contains = false;
                    int index = -1;
                    bool isFlower = false;
                    if (proj.type == ProjectileType<FloweringBud>())
                    {
                        FloweringBud flower = proj.ModProjectile as FloweringBud;
                        if (flower.effected[npc.whoAmI])
                            contains = true;
                        index = flower.enemyIndex;
                        isFlower = true;
                    }
                    if (proj.type == ProjectileType<EvilGrowth>())
                    {
                        EvilGrowth evil = proj.ModProjectile as EvilGrowth;
                        if (evil.effected[npc.whoAmI])
                            contains = true;
                    }
                    if(contains && !npc.immortal && npc.type != NPCType<BloomingHook>() && npc.realLife == -1)
                    {
                        if(isFlower)
                        {
                            flowered++;
                        }
                        else
                            darkArmed = true;
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
                        if (index == npc.whoAmI)
                        {
                            if (isFlower)
                                flowered++;
                        }
                        else
                        {
                            float pullRate = 0.5f;
                            float distMult = 0.025f;
                            if (!isFlower && !npc.boss)
                                distMult = 0.04f;
                            Vector2 toFlower = new Vector2(proj.Center.X, proj.position.Y - 8) - new Vector2(npc.Center.X, npc.position.Y + npc.height);
                            float dist = toFlower.Length();
                            toFlower = toFlower.SafeNormalize(Vector2.Zero);
                            float mult = (dist * distMult) * (npc.boss ? 0.01f : 1);
                            if (!isFlower)
                                pullRate = 0.65f;
                            toFlower *= pullRate + mult;
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
            if(npc.HasBuff(BuffType<WebbedNPC>()) || darkArmed)
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
                damage += BleedingCurse;
            }
            if(isFlowered)
            {
                npc.lifeRegen -= 8;
            }
            if(npc.HasBuff(BuffType<Infected>()))
            {
                npc.lifeRegen -= 24;
                damage += 1;
            }
            if (npc.HasBuff(BuffType<PharaohsCurse>()))
            {
                npc.lifeRegen -= 20;
                damage += 1;
                if(Main.rand.NextBool(3))
                {
                    Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
                    Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5), npc.width, npc.height, DustType<CurseDust>());
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
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.friendly && proj.active && (proj.type == ProjectileType<FloweringBud>() || proj.type == ProjectileType<EvilGrowth>()) && proj.timeLeft < 8998)
                {
                    bool contains = false;
                    if(proj.type == ProjectileType<FloweringBud>())
                    {
                        FloweringBud flower = proj.ModProjectile as FloweringBud;
                        if (flower.effected[npc.whoAmI])
                            contains = true;
                    }
                    if(proj.type == ProjectileType<EvilGrowth>())
                    {
                        EvilGrowth evil = proj.ModProjectile as EvilGrowth;
                        if (evil.effected[npc.whoAmI])
                            contains = true;
                    }
                    if (contains && npc.type != NPCType<BloomingHook>() && npc.realLife == -1)
                    {
                        Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/BiomeChest/TangleGrowthVine").Value;
                        Color color = Color.White;
                        if (proj.type == ProjectileType<EvilGrowth>())
                        {
                            color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
                            texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Evil/EvilArm").Value;
                        }
                        float scale = proj.scale;
                        if (proj.type == ProjectileType<FloweringBud>())
                            scale *= 0.7f;
                        else
                        {
                            scale *= (proj.timeLeft / (float)EvilGrowth.MaxTimeLeft);
                        }
                        Vector2 drawPos;
                        Vector2 betweenPositions = npc.Center - proj.Center;
                        float max = betweenPositions.Length() / (texture2.Width * scale);
                        for (int k = 0; k < max; k++)
                        {
                            drawPos = npc.Center + -betweenPositions * (k / max) - screenPos;
                            if (k == 0 && proj.type == ProjectileType<EvilGrowth>())
                            {
                                Texture2D texture3 = Mod.Assets.Request<Texture2D>("Projectiles/Evil/EvilHand").Value;
                                Main.spriteBatch.Draw(texture3, drawPos, null, color, betweenPositions.ToRotation() + MathHelper.Pi/2, new Vector2(texture3.Width / 2, texture3.Height / 2), scale * 1.4f, SpriteEffects.None, 0f);
                            }
                            else
                            {
                                Main.spriteBatch.Draw(texture2, drawPos, null, color, betweenPositions.ToRotation(), new Vector2(texture2.Width / 2, texture2.Height / 2), scale, SpriteEffects.None, 0f);
                            }
                        }
                    }
                }
            }
            if(npc.HasBuff(BuffType<Infected>()) && !npc.immortal)
            {
                Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[ProjectileType<Pathogen>()].Value;
                Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                Color color;
                float dimensions = (float)Math.Sqrt(npc.width * npc.height);
                int max = (int)(dimensions / 6 + 3) * 10;
                for (int k = 0; k < max; k++)
                {
                    float total = 3600f / max;
                    float counter = (float)(Main.GlobalTimeWrappedHourly * 60);
                    float length = dimensions;
                    Vector2 lengthMod = new Vector2(length / 24f, 0).RotatedBy(MathHelper.ToRadians(counter * 5));
                    Vector2 circularLength = new Vector2(length / 16f + lengthMod.X, 0).RotatedBy(MathHelper.ToRadians(k * total + counter * 2));
                    Vector2 circularPos = new Vector2(length, 0).RotatedBy(MathHelper.ToRadians(k * total * 0.1f));
                    Vector2 bonus = new Vector2(circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * total * 0.1f));
                    color = new Color(250, 250, 250, 0);
                    circularPos += bonus;
                    Vector2 drawPos = npc.Center + circularPos - screenPos;
                    color = npc.GetAlpha(color);
                    Main.spriteBatch.Draw(texture, drawPos, null, color, npc.rotation, drawOrigin, 0.33f, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
        public override void HitEffect(NPC npc, int hitDirection, double damageTaken)
        {
            if (npc.HasBuff(BuffType<Infected>()) && npc.life <= 0)
            {
                int index = npc.FindBuffIndex(BuffType<Infected>());
                int time = npc.buffTime[index];
                int damage = time / 60;
                if (Main.netMode != NetmodeID.Server)
                    SOTSUtils.PlaySound(SoundID.Item14, (int)npc.Center.X, (int)npc.Center.Y, 0.6f);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                        Projectile.NewProjectile(npc.GetSource_Misc("SOTS:HurtWhileDebuffed"), npc.Center.X, npc.Center.Y, circular.X, circular.Y, ProjectileType<Pathogen>(), damage, 0, Main.myPlayer, -1);
                    }
                }
            }
            if (npc.HasBuff(BuffType<PharaohsCurse>()) && npc.life <= 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 circular = new Vector2(4.5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                    Projectile.NewProjectile(npc.GetSource_Misc("SOTS:HurtWhileDebuffed"), npc.Center.X, npc.Center.Y, circular.X, circular.Y, ProjectileType<CurseGhost>(), (int)(npc.lifeMax * 0.1f) + 10, 0, Main.myPlayer, -1);
                }
            }
            if(npc.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                    return;
                if (shattered)
                {
                    SOTSUtils.PlaySound(SoundID.Tink, (int)npc.Center.X, (int)npc.Center.Y, 1f, -0.6f);
                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 circular = new Vector2(0, 3).RotatedBy(MathHelper.ToRadians(i * 30 + Main.rand.NextFloat(-8f, 8f)));
                        Dust dust = Dust.NewDustDirect(npc.Center - new Vector2(4, 4) + circular.SafeNormalize(Vector2.Zero) * (npc.Size.Length()) * 0.3f, 0, 0, DustID.Silver, 0, 0, 100, Scale: Main.rand.NextFloat(1.2f, 1.6f));
                        dust.velocity += circular * Main.rand.NextFloat(0.1f, 1f);
                        dust.velocity *= 0.2f;
                        dust.noGravity = true;
                        dust.color = Color.LightGray;
                    }
                }
            }
            base.HitEffect(npc, hitDirection, damageTaken);
        }
        public override void OnKill(NPC npc)
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
                    if(item.type == ItemType<HealPack>() || item.type == ItemType<ManaPack>())
                    {
                        if(item.active)
                            packCount++;
                    }
                    if (item.type == ItemType<BaguetteCrumb>())
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
                        SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
                        if (voidPlayer.soulsOnKill > 0)
                        {
                            float numberProjectiles = voidPlayer.soulsOnKill * HarvestCost(npc) * 0.1f;
                            for (int j = 0; j < numberProjectiles; j++)
                            {
                                Vector2 perturbedSpeed = new Vector2(-4.5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(180)));
                                Projectile proj = Projectile.NewProjectileDirect(npc.GetSource_Death("SOTS:SoulofLooting"), npc.Center, perturbedSpeed, ModContent.ProjectileType<SoulofLooting>(), 0, 0, Main.myPlayer, player.whoAmI, 0);
                                proj.netUpdate = true;
                            }
                        }
                        if(sPlayer.doomDrops && packCount < 40)
                        {
                            int rand = Main.rand.Next(4);
                            if (player.statLifeMax2 > player.statLife)
                                for(int j = 0; j < rand; j++)
                                    Item.NewItem(npc.GetSource_Death("SOTS:KilledByBoomstick"), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<HealPack>(), 1);
                            rand = 3 - rand;
                            if (player.statManaMax2 > player.statMana)
                                for (int j = 0; j < rand; j++)
                                    Item.NewItem(npc.GetSource_Death("SOTS:KilledByBoomstick"), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ManaPack>(), 1);
                        }
                        if (sPlayer.baguetteDrops && baguetteCount < 40)
                        {
                            int rand = Main.rand.Next(2);
                            if(rand >= 1)
                                rand += Main.rand.Next(3) / 2;
                            if (rand >= 2)
                                rand += Main.rand.Next(4) / 3;
                            if (rand >= 3)
                                rand += Main.rand.Next(5) / 4;
                            for (int j = 0; j < rand; j++)
                                Item.NewItem(npc.GetSource_Death("SOTS:KilledByBaguette"), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<BaguetteCrumb>(), 1);
                        }
                        if(sPlayer.HarvestersScythe && Main.rand.NextBool(lastHitWasCrit ? 5 : 10))
                        {
                            sPlayer.HarvestersScythe = false;
                            npc.extraValue = 0;
                            npc.NPCLoot();
                        }
                    }
                }
            }
        }
    }
}