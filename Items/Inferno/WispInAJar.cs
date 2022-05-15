using SOTS.Items.Fragments;
using SOTS.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{	
	public class WispInAJar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wisp in a Jar");
			Tooltip.SetDefault("Summons a Inferno Wisp that assists in combat\nKilled enemies explode into flames for 30% of the damage dealt to them on the killing blow");
		}
		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.DamageType = DamageClass.Summon;
            Item.width = 26;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.petFreeWisp += SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, Item.damage);
			modPlayer.BlueFireOrange = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Glass, 20).AddIngredient(ModContent.ItemType<DissolvingNether>(), 1).AddIngredient(ModContent.ItemType<BluefirePotion>(), 8).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}