using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
			Tooltip.SetDefault("Surrounds you with 4 orbital projectiles\nLaunches attackers away from you with javelins\nProjectiles disabled when hidden");
		}
		public override void SetDefaults()
		{
	
			item.damage = 30;
			item.magic = true;
            item.width = 30;     
            item.height = 36;   
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
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.PushBack = true;
			
			if(Main.myPlayer == player.whoAmI && !hideVisual)
			{ 
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 2); 
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("Rainbolt") || Main.projectile[Probe].owner != player.whoAmI || Main.projectile[Probe].ai[0] != 2)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 2);
				}
				Main.projectile[Probe].timeLeft = 6;

				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 3);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("Rainbolt") || Main.projectile[Probe2].owner != player.whoAmI || Main.projectile[Probe2].ai[0] != 3)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 3);
				}
				Main.projectile[Probe2].timeLeft = 6;
			
				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 2);
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != mod.ProjectileType("TinyPlanetTear") || Main.projectile[Probe3].owner != player.whoAmI || Main.projectile[Probe3].ai[0] != 2)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 2);
				}
				Main.projectile[Probe3].timeLeft = 6;
			
			
				if (Probe4 == -1)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"),	(int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 3);
				}
				if (!Main.projectile[Probe4].active || Main.projectile[Probe4].type != mod.ProjectileType("TinyPlanetTear") || Main.projectile[Probe4].owner != player.whoAmI || Main.projectile[Probe4].ai[0] != 3)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 3);
				}
				Main.projectile[Probe4].timeLeft = 6;

			}
		}
	}
}