using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	[AutoloadEquip(EquipType.Body)]
	public class ShatterShardChestplate : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 22;

            item.value = Item.sellPrice(0, 2, 20, 0);
			item.rare = 2;
			item.defense = 7;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shatter Shard Chestplate");
			Tooltip.SetDefault("Getting hit surrounds you with ice shards");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("FrigidCrown") && legs.type == mod.ItemType("FrigidGreaves");
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases life regen by 1";
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			player.lifeRegen++;

			modPlayer.bonusShardDamage += 3;
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int rand = Main.rand.Next(10);
			if (rand >= 0 && rand <= 4) //0,1,2,3,4 50%
				modPlayer.shardOnHit += 1;
			if (rand >= 5 && rand <= 7) //5,6,7 30%
				modPlayer.shardOnHit += 2;
			if (rand >= 8) //9 20%
				modPlayer.shardOnHit += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 20);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}