using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{	[AutoloadEquip(EquipType.Shield)]
	public class ShatterHeartShield : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shatter Heart Shield");
			Tooltip.SetDefault("Getting hit surrounds you with ice shards\nIncreases max life by 20");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 38;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 1, 10, 0);
            Item.rare = 2;
			Item.defense = 1;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int rand = Main.rand.Next(10);
			if (rand >= 0 && rand <= 5) //0,1,2,3,4,5 60%
				modPlayer.shardOnHit += 1;
			if (rand >= 6 && rand <= 8) //6,7,8 30%
				modPlayer.shardOnHit += 2;
			if (rand == 9) //9 10%
				modPlayer.shardOnHit += 3;
			modPlayer.bonusShardDamage += 2;
			player.statLifeMax2 += 20;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 8);
			recipe.AddIngredient(ItemID.LifeCrystal, 1); 
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}