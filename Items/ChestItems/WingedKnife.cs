using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class WingedKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winged Knife");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ThrowingKnife);
			item.damage = 12;
			item.thrown = true;
			item.rare = 1;
			item.width = 34;
			item.height = 26;
			item.maxStack = 1;
			item.useTime = 24;
			item.useAnimation = 24;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("WingedKnife"); 
            item.shootSpeed = 12f;
			item.consumable = false;
			item.knockBack = 1.5f;
		}
	}
}