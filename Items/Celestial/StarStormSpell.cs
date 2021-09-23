using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Celestial
{
	public class StarStormSpell : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starstorm Spell");
			Tooltip.SetDefault("A volley of celestial projectiles target your cursor");
		}
		public override void SetDefaults()
		{
            item.damage = 60;
			item.magic = true;
            item.width = 38;    
            item.height = 36; 
            item.useTime = 10; 
            item.useAnimation = 10;
            item.useStyle = 5;    
            item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 9, 0, 0);
            item.rare = 8;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = true;
            item.shootSpeed = 45f; 
			item.shoot = mod.ProjectileType("StellarStar");
			item.mana = 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarShard", 15);
			recipe.AddIngredient(null, "ShardstormSpell", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		float counter = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 cursorPos = Main.MouseWorld;
			
            int numberProjectiles = 2;  //This defines how many projectiles to shot
            for (int i = 0; i < numberProjectiles; i++)
            {
				counter += Main.rand.Next(85, 216);
				Vector2 rotateArea = new Vector2(500, 0).RotatedBy(MathHelper.ToRadians(counter));
				rotateArea += cursorPos;
				rotateArea.X += Main.rand.Next(-50, 51);
				rotateArea.Y += Main.rand.Next(-50, 51);
				Vector2 perturbedSpeed = new Vector2(item.shootSpeed, 0).RotatedBy(Math.Atan2(cursorPos.Y - rotateArea.Y, cursorPos.X - rotateArea.X));
				
                Projectile.NewProjectile(rotateArea.X, rotateArea.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 0, 1);
            }
            return false;
		}
	}
}
