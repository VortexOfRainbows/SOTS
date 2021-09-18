using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.SpecialDrops
{
	[AutoloadEquip(EquipType.Head)]
	public class PossessedHelmet : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
            item.value = Item.sellPrice(0, 2, 75, 0);
			item.rare = ItemRarityID.LightPurple;
			item.defense = 9;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Helmet");
			Tooltip.SetDefault("Increases max void by 100\n10% increased ranged crit chance");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("PossessedChainmail") && legs.type == mod.ItemType("PossessedGreaves");
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Increases void regen by 5";
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.5f;
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.vibrantArmor = true;
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 100;
			player.rangedCrit += 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VibrantHelmet", 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(null, "FragmentOfEvil", 3);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}