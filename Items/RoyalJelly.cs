using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class RoyalJelly : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Jelly");
			Tooltip.SetDefault("Increases healing recieved from potions by 40\n'I could make a very, very bad joke right now...'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 20;     
            Item.height = 32;   
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.additionalHeal += 40;
		}
	}
}