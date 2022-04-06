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
			item.width = 26;
			item.height = 44;
			item.useAnimation = 12;
			item.useTime = 12;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.value = 0;
			item.rare = ItemRarityID.Orange;
			item.maxStack = 999;
			item.autoReuse = false;
			item.consumable = true;
			ItemID.Sets.ItemNoGravity[item.type] = false; 
		}
		public override bool UseItem(Player player)
		{
			Main.PlaySound(SoundID.NPCKilled, (int)player.Center.X, (int)player.Center.Y, 39);
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 15);
			recipe.AddIngredient(ModContent.ItemType<SoulResidue>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}