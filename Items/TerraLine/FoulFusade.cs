using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class FoulFusade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Foul Spray");
			Tooltip.SetDefault("Stinky");
		}
		public override void SetDefaults()
		{
            item.damage = 32;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 28;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 20;  //how fast 
            item.useAnimation = 20;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 30000;
            item.rare = 11;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = 371; 
            item.shootSpeed = 14;
			item.expert = true;
			item.mana = 12;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ManaCrystal, 10);
			recipe.AddIngredient(ItemID.Book, 3);
			recipe.AddIngredient(ItemID.WormScarf, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
