using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;
 

namespace SOTS.Items.OreItems
{
	[AutoloadEquip(EquipType.Head)]
	
	public class DiamondSpacer: ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 24;

			item.value = 525000;
			item.rare = 6;
			item.defense = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Spacer");
			Tooltip.SetDefault("Grants permanent spelunker effect\nAlso decreases damage taken by 7%");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("DiamondCrest") && legs.type == mod.ItemType("DiamondBoots");
        }

        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Summons a super buffed slime to fight for you";
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 266, 39, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != 266)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 266, 39, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;

		}
		
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
}

		public override void UpdateEquip(Player player)
		{
					player.AddBuff(BuffID.Spelunker, 300);
			player.endurance += 0.07f;
		
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SteelBar", 20);
			recipe.AddIngredient(ItemID.Emerald, 12);
			recipe.AddIngredient(ItemID.Sapphire, 12);
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}