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
			Texture2D texture = ModContent.Request<Texture2D>(WeaponTexture(0, WeaponType, false)).Value;
            Rectangle frame = new Rectangle(0, WeaponHeight * WeaponFrame, WeaponWidth, WeaponHeight - 2);
            Vector2 origin = new Vector2(32, WeaponHeight - 34);
            Vector2 position = NPC.Center - screenPos;
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
        public int WeaponType = 0;
        public int WeaponWidth = 50;
        public int WeaponHeight = 52;
        public int WeaponFrameCounter = 0;
        public int WeaponFrame = 0;
        public int WeaponFrameMax = 8;
        public int TypeToSwapTo = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }
        public void QueueSwapWeapon(int type)
        {
            TypeToSwapTo = type;
        }
        public void SwapWeapon(int type)
        {
            WeaponFrameCounter = 0;
            WeaponWidth = 50;
            WeaponHeight = 52;
            WeaponFrameMax = 8;
            if (type == 1)
            {
                WeaponWidth = 48;
                WeaponHeight = 50;
                WeaponFrameMax = 8;
            }
            if (type == 2)
            {
                WeaponWidth = 94;
                WeaponHeight = 96;
                WeaponFrameMax = 15;
            }
            WeaponFrame = WeaponFrameMax - 1;
            WeaponType = type;
        }
        public override void FindFrame(int frameHeight)
        {
            WeaponFrameCounter++;
            if(WeaponFrameCounter > 3)
            {
                WeaponFrameCounter = 0;
                if(WeaponType != TypeToSwapTo)
                {
                    WeaponFrame++;
                    if (WeaponFrame >= WeaponFrameMax - 1) //Frame counter going up means resetting the frame back to default
                    {
                        WeaponFrame = WeaponFrameMax - 1; //The frame counter should hold a delay after this point... maybe just set the counter to a negative number?
                        SwapWeapon(TypeToSwapTo);
                    }
                }
                else if(WeaponFrame < WeaponFrameMax)
                {
                    WeaponFrame--;
                    if (WeaponFrame <= 0) //Frame counter going down means opening up the weapon
                    {
                        WeaponFrame = 0;
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
            if(AI1 % 100 == 0)
            {
                Main.NewText("Swap");
                QueueSwapWeapon(Main.rand.Next(3));
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
	}
}





















