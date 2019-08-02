using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.SoldStuff
{
	public class BunnyTurret : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bunny Turret");
			Tooltip.SetDefault("Fires a bunny at nearby enemies when tripped by a wire");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 26;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 4;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("BunnyTurretTile");
		}
	}
}