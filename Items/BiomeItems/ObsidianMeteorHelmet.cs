using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;
 

namespace SOTS.Items.BiomeItems
{
	[AutoloadEquip(EquipType.Head)]
	
	public class ObsidianMeteorHelmet: ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 20;
			item.height = 20;

			item.value = 100000;
			item.rare = 5;
			item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Meteor Helmet");
			Tooltip.SetDefault("15% increased thrown and summon damage");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ObsidianMeteorChest") && legs.type == mod.ItemType("ObsidianMeteorLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Permanent obsidian skin potion\nIncreases max minions\nSummons a buffed imp to fight for you";
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 375, 33, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != 375)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 375, 33, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;

			player.AddBuff(BuffID.ObsidianSkin, 300);
			player.maxMinions += 3;
		}
		
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}	

		public override void UpdateEquip(Player player)
		{
		
			player.minionDamage += 0.15f;
			player.thrownDamage += 0.15f;
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ObsidianScale", 20);
			recipe.AddIngredient(ItemID.MeteoriteBar, 26);
			recipe.AddIngredient(ItemID.Obsidian, 90);
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}