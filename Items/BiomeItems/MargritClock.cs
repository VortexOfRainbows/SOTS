using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.BiomeItems
{
	public class MargritClock : ModItem
	{ 	int firerate = 0;
		bool overheated = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Clock");
			Tooltip.SetDefault("Slows down projectiles that get too close");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 8));
		}
		public override void SetDefaults()
		{
            
            item.width = 52;     
            item.height = 52;     
            item.value = 110000;
            item.rare = 6;
			item.accessory = true;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			for(int i = 0; i < 1000; i++)
			{
				Projectile proj = Main.projectile[i];
				
				if(player.Center.X + 160 > proj.Center.X && player.Center.X - 160 < proj.Center.X && player.Center.Y + 160 > proj.Center.Y && player.Center.Y - 160 < proj.Center.Y && proj.hostile == true)
				{	
					
					proj.velocity.X *= 0.97f;
					proj.velocity.Y *= 0.97f;
					
					if(Main.rand.Next(60) == 0)
					{
						
					proj.velocity.X *= 0.7f;
					proj.velocity.Y *= 0.7f;
					}
					
					if(Main.rand.Next(150) == 0)
					{
						
					proj.velocity.X *= 0.4f;
					proj.velocity.Y *= 0.4f;
					}
					
					if(Main.rand.Next(900) == 0)
					{
						
					proj.velocity.X *= 0.1f;
					proj.velocity.Y *= 0.1f;
					}
					
					if(Math.Abs(proj.velocity.X) < 0.01f)
					proj.Kill();
					
					if(Math.Abs(proj.velocity.Y) < 0.01f)
					proj.Kill(); 
				}
				
				
			}
			
			
		
			
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 12);
			recipe.AddIngredient(3081, 24);
			recipe.AddIngredient(3086, 24);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
