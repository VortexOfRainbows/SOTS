using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class LavaBracer : ModItem
	{int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Bracer");
			Tooltip.SetDefault("Envelope enemies in flame when they approach you");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 30;     
            item.height = 30;   
            item.value = 50000;
            item.rare = 5;
			item.accessory = true;
			item.defense = 1;
			item.damage = 24;
			item.magic = true;
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer++;
			for(int i = 0; i < 200; i++)
    {
       NPC target = Main.npc[i];

       float shootToX = target.position.X + (float)target.width * 0.5f - player.Center.X;
       float shootToY = target.position.Y + (float)target.height * 0.5f - player.Center.Y;
       float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

       if(distance < 180f && !target.friendly && target.active)
       {
           if(timer >= 18) 
           {
               distance = 3f / distance;
   
               shootToX *= distance * 5;
               shootToY *= distance * 5;
   
               Projectile.NewProjectile(player.Center.X, player.Center.Y, shootToX, shootToY, 85, item.damage, 0, Main.myPlayer, 2.75f, 0f); //Spawning a projectile
              
           }
       }
    }
		if(timer >= 18)
		{
               timer = 0;
		}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.LeadBar, 6);
			recipe.AddIngredient(ItemID.LavaBucket, 1);
			recipe.AddIngredient(ItemID.ObsidianRose, 1);
			recipe.AddIngredient(null, "ObsidianScale", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}