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
			Item.CloneDefaults(ItemID.ThrowingKnife);
			Item.damage = 12;
			Item.thrown = true;
			Item.rare = ItemRarityID.Blue;
			Item.width = 46;
			Item.height = 36;
			Item.maxStack = 1;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.autoReuse = true;            
			Item.shoot = mod.ProjectileType("WingedKnife"); 
            Item.shootSpeed = 12f;
			Item.consumable = false;
			Item.knockBack = 1.5f;
		}
	}
}