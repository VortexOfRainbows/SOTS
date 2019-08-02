using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.SpecialDrops
{
	public class RainbowCrate : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.value = 5000000;
			item.rare = 9;
			item.maxStack = 99;
			item.useTime = 10;
			item.useAnimation = 15;
			item.placeStyle = 0;
			item.useStyle = 1;
			item.consumable = true;
			item.autoReuse = true;
			item.createTile = mod.TileType("RainbowCrateTile");
		


			
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(Main.rand.Next(1,  3929),1);
			player.QuickSpawnItem(Main.rand.Next(1,  3929),1);
			player.QuickSpawnItem(Main.rand.Next(1,  3929),1);
			player.QuickSpawnItem(Main.rand.Next(1,  3929),1);
			player.QuickSpawnItem(Main.rand.Next(1,  3929),1);
			player.QuickSpawnItem(Main.rand.Next(1,  3929),1);
			player.QuickSpawnItem(Main.rand.Next(1,  3929),1);
			player.QuickSpawnItem(Main.rand.Next(1,  3929),1);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rainbow Crate");
			Tooltip.SetDefault("Its glowing with a power of a million items...\nRight click to open");
		}
	}
}