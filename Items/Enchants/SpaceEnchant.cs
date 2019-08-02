using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class SpaceEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Relic");
			Tooltip.SetDefault("Creates a meteor deadzone where your cursor is located (can get annoying)\n14% decreased mana usage");
		}
		public override void SetDefaults()
		{
      
            item.width = 32;     
            item.height = 30;   
            item.value = 50000;
            item.rare = 9;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 24;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CoreOfCreation", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(null, "BrassBar", 16);
			recipe.AddIngredient(ItemID.MeteorHelmet,1);
			recipe.AddIngredient(ItemID.MeteorSuit,1);
			recipe.AddIngredient(ItemID.MeteorLeggings,1);
			recipe.AddIngredient(ItemID.MeteorStaff,1);
			recipe.AddIngredient(ItemID.XenoStaff,1);
			recipe.AddIngredient(ItemID.Lens,4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer += 1;
			player.manaCost -= 14;
		
					if(timer == 10)
					{
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile X position speed and randomnes
                float SpeedY = num17 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile Y position speed and randomnes
                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, 425, 33, 1, Main.myPlayer, 0.0f, 2);
            
					timer = 0;
					}
					
		}
		
	}
}
