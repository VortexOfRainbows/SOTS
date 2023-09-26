using System;
using System.Collections.Generic;
using System.Security.Permissions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
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
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace SOTS.NPCs.Boss.Polaris.NewPolaris
{	[AutoloadBossHead]
	public class NewPolaris : ModNPC
    {
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.63889f * balance * bossAdjustment);  //boss life scale in expertmode
            NPC.damage = (int)(NPC.damage * 0.75f);  //boss damage increase in expermode
        }
        public bool LoadedWeaponData = false;
        public PolarisWeaponData[] polarisWeaponData = new PolarisWeaponData[4];
        public int despawn = 0;
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
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Frostburn,
					BuffID.OnFire,
					BuffID.Ichor,
                    BuffID.CursedInferno,
                    BuffID.OnFire3
				}
			});
		}
		public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPC.aiStyle = -1;
            NPC.lifeMax = 36000;
            NPC.damage = 80; 
            NPC.defense = 28;  
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = 100000;
            NPC.npcSlots = 1f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Polaris");
            NPC.netAlways = true;
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
            int width = 500;
            npcHitbox = new Rectangle((int)NPC.Center.X - width / 2, (int)NPC.Center.Y - width / 2, width, width);
            return true;
        }
        public float TiltBasedRotation => Math.Clamp(LerpingXVelocity * 0.05f, -MathHelper.TwoPi / 16f, MathHelper.TwoPi / 16f);
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!LoadedWeaponData)
                return false;
            DrawWeapon(spriteBatch, screenPos, Color.White);
            Texture2D coreTexture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Polaris/NewPolaris/PolarisInnerCore").Value;
            Texture2D eyeTexture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Polaris/NewPolaris/PolarisEye").Value;
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
            spriteBatch.Draw(eyeTexture, position + eyeOffset, null, Color.White, 0f, eyeTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(coreTexture, position, null, Color.White, TiltBasedRotation, coreTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(myTexture, position, NPC.frame, Color.White, NPC.rotation + MathHelper.Pi + TiltBasedRotation, origin, 1f, SpriteEffects.None, 0f);
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
        public int rotationDirection = 1;
        public float WeaponExtension = 64;
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
                Vector2 position = NPC.Center + new Vector2(WeaponExtension, WeaponExtension).RotatedBy(i * MathHelper.PiOver2 + addedRotation);
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
            if (!NPC.HasValidTarget)
            {
                NPC.TargetClosest(false);
            }
            if (!LoadedWeaponData)
            {
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
            return true;
        }
        float LerpingXVelocity = 0;
        public override void AI()
        {
            if(NPC.velocity.X != 0 && Phase != 0 && Phase != 2)
            {
                rotationDirection = Math.Sign(NPC.velocity.X);
            }
            Player player = Main.player[NPC.target];
            Vector2 toPlayer = player.Center - NPC.Center;
            bool resetRotation = false;
            bool resetWeaponSpin = false;
            bool resetWeaponExtension = false;
            UpdateWeapons();
            if(Phase == 0)
            {
                resetRotation = true;
                NPC.velocity *= 0.925f;
                GoNearPlayer(320f, 1280f, 3f);
                AI0++;
                SwapAllWeapons(0);
                if (AI0 > 120)
                {
                    OpenWeapons();
                    WeaponExtension = MathHelper.Lerp(WeaponExtension, 100f, 0.06f);
                }
                else
                    CloseWeapons();
                float totalSpinTime = 540f;
                if (AI0 > 180)
                {
                    AI2++;
                    if(AI2 > totalSpinTime / 2f)
                    {
                        AI2 += 1f;
                    }
                    float spinSpeed = (float)Math.Sin(AI2 / totalSpinTime * MathHelper.Pi);
                    spinSpeed = Math.Clamp(6 * spinSpeed, 0, 6);
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
                    if(AI2 >= totalSpinTime)
                    {
                        SwapPhase(1);
                    }
                }
            }
            else if(Phase == 1)
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
                        SwapPhase(2);
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
            else if(Phase == 2)
            {
                int maxDashes = 4;
                resetWeaponSpin = resetWeaponExtension = true;
                if(AI2 < maxDashes)
                {
                    AI0++;
                    SwapAllWeapons(2);
                    if (AI0 > 60)
                    {
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
                        if (degreesIntoDash >= 300)
                        {
                            if(degreesIntoDash == 300)
                                NPC.velocity = slingBack * 40;
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
                    GoNearPlayer(360f, 640f, 4.5f);
                }
                float slowDown = 0;
                if (AI2 >= maxDashes)
                {
                    slowDown = AI2 - maxDashes;
                    AI2++;
                    CloseWeapons();
                }
                if (slowDown >= 120f)
                {
                    SwapPhase(0);
                }
                else
                {
                    if (AI0 > StartSpinning || slowDown != 0)
                    {
                        float speedFactor = (AI0 - StartSpinning) / (StartLaunching - StartHovering);
                        if (slowDown != 0)
                            speedFactor = 1 - slowDown / 120f;
                        float rotateSpeed = speedFactor * 15;
                        if (rotateSpeed > 15)
                            rotateSpeed = 15;
                        NPC.rotation += rotationDirection * rotateSpeed * MathHelper.TwoPi / 360f;
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
            if(resetRotation)
                NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, 0.1f);
            if (resetWeaponSpin)
                WeaponSpin = MathHelper.Lerp(WeaponSpin, 0, 0.06f);
            if (resetWeaponExtension)
                WeaponExtension = MathHelper.Lerp(WeaponExtension, 64f, 0.06f);
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
            LerpingXVelocity = MathHelper.Lerp(LerpingXVelocity, NPC.velocity.X, 0.085f);
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
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[NPC.target];
                Vector2 toPlayer = player.Center - NPC.Center;
                toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
                NPC.velocity -= toPlayer * 8f;
                for (int i= 0; i < 5; i++)
                {
                    Vector2 outward = new Vector2(6.4f + i * 3.6f, 0).RotatedBy(toPlayer.ToRotation()) + Main.rand.NextVector2Circular(3f, 3f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outward, ModContent.ProjectileType<PolarisMines>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, Main.rand.Next(2), Main.rand.Next(60));
                }
            }
        }
        public void SwapPhase(int phase)
        {
            Player player = Main.player[NPC.target];
            Vector2 toPlayer = player.Center - NPC.Center;
            Phase = phase;
            AI0 = 0;
            AI1 = 0;
            AI2 = 0;
            if (phase == 0)
            {
                AI0 = 60;
                AI1 = 20;
            }
            if (phase == 2)
            {
                rotationDirection = Math.Sign(toPlayer.X);
            }
            NPC.netUpdate = true;
        }
        public override void OnKill()
        {
            SOTSWorld.downedAmalgamation = true;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PolarisBossBag>()));
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AbsoluteBar>(), 1, 26, 34));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.FrostCore, 1, 1, 2));
            npcLoot.Add(notExpertRule);
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<PolarisRelic>()));
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
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
                float heightOfHelix = 1 - v / max;
                for (int b = -1; b <= 1; b++)
                {
                    float scaleY = 1f;
                    float size = 24;
                    float sinusoidMod = (0.75f + 0.25f * (float)Math.Sin(MathHelper.ToRadians(scaleY * SOTSWorld.GlobalCounter * 3f))) * b;
                    float sizeOfHelix = sinusoidMod * heightOfHelix * size;
                    sizeOfHelix *= (float)Math.Sin(MathHelper.ToRadians(v * 30 + SOTSWorld.GlobalCounter * 3));
                    if (b == 0 || Math.Abs(sizeOfHelix) > 2f)
                    {
                        Vector2 sinusoid = new Vector2(0, sizeOfHelix).RotatedBy(rotation);
                        spriteBatch.Draw(helixText, drawPos - screenPosition + sinusoid, null, Color.Lerp(Color1, Color2, v / max) * 0.125f, rotation, origin2, new Vector2(0.5f, scaleY * heightOfHelix * 0.75f), SpriteEffects.None, 0f);
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 screenPosition, Color color)
        {
            Texture2D texture = ModContent.Request<Texture2D>(NewPolaris.WeaponTexture(redOrBlue, Type, false)).Value;
            int frameToUse = Frame;
            Rectangle frame = new Rectangle(0, Height * frameToUse, Width, Height - 2);
            Vector2 origin = new Vector2(32, Height - 34);
            Vector2 position = this.position - screenPosition;
            spriteBatch.Draw(texture, position, frame, color, rotation + MathHelper.Pi, origin, 1f, SpriteEffects.None, 0f);
        }
        public void Update(Vector2 updatedPosition)
        {
            active = true;
            if (TypeToSwapTo == Type) //If the switch is completed
            {
                if (Frame == 0)
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
                    if(Type == 2 && previousRotationDirection != rotationDirection)
                    {
                        if (SaberTrail != null)
                            SaberTrail.OnDestroy();
                        ReRegisterPrimTrail = true;
                    }
                    AI++;
                }
                else
                {
                    if (SaberTrail != null)
                    {
                        SaberTrail.OnDestroy();
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
                    if (SaberTrail != null)
                        SaberTrail.OnDestroy();
                }
            }
            position = updatedPosition;
            previousRotationDirection = rotationDirection;
        }
        public void UpdateFrame()
        {
            int frameSpeed = 3;
            if (Type == 2)
                frameSpeed = 2;
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
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(owner.GetSource_FromAI(), position + outward * 4, outward * 4, ModContent.ProjectileType<PolarBullet>(), owner.GetBaseDamage() / 2, 0, Main.myPlayer, 0, 1 - colorStyle);
                    }
                    SOTSUtils.PlaySound(SoundID.Item11, position, 0.80f, -0.4f);
                }
                if (Type == 1)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(owner.GetSource_FromAI(), position + outward * 32, outward * 4, ModContent.ProjectileType<PolarisBeam>(), owner.GetBaseDamage() / 2, 0, Main.myPlayer, colorStyle);
                    }
                    SOTSUtils.PlaySound(SoundID.Item33, position, 0.70f, -0.1f);
                }
            }
        }
    }
}