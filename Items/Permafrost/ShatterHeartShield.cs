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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 36;     
            Item.height = 44;   
            Item.value = Item.sellPrice(0, 1, 10, 0);
            Item.rare = ItemRarityID.Green;
			Item.defense = 1;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
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
			CreateRecipe(1).AddIngredient<FrigidBar>(8).AddIngredient(ItemID.LifeCrystal, 1).AddTile(TileID.Anvils).Register();
		}
	}
}