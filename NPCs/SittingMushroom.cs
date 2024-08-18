using Microsoft.Xna.Framework;
using SOTS.Common.GlobalNPCs;
using SOTS.Items.Banners;
using SOTS.Items.Earth;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class SittingMushroom : ModNPC
    {
        public override void SetStaticDefaults()
        {
			Main.npcFrameCount[NPC.type] = 8;  
        }
        public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 40;   
            NPC.damage = 20; 
            NPC.defense = 6;  
            NPC.knockBackResist = 0.1f;
            NPC.width = 30;
            NPC.height = 30;
            NPC.value = 125;
            NPC.npcSlots = 1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath16;
            NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ItemType<SittingMushroomBanner>();
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.velocity.X *= 0.9f;
			if(Main.rand.NextBool(40))
			{
				Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), 28, 10, DustID.GlowingMushroom, 0, -2f, 250, new Color(100, 100, 100, 250), 0.8f);
			}
			NPC.ai[0]++;
			if(NPC.ai[0] >= 180)
			{
				Vector2 distance = NPC.Center - player.Center;
				if(distance.Length() <= 150 || NPC.ai[1] >= 1)
				{
					NPC.ai[1]++;
				}
			}
			if(NPC.ai[1] == 1)
			{
				NPC.velocity.Y -= 7f;
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
			}
			if((int)NPC.ai[1] % 7 == 0 && NPC.ai[1] >= 7)
			{
				Vector2 distance = NPC.Center - player.Center;
				distance = distance.SafeNormalize(Vector2.Zero);
				distance *= -8f;
				int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
				if(Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, distance.X, distance.Y, ModContent.ProjectileType<Projectiles.SporeCloud>(), damage2, 2f, Main.myPlayer);
				SOTSUtils.PlaySound(SoundID.Item34, NPC.Center);
			}
			if (NPC.ai[1] >= 42)
			{
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
			}
		}
		int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			float frameSpeed = 8f;
			NPC.frameCounter++;
			if (NPC.frameCounter >= frameSpeed) 
			{
				NPC.frameCounter -= frameSpeed;
				NPC.frame.Y += frame;
				if(NPC.frame.Y >= 8 * frame)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.Player.ZoneGlowshroom || spawnInfo.SpawnTileType == TileID.MushroomGrass)
			{
				if(!Main.hardMode)
					return 0.2f;
				return 0.1f;
			}
			if (spawnInfo.Player.ZoneRockLayerHeight)
			{
				return 0.005f;
			}
			return 0;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemType<SporeSprayer>(), 100));
			npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<FragmentOfEarth>(), 2, 1));
			LeadingConditionRule onFire = new LeadingConditionRule(new Common.ItemDropConditions.OnFireCondition());
			onFire.OnSuccess(ItemDropRule.Common(ItemType<CookedMushroom>(), 1, 3, 4));
			onFire.OnFailedConditions(ItemDropRule.Common(ItemType<CookedMushroom>(), 10, 3, 4)
				.OnFailedRoll(ItemDropRule.Common(ItemID.GlowingMushroom, 1, 7, 12)));
		}
        public override void OnKill()
		{
			for (int k = 0; k < 7; k++)
			{
				Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
				int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center.X, NPC.Center.Y, circularLocation.X, circularLocation.Y, ProjectileType<Projectiles.SporeCloud>(), damage2, 2f, Main.myPlayer);
			}
        }
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < hit.Damage / (double)NPC.lifeMax * 20.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GlowingMushroom, (float)(2 * hit.HitDirection), -2f, 250, new Color(100, 100, 100, 250), 0.8f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 10; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GlowingMushroom, (float)(2 * hit.HitDirection), -2f, 250, new Color(100, 100, 100, 250), 0.8f);
				}
				SOTSUtils.PlaySound(SoundID.Item34, NPC.Center);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/SittingMushroomGore1"), 1f);
			}
		}
	}
}





















