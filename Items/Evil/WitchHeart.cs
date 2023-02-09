using SOTS.Items.Fragments;
using SOTS.Items.Potions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Evil
{	
	public class WitchHeart : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 5));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 26;     
            Item.height = 36;
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritNightmare = true;
			player.GetCritChance(DamageClass.Generic) += 5;
			player.statLifeMax2 += 20;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LifeFruit, 4).AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1).AddIngredient(ModContent.ItemType<NightmarePotion>(), 8).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}