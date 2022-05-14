using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Earth
{
	[AutoloadEquip(EquipType.Head)]
	public class VibrantHelmet : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Helmet");
			Tooltip.SetDefault("Increases max void by 50\n5% increased ranged crit chance");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VibrantChestplate>() && legs.type == ModContent.ItemType<VibrantLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Increases void gain by 2\nGrants autofire to the Vibrant Pistol at the cost of accuracy";
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 2f;
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.VibrantArmor = true;
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 50;
			player.rangedCrit += 5;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}