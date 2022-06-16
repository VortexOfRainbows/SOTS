using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Fragments;
using SOTS.Items.Fishing;

namespace SOTS.Items.Potions
{
	public class DoubleVisionPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Double Vision Potion");
			Tooltip.SetDefault("Adds additional lines to your fishing rod, stacks with itself\nMaxes out at 6 additional lines");
			this.SetResearchCost(20);
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Buffs.DoubleVision>();   
			int minute = 3600;
            Item.buffTime = minute * 6;
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = ItemUseStyleID.EatFood;      
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;
		}
		public override bool? UseItem(Player player) 
		{
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			if(modPlayer.doubledActive == 0)
			{
				modPlayer.doubledAmount = 0;
			}
			modPlayer.doubledAmount++;
			if(modPlayer.doubledAmount > 6)
			{
				modPlayer.doubledAmount = 6;
			}
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient<Curgeon>(1).AddIngredient<PhantomFish>(1).AddIngredient<SeaSnake>(1).AddIngredient<FragmentOfTide>(1).AddIngredient(ItemID.Shiverthorn, 1).AddTile(13).Register();
		}
	}
}