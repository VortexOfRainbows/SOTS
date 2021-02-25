using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Head)]
	public class NatureWreath : ModItem
	{	int Probe = -1;
		int Probe2 = -1;
		int Probe3 = -1;
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 18;
            item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 1;
			item.defense = 1;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Wreath");
			Tooltip.SetDefault("Increased max minions");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("NatureShirt") && legs.type == mod.ItemType("NatureLeggings");
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Summons three Blooming Hooks to assist in combat";
			if (Main.myPlayer == player.whoAmI)
			{
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("BloomingHook"), (int)(11 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0.4f, player.whoAmI);
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("BloomingHook"))
				{
					Probe = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("BloomingHook"), (int)(11 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0.4f, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("BloomingHook"), (int)(11 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0.4f, player.whoAmI);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("BloomingHook"))
				{
					Probe2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("BloomingHook"), (int)(11 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0.4f, player.whoAmI);
				}
				Main.projectile[Probe2].timeLeft = 6;
				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("BloomingHook"), (int)(11 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0.4f, player.whoAmI);
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != mod.ProjectileType("BloomingHook"))
				{
					Probe3 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("BloomingHook"), (int)(11 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0.4f, player.whoAmI);
				}
				Main.projectile[Probe3].timeLeft = 6;
			}
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Wormwood", 20);
			recipe.AddIngredient(null, "FragmentOfNature", 5);
			recipe.SetResult(this);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}

	}
}