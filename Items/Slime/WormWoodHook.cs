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
			item.CloneDefaults(ItemID.AmethystHook);
			item.damage = 24;
			item.melee = true;
			item.knockBack = 0;
            item.width = 32;  
            item.height = 32;   
            item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<PinkyHook>();
			item.shootSpeed = 15f;
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
