using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class PermafrostMedallion : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Permafrost Medallion");
			Tooltip.SetDefault("Surrounds you with a blizzard of artifact probes");
		}
		public override void SetDefaults()
		{
			item.damage = 36;
			item.summon = true;
            item.width = 34;     
            item.height = 38;   
            item.value = Item.sellPrice(0, 5, 50, 0);
            item.rare = 7;
			item.accessory = true;
		}
		int[] Probes = { -1, -1, -1, -1, -1, -1, -1, -1 };
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 7);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			int type = mod.ProjectileType("BlizzardProbe");
			if(player.whoAmI == Main.myPlayer)
			{
				for (int i = 0; i < Probes.Length; i++)
				{
					if (Probes[i] == -1)
					{
						Probes[i] = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, (int)(item.damage * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, i, i * 15);
					}
					if (!Main.projectile[Probes[i]].active || Main.projectile[Probes[i]].type != type || Main.projectile[Probes[i]].ai[0] != i)
					{
						Probes[i] = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, (int)(item.damage * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, i, i * 15);
					}
					Main.projectile[Probes[i]].timeLeft = 6;
				}
			}
		}
	}
}