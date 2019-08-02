using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;                      //this are the references that we are going to use
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.BiomeItems //The directory for your .cs and .png; Example: Mod Sources/TutorialMOD/Items
{
    public class MargritYoyo : ModItem    //make sure the sprite file is named like the class name (CustomYoyo)
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Yoyo");
			Tooltip.SetDefault("Attracts enemy projectiles");
		}
 
 
        public override void SetDefaults()
        {
 
            item.damage = 22;//The damage stat for the Weapon.
            item.melee = true;     //This defines if it does Melee damage and if its effected by Melee increasing Armor/Accessories.
            item.width = 30;
			item.height = 26;
            item.useTime = 22;  
            item.useAnimation = 22;   
            item.useStyle = 5;
            item.channel = true;
            item.knockBack = 0.267f;
            item.value = 110000;
            item.rare = 6;//The color the title of your Weapon when hovering over it ingame  
            item.autoReuse = false;  //Weather your Weapon will be used again after use while holding down, if false you will need to click again after use to use it again.
            item.shoot = mod.ProjectileType("MargritYoyo"); //This defines what type of projectile this weapon will shot            
            item.noUseGraphic = true; //  Do not use the item graphic when using the item (we just want the yoyo projectile to spawn).
            item.noMelee = true;// Makes sure that the animation when using the item doesn't hurt NPCs.
            item.UseSound = SoundID.Item1; //The sound played when using your Weapon
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 18);
			recipe.AddIngredient(3081, 18);
			recipe.AddIngredient(3086, 24);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}