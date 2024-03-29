using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Body)]
	public class ShatterShardChestplate : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
            Item.value = Item.sellPrice(0, 1, 40, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 10;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			SetupDrawing();
		}
		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			ArmorIDs.Body.Sets.HidesHands[equipSlotBody] = false;
			ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[equipSlotBody] = true;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[equipSlotBody] = true;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<FrigidCrown>() && legs.type == ModContent.ItemType<FrigidGreaves>();
		}
        public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.ShatterShard");
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.lifeRegen += 2;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.buffImmune[BuffID.Frostburn] = true;
			modPlayer.bonusShardDamage += 3;
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
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
			CreateRecipe(1).AddIngredient<FrigidBar>(20).AddTile(TileID.Anvils).Register();
		}

	}
}