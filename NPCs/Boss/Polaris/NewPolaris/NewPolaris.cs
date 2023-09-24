using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SOTS.Items.Banners;
using SOTS.Items.Chaos;
using SOTS.Items.Permafrost;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Permafrost;
using SOTS.WorldgenHelpers;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

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
            Main.npcFrameCount[NPC.type] = 1;
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
            NPC.lifeMax = 36000;
            NPC.damage = 80; 
            NPC.defense = 28;  
            NPC.knockBackResist = 0f;
            NPC.width = 200;
            NPC.height = 200;
            NPC.value = 100000;
            NPC.npcSlots = 1f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Polaris");
            NPC.netAlways = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawWeapon(spriteBatch, screenPos, Color.White);
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
                Texture2D texture = ModContent.Request<Texture2D>(WeaponTexture(0, Weapon.Type, false)).Value;
                Rectangle frame = new Rectangle(0, Weapon.Height * Weapon.Frame, Weapon.Width, Weapon.Height - 2);
                Vector2 origin = new Vector2(32, Weapon.Height - 34);
                Vector2 position = NPC.Center - screenPos + new Vector2(48, 48).RotatedBy(i * MathHelper.PiOver2);
                spriteBatch.Draw(texture, position, frame, drawColor, (i - 1) * MathHelper.PiOver2, origin, 1f, SpriteEffects.None, 0f);
            }
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }
        public void QueueSwapWeapon(PolarisWeaponData Weapon, int type)
        {
            Weapon.TypeToSwapTo = type;
        }
        public void SwapWeapon(PolarisWeaponData Weapon, int type)
        {
            Weapon.Width = 50;
            Weapon.Height = 52;
            Weapon.FrameMax = 8;
            if (type == 1)
            {
                Weapon.Width = 48;
                Weapon.Height = 50;
                Weapon.FrameMax = 8;
            }
            if (type == 2)
            {
                Weapon.Width = 94;
                Weapon.Height = 96;
                Weapon.FrameMax = 15;
            }
            Weapon.Frame = Weapon.FrameMax - 1;
            Weapon.Type = type;
        }
        public override void FindFrame(int frameHeight)
        {
            foreach(PolarisWeaponData Weapon in polarisWeaponData)
            {
                Weapon.FrameCounter++;
                if (Weapon.FrameCounter > 3)
                {
                    Weapon.FrameCounter = 0;
                    if (Weapon.Type != Weapon.TypeToSwapTo)
                    {
                        Weapon.Frame++;
                        if (Weapon.Frame >= Weapon.FrameMax - 1) //Frame counter going up means resetting the frame back to default
                        {
                            Weapon.Frame = Weapon.FrameMax - 1; //The frame counter should hold a delay after this point... maybe just set the counter to a negative number?
                            SwapWeapon(Weapon, Weapon.TypeToSwapTo);
                            Weapon.FrameCounter = -60;
                        }
                    }
                    else if (Weapon.Frame < Weapon.FrameMax)
                    {
                        Weapon.Frame--;
                        if (Weapon.Frame <= 0) //Frame counter going down means opening up the weapon
                        {
                            Weapon.Frame = 0;
                        }
                    }
                }
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
            if(AI1 % 200 == 0)
            {
                Main.NewText("Swap");
                foreach (PolarisWeaponData Weapon in polarisWeaponData)
                {
                    int nextType = Main.rand.Next(3);
                    while (nextType == Weapon.Type)
                    {
                        nextType = Main.rand.Next(3);
                    }
                    QueueSwapWeapon(Weapon, nextType);
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
			if(!NPC.HasValidTarget)
			{
				NPC.TargetClosest(false);
			}
			//Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
		}
        public override bool PreAI()
        {
            if(!LoadedWeaponData)
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
    }
    public class PolarisWeaponData
    { 
        public int Type = 0;
        public int Width = 50;
        public int Height = 52;
        public int FrameCounter = 0;
        public int Frame = 0;
        public int FrameMax = 8;
        public int TypeToSwapTo = 0;
        public PolarisWeaponData() {
            Type = 0;
            Width = 50;
            Height = 52;
            FrameCounter = 0;
            Frame = 0;
            FrameMax = 8;
            TypeToSwapTo = 0;
        }
        public void Draw()
        {

        }
        public void Update()
        {

        }
        public void UpdateFrame()
        {
            
        }
    }
}





















