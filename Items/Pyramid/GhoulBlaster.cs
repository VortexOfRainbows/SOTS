using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;


namespace SOTS.Items.Pyramid
{
	public class GhoulBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghoul Blaster");
			Tooltip.SetDefault("Unloads almost as fast as the trigger is pulled\nTransforms bullets into cursed singularities every 6th shot");
		}
		public override void SetDefaults()
		{
            item.damage = 28;
            item.ranged = true;
            item.width = 42; 
            item.height = 26;
            item.useTime = 6;
            item.useAnimation = 6;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 3f;
            item.value = Item.sellPrice(0, 5, 25, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item41;
            item.autoReuse = false;
            item.shoot = 10; //not really important 
            item.shootSpeed = 13.5f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PhoenixBlaster, 1);
			recipe.AddIngredient(null, "RoyalMagnum", 1);
			recipe.AddIngredient(null, "CursedMatter", 4);
			recipe.AddIngredient(null, "SoulResidue", 12);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-0.25f, 0);
		}
		int shotNum = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			item.reuseDelay = 0;
			shotNum++;
			if(shotNum == 5)
			{
				item.reuseDelay = 12;
			}
			if(shotNum >= 6)
			{
				Main.PlaySound(SoundID.Item38, (int)(position.X), (int)(position.Y));
				shotNum = 0;
				Projectile.NewProjectile(position.X, position.Y, speedX * 0.9f, speedY * 0.9f, mod.ProjectileType("CurseSingularity"), damage, knockBack, player.whoAmI);
				return false;
			}
			return true; 
		}
	}
}
