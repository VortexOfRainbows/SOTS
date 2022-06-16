using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Fragments;

namespace SOTS.Items.Void
{
	public class VioletStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet Crescent");
			Tooltip.SetDefault("Increases max void by 50 and void regeneration speed 5%\nCan only be used once");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = 0;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
			Item.autoReuse = false;
			Item.consumable = true;
			ItemID.Sets.ItemNoGravity[Item.type] = false;
		}
		public override bool? UseItem(Player player)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath39, player.Center);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (voidPlayer.voidStar < 1)
			{
				voidPlayer.voidMeterMax += 50;
				VoidPlayer.VoidEffect(player, 50);
				voidPlayer.voidStar += 1;
				return true;
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingEarth>(), 1).AddIngredient(ItemID.ManaCrystal, 1).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 5).AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 5).AddIngredient(ItemID.ShadowScale, 15).AddTile(TileID.Anvils).Register();
		}
	}
}