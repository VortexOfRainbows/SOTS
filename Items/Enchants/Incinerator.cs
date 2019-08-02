using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class Incinerator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic VII : Incinerator");
			Tooltip.SetDefault("Fires a 2 beams of homing fire");
		}
		public override void SetDefaults()
		{
            item.damage = 44;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 44;     //gun image width
            item.height = 28;   //gun image  height
            item.useTime = 2;  //how fast 
            item.useAnimation = 4;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000000000;
            item.rare = 9;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PlanetaryFlame2"); 
            item.shootSpeed = 13.5f;
			item.mana = 2;
			item.expert = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GildedDragon", 1);
			recipe.AddIngredient(null, "PolarisStarIIIIII", 1);
			recipe.AddIngredient(null, "GammaGattling", 1);
			recipe.AddIngredient(null, "BlizzardsBliss", 1);
			recipe.AddIngredient(null, "TheHardCore", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
		
        Projectile.NewProjectile(position.X - (speedY), position.Y + (speedX), speedX, speedY, mod.ProjectileType("PlanetaryFlame2"), damage, knockBack, player.whoAmI);
			
        Projectile.NewProjectile(position.X + (speedY), position.Y - (speedX), speedX, speedY, mod.ProjectileType("PlanetaryFlame2"), damage, knockBack, player.whoAmI);
			
		   
		   
		   
              return false; 
		}
	}
}
