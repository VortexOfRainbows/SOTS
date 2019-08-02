using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamOre2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Condemned Bottle");
			Tooltip.SetDefault("A bottle used to seal the infamous Alpha Virus\nThere is only one thing left to do now...");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 26;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 16;
			item.useTime = 16;
			item.useStyle = 1;
			item.rare = 9;
			item.value = 0;
			item.shoot = mod.ProjectileType("IceCreamBottle");
			item.shootSpeed = 12;
			item.consumable = true;
			item.noUseGraphic = true;
		}
	}
}