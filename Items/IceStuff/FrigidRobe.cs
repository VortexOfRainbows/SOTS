using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	[AutoloadEquip(EquipType.Body)]
	public class FrigidRobe : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 28;
            item.value = Item.sellPrice(0, 1, 20, 0);
			item.rare = 2;
			item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Robe");
			Tooltip.SetDefault("Frigid Javelin gains better bouncing capabilities");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("FrigidCrown") && legs.type == mod.ItemType("FrigidGreaves");
		}
		public override void UpdateArmorSet(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			player.setBonus = "Frigid Javelin no longer costs void";
			modPlayer.frigidJavelinNoCost = true;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.frigidJavelinBoost += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 16);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}