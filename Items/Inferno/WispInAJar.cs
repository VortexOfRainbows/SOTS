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
			this.SetResearchCost(1);
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
			if(!hideVisual)
				modPlayer.petFreeWisp += SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, Item.damage);
			modPlayer.BlueFireOrange = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Glass, 20).AddIngredient(ModContent.ItemType<DissolvingNether>(), 1).AddIngredient(ModContent.ItemType<BluefirePotion>(), 8).AddTile(TileID.MythrilAnvil).Register();
        }
        public override bool WeaponPrefix() => false;
        public override bool MagicPrefix() => false;
        public override bool MeleePrefix() => false;
        public override bool RangedPrefix() => false;
    }
}