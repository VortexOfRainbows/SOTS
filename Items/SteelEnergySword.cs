using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class SteelEnergySword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steel Energy Sword");
		}
		public override void SetDefaults()
		{

			item.damage = 24;
			item.melee = true;
			item.width = 56;
			item.height = 56;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 125000;
			item.rare = 6;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = 709; 
			item.expert = true;
            item.shootSpeed = 17;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SteelBar", 24);
			recipe.AddIngredient(ItemID.Bone, 32);
			recipe.AddIngredient(null, "CoreOfExpertise", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}