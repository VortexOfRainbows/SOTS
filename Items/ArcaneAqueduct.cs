using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace SOTS.Items
{
	public class ArcaneAqueduct : ModItem
	{	int Probe = -1;
		int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Aqueduct");
			Tooltip.SetDefault("Surrounds you with 2 orbital projectiles");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 8));
		}
		public override void SetDefaults()
		{
	
			item.damage = 14;
			item.magic = true;
            item.width = 34;     
            item.height = 34;   
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WaterBolt, 1);
			recipe.AddIngredient(ItemID.AquaScepter, 1);
			recipe.AddIngredient(3066, 25); //smooth marble
			recipe.AddIngredient(null, "FragmentOfTide", 4);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("Rainbolt") || Main.projectile[Probe].owner != player.whoAmI || Main.projectile[Probe].ai[0] != 0)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;

				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 1);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("Rainbolt") || Main.projectile[Probe2].owner != player.whoAmI || Main.projectile[Probe2].ai[0] != 1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 1);
				}
				Main.projectile[Probe2].timeLeft = 6;
			}
		}
	}
}