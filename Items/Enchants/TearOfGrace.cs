using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class TearOfGrace : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic VI : Tear Of Grace");
			Tooltip.SetDefault("Foul rain");
		}
		public override void SetDefaults()
		{
            item.damage = 90;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 28;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 2;  //how fast 
            item.useAnimation = 6;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 150000;
            item.rare = 7;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TearFallGrace"); 
            item.shootSpeed = 31.5f;
			item.mana = 7;
			item.crit = 8;
			item.expert = true;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 1;
            for (int index = 0; index < numberProjectiles; ++index)
            {
                Projectile.NewProjectile(player.Center.X, player.Center.Y - 520, (float)(Main.rand.Next(-2400,2401)/100f), 7.25f, type, damage, knockBack, Main.myPlayer, 0.0f, 2);
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
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
                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, 2);
            }
            return false;
	}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TearRain", 1);
			recipe.AddIngredient(3570, 1);
			recipe.AddIngredient(null,"BlizzardsBliss", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null, "CoreOfStatus", 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		
	}
}
