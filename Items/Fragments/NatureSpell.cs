using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Fragments
{
	public class NatureSpell : ModItem
	{	int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower Spell");
			Tooltip.SetDefault("Launches a seed that latches onto enemies\nWhen the seed blooms, it does 400% damage");
		}
		public override void SetDefaults()
		{
            item.damage = 5; 
            item.magic = true; 
            item.width = 28;   
            item.height = 30;   
            item.useTime = 30;   
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true;  
            item.knockBack = 2.5f;
			item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item8;
            item.shoot = mod.ProjectileType("FlowerSeed"); 
            item.shootSpeed = 10f;
			item.mana = 4;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddIngredient(null, "FragmentOfNature", 4);
			recipe.AddIngredient(ItemID.YellowMarigold, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
					int Probe = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
					Main.projectile[Probe].ai[1] = 4;
				return false; 
		}
	}
}
