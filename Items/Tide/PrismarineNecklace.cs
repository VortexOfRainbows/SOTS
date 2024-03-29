using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tide
{	
	public class PrismarineNecklace : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 20));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 46;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.rippleBonusDamage += 4;
			if(!hideVisual)
				modPlayer.rippleEffect = true;
			player.statLifeMax2 += 20;
			modPlayer.rippleTimer++;
			player.GetArmorPenetration(DamageClass.Generic) += 8;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HeartOfTheSea>(), 1).AddIngredient(ItemID.SharkToothNecklace, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}