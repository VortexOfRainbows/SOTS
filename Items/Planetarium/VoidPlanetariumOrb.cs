using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Planetarium
{
	public class VoidPlanetariumOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Void Planetarium Orb");
			Tooltip.SetDefault("A dark image is housed inside\nIt can be extracted via an extractinator\nMade out of the same material as a normal planetarium orb, maybe it would be possible to use it if your health was low");
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
			if(player.statLife > 5)
			{
				item.stack--;
				player.QuickSpawnItem(mod.ItemType("PlanetariumOrb"));
			}
		}
		public override void ExtractinatorUse(ref int resultType, ref int resultStack)
		{ 		
			Player player = Main.player[Main.myPlayer];
		
			int planetariumRandom = Main.rand.Next(3);
			
			
			if(planetariumRandom == 0)
			resultType = mod.ItemType("GreatFilter");
		
			if(planetariumRandom == 1)
			resultType = mod.ItemType("AtomicDivision");
		
			if(planetariumRandom == 2)
			resultType = mod.ItemType("VoidRift");
		
	
			resultStack = 1;	
		}
	}
}