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
            Item.damage = 28;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42; 
            Item.height = 26;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 5, 25, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = false;
            Item.shoot = 10; //not really important 
            Item.shootSpeed = 13.5f;
			Item.useAmmo = AmmoID.Bullet;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
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
			Item.reuseDelay = 0;
			shotNum++;
			if(shotNum == 5)
			{
				Item.reuseDelay = 12;
			}
			if(shotNum >= 6)
			{
				SoundEngine.PlaySound(SoundID.Item38, (int)(position.X), (int)(position.Y));
				shotNum = 0;
				Projectile.NewProjectile(position.X, position.Y, speedX * 0.9f, speedY * 0.9f, mod.ProjectileType("CurseSingularity"), damage, knockBack, player.whoAmI);
				return false;
			}
			return true; 
		}
	}
}
