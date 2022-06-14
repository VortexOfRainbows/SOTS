using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class ZombieHand : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie Hand");
			Tooltip.SetDefault("Allows melee swings to harm Town NPCs\n'Finally, I can kill the painter!'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 22;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CanKillNPC = true;
		}
	}
}