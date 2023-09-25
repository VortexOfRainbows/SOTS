using System;
using System.Collections.Generic;
using System.Security.Permissions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SOTS.Items.Banners;
using SOTS.Items.Chaos;
using SOTS.Items.ChestItems;
using SOTS.Items.Permafrost;
using SOTS.Prim;
using SOTS.Prim.Trails;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Permafrost;
using SOTS.WorldgenHelpers;
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
        public bool LoadedWeaponData = false;
        public PolarisWeaponData[] polarisWeaponData = new PolarisWeaponData[4];
        public int despawn = 0;
		private float AI0
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float AI1
        {
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float AI2
        {
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float AI3
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
					BuffID.Ichor
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
                if(Weapon.Type == 2 && Weapon.Frame == 0 && Weapon.TypeToSwapTo == 2)
                {
                    float collisionpoint = 0;
                    if (Collision.CheckAABBvLineCollision(target.position, target.Size, Weapon.position + (Weapon.position - NPC.Center).SafeNormalize(Vector2.Zero) * 84, NPC.Center, 18, ref collisionpoint))
                    {
                        return true;
                    }
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
                eyeOffset = toPlayer.SafeNormalize(Vector2.Zero) * 4;
            }
            spriteBatch.Draw(eyeTexture, position + eyeOffset, null, Color.White, 0f, eyeTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(coreTexture, position, null, Color.White, NPC.velocity.X * 0.05f, coreTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(myTexture, position, NPC.frame, Color.White, NPC.rotation + MathHelper.Pi + NPC.velocity.X * 0.05f, origin, 1f, SpriteEffects.None, 0f);
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
                Weapon.Draw(spriteBatch, screenPos, drawColor);
            }
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }
        public override void FindFrame(int frameHeight)
        {
            foreach(PolarisWeaponData Weapon in polarisWeaponData)
            {
                Weapon.UpdateFrame();
            }
        }
        public int rotationDirection = 1;
        public void UpdateWeapons()
        {
            for (int i = 0; i < polarisWeaponData.Length; i++)
            {
                PolarisWeaponData Weapon = polarisWeaponData[i];
                if (AI1 % 200 == 0)
                {
                    int nextType = Main.rand.Next(3);
                    while (nextType == Weapon.Type)
                    {
                        nextType = Main.rand.Next(3);
                    }
                    Weapon.TypeToSwapTo = nextType;
                    SOTSUtils.PlaySound(SoundID.MenuClose, Weapon.position, 1.1f, -0.5f);
                }
                Vector2 position = NPC.Center + new Vector2(64, 64).RotatedBy(i * MathHelper.PiOver2 + NPC.rotation + NPC.velocity.X * 0.05f);
                Weapon.Update(position);
                float rotation = (i - 1) * MathHelper.PiOver2 + NPC.rotation + NPC.velocity.X * 0.05f;
                Weapon.rotation = rotation;
                Weapon.rotationDirection = rotationDirection;
                Weapon.redOrBlue = 1 - i / 2;
            }
        }
        public override void PostAI()
        {
            Player player = Main.player[NPC.target];
            if (player.dead)
            {
                despawn++;
            }
            if (despawn >= 600)
            {
                NPC.active = false;
            }
            AI1++; 
            Vector2 toPlayer = player.Center - NPC.Center;
            NPC.velocity *= 0.95f;
            NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.2f;
            rotationDirection = Math.Sign(NPC.velocity.X);
            NPC.rotation += rotationDirection * MathHelper.TwoPi / 360f;
            UpdateWeapons();
            AI2++;
            if(AI2 > 360)
            {
                AI2 = -60;
                LaunchMines();
            }
            if(AI2 > 0)
            {
                if (AI2 % 30 == 0)
                {
                    for (int i = 0; i < polarisWeaponData.Length; i++)
                    {
                        PolarisWeaponData Weapon = polarisWeaponData[i];
                        Weapon.LaunchAttack(NPC, i / 2);
                    }
                }
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.63889f * balance * bossAdjustment);  //boss life scale in expertmode
            NPC.damage = (int)(NPC.damage * 0.75f);  //boss damage increase in expermode
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
		public override void AI()
		{
			//Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
		}
        public override bool PreAI()
        {
            if (!NPC.HasValidTarget)
            {
                NPC.TargetClosest(false);
            }
            if (!LoadedWeaponData)
            {
                LoadedWeaponData = true;
                polarisWeaponData = new PolarisWeaponData[]
                {
                    new PolarisWeaponData(),
                    new PolarisWeaponData(),
                    new PolarisWeaponData(),
                    new PolarisWeaponData()
                };
            }
            return true;
        }
        public void LaunchMines()
        {
            SOTSUtils.PlaySound(SoundID.Item94, NPC.Center, 1.0f, -0.5f);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[NPC.target];
                Vector2 toPlayer = player.Center - NPC.Center;
                toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
                NPC.velocity -= toPlayer * 12f;
                for (int i= -2; i <= 2; i++)
                {
                    Vector2 outward = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i * 15) + toPlayer.ToRotation()) + Main.rand.NextVector2Circular(4, 4);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 2, outward * Main.rand.NextFloat(0.75f, 1.25f), ModContent.ProjectileType<PolarisMines>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, Main.rand.Next(2), Main.rand.Next(60));
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
        public PolarisWeaponData() {
            Type = 0;
            Width = 50;
            Height = 52;
            FrameCounter = 0;
            Frame = 0;
            FrameMax = 8;
            TypeToSwapTo = 0;
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
                        PolarisSaberTrail trail = new PolarisSaberTrail(this, rotationDirection, Color2.ToVector4() * 0.5f, Color1.ToVector4() * 0.5f, 48, 0);
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
                if (Type != TypeToSwapTo)
                {
                    Frame++;
                    if (Frame >= FrameMax - 1) //Frame counter going up means resetting the frame back to default
                    {
                        Frame = FrameMax - 1; //The frame counter should hold a delay after this point... maybe just set the counter to a negative number?
                        SwapWeapon(TypeToSwapTo);
                        FrameCounter = -60;
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
        public void LaunchAttack(NPC owner, int style = 0)
        {
            if(Frame == 0 && TypeToSwapTo == Type)
            {
                Vector2 outward = new Vector2(-1, 0).RotatedBy(rotation - MathHelper.PiOver4);
                if (Type == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(owner.GetSource_FromAI(), position + outward * 4, outward * 4, ProjectileID.Bullet, 0, 0, Main.myPlayer);
                    }
                    SOTSUtils.PlaySound(SoundID.Item11, position, 1.0f, -0.4f);
                }
                if (Type == 1)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(owner.GetSource_FromAI(), position + outward * 32, outward * 4, ModContent.ProjectileType<PolarisBeam>(), owner.GetBaseDamage() / 2, 0, Main.myPlayer, style);
                    }
                    SOTSUtils.PlaySound(SoundID.Item33, position, 1.0f, -0.1f);
                }
            }
        }
    }
}