using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CursedImpale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Impale");
			Tooltip.SetDefault("Releases a short ranged burst of energy");
		}
		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.melee = true;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;            
			Item.shoot = mod.ProjectileType("CurseSpear"); 
            Item.shootSpeed = 5;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 7);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				//Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("RubyBurst"), damage, knockBack, player.whoAmI);
			}
			return false; 
		}
	}
}
	
