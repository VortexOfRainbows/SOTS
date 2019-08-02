using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BatGear
{
	[AutoloadEquip(EquipType.Head)]
	
	public class BatHat : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 20;
			item.height = 20;

			item.value = 75000;
			item.rare = 5;
			item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bat Hat");
			Tooltip.SetDefault("12% increased melee and summon damage");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BatChest") && legs.type == mod.ItemType("BatBoots");
        }

        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Summons a buffed Vampire Probe to assist in combat";
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("VampireProbe"), 21, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("VampireProbe"))
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("VampireProbe"), 21, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;


			
		}
		

		public override void UpdateEquip(Player player)
		{
		
			player.minionDamage += 0.12f;
			player.meleeDamage += 0.12f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VampireStaff", 1);
			recipe.AddIngredient(null, "GelBar", 24);
			recipe.AddIngredient(ItemID.Leather, 20);
			recipe.AddIngredient(null, "GoblinRockBar", 28);
			recipe.AddIngredient(ItemID.Bone, 40);

			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}