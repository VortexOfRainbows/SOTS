using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Pyramid
{
	public class VoidenAnkh : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voiden Ankh");
			Tooltip.SetDefault("Increases max void by 20 and void regen by 0.5\nCaps out at 5");
		}
		public override void SetDefaults()
		{

			item.width = 18;
			item.height = 30;
			item.useAnimation = 12;
			item.useTime = 12;
			item.useStyle = 3;
			item.value = 0;
			item.rare = 5;
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
			
			if(voidPlayer.voidAnkh < 5)
			{
			voidPlayer.voidMeterMax += 20;
			voidPlayer.voidAnkh += 1;
			return true;
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 20);
			recipe.AddIngredient(null, "SoulResidue", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}