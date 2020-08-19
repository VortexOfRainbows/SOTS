using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class FrozenTreasureSlime : ModNPC
	{	int initiateSize = 1;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Frozen Treasure Slime");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = 1;
            npc.lifeMax = 60;  
            npc.damage = 38; 
            npc.defense = 3;  
            npc.knockBackResist = 0.8f;
            npc.width = 32;
            npc.height = 28;
            animationType = NPCID.BlueSlime;
			Main.npcFrameCount[npc.type] = 2;  
            npc.value = 1600;
            npc.npcSlots = .5f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.netAlways = false;
			npc.alpha = 90;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
			banner = npc.type;
			bannerItem = ItemType<FrozenTreasureSlimeBanner>();
		}
		public override bool PreAI()
		{
			if(initiateSize == 1)
			{
				initiateSize = -1;
				npc.scale = 1.3f;
				npc.width = (int)(npc.width * npc.scale);
				npc.height = (int)(npc.height * npc.scale);
			}
			return true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.spawnTileY <= Main.rockLayer && spawnInfo.spawnTileY >= Main.worldSurface){
				return spawnInfo.spawnTileType == TileID.IceBlock || spawnInfo.spawnTileType == TileID.SnowBlock ? .11f : 0f;
			}
			else if(spawnInfo.spawnTileY <= Main.maxTilesY - 200 && spawnInfo.spawnTileY >= Main.rockLayer){
				return spawnInfo.spawnTileType == TileID.IceBlock || spawnInfo.spawnTileType == TileID.SnowBlock ? .16f : 0f;
			}
			return 0f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 100.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, new Color(106, 210, 255, 100), 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, new Color(106, 210, 255, 100), 1f);
				}
			}
		}
		public override void NPCLoot()
		{
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Gel), Main.rand.Next(4) + 3);
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Peanut"), Main.rand.Next(12) + 1);	
			
			if(Main.rand.Next(2) == 0 || Main.expertMode)
			{
				int rand = Main.rand.Next(30);
				if(rand == 0){
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.SpelunkerPotion, 1);
				}
				if(rand == 1)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.FeatherfallPotion, 1);
				}
				if(rand == 2)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.NightOwlPotion, 1);
				}
				if(rand == 3)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.WaterWalkingPotion, 1);
				}
				if(rand == 4)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.ArcheryPotion, 1);
				}
				if(rand == 5)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.GravitationPotion, 1);
				}
				if(rand == 6)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.ThornsPotion, 1);
				}
				if(rand == 7)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.InvisibilityPotion, 1);
				}
				if(rand == 8)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.HunterPotion, 1);
				}
				if(rand == 9)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  2329, 1); //dangersense
				}
				if(rand == 10)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.TeleportationPotion, 1);
				}
				if(rand == 11)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.RegenerationPotion, 1);
				}
				if(rand == 12)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.SwiftnessPotion, 1);
				}
				if(rand == 13)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.ShinePotion, 1);
				}
				if(rand == 14)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.IronskinPotion, 1);
				}
				if(rand == 15)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.MiningPotion, 1);
				}
				if(rand == 16)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.BuilderPotion, 1);
				}
				if(rand > 16){
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.HealingPotion, Main.rand.Next(2) + 2);
				}
			}

			if(Main.rand.Next(15) == 0 || (Main.expertMode && Main.rand.Next(40) == 0))
			{
				int rand = Main.rand.Next(10);
				if(rand == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.IceBoomerang, 1);
				}
				if(rand == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.IceBlade, 1);
				}
				if(rand == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.IceSkates, 1);
				}
				if(rand == 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.SnowballCannon, 1);
				}
				if(rand == 4)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.BlizzardinaBottle, 1);
				}
				if(rand == 5)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.FlurryBoots, 1);
				}
				if(rand == 6)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.IceMirror, 1);
				}
				if(rand == 7)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.SnowballLauncher, 1);
				}
				if(rand == 8)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.VikingHelmet, 1);
				}
				if(rand == 9)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("CryoCannon"), 1);
				}
			}
		}	
	
	}
}