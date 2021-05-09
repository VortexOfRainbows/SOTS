using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{	
	public class HeartOfTheSea : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart Of The Sea");
			Tooltip.SetDefault("Release waves of damage periodically\nRelease more waves at lower health\nWaves ignore up to 8 defense\nIncreases max life by 20\nProjectiles disabled when hidden");
		}
		public override void SetDefaults()
		{
            item.width = 42;     
            item.height = 40;   
            item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.rippleBonusDamage += 16;
			if(!hideVisual)
				modPlayer.rippleEffect = true;
			player.statLifeMax2 += 20;
			if(player.HasBuff(ModContent.BuffType<RippleBuff>()))
			{
				modPlayer.rippleTimer++;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1);
			recipe.AddIngredient(null, "RipplePotion", 8);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}