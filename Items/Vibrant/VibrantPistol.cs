using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Vibrant
{
	public class VibrantPistol : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Pistol");
			Tooltip.SetDefault("Fires almost as fast as you can pull the trigger");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 12;
            item.ranged = true;
            item.width = 30;
            item.height = 20;
            item.useTime = 5; 
            item.useAnimation = 5;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 2f;  
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item11;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("VibrantBolt"); 
            item.shootSpeed = 24f;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 1;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VeryGlowyMushroom", 1);
			recipe.AddRecipeGroup("IronBar", 12);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}
