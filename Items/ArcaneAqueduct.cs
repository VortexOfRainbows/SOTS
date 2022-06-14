using SOTS.Items.Fragments;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class ArcaneAqueduct : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Aqueduct");
			Tooltip.SetDefault("Surrounds you with 2 orbital projectiles");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 5));
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Magic;
            Item.width = 28;     
            Item.height = 44;   
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.WaterBolt, 1).AddIngredient(ItemID.AquaScepter, 1).AddIngredient(ItemID.MarbleBlock, 25).AddIngredient<FragmentOfTide>(4).AddTile(TileID.TinkerersWorkbench).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.aqueductDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Magic, Item.damage);
			modPlayer.aqueductNum += 2;
		}
	}
}