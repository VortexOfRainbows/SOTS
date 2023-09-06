using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{	[AutoloadEquip(EquipType.Shield)]
	public class MarbleDefender : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).PushBack = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Marble, 50).AddIngredient<FragmentOfEarth>(4).AddTile(TileID.Anvils).Register();
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
}