using SOTS.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class WormWoodHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Hook");
			Tooltip.SetDefault("Retracts upon hitting an enemy");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.damage = 24;
			Item.melee = true;
			Item.knockBack = 0;
            Item.width = 32;  
            Item.height = 32;   
            Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<PinkyHook>();
			Item.shootSpeed = 15f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 16);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 24);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
