using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Permissions;
using Ionic.Zip;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SOTS.Dusts;
using SOTS.FakePlayer;
using SOTS.Items.Banners;
using SOTS.Items.Chaos;
using SOTS.Items.ChestItems;
using SOTS.Items.Permafrost;
using SOTS.Prim;
using SOTS.Prim.Trails;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Permafrost;
using SOTS.WorldgenHelpers;
using Steamworks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace SOTS.NPCs.Boss.Polaris.NewPolaris
{	[AutoloadBossHead]
	public class NewPolaris : ModNPC
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            if(LoadedWeaponData)
            {
                writer.Write(polarisWeaponData[0].TypeToSwapTo);
                writer.Write(polarisWeaponData[1].TypeToSwapTo);
                writer.Write(polarisWeaponData[2].TypeToSwapTo);
                writer.Write(polarisWeaponData[3].TypeToSwapTo);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    writer.Write((int)0);
            }
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            if (LoadedWeaponData)
            {
                polarisWeaponData[0].TypeToSwapTo = reader.ReadInt32();
                polarisWeaponData[1].TypeToSwapTo = reader.ReadInt32();
                polarisWeaponData[2].TypeToSwapTo = reader.ReadInt32();
                polarisWeaponData[3].TypeToSwapTo = reader.ReadInt32();
            }
            else
            {
                for(int i = 0; i < 4; i++)
                    reader.ReadInt32();
            }
        }
        public static class AttackID
        {
            public static int BulletStorm = 0;
            public static int BeamsAndMineShotgun = 1;
            public static int SecondPhaseTransitionAttack = 2;
            public static int LaserSpinAttack = 3;
            public static int DashWithBeams1 = 4;
            public static int DashWithBeams2 = 5;
            public static int MortarRain = 6;
            public static int QuickBeamCircle = 7;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.63889f * balance * bossAdjustment);  //boss life scale in expertmode
            NPC.damage = (int)(NPC.damage * 0.75f);  //boss damage increase in expermode
        }
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if(needsASecondPhaseTransition && !SecondPhase)
            {
                modifiers.SourceDamage *= 0.6f;
                modifiers.FinalDamage *= 0.9f;
                modifiers.Defense += 0.25f;
            }
            float survivalResistance = NPC.life / NPC.lifeMax;
            survivalResistance = Math.Clamp(survivalResistance, 0, 1);
            modifiers.FinalDamage *= (0.72f + 0.28f * survivalResistance);
        }
        public bool LoadedWeaponData = false;
        public PolarisWeaponData[] polarisWeaponData = new PolarisWeaponData[4];
        public int despawn = 0;
        public bool SecondPhase = false;
        public bool hasUsedBeamAttackAlready = false;
        public bool hasUsedLaserSpinWithOnlySwordsAlready = false;
        public bool hasUsedLaserSpinWithOnlyGunsAlready = false;
        public bool hasUsedLaserSpinWithOnlyBeamsAlready = false;
        public bool needsASecondPhaseTransition = false;
        public float PhaseTwoTransitionPercent => Main.expertMode ? 0.65f : 0.5f;
        private float Phase
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float AI0
        {
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float AI1
        {
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float AI2
        {
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "SOTS/BossCL/PolarisPortrait",
                PortraitScale = 0.5f, // Portrait refers to the full picture when clicking on the icon in the bestiary
                Scale = 0.5f,
                PortraitPositionYOverride = 10f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
		}
		public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPC.aiStyle = -1;
            NPC.lifeMax = 36000;
            NPC.damage = 90; 
            NPC.defense = 30;  
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 50, 0, 0);
            NPC.npcSlots = 1f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Polaris");
            NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (!LoadedWeaponData)
                return false;
            foreach(PolarisWeaponData Weapon in polarisWeaponData)
            {
                float addedDistance = 16;
                if (Weapon.Type == 2 && Weapon.Frame == 0 && Weapon.TypeToSwapTo == 2)
                {
                    addedDistance = 84;
                }
                float collisionpoint = 0;
                if (Collision.CheckAABBvLineCollision(target.position, target.Size, Weapon.position + (Weapon.position - NPC.Center).SafeNormalize(Vector2.Zero) * addedDistance, NPC.Center, 18, ref collisionpoint))
                {
                    return true;
                }
            }
            if(target.Distance(NPC.Center) < 110)
            {
                return true;
            }
            return false;
        }
        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            int width = 1200;
            npcHitbox = new Rectangle((int)NPC.Center.X - width / 2, (int)NPC.Center.Y - width / 2, width, width);
            return true;
        }
        public float TiltBasedRotation => Math.Clamp(LerpingXVelocity * 0.05f, -MathHelper.TwoPi / 16f, MathHelper.TwoPi / 16f);
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!LoadedWeaponData)
                return false;
            drawColor = Color.Lerp(drawColor, Color.White, 0.25f);
            DrawWeapon(spriteBatch, screenPos, drawColor);
            Texture2D coreTexture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Polaris/NewPolaris/PolarisInnerCore").Value;
            Texture2D eyeTexture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Polaris/NewPolaris/PolarisEye").Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Polaris/NewPolaris/NewPolarisGlow").Value;
            Texture2D myTexture = Terraria.GameContent.TextureAssets.Npc[Type].Value;
            Vector2 position = NPC.Center - screenPos;
            Vector2 origin = new Vector2(myTexture.Width / 2, myTexture.Height / 2 / Main.npcFrameCount[NPC.type]);
            Vector2 eyeOffset = Vector2.Zero;
            if(NPC.HasValidTarget)
            {
                Player p = Main.player[NPC.target];
                Vector2 toPlayer = p.Center - NPC.Center;
                eyeOffset = toPlayer.SafeNormalize(Vector2.Zero) * 4 * EyeRecoil;
            }
            if(CoreAttack && coreAttackProgress != 0 && coreAttackProgress < TotalCoreAttackCooldown)
            {
                float percent = coreAttackProgress / TotalCoreAttackCooldown;
                for (int i = 0; i < 20; i++)
                {
                    Vector2 circular = new Vector2(32 * (1 - percent), 0).RotatedBy(i * MathHelper.TwoPi / 20f + NPC.rotation + TiltBasedRotation);
                    spriteBatch.Draw(glowTexture, position + circular, NPC.frame, new Color(100, 100, 100, 0) * percent * percent * 0.5f, NPC.rotation + MathHelper.Pi + TiltBasedRotation, origin, 1f, SpriteEffects.None, 0f);
                }
                for (int i = 0; i < 4; i++)
                {
                    Vector2 outward = new Vector2(0, 1).RotatedBy(i * MathHelper.PiOver2 + NPC.rotation + TiltBasedRotation);
                    Projectiles.Permafrost.PolarisLaser.Draw(spriteBatch, NPC.Center + outward * 96, NPC.Center + outward * 2096, i, percent * 0.25f, percent);
                }
            }
            spriteBatch.Draw(eyeTexture, position + eyeOffset, null, Color.White, 0f, eyeTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(coreTexture, position, null, drawColor, TiltBasedRotation, coreTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(myTexture, position, NPC.frame, drawColor, NPC.rotation + MathHelper.Pi + TiltBasedRotation, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowTexture, position, NPC.frame, Color.White, NPC.rotation + MathHelper.Pi + TiltBasedRotation, origin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public static string WeaponTexture(int color = 0, int weaponType = 0, bool glow = false)
        {
            string requestedTexture = "SOTS/NPCs/Boss/Polaris/NewPolaris/"; 
            requestedTexture += "Closed";
            requestedTexture += "PolarisWeaponType";
            if (color == 0)
            {
                requestedTexture += "Blue";
            }
            else
            {
                requestedTexture += "Red";
            }
            if (weaponType == 0)
            {
                requestedTexture += "Cannon";
            }
            else if (weaponType == 1)
            {
                requestedTexture += "Laser";
            }
            else
            {
                requestedTexture += "Saber";
            }
            if (glow)
            {
                requestedTexture += "Glow";
            }
            return requestedTexture;
        }
		public void DrawWeapon(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            for(int i = 0; i < polarisWeaponData.Length; i++)
            {
                PolarisWeaponData Weapon = polarisWeaponData[i];
                Weapon.DrawTether(spriteBatch, screenPos, NPC);
                Weapon.Draw(spriteBatch, screenPos, drawColor);
            }
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }
        public override void FindFrame(int frameHeight)
        {

        }
        public float coreAttackProgress = 0f;
        public float TicksPerCoreAnimation
        {
            get
            {
                if(Main.expertMode)
                {
                    return 7f;
                }
                return 8f;
            }
        }
        public float TotalCoreAttackCooldown => TicksPerCoreAnimation * (Main.npcFrameCount[Type] - 1);
        public void DoCoreUpdates()
        {
            int frameHeight = 236;
            if (CoreAttack)
            {
                NPC.frameCounter++;
                coreAttackProgress++;
            }
            if (NPC.frameCounter >= TicksPerCoreAnimation && CoreAttack)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y == 1 * frameHeight)
                {
                    SOTSUtils.PlaySound(SoundID.Item15, NPC.Center, 1.8f, -0.4f);
                }
                if(NPC.frame.Y == 5 * frameHeight)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 outward = new Vector2(0, 1).RotatedBy(i * MathHelper.PiOver2 + NPC.rotation + TiltBasedRotation);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 96, outward * 4, ModContent.ProjectileType<Projectiles.Permafrost.PolarisLaser>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, i);
                        }
                    }
                }
                if (NPC.frame.Y >= 6 * frameHeight)
                {
                    coreAttackProgress = 0;
                    NPC.frame.Y = 0;
                    CoreAttack = false;
                }
            }
            if(!CoreAttack)
            {
                coreAttackProgress = 0;
                NPC.frame.Y = 0;
            }
        }
        public bool CoreAttack = false;
        public int rotationDirection = 1;
        public float WeaponExtension = 64;
        public float WeaponExtensionSabers = 64;
        public float WeaponSpin = 0;
        public float EyeRecoil = 1f;
        public void SwapAllWeapons(int type)
        {
            for (int i = 0; i < polarisWeaponData.Length; i++)
            {
                PolarisWeaponData Weapon = polarisWeaponData[i];
                Weapon.TypeToSwapTo = type;
            }
        }
        public void CloseWeapons()
        {
            for (int i = 0; i < polarisWeaponData.Length; i++)
            {
                PolarisWeaponData Weapon = polarisWeaponData[i];
                Weapon.close = true;
            }
        }
        public void OpenWeapons()
        {
            for (int i = 0; i < polarisWeaponData.Length; i++)
            {
                PolarisWeaponData Weapon = polarisWeaponData[i];
                Weapon.close = false;
            }
        }
        public void UpdateWeapons()
        {
            if (WeaponExtensionSabers < WeaponExtension)
                WeaponExtensionSabers = WeaponExtension;
            WeaponSpin = MathHelper.WrapAngle(WeaponSpin);
            for (int i = 0; i < polarisWeaponData.Length; i++)
            {
                PolarisWeaponData Weapon = polarisWeaponData[i];
                /*if (AI1 % 200 == 0)
                {
                    int nextType = Main.rand.Next(3);
                    while (nextType == Weapon.Type)
                    {
                        nextType = Main.rand.Next(3);
                    }
                    Weapon.TypeToSwapTo = nextType;
                    SOTSUtils.PlaySound(SoundID.MenuClose, Weapon.position, 1.1f, -0.5f);
                }*/
                float addedRotation = NPC.rotation + TiltBasedRotation + WeaponSpin;
                float extend = WeaponExtension;
                if (Weapon.Type == 2)
                    extend = WeaponExtensionSabers;
                Vector2 position = NPC.Center + new Vector2(extend, extend).RotatedBy(i * MathHelper.PiOver2 + addedRotation);
                Weapon.Update(position);
                float rotation = (i - 1) * MathHelper.PiOver2 + addedRotation;
                Weapon.rotation = rotation;
                Weapon.rotationDirection = rotationDirection;
                Weapon.redOrBlue = 1 - i / 2;
                Weapon.UpdateFrame();
            }
        }
        public override bool PreAI()
        {
            if (SOTSWorld.PolarisLightingFadeIn < 1)
                SOTSWorld.PolarisLightingFadeIn += 0.0125f;
            if (SOTSWorld.PolarisLightingFadeIn > 1)
                SOTSWorld.PolarisLightingFadeIn = 1;
            if (!NPC.HasValidTarget)
            {
                NPC.TargetClosest(false);
            }
            if (!LoadedWeaponData)
            {
                Phase = -1;
                SwapPhase(0);
                AI0 = 0;
                LoadedWeaponData = true;
                polarisWeaponData = new PolarisWeaponData[]
                {
                    new PolarisWeaponData(),
                    new PolarisWeaponData(),
                    new PolarisWeaponData(),
                    new PolarisWeaponData()
                };
                NPC.netUpdate = true;
            }
            if(!SecondPhase)
            {
                if(NPC.life < NPC.lifeMax * PhaseTwoTransitionPercent)
                {
                    needsASecondPhaseTransition = true;
                }
            }
            else
                needsASecondPhaseTransition = false;
            return true;
        }
        float LerpingXVelocity = 0;
        public override void AI()
        {
            /*if(NPC.velocity.X != 0 && Phase != 0 && Phase != 2 && Phase != 4 && Phase != 4)
            {
                rotationDirection = Math.Sign(NPC.velocity.X);
            }*/
            Player player = Main.player[NPC.target];
            Vector2 toPlayer = player.Center - NPC.Center;
            bool resetRotation = false;
            bool resetWeaponSpin = false;
            bool resetWeaponExtension = false;
            UpdateWeapons();
            if(Phase == AttackID.BulletStorm)
            {
                NPC.velocity *= 0.925f;
                GoNearPlayer(320f, 1280f, 3f);
                AI0++;
                //if(!hasUsedFirstAttackOnceAlready)
                    SwapAllWeapons(0);
                /*else
                {
                    for (int i = 0; i < polarisWeaponData.Length; i++)
                    {
                        PolarisWeaponData Weapon = polarisWeaponData[i];
                        Weapon.TypeToSwapTo = 1 - i % 2;
                    }
                }*/
                if (AI0 > 120)
                {
                    OpenWeapons();
                    WeaponExtension = MathHelper.Lerp(WeaponExtension, 100f, 0.06f);
                }
                else
                {
                    resetRotation = true;
                    resetWeaponSpin = true;
                    CloseWeapons();
                }
                float totalSpinTime = 540f;
                if (AI0 > 180)
                {
                    AI2++;
                    if(AI2 > totalSpinTime / 2f)
                    {
                        AI2 += 1f;
                    }
                    float spinSpeed = (float)Math.Sin(AI2 / totalSpinTime * MathHelper.Pi);
                    spinSpeed = Math.Clamp(6.2f * spinSpeed, 0, 6.2f);
                    WeaponSpin += MathHelper.ToRadians(spinSpeed) * rotationDirection;
                    if (AI0 > AI1 + 180)
                    {
                        for (int i = 0; i < polarisWeaponData.Length; i++)
                        {
                            PolarisWeaponData Weapon = polarisWeaponData[i];
                            Weapon.LaunchAttack(NPC, i / 2, 0);
                        }
                        AI0 -= AI1;
                        if (AI2 > totalSpinTime / 2f)
                            AI1 += 2f;
                        else
                            AI1 -= 1.5f;
                        int lowestSpeed = 4;
                        if (Main.expertMode)
                            lowestSpeed = 3;
                        AI1 = Math.Clamp(AI1, lowestSpeed, 600f);
                    }
                    if (SecondPhase)
                    {
                        if (AI2 > 120 && AI2 < totalSpinTime - 90f)
                        {
                            CoreAttack = true;
                            float rotateSpeed = 1.75f;
                            float percent = coreAttackProgress / TotalCoreAttackCooldown * rotateSpeed;
                            NPC.rotation += MathHelper.ToRadians(percent) * rotationDirection;
                            WeaponSpin -= MathHelper.ToRadians(percent) * rotationDirection;
                        }
                    }    
                    if(AI2 >= totalSpinTime)
                    {
                        SwapPhase(AttackID.BeamsAndMineShotgun);
                    }
                }
            }
            else if(Phase == AttackID.BeamsAndMineShotgun)
            {
                resetRotation = resetWeaponSpin = resetWeaponExtension = true;
                AI0++;
                if (AI0 > 60)
                {
                    OpenWeapons();
                    SwapAllWeapons(1);
                }
                else
                    CloseWeapons();
                int aiCycle = 180;
                if (Main.expertMode)
                    aiCycle = 150;
                float aiThisCycle = AI0 % aiCycle;
                if (aiThisCycle == 0)
                {
                    AI1++;
                    if(AI1 >= 5)
                    {
                        hasUsedBeamAttackAlready = true;
                        SwapPhase(AttackID.SecondPhaseTransitionAttack);
                    }
                    else
                    {
                        EyeRecoil = -1;
                        LaunchMines();
                    }
                }
                else if(AI0 > aiCycle && AI0 % aiCycle <= 60)
                {
                    int shootSpeed = 10;
                    if (Main.expertMode)
                        shootSpeed = 8;
                    if((aiThisCycle - 20) % shootSpeed == 0 && aiThisCycle >= 20)
                    {
                        float spreadMult = 1 - ((aiThisCycle - 20) / 40f);
                        for (int i = 0; i < polarisWeaponData.Length; i++)
                        {
                            PolarisWeaponData Weapon = polarisWeaponData[i];
                            for(int j = -1; j <= 1; j += 2)
                            {
                                if (spreadMult == 0)
                                    j = 0;
                                Weapon.LaunchAttack(NPC, i / 2, MathHelper.ToRadians(j * 45 * spreadMult));
                            }
                        }
                    }
                    NPC.velocity *= 0.975f;
                }
                else
                {
                    NPC.velocity *= 0.935f;
                    GoNearPlayer(320, 640, 7.5f);
                }
            }
            else if(Phase == AttackID.SecondPhaseTransitionAttack || Phase == AttackID.DashWithBeams1 || Phase == AttackID.DashWithBeams2)
            {
                int maxDashes = 4;
                resetWeaponSpin = resetWeaponExtension = true;
                if (AI2 < maxDashes)
                {
                    AI0++;
                    if (Phase == 2)
                    {
                        SwapAllWeapons(2);
                    }
                    else
                    {
                        for (int i = 0; i < polarisWeaponData.Length; i++)
                        {
                            PolarisWeaponData Weapon = polarisWeaponData[i];
                            Weapon.TypeToSwapTo = 2 - ((i + (int)Phase) % 2);
                        }
                    }
                    if (AI0 > 60)
                    {
                        if(AI0 == 150 && needsASecondPhaseTransition && !SecondPhase)
                        {
                            SOTSUtils.PlaySound(SoundID.Item119, NPC.Center, 1.25f, -0.3f);
                        }
                        OpenWeapons();
                    }
                    else
                    {
                        CloseWeapons();
                    }
                }
                float StartSpinning = 120;
                float StartHovering = 60;
                float StartLaunching = 240;
                float LaunchSpeed = 8f;
                if (AI0 >= StartHovering)
                {
                    float degreesIntoDash = AI1 * LaunchSpeed;
                    if (degreesIntoDash < 240)
                        NPC.velocity *= 0.925f;
                    float sinusoid = (float)Math.Sin(MathHelper.ToRadians(AI0 + AI2));
                    Vector2 circularPosition = new Vector2(720 * -rotationDirection, 0).RotatedBy(MathHelper.ToRadians(15 * sinusoid));
                    if (AI0 > StartLaunching)
                    {
                        Vector2 slingBack = toPlayer.SafeNormalize(Vector2.Zero);
                        if ((int)(degreesIntoDash / LaunchSpeed) == (int)(320 / LaunchSpeed))
                        {
                            NPC.velocity = slingBack * 28;
                            SOTSUtils.PlaySound(SoundID.Item94, NPC.Center, 1.1f, 0.35f);
                        }
                        if ((int)(degreesIntoDash / LaunchSpeed) == (int)(50 / LaunchSpeed))
                        {
                            SOTSUtils.PlaySound(SoundID.Item23, NPC.Center, 1.0f, -0.7f);
                        }
                        if ((int)(degreesIntoDash / LaunchSpeed) == (int)(150 / LaunchSpeed))
                        {
                            SOTSUtils.PlaySound(SoundID.Item23, NPC.Center, 1.1f, -0.6f);
                        }
                        if ((int)(degreesIntoDash / LaunchSpeed) == (int)(250 / LaunchSpeed))
                        {
                            SOTSUtils.PlaySound(SoundID.Item23, NPC.Center, 1.2f, -0.5f);
                        }
                        if (degreesIntoDash >= 300)
                        {
                            int shotCooldown = 4;
                            if (Main.expertMode)
                            {
                                NPC.velocity *= 0.992f;
                                NPC.velocity += slingBack * 1.00f;
                                shotCooldown = 3;
                            }
                            if((int)AI1 % shotCooldown == 0)
                            {
                                if(Phase != 2)
                                {
                                    for (int i = 0; i < polarisWeaponData.Length; i++)
                                    {
                                        PolarisWeaponData Weapon = polarisWeaponData[i];
                                        Weapon.LaunchAttack(NPC, 2 + i / 2, 0);
                                    }
                                }
                                else if(AI1 % (shotCooldown * 2) == 0)
                                {
                                    if (SecondPhase)
                                    {
                                        SOTSUtils.PlaySound(SoundID.Item42, NPC.Center, 1.0f, -1f);
                                        if (Main.netMode != NetmodeID.MultiplayerClient)
                                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(1, 1), ModContent.ProjectileType<PolarisMines>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, ((int)AI1 / (shotCooldown * 2)) % 2, 180 + Main.rand.Next(60));
                                    }    
                                    //else
                                    //    SpawnShard(1, 0f);
                                }
                            }
                            if (degreesIntoDash > 300 + LaunchSpeed * 60)
                            {
                                AI0 = StartLaunching - 10;
                                AI1 = 0;
                                rotationDirection *= -1;
                                AI2++;
                                if(AI2 >= maxDashes)
                                {
                                    AI0 = -100;
                                }
                            }
                        }
                        else
                        {
                            float sinusoid2 = (float)Math.Sin(MathHelper.ToRadians(degreesIntoDash - 50)) * degreesIntoDash / 50f;
                            NPC.velocity = -slingBack * 5.4f * sinusoid2;
                        }
                        AI1++;
                    }
                    else
                    {
                        Vector2 toCircular = circularPosition + player.Center - NPC.Center;
                        float speed = 6f * Math.Clamp(toCircular.Length() / 160f, 0, 2);
                        if (speed > toCircular.Length())
                            speed = 0;
                        NPC.velocity += toCircular.SafeNormalize(Vector2.Zero) * speed * 0.2f;
                    }
                }
                else
                {
                    NPC.velocity *= 0.95f;
                    GoNearPlayer(400f, 640f, 9.5f);
                }
                float slowDown = 0;
                if(slowDown != 0)
                {
                    NPC.velocity *= 0.95f;
                }
                if (AI2 >= maxDashes)
                {
                    slowDown = AI2 - maxDashes;
                    AI2++;
                    CloseWeapons();
                }
                if (slowDown >= 90f)
                {
                    SecondPhase = true;
                    SwapPhase(3);
                }
                else
                {
                    if (AI0 > StartSpinning || slowDown != 0)
                    {
                        float speedFactor = (AI0 - StartSpinning) / (StartLaunching - StartHovering);
                        if (slowDown != 0)
                            speedFactor = 1 - slowDown / 90f;
                        float rotateSpeed = speedFactor * 15;
                        if (rotateSpeed > 15)
                            rotateSpeed = 15;
                        NPC.rotation += rotationDirection * rotateSpeed * MathHelper.TwoPi / 360f;
                    }
                }
            }
            else if (Phase == AttackID.LaserSpinAttack)
            {
                NPC.velocity *= 0.935f;
                GoNearPlayer(400f, 540f, 6f);
                if(AI0 < 60)
                {
                    if (SecondPhase && !hasUsedLaserSpinWithOnlySwordsAlready)
                    {
                        SwapAllWeapons(2);
                    }
                    else if(!hasUsedLaserSpinWithOnlyGunsAlready)
                    {
                        SwapAllWeapons(0);
                    }
                    else if(!hasUsedLaserSpinWithOnlyBeamsAlready)
                    {
                        SwapAllWeapons(1);
                    }
                    else
                    {
                        int weaponsAllowed = 2;
                        if (SecondPhase)
                            weaponsAllowed = 3;
                        for (int i = 0; i < polarisWeaponData.Length; i++)
                        {
                            PolarisWeaponData Weapon = polarisWeaponData[i];
                            Weapon.TypeToSwapTo = Main.rand.Next(weaponsAllowed);
                        }
                    }
                    AI0 = 60;
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                        NPC.netUpdate = true;
                }
                AI0++;
                if (AI0 > 120)
                {
                    float speedMultiplier = AI0 / 120f - 1;
                    float endMultiplier = 1f;
                    if (AI0 > 800)
                    {
                        CloseWeapons();
                        endMultiplier = 1 - ((AI0 - 800f) / 60f);
                    }
                    else
                        OpenWeapons();
                    if (endMultiplier < 0)
                    {
                        endMultiplier = 0;
                    }
                    if (speedMultiplier > 2)
                        speedMultiplier = 2;
                    speedMultiplier *= endMultiplier;
                    float extendOutDist = 64 + speedMultiplier * 480;
                    float extendOutDistNotSaber = 64 + speedMultiplier * 100;
                    if (extendOutDistNotSaber > 100)
                        extendOutDistNotSaber = 100;
                    if (extendOutDist > 480)
                        extendOutDist = 480;
                    float spinSpeed = speedMultiplier * 0.5f;
                    WeaponExtension = MathHelper.Lerp(WeaponExtension, extendOutDistNotSaber, 0.1f);
                    WeaponExtensionSabers = MathHelper.Lerp(WeaponExtensionSabers, extendOutDist, 0.1f);
                    NPC.rotation -= MathHelper.ToRadians(spinSpeed * 0.54f) * rotationDirection;
                    WeaponSpin = -NPC.rotation + MathHelper.ToRadians(AI1 * 0.54f) * rotationDirection;
                    AI1 += spinSpeed;
                    if(!CoreAttack)
                        AI2++;
                    if (AI0 > 180 && AI0 < 800)
                    {
                        float sinusoidSpeed = (float)Math.Sin(MathHelper.Pi * (AI0 - 120f) / 680f);
                        float weaponMinFirerate = 63 * (1 - sinusoidSpeed * sinusoidSpeed) + 5;
                        if (AI0 % 60 == 0)
                        {
                            for (int i = 0; i < polarisWeaponData.Length; i++)
                            {
                                PolarisWeaponData Weapon = polarisWeaponData[i];
                                if(Weapon.Type == 1)
                                {
                                    for (int j = -2; j <= 2; j++)
                                    {
                                        if(j != 0)
                                            Weapon.LaunchAttack(NPC, i / 2 + 2, MathHelper.ToRadians(j * 20));
                                    }
                                }
                                else if(Weapon.Type == 0)
                                    Weapon.LaunchAttack(NPC, i / 2 + 2, 0);
                            }
                        }
                        if (AI2 >= weaponMinFirerate && !CoreAttack)
                        {
                            AI2 = 0;
                            CoreAttack = true;
                        }
                    }
                }
                else
                {
                    resetRotation = resetWeaponSpin = resetWeaponExtension = true;
                    CloseWeapons();
                }
                if (AI0 > 890)
                {
                    if(SecondPhase && !hasUsedLaserSpinWithOnlySwordsAlready)
                    { 
                        hasUsedLaserSpinWithOnlySwordsAlready = true;
                    }
                    else
                    {
                        if (!hasUsedLaserSpinWithOnlyGunsAlready)
                        {
                            hasUsedLaserSpinWithOnlyGunsAlready = true;
                        }
                        else if (!hasUsedLaserSpinWithOnlyBeamsAlready)
                        {
                            hasUsedLaserSpinWithOnlyBeamsAlready = true;
                        }
                    }
                    if(!SecondPhase)
                        SwapPhase(AttackID.BulletStorm);
                    else
                        SwapPhase(AttackID.QuickBeamCircle);
                }
            }
            else if(Phase == AttackID.MortarRain)
            {
                resetRotation = true;
                AI0++;
                if(AI0 < 60)
                {
                    NPC.velocity *= 0.94f;
                    resetWeaponSpin = resetWeaponExtension = true;
                    AI1 = (-toPlayer.RotatedBy(MathHelper.PiOver2)).ToRotation();
                    CloseWeapons();
                }
                else
                {
                    float sin = (float)Math.Sin(MathHelper.ToRadians(Math.Clamp(AI2, 0, 450) * 2f) * rotationDirection);
                    float dashPosition = 600 * sin;
                    NPC.velocity *= 0.875f;
                    Vector2 positionToGoTo = new Vector2(0, -400).RotatedBy(AI1);
                    positionToGoTo.Y *= 0.8f;
                    positionToGoTo += player.Center;
                    positionToGoTo.X += dashPosition;
                    Vector2 goTo = positionToGoTo - NPC.Center;
                    float speed = 6 * MathHelper.Clamp(goTo.Length() / 240f, 0, 1);
                    if(goTo.Length() < speed)
                    {
                        speed = goTo.Length();
                    }
                    NPC.velocity += goTo.SafeNormalize(Vector2.Zero) * speed;
                    float transitionSpeed = 1 - (AI0 - 60f) / 180f;
                    transitionSpeed = Math.Clamp(transitionSpeed, 0, 1);
                    AI1 *= transitionSpeed;
                    if(Math.Abs(AI1) <= MathHelper.ToRadians(0.75f))
                    {
                        AI2++;
                        int shotRate = 4;
                        int mortarRate = 6;
                        if (Main.expertMode)
                        {
                            mortarRate = 5;
                            shotRate = 3;
                        }
                        if(AI2 < 420)
                        {
                            if (Math.Abs(dashPosition) > 520)
                            {
                                if (Math.Abs(dashPosition) > 550 && SecondPhase && AI2 % shotRate == 0 && AI2 > 60)
                                {
                                    for (int i = 0; i < polarisWeaponData.Length; i++)
                                    {
                                        PolarisWeaponData Weapon = polarisWeaponData[i];
                                        Weapon.LaunchAttack(NPC, i / 2 + 2, 0);
                                    }
                                }
                            }
                            else if(AI2 > 50 && AI2 % mortarRate == 0)
                            {
                                SOTSUtils.PlaySound(SoundID.Item42, NPC.Center, 1.0f, -1f);
                                if(Main.netMode != NetmodeID.MultiplayerClient)
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(sin * 1.5f, 1), ModContent.ProjectileType<PolarMortar>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, 0, ((int)AI2 / mortarRate) % 2);
                            }
                        }
                    }
                    if(AI2 > 420)
                    {
                        resetWeaponSpin = resetWeaponExtension = true;
                    }
                    else
                    {
                        if(SecondPhase)
                        {
                            WeaponSpin += MathHelper.ToRadians(5 * (1 - transitionSpeed)) * rotationDirection;
                            WeaponExtension = MathHelper.Lerp(WeaponExtension, 112f, 0.05f);
                            SwapAllWeapons(1);
                            OpenWeapons();
                        }
                    }
                    if(AI2 > 480)
                    {
                        if (SecondPhase)
                            SwapPhase(AttackID.SecondPhaseTransitionAttack);
                        else if (Main.rand.NextBool(2))
                            SwapPhase(AttackID.BulletStorm);
                        else
                            SwapPhase(AttackID.LaserSpinAttack);
                    }
                }
            }
            else if(Phase == AttackID.QuickBeamCircle)
            {
                resetRotation = true;
                resetWeaponSpin = true;
                NPC.velocity *= 0.925f;
                GoNearPlayer(360f, 720f, 3f);
                AI0++;
                //if(!hasUsedFirstAttackOnceAlready)
                SwapAllWeapons(1);
                /*else
                {
                    for (int i = 0; i < polarisWeaponData.Length; i++)
                    {
                        PolarisWeaponData Weapon = polarisWeaponData[i];
                        Weapon.TypeToSwapTo = 1 - i % 2;
                    }
                }*/
                if (AI0 > 100)
                {
                    OpenWeapons();
                    WeaponExtension = MathHelper.Lerp(WeaponExtension, 100f, 0.06f);
                }
                else
                {
                    CloseWeapons();
                }
                if (AI0 > 180)
                {
                    NPC.velocity *= 0.75f;
                    if (AI0 > AI1 + 180)
                    {
                        float sinusoid = (float)Math.Sin(MathHelper.ToRadians(AI2 * 6f));
                        for (int i = 0; i < polarisWeaponData.Length; i++)
                        {
                            PolarisWeaponData Weapon = polarisWeaponData[i];
                            for (int j = -1; j <= 1; j += 2)
                            {
                                Weapon.LaunchAttack(NPC, i / 2, MathHelper.ToRadians(j * 90 * sinusoid));
                            }
                        }
                        AI0 -= AI1;
                        if (AI1 > 3)
                            AI1--;
                        AI2++;
                    }
                    if (AI2 >= 60f)
                    {
                        SwapPhase(AttackID.BulletStorm);
                    }
                }
            }
            IdleCounter++;
            if(EyeRecoil <= 1)
            {
                EyeRecoil += (EyeRecoil + 1) / 20f + 0.01f;
            }
            EyeRecoil = Math.Clamp(EyeRecoil, -1, 1);
            //NPC.rotation += rotationDirection * MathHelper.TwoPi / 360f;
            NPC.rotation = MathHelper.WrapAngle(NPC.rotation);
            WeaponSpin = MathHelper.WrapAngle(WeaponSpin);
            if (resetRotation)
                NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, 0.1f);
            if (resetWeaponSpin)
            {
                WeaponSpin = MathHelper.Lerp(WeaponSpin, 0, 0.06f);
            }
            if (resetWeaponExtension)
            {
                WeaponExtensionSabers = MathHelper.Lerp(WeaponExtensionSabers, 64f, 0.06f);
                WeaponExtension = MathHelper.Lerp(WeaponExtension, 64f, 0.06f);
            }
            DoCoreUpdates();
        }
        public float IdleCounter = 0;
        public void GoNearPlayer(float TooCloseDist = 320f, float TooFarDist = 640f, float baseSpeed = 7.5f)
        {
            Player player = Main.player[NPC.target];
            Vector2 vectorToPlayer = player.Center - NPC.Center;
            int i = (int)NPC.Center.X / 16;
            int j = (int)NPC.Center.Y / 16;
            float idleAnim = (float)Math.Sin(MathHelper.ToRadians(IdleCounter * 3.5f)) * 1.5f;
            float length = vectorToPlayer.Length();
            if (SOTSWorldgenHelper.TrueTileSolid(i, j))
            {
                baseSpeed /= 3f;
                TooCloseDist /= 3f;
                TooFarDist /= 3f;
            }
            float speedMult = baseSpeed + idleAnim;
            if (length > TooFarDist)
            {
                float distBonus = (float)Math.Pow(length - TooFarDist, 1.05) * 0.015f;
                if (distBonus > 12)
                    distBonus = 12;
                speedMult += distBonus;
            }
            speedMult = speedMult * Math.Clamp((length - TooCloseDist) / TooCloseDist, -1, 1);
            if (speedMult < 0)
            {
                speedMult *= 0.25f;
            }
            NPC.velocity += vectorToPlayer.SafeNormalize(Vector2.Zero) * speedMult * 0.25f;
        }
        public override void PostAI()
        {
            if(Phase != 3)
                LerpingXVelocity = MathHelper.Lerp(LerpingXVelocity, NPC.velocity.X, 0.085f);
            else
                LerpingXVelocity = MathHelper.Lerp(LerpingXVelocity, 0, 0.01f);
            Player player = Main.player[NPC.target];
            if (player.dead)
            {
                despawn++;
            }
            else
            {
                despawn--;
                if (despawn < 0)
                    despawn = 0;
            }
            if (despawn >= 300)
            {
                NPC.active = false;
            }
        }
        public void LaunchMines()
        {
            SOTSUtils.PlaySound(SoundID.Item94, NPC.Center, 0.70f, -0.5f);
            Player player = Main.player[NPC.target];
            Vector2 toPlayer = player.Center - NPC.Center;
            toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
            NPC.velocity -= toPlayer * 8f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i= 0; i < 5; i++)
                {
                    Vector2 outward = new Vector2(6.4f + i * 3.6f, 0).RotatedBy(toPlayer.ToRotation()) + Main.rand.NextVector2Circular(3f, 3f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outward, ModContent.ProjectileType<PolarisMines>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, Main.rand.Next(2), Main.rand.Next(60));
                }
            }
        }
        public void SwapPhase(int phase)
        {
            if (Main.expertMode && Phase != -1)
                SpawnShard(4, 0.6f);
            if (!SecondPhase)
            {
                if (needsASecondPhaseTransition)
                {
                    phase = 2;
                }
                if (phase == 2 && !needsASecondPhaseTransition)
                {
                    phase = 3;
                }
            }
            if(hasUsedBeamAttackAlready)
            {
                if (phase == AttackID.BeamsAndMineShotgun || phase == AttackID.MortarRain)
                {
                    if (Main.rand.NextBool(2) || SecondPhase)
                        phase = AttackID.MortarRain;
                    else
                        phase = AttackID.BeamsAndMineShotgun;
                }
            }
            Player player = Main.player[NPC.target];
            Vector2 toPlayer = player.Center - NPC.Center;
            Phase = phase;
            AI0 = 0;
            AI1 = 0;
            AI2 = 0;
            if (phase == 0 || phase == AttackID.QuickBeamCircle)
            {
                int sign = Math.Sign(toPlayer.X);
                if (sign != 0)
                    rotationDirection = sign;
                AI0 = 60;
                AI1 = 20;
            }
            if(phase == 1)
                hasUsedBeamAttackAlready = true;
            if (phase == 2 || phase == 3)
            {
                int sign = Math.Sign(toPlayer.X);
                if(sign != 0)
                    rotationDirection = sign;
                if(phase != 3)
                {
                    if (SecondPhase)
                    {
                        needsASecondPhaseTransition = false;
                        if(!Main.rand.NextBool(3))
                        {
                            Phase = Main.rand.Next(4, 6);
                        }
                    }
                }
                else
                {
                    rotationDirection *= -1;
                }
            }
            if(phase == AttackID.MortarRain)
            {
                int sign = Math.Sign(toPlayer.X);
                if (sign != 0)
                    rotationDirection = sign;
            }
            NPC.netUpdate = true;
        }
        public void SpawnShard(int amt = 1, float randomnessMult = 1f)
        {
            Player player = Main.player[NPC.target];
            SOTSUtils.PlaySound(SoundID.Item44, (int)NPC.Center.X, (int)NPC.Center.Y, 1.0f, -0.4f);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int damage = NPC.GetBaseDamage() / 2;
                for (int i = 0; i < amt; i++)
                {
                    float max = 50 + 25 * i;
                    Vector2 spawnPosition = NPC.Center + new Vector2(Main.rand.NextFloat(max), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPosition, Vector2.Zero, ModContent.ProjectileType<PolarMortar>(), damage, 0, Main.myPlayer, player.Center.X + Main.rand.NextFloat(-360, 360) * randomnessMult, player.Center.Y - Main.rand.NextFloat(140, 220 * randomnessMult));
                }
            }
        }
        public override void OnKill()
        {
            SOTSWorld.downedAmalgamation = true;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PolarisBossBag>()));
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SoulOfPlight>(), 1, 25, 40));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.HallowedBar, 1, 15, 30));
            npcLoot.Add(notExpertRule);
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<PolarisRelic>()));
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                int goreNum = 1;
                int goreNum2 = 3;
                Vector2 goreOrigin = new Vector2(44, 10);
                for (int i = 1; i <= 4; i++)
                {
                    if (i == 2)
                    { 
                        goreOrigin = new Vector2(76, 16);
                        goreNum = 4; 
                        goreNum2 = 3;
                    }
                    if (i == 3)
                    {
                        goreOrigin = new Vector2(44, 90);
                        goreNum = 5; 
                        goreNum2 = 2;
                    }
                    if (i == 4)
                    {
                        goreOrigin = new Vector2(10, 16);
                        goreNum = 6;
                        goreNum2 = 2;
                    }
                    Vector2 circular = new Vector2(0, 32).RotatedBy(MathHelper.ToRadians(i * 90));
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular - goreOrigin, circular * 0.15f, ModGores.GoreType("Gores/Polaris/PolarisGore" + goreNum), 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular.RotatedBy(MathHelper.PiOver4), circular.RotatedBy(MathHelper.PiOver4) * 0.15f, ModGores.GoreType("Gores/Polaris/PolarisGore7"), 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + circular.RotatedBy(MathHelper.PiOver4) * 2, circular.RotatedBy(MathHelper.PiOver4) * 0.15f, ModGores.GoreType("Gores/Polaris/PolarisGore" + goreNum2), 1f);
                }
                for(int i= 0; i < 120; i++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(i / 120f * MathHelper.TwoPi);
                    Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y) - new Vector2(5) + circular * 32, NPC.width, NPC.height, ModContent.DustType<Dusts.CopyDust4>());
                    dust.velocity *= 0.5f;
                    dust.velocity += circular * 8;
                    dust.velocity += NPC.velocity * 0.1f;
                    dust.noGravity = true;
                    dust.scale += 0.75f;
                    dust.color = Color.Lerp(new Color(100, 100, 250, 200), new Color(250, 100, 100, 200), (float)Math.Cos(MathHelper.Pi / 120f * i));
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.5f;
                    dust.alpha = NPC.alpha;
                }
            }
        }
    }
    public class PolarisWeaponData : Entity
    { 
        /// <summary>
        /// 0 = Cannon,
        /// 1 = Laser,
        /// 2 = Saber
        /// </summary>
        public int Type = 0;
        public int redOrBlue = 0;
        public int Width = 50;
        public int Height = 52;
        public int FrameCounter = 0;
        public int Frame = 0;
        public int FrameMax = 8;
        public int TypeToSwapTo = 0;
        public float AI = 0;
        public float rotation;
        public int rotationDirection = 1;
        public int previousRotationDirection = 1;
        public bool ReRegisterPrimTrail = false;
        public PrimTrail SaberTrail = null;
        public bool close = false;
        public bool wasClosedLastTick = false;
        public PolarisWeaponData() {
            Type = 0;
            Width = 50;
            Height = 52;
            FrameCounter = 0;
            FrameMax = 8;
            Frame = FrameMax - 1;
            TypeToSwapTo = 0;
        }
        public void DrawTether(SpriteBatch spriteBatch, Vector2 screenPosition, NPC owner)
        {
            Color Color1 = new Color(187, 11, 76, 0);
            Color Color2 = new Color(202, 234, 247, 0);
            if (redOrBlue == 0)
            {
                Color1 = new Color(64, 74, 204, 0);
                Color2 = new Color(255, 221, 233, 0);
            }
            Texture2D helixText = ModContent.Request<Texture2D>("SOTS/Projectiles/Permafrost/PolarisTrail").Value;
            Vector2 center = position;
            Vector2 ownerCenter = owner.Center;
            Vector2 toOwner = ownerCenter - position;
            ownerCenter -= toOwner.SafeNormalize(Vector2.Zero) * 48f;
            center += toOwner.SafeNormalize(Vector2.Zero) * 16f;
            float length = Vector2.Distance(center, owner.Center);
            float textureSize = helixText.Width;
            float max = length / textureSize;
            Vector2 origin2 = new Vector2(helixText.Width / 2, helixText.Height / 2);
            for (float v = 0; v < max; v += 0.125f)
            {
                Vector2 drawPos = Vector2.Lerp(center, ownerCenter, v / max);
                Vector2 direction = ownerCenter - center;
                float rotation = direction.ToRotation();
                float heightOfHelix = 1 - v / max * 0.7f;
                for (int b = -1; b <= 1; b++)
                {
                    float scaleY = 1f;
                    float size = 24;
                    float sinusoidMod = (0.75f + 0.25f * (float)Math.Sin(MathHelper.ToRadians(scaleY * SOTSWorld.GlobalCounter * 3f))) * b;
                    float sizeOfHelix = sinusoidMod * heightOfHelix * size;
                    float sinMult = (float)Math.Sin(MathHelper.ToRadians(v * 30 + SOTSWorld.GlobalCounter * 3));
                    sizeOfHelix *= sinMult;
                    Vector2 sinusoid = new Vector2(0, sizeOfHelix).RotatedBy(rotation);
                    spriteBatch.Draw(helixText, drawPos - screenPosition + sinusoid, null, Color.Lerp(Color1, Color2, v / max) * 0.125f * (b == 0 ? Math.Abs(sinMult) : 0.5f + 0.5f * Math.Abs(sinMult)), rotation, origin2, new Vector2(0.5f, scaleY * heightOfHelix * 0.75f), SpriteEffects.None, 0f);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 screenPosition, Color color)
        {
            Texture2D texture = ModContent.Request<Texture2D>(NewPolaris.WeaponTexture(redOrBlue, Type, false)).Value;
            Texture2D glow = ModContent.Request<Texture2D>(NewPolaris.WeaponTexture(redOrBlue, Type, true)).Value;
            int frameToUse = Frame;
            Rectangle frame = new Rectangle(0, Height * frameToUse, Width, Height - 2);
            Vector2 origin = new Vector2(32, Height - 34);
            Vector2 position = this.position - screenPosition;
            Color lightColor = Lighting.GetColor((int)this.position.X / 16, (int)this.position.Y / 16, Color.White);
            spriteBatch.Draw(texture, position, frame, Color.Lerp(lightColor, Color.White, 0.25f), rotation + MathHelper.Pi, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glow, position, frame, Color.White, rotation + MathHelper.Pi, origin, 1f, SpriteEffects.None, 0f);
        }
        public void Update(Vector2 updatedPosition)
        {
            active = true;
            if (TypeToSwapTo == Type) //If the switch is completed
            {
                if (Frame == 0)
                {
                    if(Main.netMode != NetmodeID.Server)
                    {
                        if (Type == 2 && (AI == 0 || ReRegisterPrimTrail))
                        {
                            Color Color1 = new Color(187, 11, 76, 0);
                            Color Color2 = new Color(202, 234, 247, 0);
                            if (redOrBlue == 0)
                            {
                                Color1 = new Color(64, 74, 204, 0);
                                Color2 = new Color(255, 221, 233, 0);
                            }
                            PolarisSaberTrail trail = new PolarisSaberTrail(this, rotationDirection, Color2.ToVector4() * 0.5f, Color1.ToVector4() * 0.5f, 30, 0);
                            SOTS.primitives.CreateTrail(trail);
                            SaberTrail = trail;
                            ReRegisterPrimTrail = false;
                        }
                        if (Type == 2 && (previousRotationDirection != rotationDirection || wasClosedLastTick != close))
                        {
                            if (SaberTrail != null)
                                SaberTrail.OnDestroy();
                            ReRegisterPrimTrail = true;
                        }
                    }
                    AI++;
                }
                else
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (SaberTrail != null)
                        {
                            SaberTrail.OnDestroy();
                        }
                        ReRegisterPrimTrail = true;
                    }
                }
                if (Frame == 4 && FrameCounter == 0)
                {
                    SOTSUtils.PlaySound(SoundID.MenuOpen, position, 1.4f, -0.75f);
                }
            }
            else
            {
                if (TypeToSwapTo != 2)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (SaberTrail != null)
                            SaberTrail.OnDestroy();
                    }
                    ReRegisterPrimTrail = true;
                }
            }
            position = updatedPosition;
            previousRotationDirection = rotationDirection;
            wasClosedLastTick = close;
        }
        public void UpdateFrame()
        {
            int frameSpeed = 3;
            FrameCounter++;
            if (FrameCounter > frameSpeed)
            {
                FrameCounter = 0;
                if (Type != TypeToSwapTo || close)
                {
                    Frame++;
                    if (Frame >= FrameMax - 1) //Frame counter going up means resetting the frame back to default
                    {
                        Frame = FrameMax - 1; //The frame counter should hold a delay after this point... maybe just set the counter to a negative number?
                        if(!close)
                        {
                            SwapWeapon(TypeToSwapTo);
                            FrameCounter = -60;
                        }
                    }
                }
                else if (Frame < FrameMax)
                {
                    Frame--;
                    if (Frame <= 0) //Frame counter going down means opening up the weapon
                    {
                        Frame = 0;
                    }
                }
            }
        }
        public void SwapWeapon(int type)
        {
            Width = 50;
            Height = 52;
            FrameMax = 8;
            if (type == 1)
            {
                Width = 48;
                Height = 50;
                FrameMax = 8;
            }
            if (type == 2)
            {
                Width = 94;
                Height = 96;
                FrameMax = 15;
            }
            Frame = FrameMax - 1;
            Type = type;
            AI = 0;
        }
        public void LaunchAttack(NPC owner, int style = 0, float spread = 0)
        {
            int attackStyle = style / 2;
            int colorStyle = style % 2;
            if(Frame == 0 && TypeToSwapTo == Type)
            {
                Vector2 outward = new Vector2(-1, 0).RotatedBy(rotation - MathHelper.PiOver4 + spread);
                if (Type == 0)
                {
                    if (attackStyle == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(owner.GetSource_FromAI(), position + outward * 4, outward * 4, ModContent.ProjectileType<PolarBullet>(), owner.GetBaseDamage() / 2, 0, Main.myPlayer, 0, 1 - colorStyle);
                        }
                        SOTSUtils.PlaySound(SoundID.Item11, position, 0.90f, -0.4f);
                    }
                    else
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(owner.GetSource_FromAI(), position + outward * 4, outward * 20, ModContent.ProjectileType<PolarisMines>(), owner.GetBaseDamage() / 2, 0, Main.myPlayer, colorStyle, 400);
                        }
                        SOTSUtils.PlaySound(SoundID.Item61, position, 1.10f, -0.1f);
                    }
                }
                if (Type == 1)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(owner.GetSource_FromAI(), position + outward * 32, outward * 4, ModContent.ProjectileType<PolarisBeam>(), owner.GetBaseDamage() / 2, 0, Main.myPlayer, colorStyle + attackStyle * 2);
                    }
                    SOTSUtils.PlaySound(SoundID.Item33, position, 0.70f, -0.1f);
                }
            }
        }
    }
}