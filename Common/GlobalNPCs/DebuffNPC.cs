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
using SOTS.Projectiles.Planetarium;
using SOTS.Items.OreItems;
using SOTS.Items.Planetarium.FromChests;
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
using SOTS.Projectiles.Blades;
using SOTS.Buffs.Debuffs;
using SOTS.Projectiles.Pyramid.GhostPepper;
using SOTS.NPCs.Anomaly;
using SOTS.Projectiles.Tide;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.FakePlayer;
using SOTS.Projectiles;
using Terraria.GameContent;
using System.Drawing.Drawing2D;
using SOTS.Helpers;

namespace SOTS.Common.GlobalNPCs
{
    public class DebuffNPC : GlobalNPC
    {
        public static int[] nerfBeeNPC;
        public static int[] nerfBeeBoss;
        public static int[] nerfBeeProj;
        public static int[] isSubspaceSerpent;
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
                NPCType<PutridPinkyPhase2>(), NPCType<NPCs.Boss.Curse.PharaohsCurse>(), NPCType<TheAdvisorHead>(), NPCType<NewPolaris>(), NPCType<Polaris>(), NPCType<SubspaceSerpentHead>(), NPCType<NPCs.Boss.Glowmoth.Glowmoth>()};
            vanillaBoss = new int[] { NPCID.KingSlime, NPCID.EyeofCthulhu, NPCID.EaterofWorldsHead, NPCID.BrainofCthulhu, NPCID.QueenBee, NPCID.SkeletronHead, NPCID.WallofFlesh, NPCID.Spazmatism,
                NPCID.Retinazer, NPCID.TheDestroyer, NPCID.SkeletronPrime, NPCID.Plantera, NPCID.Golem, NPCID.DukeFishron, NPCID.CultistBoss, NPCID.MoonLordCore, NPCID.HallowBoss, NPCID.QueenSlimeBoss};
            miniBosses = new int[] { NPCID.Mothron, NPCID.IceQueen, NPCID.SantaNK1, NPCID.Everscream, NPCID.MourningWood, NPCID.Pumpking, NPCID.GoblinSummoner, NPCID.MartianSaucerCore, NPCID.LunarTowerSolar, NPCID.LunarTowerNebula, NPCID.LunarTowerStardust, NPCID.LunarTowerVortex };
            nerfBeeNPC = new int[] { NPCType<PutridHook>() };
            nerfBeeBoss = new int[] { NPCType<PutridPinkyPhase2>(), NPCType<NPCs.Boss.Curse.PharaohsCurse>(), NPCType<TheAdvisorHead>() };
            nerfBeeProj = new int[] { ProjectileID.Bee, ProjectileID.GiantBee };
            isSubspaceSerpent = new int[] { NPCType<SubspaceSerpentBody>(), NPCType<SubspaceSerpentHead>(), NPCType<SubspaceSerpentTail>() };
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
        public int BlazingCurse = 0;
        public int AnomalyCurse = 0;
        public int BlightCurse = 0;
        public int CrystalCurse = 0;
        public float VoidspaceCurse = 0;
        public int OwnerOfVoidspaceCurseDamage = -1;
        public bool TriggeredCrystalCurse = false;
        private int CrystalCurseTimer = 0;
        private int HighestCrystalCurseNumber = -1;
        public int timeFrozen = 0;
        public bool netUpdateTime = false;
        public bool frozen = false;
        public float aiSpeedCounter = 0;
        public float aiSpeedMultiplier = 1;
        public const float timeBeforeFullFreeze = 30f;
        public float frozenForTime = 0;
        private int DendroDamage = 0;
        public List<int> ammoRegatherList = new List<int>();
        //public bool hasJustSpawned = true;
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
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
            if (npc.type == NPCType<Planetoid>())
            {
                int vDamage = 15;
                VoidPlayer.VoidDamage(Mod, target, vDamage);
            }
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
            DebuffNPC debuffNPC;
            bool found = realNPC.TryGetGlobalNPC<DebuffNPC>(out debuffNPC);
            if (found)
            {
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
            }
            else return true;
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
                packet.Write(BlazingCurse);
                packet.Write(AnomalyCurse);
                packet.Write(BlightCurse);
                packet.Write(OwnerOfVoidspaceCurseDamage);
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
            else if(type == 2)
            {
                byte playerWhoAmI = (byte)player.whoAmI;
                var packet = Mod.GetPacket();
                packet.Write((byte)SOTSMessageType.SyncGlobalNPC2);
                packet.Write(playerWhoAmI);
                packet.Write(npc.whoAmI);
                packet.Write(CrystalCurse);
                packet.Write(TriggeredCrystalCurse);
                packet.Send();
            }
        }
        public void DrawTimeFreeze(NPC npc, SpriteBatch spriteBatch)
        {
            if(!npc.GetGlobalNPC<GlobalEntityNPC>().RecentlyTeleported)
                GlobalEntity.DrawTimeFreeze(npc, spriteBatch, 1 - aiSpeedMultiplier);
        }
        public void DrawPermanentDebuffs(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor, Texture2D texture, ref int DebuffVariable, ref int Height)
        {
            if (DebuffVariable <= 0)
                return;
            int size = 0;
            for (int i = DebuffVariable; i > 0; i /= 10)
            {
                size++;
            }
            Vector2 pos = new Vector2(npc.Center.X, npc.position.Y);
            pos.X += size * ((texture.Width / 11f) - 2) / 2f;
            pos.X += 4;
            pos.Y -= Height;
            Vector2 origin = new Vector2(texture.Width / 22, texture.Height / 2);
            Rectangle frame;
            for (int i = DebuffVariable; i > 0; i /= 10)
            {
                int currentNum = i % 10;
                frame = new Rectangle(1 + ((1 + currentNum) * (texture.Width / 11)), 1, texture.Width / 11 - 2, texture.Height - 2);
                spriteBatch.Draw(texture, pos - screenPos, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                pos.X -= (texture.Width / 11f) - 2;
            }
            pos.X -= 4;
            frame = new Rectangle(0, 0, texture.Width / 11, texture.Height);
            pos.Y -= 1;
            spriteBatch.Draw(texture, pos - screenPos, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
            Height += 24;
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int height = 18;
            DrawPermanentDebuffs(npc, spriteBatch, screenPos, Color.White, Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/PlatinumCurse").Value, ref PlatinumCurse, ref height);
            DrawPermanentDebuffs(npc, spriteBatch, screenPos, ColorHelper.SoulLootingColor, Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/Harvesting").Value, ref HarvestCurse, ref height);
            DrawPermanentDebuffs(npc, spriteBatch, screenPos, ColorHelper.DestabilizeColor, Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/Destabilized").Value, ref DestableCurse, ref height);
            DrawPermanentDebuffs(npc, spriteBatch, screenPos, new Color(255, 0, 0), Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/Bleeding").Value, ref BleedingCurse, ref height);
            DrawPermanentDebuffs(npc, spriteBatch, screenPos, new Color(255, 200, 10), Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/BurntDefense").Value, ref BlazingCurse, ref height);
            DrawPermanentDebuffs(npc, spriteBatch, screenPos, ColorHelper.VoidAnomaly, Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/AnomalyCurse").Value, ref AnomalyCurse, ref height);
            DrawPermanentDebuffs(npc, spriteBatch, screenPos, ColorHelper.ToothAcheLime, Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/BlightCurse").Value, ref BlightCurse, ref height);
            DrawPermanentDebuffs(npc, spriteBatch, screenPos, Color.White, Mod.Assets.Request<Texture2D>("Common/GlobalNPCs/CrystalCurse").Value, ref CrystalCurse, ref height);
        }
        public void StackDebuff(NPC npc, Player player, ref int Debuff, int amount = 1, int netType = 0)
        {
            if(player.SOTSPlayer().AcidInject)
            {
                if (Debuff <= 0)
                    Debuff += amount;
            }
            Debuff += amount;
            if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                SendClientChanges(player, npc, netType);
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            lastHitWasCrit = hit.Crit;
            if (npc.immortal)
            {
                return;
            }
            if (item.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed) || item.DamageType.CountsAsClass(DamageClass.Melee))
            {
                if (Main.myPlayer == player.whoAmI && SOTSPlayer.ModPlayer(player).SerpentSpine)
                {
                    if (Main.rand.NextFloat(1) < 1f / ((BlazingCurse + 2f) * (BlazingCurse + 2f)))
                        StackDebuff(npc, player, ref BlazingCurse, 1, 0);
                }
            }
            if ((item.type == ItemType<AncientSteelSword>() || item.type == ItemType<AncientSteelGreatPickaxe>() || item.type == ItemType<AncientSteelGreatHamaxe>()) && hit.Crit)
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
                    StackDebuff(npc, player, ref BleedingCurse, 1, 0);
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            lastHitWasCrit = hit.Crit;
            if (projectile.type == ProjectileType<HarvestLock>())
            {
                VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
                int amt = HarvestCost(npc);
                if (!npc.immortal)
                {
                    var index = CombatText.NewText(npc.Hitbox, ColorHelper.SoulLootingColor.MultiplyRGB(Color.White), -amt);
                    if (Main.netMode == NetmodeID.Server && index != 100)
                    {
                        var combatText = Main.combatText[index];
                        NetMessage.SendData(MessageID.CombatTextInt, -1, -1, null, (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, (float)-amt, 0, 0, 0);
                    }
                    StackDebuff(npc, player, ref HarvestCurse, 1, 0);
                    voidPlayer.lootingSouls -= amt;
                }
            }
            if (npc.immortal)
            {
                return;
            }
            if (projectile.type == ProjectileType<StarShard>())
            {
                StackDebuff(npc, player, ref CrystalCurse, 1, 2);
            }
            if ((projectile.type == ProjectileType<StarshardSlash>() && projectile.ModProjectile is StarshardSlash s && s.thisSlashNumber == 5)
                || ((projectile.type == ProjectileType<CrystalExplosionBig>() || projectile.type == ProjectileType<CrystalExplosionSmall>()) && (int)projectile.ai[1] != npc.whoAmI))
            {
                TriggeredCrystalCurse = true;
                if (Main.myPlayer == projectile.owner && npc.life < 0)
                {
                    if (HighestCrystalCurseNumber < 4)
                    {
                        Projectile.NewProjectile(npc.GetSource_Misc("SOTS:HurtWhileDebuffed"), npc.position + new Vector2(Main.rand.NextFloat(npc.width), Main.rand.NextFloat(npc.height)), -npc.velocity, ProjectileType<CrystalExplosionSmall>(), 18, 2f, Main.myPlayer, 0, -1);
                    }
                    else
                    {
                        Projectile.NewProjectile(npc.GetSource_Misc("SOTS:HurtWhileDebuffed"), npc.Center, -npc.velocity, ProjectileType<CrystalExplosionBig>(), 12 * (HighestCrystalCurseNumber + 1), 3f, Main.myPlayer, 0, -1);
                    }
                }
                if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                    SendClientChanges(player, npc, 2);
            }
            if (Main.myPlayer == player.whoAmI)
            {
                if (projectile.type == ProjectileType<PlagueBeam>())
                {
                    StackDebuff(npc, player, ref BlightCurse, 1, 0);
                }
                if (projectile.type == ProjectileType<SeleneSlash>() || (projectile.type == ProjectileType<SkipSlash>() && Main.rand.NextBool(5)))
                {
                    StackDebuff(npc, player, ref AnomalyCurse, 1, 0);
                }
            }
            if (projectile.type == ProjectileType<Projectiles.Temple.Helios>())
            {
                StackDebuff(npc, player, ref BlazingCurse, 1, 0);
            }
            if(projectile.CountsAsClass(DamageClass.SummonMeleeSpeed) || projectile.CountsAsClass(DamageClass.Melee))
            {
                if (Main.myPlayer == player.whoAmI && SOTSPlayer.ModPlayer(player).SerpentSpine)
                {
                    if (Main.rand.NextFloat(1) < 1f / ((BlazingCurse + 2f) * (BlazingCurse + 2f)))
                        StackDebuff(npc, player, ref BlazingCurse, 1, 0);
                }
            }
            if (projectile.type == ProjectileType<DeathSpiralProj>() || (projectile.type == ProjectileType<BloodSpark>() && hit.Crit))
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
                    StackDebuff(npc, player, ref BleedingCurse, 1, 0);
            }
        }
        bool hitByRay = false;
        bool lastHitWasCrit = false;
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            if (npc.HasBuff(BuffType<Shattered>()) && projectile.CountsAsClass(DamageClass.Melee) && projectile.type != ProjectileType<Projectiles.Evil.AncientSteelHalberd>())
            {
                modifiers.Defense *= 0;
                npc.DelBuff(npc.FindBuffIndex(BuffType<Shattered>()));
                modifiers.SetCrit();
            }
            else if(npc.HasBuff<DendroChain>())
            {
                modifiers.Defense.Flat -= 20;
            }
            if (npc.immortal)
            {
                return;
            }
            if(Main.rand.NextFloat(100f) < 5f * DestableCurse)
            {
                modifiers.SetCrit();
            }
            if (projectile.type == ProjectileType<CodeVolley>() || projectile.type == ProjectileType<CodeBurst>())
            {
                if(projectile.type == ProjectileType<CodeVolley>())
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.7f, 1 + DestableCurse * 0.45f) && DestableCurse < 20)
                        StackDebuff(npc, player, ref DestableCurse, 1, 0);
                }
                if (projectile.type == ProjectileType<CodeBurst>() && projectile.ai[1] != -1)
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.3f, 1 + DestableCurse * 0.45f) && DestableCurse < 20)
                        StackDebuff(npc, player, ref DestableCurse, 1, 0);
                }

                if (projectile.type == ProjectileType<CodeBurst>() && projectile.ai[1] == -1)
                {
                    if (Main.rand.NextFloat(100f) < 100 * Math.Pow(0.25f, 1 + DestableCurse * 0.5f) && DestableCurse < 20)
                        StackDebuff(npc, player, ref DestableCurse, 1, 0);
                }
            }
            if (projectile.type == ProjectileType<DestabilizingBeam>() && !hitByRay)
            {
                hitByRay = true;
                StackDebuff(npc, player, ref DestableCurse, 4, 0);
            }
            if(projectile.type == ProjectileType<BetrayersSlash>())
            {
                if(BleedingCurse < 1)
                {
                    StackDebuff(npc, player, ref BleedingCurse, 1, 0);
                }
            }
            if (nerfBeeProj.Contains(projectile.type))
            {
                if (nerfBeeBoss.Contains(npc.type))
                    modifiers.SourceDamage *= 0.8f;
                if (nerfBeeNPC.Contains(npc.type))
                    modifiers.SourceDamage *= 0.6f;
            }
            if(projectile.type == ProjectileType<RealityShatter>())
            {
                if (isSubspaceSerpent.Contains(npc.type))
                    modifiers.SourceDamage *= 0.3f;
                else if(npc.boss)
                {
                    modifiers.SourceDamage *= 0.8f;
                }
            }
            if(isSubspaceSerpent.Contains(npc.type))
            {
                if(projectile.type == ProjectileType<ChaosBeam>())
                {
                    modifiers.SourceDamage *= 0.75f;
                }
            }
            if(BlazingCurse > 0 || AnomalyCurse > 0)
            {
                modifiers.SourceDamage *= (1.0f + 0.03f * BlazingCurse + 0.005f * AnomalyCurse);
            }
            if (player.SOTSPlayer().VoidspaceFlames)
                ApplyVoidspaceCurse(npc, player);
            if (projectile.type == ProjectileType<PlagueBeam>())
            {
                modifiers.FinalDamage *= 1.0f + 0.2f * BlightCurse;
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (npc.immortal)
            {
                return;
            }
            if (npc.HasBuff(BuffType<Shattered>()) && item.CountsAsClass(DamageClass.Melee))
            {
                modifiers.Defense *= 0;
                npc.DelBuff(npc.FindBuffIndex(BuffType<Shattered>()));
                modifiers.SetCrit();
            }
            else if (npc.HasBuff<DendroChain>())
            {
                modifiers.Defense.Flat -= 20;
            }
            if (Main.rand.NextFloat(100f) < 5f * DestableCurse)
            {
                modifiers.SetCrit();
            }
            if (item.type == ItemType<PlatinumScythe>() || item.type == ItemType<SectionChiefsScythe>())
            {
                if (PlatinumCurse < 10)
                    StackDebuff(npc, player, ref PlatinumCurse, 1, 0);
            }
            if (BlazingCurse > 0 || AnomalyCurse > 0)
            {
                modifiers.SourceDamage *= (1.0f + 0.03f * BlazingCurse + 0.005f * AnomalyCurse);
            }
            if(player.SOTSPlayer().VoidspaceFlames)
                ApplyVoidspaceCurse(npc, player);
        }
        public void ApplyVoidspaceCurse(NPC npc, Player player)
        {
            if (OwnerOfVoidspaceCurseDamage < 0)
            {
                OwnerOfVoidspaceCurseDamage = player.whoAmI;
                if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient)
                    SendClientChanges(player, npc);
            }
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
            if (CrystalCurse > 0)
            {
                HighestCrystalCurseNumber = Math.Max(HighestCrystalCurseNumber, CrystalCurse);
                if (TriggeredCrystalCurse)
                {
                    if (CrystalCurseTimer <= 0)
                    {
                        CrystalCurse--;

                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (CrystalCurse > 0 || HighestCrystalCurseNumber < 4)
                            {
                                Projectile.NewProjectile(npc.GetSource_Misc("SOTS:HurtWhileDebuffed"), npc.position + new Vector2(Main.rand.NextFloat(npc.width), Main.rand.NextFloat(npc.height)), -npc.velocity, ProjectileType<CrystalExplosionSmall>(), 18, 2f, Main.myPlayer, 0, npc.whoAmI);
                            }
                            else
                            {
                                Projectile.NewProjectile(npc.GetSource_Misc("SOTS:HurtWhileDebuffed"), npc.Center, -npc.velocity, ProjectileType<CrystalExplosionBig>(), 12 * (HighestCrystalCurseNumber + 1), 3f, Main.myPlayer, 0, npc.whoAmI);
                            }
                        }
                        npc.velocity *= 0.9f; //Each explosion will drastically slow down the enemy

                        CrystalCurseTimer = 5;
                    }
                    CrystalCurseTimer--;
                }
            }
            else
            {
                HighestCrystalCurseNumber = -1;
                TriggeredCrystalCurse = false;
                CrystalCurseTimer = 0;
            }
            if (npc.immortal)
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
            if(AnomalyCurse > 0)
            {
                int capVisuals = AnomalyCurse;
                if (capVisuals > 20)
                    capVisuals = 20;
                for (int i = 0; i < capVisuals; i++)
                {
                    if (Main.rand.NextBool(20 + i))
                    {
                        Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5f), npc.width, npc.height, DustType<PixelDust>());
                        dust.velocity *= 0.5f;
                        dust.color = ColorHelper.VoidAnomaly;
                        dust.color.A = 0;
                        dust.noGravity = true;
                        dust.fadeIn = 12f;
                        dust.scale = 2.0f;
                    }
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
            bool isBubbled = false;
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
                                else if(!proj.TryGetGlobalProjectile<FakePlayerProjectile>(out FakePlayerProjectile fpp) || fpp.FakeOwnerIdentity == -1)
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
                        StackDebuff(npc, player, ref BleedingCurse, 1, 0);
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
                if(proj.active && proj.type == ProjectileType<HydroBubble>() && (int)proj.ai[1] == npc.whoAmI)
                {
                    if(proj.ModProjectile is HydroBubble hydroBubble)
                    {
                        if(hydroBubble.AiCounter > hydroBubble.ChargeTime)
                        {
                            isBubbled = true;
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
            if(npc.HasBuff(BuffType<WebbedNPC>()) || darkArmed || isBubbled)
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
            bool DreamLamp = npc.HasBuff<DendroChain>();
            if(DreamLamp)
            {
                int damageToAdd = 0;
                DendroChainNPCOperators.InitiateNPCDamageStats(npc, ref damageToAdd);
                DendroDamage += damageToAdd;
                DendroChainNPCOperators.PullOtherNPCs(npc);
                if (!npc.boss)
                    finalSlowdown *= 0.8f;
            }
            else
            {
                DendroDamage = 0;
            }
            npc.position -= npc.velocity * (1 - dartVeloMult * flowerVeloMult * finalSlowdown);
            if (BlightCurse > 0)
            {
                if (previousBlight < BlightCurse)
                {
                    if(Main.netMode != NetmodeID.Server)
                    {
                        for (int i = 12; i > 0; i--)
                        {
                            Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5, 5), npc.width, npc.height, ModContent.DustType<PixelDust>(), 0, 0, 0, PlagueSpitter.SpitterColor, 1f);
                            dust.noGravity = true;
                            dust.velocity = dust.velocity * Main.rand.NextFloat() + npc.velocity * Main.rand.NextFloat(0.0f, 1f);
                            dust.fadeIn = 4;
                            dust.scale = Main.rand.Next(4, 9) / 4f;
                            dust.color.A = 100;
                        }
                    }
                    TimeSinceHitByBlight = 0;
                }
                else
                {
                    if(Main.netMode != NetmodeID.Server)
                    {
                        if (Main.rand.NextBool(4))
                        {
                            Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5, 5), npc.width, npc.height, ModContent.DustType<PixelDust>(), 0, 0, 0, PlagueSpitter.SpitterColor, 1f);
                            dust.noGravity = true;
                            dust.velocity = dust.velocity * Main.rand.NextFloat() + npc.velocity * Main.rand.NextFloat(0.0f, 1f);
                            dust.fadeIn = 4;
                            dust.scale = Main.rand.Next(4, 9) / 4f;
                            dust.color.A = 100;
                        }
                    }
                }
                if (TimeSinceHitByBlight >= 30)
                {
                    TimeSinceHitByBlight = 25;
                    BlightCurse--;
                }
                TimeSinceHitByBlight++;
            }
            else
                TimeSinceHitByBlight = 0;
            previousBlight = BlightCurse;
        }
        bool isFlowered = false;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if(PlatinumCurse > 0)
            {
                npc.lifeRegen -= PlatinumCurse * 6;
                damage = PlatinumCurse;
            }
            if (BleedingCurse > 0)
            {
                npc.lifeRegen -= BleedingCurse * 10;
                damage += BleedingCurse;
            }
            if (AnomalyCurse > 0)
            {
                npc.lifeRegen -= AnomalyCurse * 4;
                damage += (AnomalyCurse + 1) / 2;
            }
            if (BlightCurse > 0)
            {
                npc.lifeRegen -= BlightCurse * 20;
                damage += BlightCurse + 1;
            }
            if (isFlowered)
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
            if (OwnerOfVoidspaceCurseDamage >= 0)
            {
                npc.lifeRegen -= 20;
                VoidspaceCurse += 1 / 6f;
                if (Main.rand.NextBool(5))
                {
                    Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
                    Dust dust = Dust.NewDustDirect(npc.position - new Vector2(5), npc.width, npc.height, DustType<CopyDust4>());
                    dust.velocity *= 1.5f;
                    dust.velocity += 1.5f * circular.SafeNormalize(Vector2.Zero);
                    dust.scale = 1.75f;
                    dust.noGravity = true;
                    dust.color = new Color(70, 255, 60);
                    dust.fadeIn = 0.2f;
                }
            }
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
                            color = new Color(ColorHelper.EvilColor.R, ColorHelper.EvilColor.G, ColorHelper.EvilColor.B);
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
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
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
                if (Main.netMode != NetmodeID.MultiplayerClient && TriggeredCrystalCurse)
                {
                    if (HighestCrystalCurseNumber < 4)
                    {
                        Projectile.NewProjectile(npc.GetSource_Misc("SOTS:HurtWhileDebuffed"), npc.position + new Vector2(Main.rand.NextFloat(npc.width), Main.rand.NextFloat(npc.height)), -npc.velocity, ProjectileType<CrystalExplosionSmall>(), 18, 2f, Main.myPlayer, 0, -1);
                    }
                    else
                    {
                        Projectile.NewProjectile(npc.GetSource_Misc("SOTS:HurtWhileDebuffed"), npc.Center, -npc.velocity, ProjectileType<CrystalExplosionBig>(), 12 * (HighestCrystalCurseNumber + 1), 3f, Main.myPlayer, 0, -1);
                    }
                }
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
                if (VoidspaceCurse >= 1 && (npc.realLife == -1 || npc.realLife == npc.whoAmI))
                {
                    if (Main.myPlayer == OwnerOfVoidspaceCurseDamage)
                    {
                        Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Vector2.Zero, ModContent.ProjectileType<VoidspaceFlameHitbox>(), (int)VoidspaceCurse, 0f, OwnerOfVoidspaceCurseDamage, npc.width, npc.height);
                    }
                }
            }
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
                int heartCount = 0;
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
                    if (item.type == ItemID.Heart)
                    {
                        if (item.active)
                            heartCount++;
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
                        if (sPlayer.RubyRing && heartCount < 40 && !npc.SpawnedFromStatue)
                        {
                            if(lastHitWasCrit || Main.rand.NextBool(2))
                                Item.NewItem(npc.GetSource_Death("SOTS:KilledWithVulture"), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart, 1);
                        }
                        if (sPlayer.HarvestersScythe && Main.rand.NextBool(lastHitWasCrit ? 5 : 10))
                        {
                            sPlayer.HarvestersScythe = false;
                            npc.extraValue = 0;
                            npc.NPCLoot();
                        }
                        if (sPlayer.EmeraldRing && Main.rand.NextBool(7))
                        {
                            sPlayer.EmeraldRing = false;
                            npc.extraValue = 0;
                            npc.NPCLoot();
                        }
                    }
                }
            }
            bool DreamLamp = npc.HasBuff<DendroChain>();
            if (DreamLamp)
            {
                int damageToAdd = 0;
                DendroChainNPCOperators.InitiateNPCDamageStats(npc, ref damageToAdd);
                DendroDamage += damageToAdd;
                DendroChainNPCOperators.PullOtherNPCs(npc);
            }
            if (DendroDamage > 0)
            {
                DendroChainNPCOperators.HurtOtherNPCs(npc, DendroDamage);
            }
        }
        private float previousBlight;
        private float TimeSinceHitByBlight = 0;
        public void AddAmmoToList(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if(SOTSPlayer.ModPlayer(player).AmmoRegather && projectile.CountsAsClass(DamageClass.Ranged))
            {
                int currentAmmoSize = ammoRegatherList.Count;
                float chanceOfAddingAmmo = 1f / (4 + currentAmmoSize); //25% base chance to add ammo to the pool, then 20, then 16.667, then 14%, then 12.5%, then 11%, then 10%, etc
                if(Main.rand.NextFloat(1) < chanceOfAddingAmmo)
                {
                    SOTSProjectile instancedProj = projectile.GetGlobalProjectile<SOTSProjectile>();
                    int ammoType = instancedProj.AmmoUsedID;
                    if(ammoType != -1)
                    {
                        ammoRegatherList.Add(ammoType);
                    }
                }
            }
        }
        public void DrainPermanentDebuffs(NPC npc)
        {
            if(PlatinumCurse > 0)
                PlatinumCurse--;
            if (DestableCurse > 0)
                DestableCurse--;
            if (BleedingCurse > 0)
                BleedingCurse--;
            if (BlazingCurse > 0)
                BlazingCurse--;
            if (AnomalyCurse > 0)
                AnomalyCurse--;
        }
    }
}