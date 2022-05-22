using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Void
{
	public class VoidenAnkh : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voiden Ankh");
			Tooltip.SetDefault("Increases max void by 20 and void regeneration speed by 2%\nCaps out at 5");
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 44;
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
			Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCKilled, (int)player.Center.X, (int)player.Center.Y, 39);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if(voidPlayer.voidAnkh < 5)
			{
				voidPlayer.voidMeterMax += 20;
				VoidPlayer.VoidEffect(player, 20);
				voidPlayer.voidAnkh += 1;
				return true;
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CursedMatter>(), 15).AddIngredient(ModContent.ItemType<SoulResidue>(), 10).AddTile(TileID.Anvils).Register();
		}
	}
}