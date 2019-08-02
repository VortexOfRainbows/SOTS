using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class DoubleVision : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Double Vision");
			
			Tooltip.SetDefault("Adds an additional line to your fishing rod\nCan stack with itself");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 32;
			item.value = 55500;
			item.rare = 9;
			item.maxStack = 30;
            item.buffType = mod.BuffType("Doubled");    //this is where you put your Buff name
            item.buffTime = 36000;  
            item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            item.useStyle = 2;                 //this is how the item is holded when used
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.consumable = true;       
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 4);
			recipe.AddIngredient(null, "Plasmawhale", 1);
			recipe.AddIngredient(null, "TinyPlanetFish", 1);
			recipe.AddIngredient(ItemID.Moonglow, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}public override bool CanUseItem(Player player)
		{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			if(modPlayer.doubledActive == 0)
			{
				modPlayer.doubledAmount = 0;
			}
			modPlayer.doubledAmount++;
			
			return true;
		
		
	}
	}
}