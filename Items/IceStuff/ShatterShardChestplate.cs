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
            item.value = Item.sellPrice(0, 1, 40, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 10;
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
			player.setBonus = "Increases life regen by 2\nImmunity to Chilled, Frozen, and Frostburn";
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			player.lifeRegen += 2;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.buffImmune[BuffID.Frostburn] = true;
			modPlayer.bonusShardDamage += 3;
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int rand = Main.rand.Next(10);
			if (rand >= 0 && rand <= 2) //0,1,2 30%
				modPlayer.shardOnHit += 1;
			if (rand >= 3 && rand <= 6) //3,4,5,6 40%
				modPlayer.shardOnHit += 2;
			if (rand >= 7) //7, 8, 9 30%
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