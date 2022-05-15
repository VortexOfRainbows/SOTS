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
			Item.width = 28;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = 4;
			Item.defense = 3;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Patch Leather Hat");
			Tooltip.SetDefault("Increases max minions by 1");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == Mod.Find<ModItem>("PatchLeatherTunic") .Type&& legs.type == Mod.Find<ModItem>("PatchLeatherPants").Type;
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
				if(Main.player[proj.owner] == player && proj.type == Mod.Find<ModProjectile>("FlyingSnake") .Type&& proj.active)
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
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, Mod.Find<ModProjectile>("FlyingSnake").Type, (int)(14 * (1 + (player.GetDamage(DamageClass.Summon) - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 1);
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != Mod.Find<ModProjectile>("FlyingSnake").Type)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, Mod.Find<ModProjectile>("FlyingSnake").Type, (int)(14 * (1 + (player.GetDamage(DamageClass.Summon) - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 1);
				}

				Main.projectile[Probe].timeLeft = 6;

				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, Mod.Find<ModProjectile>("FlyingSnake").Type, (int)(14 * (1 + (player.GetDamage(DamageClass.Summon) - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 2);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != Mod.Find<ModProjectile>("FlyingSnake").Type)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, Mod.Find<ModProjectile>("FlyingSnake").Type, (int)(14 * (1 + (player.GetDamage(DamageClass.Summon) - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 2);
				}

				Main.projectile[Probe2].timeLeft = 6;

				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, Mod.Find<ModProjectile>("FlyingSnake").Type, (int)(14 * (1 + (player.GetDamage(DamageClass.Summon) - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 3);
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != Mod.Find<ModProjectile>("FlyingSnake").Type)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, Mod.Find<ModProjectile>("FlyingSnake").Type, (int)(14 * (1 + (player.GetDamage(DamageClass.Summon) - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 3);
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Snakeskin>(), 24).AddRecipeGroup("SOTS:EvilMaterial", 8).AddTile(TileID.Anvils).Register();
		}
	}
}