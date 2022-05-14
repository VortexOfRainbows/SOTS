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
			Item.damage = 36;
			Item.DamageType = DamageClass.Summon;
            Item.width = 34;     
            Item.height = 38;   
            Item.value = Item.sellPrice(0, 5, 50, 0);
            Item.rare = 7;
			Item.accessory = true;
		}
		int[] Probes = { -1, -1, -1, -1, -1, -1, -1, -1 };
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
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
						Probes[i] = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, (int)(Item.damage * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, i, i * 15);
					}
					if (!Main.projectile[Probes[i]].active || Main.projectile[Probes[i]].type != type || Main.projectile[Probes[i]].ai[0] != i)
					{
						Probes[i] = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, (int)(Item.damage * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, i, i * 15);
					}
					Main.projectile[Probes[i]].timeLeft = 6;
				}
			}
		}
	}
}