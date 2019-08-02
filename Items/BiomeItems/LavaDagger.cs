using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class LavaDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Dagger");

		}
		public override void SetDefaults()
		{

			item.damage = 26;
			item.thrown = true;
			item.width = 36;
			item.height = 36;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 34; 
            item.shootSpeed = 12;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ObsidianScale", 7);
			recipe.AddIngredient(ItemID.MeteoriteBar, 5);
			recipe.AddIngredient(ItemID.Obsidian, 5);
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.AddIngredient(ItemID.LavaBucket, 2);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}