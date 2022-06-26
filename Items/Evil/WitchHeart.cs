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
			DisplayName.SetDefault("Witch's Heart");
			Tooltip.SetDefault("Increases critical strike chance by 5%\nCritical strikes unleash Nightmare Arms that do 10% damage and pull enemies together\nHas a 6 second cooldown\nIncreases max life by 20");
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
			player.GetCritChance(DamageClass.Magic) += 5;
			player.GetCritChance(DamageClass.Melee) += 5;
			player.GetCritChance(DamageClass.Ranged) += 5;
			player.GetCritChance(DamageClass.Throwing) += 5;
			player.statLifeMax2 += 20;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LifeFruit, 4).AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1).AddIngredient(ModContent.ItemType<NightmarePotion>(), 8).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}