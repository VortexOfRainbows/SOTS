using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class GobletOfEntrails : ModItem
	{	
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = false;
            Item.healLife = 113;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3 with { Pitch = -0.5f };
            Item.potion = true;
        }
        public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
        {

        }
    }
}