using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class WormWoodCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Crystal");
			Tooltip.SetDefault("A crystal that explodes into gelatinous projectiles");
		}
		public override void SetDefaults()
		{
            item.damage = 16; 
            item.ranged = true; 
            item.width = 22; 
            item.height = 22; 
            item.useTime = 35;  
            item.useAnimation = 35;
            item.useStyle = 1;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 0, 0, 60);
            item.rare = 4;
			item.consumable = true;
			item.maxStack = 999;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot =  mod.ProjectileType("PinkyCrystal"); 
            item.shootSpeed = 12;
			item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 16);
			recipe.AddIngredient(null, "Wormwood", 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 666);
			recipe.AddRecipe();
		}
	}
}
