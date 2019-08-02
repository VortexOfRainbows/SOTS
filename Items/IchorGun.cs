using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class IchorGun: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Gun");
			Tooltip.SetDefault("From the Crimson");
		}
		public override void SetDefaults()
		{
            item.damage = 24;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 40;     //gun image width
            item.height = 20;   //gun image  height
            item.useTime = 13;  //how fast 
            item.useAnimation = 12;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 10000;
            item.rare = 4;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 279; 
            item.shootSpeed = 20;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PhoenixBlaster, 1);
			recipe.AddIngredient(ItemID.Vertebrae, 20);
			recipe.AddIngredient(null, "CrusherEmblem", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
