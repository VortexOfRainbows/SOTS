using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class ElixirEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elixir Relic");
			Tooltip.SetDefault("Converts Musket Balls into Fusion Bolts\n25% increased ranged speed\n10% decrease to all damage");
		}
		public override void SetDefaults()
		{
      
            item.width = 28;     
            item.height = 36;   
            item.value = 50000;
            item.rare = 6;
			item.accessory = true;
			item.defense = 0;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"AntimaterialMandible", 5);
			recipe.AddIngredient(null,"MusketeerHat", 1);
			recipe.AddIngredient(null,"MusketeerShirt", 1);
			recipe.AddIngredient(null,"MusketeerLeggings", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
				player.rangedDamage -= 0.1f;
				player.meleeDamage -= 0.1f;
				player.magicDamage -= 0.1f;
				player.minionDamage -= 0.1f;
				player.thrownDamage -= 0.1f;
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.musketHat = true;
				
			for(int i = 0; i < 1000; i++)
			{
				Projectile musketBall = Main.projectile[i];
				if(musketBall.type == 14)
				{
					musketBall.Kill();
					Vector2 projVelocity1 = new Vector2(musketBall.velocity.X, musketBall.velocity.Y).RotatedBy(MathHelper.ToRadians(45));
					Vector2 projVelocity2 = new Vector2(musketBall.velocity.X, musketBall.velocity.Y).RotatedBy(MathHelper.ToRadians(315));
					Projectile.NewProjectile(musketBall.Center.X, musketBall.Center.Y, projVelocity1.X * 0.35f, projVelocity1.Y * 0.35f, mod.ProjectileType("Fusion1"), (int)(musketBall.damage * 1f), musketBall.knockBack, Main.myPlayer);
					Projectile.NewProjectile(musketBall.Center.X, musketBall.Center.Y, projVelocity2.X * 0.35f, projVelocity2.Y * 0.35f, mod.ProjectileType("Fusion2"), (int)(musketBall.damage * 1f), musketBall.knockBack, Main.myPlayer);
					
				}
			}
					
		}
		
	}
}
