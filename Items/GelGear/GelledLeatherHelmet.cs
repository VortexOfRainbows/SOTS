using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Head)]
	
	public class GelledLeatherHelmet : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 24;
			item.height = 26;

            item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 3;
			item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gelled Helmet");
			Tooltip.SetDefault("5% increased ranged and summon damage\nIncreases max minions");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("GelledLeatherJacket") && legs.type == mod.ItemType("GelledLeatherLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Summons a Slime Probe to assist in combat";
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SlimeProbe"), 9, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("SlimeProbe"))
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SlimeProbe"), 9, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
		}
		public override void UpdateEquip(Player player)
		{
		
			player.minionDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 24);
			recipe.AddIngredient(ItemID.Leather, 20);
			recipe.AddIngredient(null, "SlimeyFeather", 20);
			recipe.AddIngredient(ItemID.SlimeStaff, 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}