using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace SOTS.Items
{	[AutoloadEquip(EquipType.Shield)]
	public class ChiseledBarrier : ModItem
	{	int Probe = -1;
		int Probe2 = -1;	
		int Probe3 = -1;
		int Probe4 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chiseled Barrier");
			Tooltip.SetDefault("Surrounds you with 4 orbital projectiles\nLaunches attackers away from you with javelins");
		}
		public override void SetDefaults()
		{
	
			item.damage = 30;
			item.magic = true;
            item.width = 34;     
            item.height = 48;   
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.rare = 6;
			item.accessory = true;
			item.defense = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MarbleDefender", 1);
			recipe.AddIngredient(null, "ArcaneAqueduct", 1);
			recipe.AddIngredient(null, "TinyPlanet", 1);
			recipe.AddIngredient(null, "RedPowerChamber", 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		float rotation = 0;
		float rotation2 = 0;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.PushBack = true;
			
			rotation += 4f;
			rotation2 += 1f;
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 27, (int)(item.damage * ((player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI); //waterbolt proj
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != 27)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 27, (int)(item.damage * ((player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
				if (Probe != -1)
				{
					Projectile proj = Main.projectile[Probe];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(rotation));
					initialLoop.X /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
					//Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0, 0, 14, item.damage, 0, player.whoAmI); //this was for testing shape
				}
				
				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 27, (int)(item.damage * ((player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != 27)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 27, (int)(item.damage * ((player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				Main.projectile[Probe2].timeLeft = 6;
				if (Probe2 != -1)
				{
					Projectile proj = Main.projectile[Probe2];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(-128, 0).RotatedBy(MathHelper.ToRadians(rotation));
					initialLoop.Y /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
					//Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0, 0, 14, item.damage, 0, player.whoAmI);
				}
				
				
				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * ((player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
					Main.projectile[Probe3].ai[1] = 180;
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != mod.ProjectileType("TinyPlanetTear"))
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * ((player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
					Main.projectile[Probe3].ai[1] = 180;
				}
				Main.projectile[Probe3].timeLeft = 6;
				
				
				if (Probe4 == -1)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"),	(int)(item.damage * ((player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				if (!Main.projectile[Probe4].active || Main.projectile[Probe4].type != mod.ProjectileType("TinyPlanetTear"))
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * ((player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				Main.projectile[Probe4].timeLeft = 6;
				
			
		}
	}
}