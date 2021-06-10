using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class BiomassBarrage : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Biomass Bloom");
			Tooltip.SetDefault("Launches a cluster of acorns");
		}
		public override void SetDefaults()
		{
            item.damage = 24; 
            item.magic = true; 
            item.width = 34;   
            item.height = 36;   
            item.useTime = 30;   
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true;  
            item.knockBack = 3.25f;
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item8;
            item.shoot = mod.ProjectileType("AcornsOfJustice"); 
            item.shootSpeed = 17.5f;
			item.autoReuse = true;
			item.mana = 11;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BiomassBlast", 1);
			recipe.AddIngredient(null, "CursedMatter", 3);
			recipe.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BiomassBlast", 1);
			recipe.AddIngredient(null, "CursedMatter", 3);
			recipe.AddIngredient(ItemID.MythrilBar, 12);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
