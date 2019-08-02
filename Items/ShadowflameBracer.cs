using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class ShadowflameBracer : ModItem
	{int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Bracer");
			Tooltip.SetDefault("Envelope enemies in shadowflame when they approach you");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 30;     
            item.height = 30;   
            item.value = 125000;
            item.rare = 6;
			item.accessory = true;
			item.defense = 3;
			item.damage = 40;
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

       if(distance < 240f && !target.friendly && target.active)
       {
           if(timer >= 3) 
           {
               distance = 3f / distance;
   
               shootToX *= distance * 5;
               shootToY *= distance * 5;
   
               Projectile.NewProjectile(player.Center.X, player.Center.Y, shootToX, shootToY, mod.ProjectileType("ShadowflameCopy"), item.damage, 0, Main.myPlayer, 3.75f, 0f); //Spawning a projectile
              
           }
       }
    }
		if(timer >= 3)
		{
               timer = 0;
		}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(3053, 1);
			recipe.AddIngredient(null,"LavaBracer", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}