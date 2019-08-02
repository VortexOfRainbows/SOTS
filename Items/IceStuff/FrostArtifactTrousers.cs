using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	[AutoloadEquip(EquipType.Legs)]
	public class FrostArtifactTrousers : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;
			
            item.value = Item.sellPrice(0, 6, 25, 0);
			item.rare = 7;
			item.defense = 15;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Artifact Trousers");
			Tooltip.SetDefault("10% increased melee and movement speed\nSummons a Frost Probe above you that assists in combat");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("FrostArtifactChestplate") && head.type == mod.ItemType("FrostArtifactHelmet");
        }

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.1f;
			player.meleeSpeed += 0.1f;
			
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.Center.X, player.Center.Y - 150, 0, 0, mod.ProjectileType("FrostProbe"), 60, 1, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("FrostProbe"))
				{
					Probe = Projectile.NewProjectile(player.Center.X, player.Center.Y - 150, 0, 0, mod.ProjectileType("FrostProbe"), 60, 1, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
				
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(686, 1);
			recipe.AddIngredient(null, "AbsoluteBar", 20);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}

	}
}