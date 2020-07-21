using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Vibrant
{
	[AutoloadEquip(EquipType.Head)]
	
	public class VibrantHelmet : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 24;
            item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = 1;
			item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Helmet");
			Tooltip.SetDefault("Increases max void by 50\n3% increased ranged crit chance");
			
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("VibrantChestplate") && legs.type == mod.ItemType("VibrantLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Increases void regen by 2\nGrants autofire to the Vibrant Pistol at the cost of accuracy";
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.2f;
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.vibrantArmor = true;
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 50;
			player.rangedCrit += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronHelmet, 1);
			recipe.AddIngredient(null, "VeryGlowyMushroom", 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadHelmet, 1);
			recipe.AddIngredient(null, "VeryGlowyMushroom", 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 20);
			recipe.AddIngredient(null, "VeryGlowyMushroom", 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}