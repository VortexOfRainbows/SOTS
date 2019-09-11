using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	[AutoloadEquip(EquipType.Body)]
	public class MeguminShirt : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 38;
			item.height = 28;

			item.value = 125000;
			item.rare = 10;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Archmage's Cloak");
			Tooltip.SetDefault("While mana is below 75%, hitting enemies gains a moderate chance to drop mana stars or heal hearts\nMagic damage increased by 10%\nMana cost increased by 34%\n34% decrease to all other damage types");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("MeguminHat") && legs.type == mod.ItemType("MeguminLeggings");
        }

		public override void UpdateEquip(Player player)
		{
			player.manaCost += 0.34f;
				player.magicDamage += .1f;
				player.meleeDamage -= 0.34f;
				player.rangedDamage -= 0.34f;
				player.minionDamage -= 0.34f;
				player.thrownDamage -= 0.34f;
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
				
			if(player.statMana < (player.statManaMax2 + player.statManaMax) * 0.75f)
			{
                modPlayer.megShirt = true;
			}
			else
			{
                modPlayer.megShirt = false;
			}
		}
		public override void AddRecipes()
		{
			/*
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 20);
			recipe.AddIngredient(ItemID.Leather, 16);
			recipe.AddIngredient(null, "GoblinRockBar", 24);
			recipe.AddIngredient(ItemID.Bone, 35);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
			*/
		}

	}
}