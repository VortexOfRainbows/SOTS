using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class PinkJellyfishStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Jellyfish Staff");
			Tooltip.SetDefault("Fires pink lightning which chains off enemies");
		}
		public override void SetDefaults()
		{
            item.damage = 18;
            item.magic = true; 
            item.width = 32;    
            item.height = 32; 
            item.useTime = 19; 
            item.useAnimation = 19;
            item.useStyle = 5;    
            item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item93;
            item.noMelee = true; 
            item.autoReuse = false;
            item.shootSpeed = 14.5f; 
			item.shoot = mod.ProjectileType("PinkLightning");
			Item.staff[item.type] = true; 
			item.mana = 10;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Amethyst, 5);
			recipe.AddIngredient(null, "FragmentOfTide", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
                  Vector2 perturbedSpeed = new Vector2(speedX * .1f, speedY * .1f).RotatedByRandom(MathHelper.ToRadians(90)); 
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                  Projectile.NewProjectile(position.X, position.Y, -perturbedSpeed.X, -perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				  
                  Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            return false;
		}
	}
}
