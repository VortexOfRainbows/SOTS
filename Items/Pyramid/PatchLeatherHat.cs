using SOTS.Projectiles.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

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
			Item.rare = ItemRarityID.Orange;
			Item.defense = 3;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			SetupDrawing();
		}
		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawHatHair[equipSlotHead] = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PatchLeatherTunic>() && legs.type == ModContent.ItemType<PatchLeatherPants>();
        }
		int Probe = -1;
		int Probe2 = -1;
		int Probe3 = -1;
		public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.PatchLeather");
			int counter = 0;
			for(int i = 0; i < 1000; i++)
			{
				Projectile proj = Main.projectile[i];
				if(Main.player[proj.owner] == player && proj.type == ModContent.ProjectileType<FlyingSnake>() && proj.active)
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
				int damage = SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, 14);
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.GetSource_Misc("SOTS:Pets"), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<FlyingSnake>(), damage, 0, player.whoAmI, 1);
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != ModContent.ProjectileType<FlyingSnake>())
				{
					Probe = Projectile.NewProjectile(player.GetSource_Misc("SOTS:Pets"), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<FlyingSnake>(), damage, 0, player.whoAmI, 1);
				}

				Main.projectile[Probe].timeLeft = 6;

				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.GetSource_Misc("SOTS:Pets"), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<FlyingSnake>(), damage, 0, player.whoAmI, 2);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != ModContent.ProjectileType<FlyingSnake>())
				{
					Probe2 = Projectile.NewProjectile(player.GetSource_Misc("SOTS:Pets"), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<FlyingSnake>(), damage, 0, player.whoAmI, 2);
				}

				Main.projectile[Probe2].timeLeft = 6;

				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.GetSource_Misc("SOTS:Pets"), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<FlyingSnake>(), damage, 0, player.whoAmI, 3);
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != ModContent.ProjectileType<FlyingSnake>())
				{
					Probe3 = Projectile.NewProjectile(player.GetSource_Misc("SOTS:Pets"), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<FlyingSnake>(), damage, 0, player.whoAmI, 3);
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