using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Void
{
	public class VioletStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet Star");
			Tooltip.SetDefault("Increases max void by 50 and void regen by 1\nCan only be used once");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 24;
			item.useAnimation = 12;
			item.useTime = 12;
			item.useStyle = 3;
			item.value = 0;
			item.rare = 3;
			item.maxStack = 999;
			item.autoReuse = false;
			item.consumable = true;
			ItemID.Sets.ItemNoGravity[item.type] = false; 
		}
		public override bool UseItem(Player player)
		{
			Main.PlaySound(4, (int)(player.Center.X), (int)(player.Center.Y), 39);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
			if(voidPlayer.voidStar < 1)
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingEarth", 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddIngredient(null, "FragmentOfEvil", 5);
			recipe.AddIngredient(null, "FragmentOfOtherworld", 5);
			recipe.AddIngredient(ItemID.ShadowScale, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}