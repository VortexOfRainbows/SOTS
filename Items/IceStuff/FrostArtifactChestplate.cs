using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	[AutoloadEquip(EquipType.Body)]
	public class FrostArtifactChestplate : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 20;

            item.value = Item.sellPrice(0, 6, 50, 0);
			item.rare = 7;
			item.defense = 24;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Artifact Breastplate");
			Tooltip.SetDefault("10% increased melee and ranged critical strike chance\nA Frost Spike rotates around you");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("FrostArtifactHelmet") && legs.type == mod.ItemType("FrostArtifactTrousers");
        }

		public override void UpdateEquip(Player player)
		{
				player.meleeCrit += 10;
				player.rangedCrit += 10;
				
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.Center.X, player.Center.Y - 150, 0, 0, mod.ProjectileType("FrostSpike"), 60, 1, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("FrostSpike"))
				{
					Probe = Projectile.NewProjectile(player.Center.X, player.Center.Y - 150, 0, 0, mod.ProjectileType("FrostSpike"), 60, 1, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(685, 1);
			recipe.AddIngredient(null, "AbsoluteBar", 24);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}

	}
}