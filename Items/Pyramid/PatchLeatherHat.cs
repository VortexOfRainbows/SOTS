using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Head)]
	
	public class PatchLeatherHat : ModItem
	{	
		public override void SetDefaults()
		{

			item.width = 28;
			item.height = 16;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = 4;
			item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Patch Leather Hat");
			Tooltip.SetDefault("Increases max minions");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("PatchLeatherTunic") && legs.type == mod.ItemType("PatchLeatherPants");
        }
		int Probe = -1;
		int Probe2 = -1;
		int Probe3 = -1;
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawAltHair = true;
		}
		public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Summons a flock of flying snakes to assist in combat";
			int counter = 0;
			for(int i = 0; i < 1000; i++)
			{
				Projectile proj = Main.projectile[i];
				if(Main.player[proj.owner] == player && proj.type == mod.ProjectileType("FlyingSnake") && proj.active)
				{
					counter++;
				}
			}
			if(Main.myPlayer == player.whoAmI)
			{
				if (counter < 3)
				{
					Probe = -1;
					Probe2 = -1;
					Probe3 = -1;
				}
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FlyingSnake"), (int)(14 * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 1);
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("FlyingSnake"))
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FlyingSnake"), (int)(14 * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 1);
				}

				Main.projectile[Probe].timeLeft = 6;

				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FlyingSnake"), (int)(15 * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 2);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("FlyingSnake"))
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FlyingSnake"), (int)(15 * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 2);
				}

				Main.projectile[Probe2].timeLeft = 6;

				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FlyingSnake"), (int)(16 * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 3);
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != mod.ProjectileType("FlyingSnake"))
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FlyingSnake"), (int)(16 * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 3);
				}
				Main.projectile[Probe3].timeLeft = 6;
			}
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Snakeskin", 24);
			recipe.AddIngredient(ItemID.ShadowScale, 12);
			recipe.AddIngredient(ItemID.Leather, 8);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Snakeskin", 24);
			recipe.AddIngredient(ItemID.TissueSample, 12);
			recipe.AddIngredient(ItemID.Leather, 8);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}