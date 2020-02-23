using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class Sharanga : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharanga");
			Tooltip.SetDefault("Fires supercharged hellfire arrows");
		}
		public override void SetDefaults()
		{
            item.damage = 25; 
            item.ranged = true;  
            item.width = 26;   
            item.height = 50; 
            item.useTime = 25; 
            item.useAnimation = 25;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SharangaBolt"); 
            item.shootSpeed = 21.5f;
			item.useAmmo = ItemID.WoodenArrow;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("SharangaBolt"), damage, knockBack, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellwingBow, 1);
			recipe.AddIngredient(ItemID.MoltenFury, 1);
			recipe.AddIngredient(null, "FragmentOfInferno", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
