using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Enchants
{  
    public class Tesseract : ModItem
    {
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XXIV : Tesseract");
			Tooltip.SetDefault("Summons a rotational orb to assist you in combat\nIncreases max minions by 4\nE");
		}
        public override void SetDefaults()
        {
           
            item.damage = 135;
            item.summon = true;
            item.mana = 10;
            item.width = 48;
            item.height = 48;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 1000000000;
            item.rare = 11;
            item.UseSound = SoundID.Item44;
			item.expert = true;
            item.shoot = mod.ProjectileType("Tesseract");
            item.shootSpeed = 7f;
			item.buffType = mod.BuffType("Tesseract");
            item.buffTime = 3600;
			item.autoReuse = true;
        }
		public override void UpdateInventory(Player player)
		{
			player.maxMinions += 4;
		}
          public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
			  Vector2 vector14;
					
						if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0, type, damage, 1, Main.myPlayer, 0.0f, 1);
            
					return false;
					
          }  
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"EMaterial", 1);
			recipe.AddIngredient(null,"PlanetaryLatch", 1);
			recipe.AddIngredient(null,"ElementalKing", 1);
			recipe.AddIngredient(null,"PrismStaff", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"TheHardCore", 7);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}