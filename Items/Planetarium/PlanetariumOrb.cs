using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Planetarium
{
	public class PlanetariumOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Planetarium Orb");
			Tooltip.SetDefault("A glowing image is housed inside\nIt can be extracted via an extractinator\nIt seems to dim as your health lowers");
			ItemID.Sets.ExtractinatorMode[item.type] = item.type;
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.consumable = true;
			item.createTile = mod.TileType("PlanetariumBlock");
		}
		public override void UpdateInventory(Player player)
		{
			if(player.statLife <= 1)
			{
				item.stack--;
				player.QuickSpawnItem(mod.ItemType("VoidPlanetariumOrb"));
			}
		}

		public override void ExtractinatorUse(ref int resultType, ref int resultStack)
		{ 		Player player = Main.player[Main.myPlayer];
			int planetariumRandom = Main.rand.Next(28);
			
			
			if(planetariumRandom == 0)
			resultType = mod.ItemType("Alchelocin");
		
			if(planetariumRandom == 1)
			resultType = mod.ItemType("Recon");
			
			if(planetariumRandom == 2)
			resultType = mod.ItemType("Strike");
		
			if(planetariumRandom == 3)
			resultType = mod.ItemType("Luna");
		
			if(planetariumRandom == 4)
			resultType = mod.ItemType("Gravity");
			
			if(planetariumRandom == 5)
			resultType = mod.ItemType("Enigma");
		
			if(planetariumRandom == 6)
			resultType = mod.ItemType("Sol");
		
			if(planetariumRandom == 7)
			resultType = mod.ItemType("Terra");
			
			if(planetariumRandom == 8)
			resultType = mod.ItemType("Heart");
				
			if(planetariumRandom == 9)
			resultType = mod.ItemType("Nova");
		
			if(planetariumRandom == 10)
			resultType = mod.ItemType("Vacuum");
			
			if(planetariumRandom == 11)
			resultType = mod.ItemType("Saturnus");
		
			if(planetariumRandom == 12)
			resultType = mod.ItemType("Entropy");
		
			if(planetariumRandom == 13)
			resultType = mod.ItemType("Patience");
			
			if(planetariumRandom == 14)
			resultType = mod.ItemType("Pluto");
		
			if(planetariumRandom == 15)
			resultType = mod.ItemType("Neptune");
		
			if(planetariumRandom == 16)
			resultType = mod.ItemType("Ceres");
			
			if(planetariumRandom == 17)
			resultType = mod.ItemType("Mars");
		
			if(planetariumRandom == 18)
			resultType = mod.ItemType("Jupiter");
		
			if(planetariumRandom == 19)
			resultType = mod.ItemType("Ouranus");
			
			if(planetariumRandom == 20)
			resultType = mod.ItemType("Juno");
		
			if(planetariumRandom == 21)
			resultType = mod.ItemType("Andromeda");
			
			if(planetariumRandom == 22)
			resultType = mod.ItemType("Typhon");
		
			if(planetariumRandom == 23)
			resultType = mod.ItemType("Orion");
		
			if(planetariumRandom == 24)
			resultType = mod.ItemType("Pallas");
			
			if(planetariumRandom == 25)
			resultType = mod.ItemType("Vesta");
			
			if(planetariumRandom == 26)
			resultType = mod.ItemType("Eclipse");
		
			if(planetariumRandom == 27)
			resultType = mod.ItemType("Perseus");
	
			resultStack = 1;	
		}
	}
}