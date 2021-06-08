using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{	[AutoloadEquip(EquipType.Shield)]
	public class MarbleDefender : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marble Defender");
			Tooltip.SetDefault("Launches attackers away from you with javelins");
		}
		public override void SetDefaults()
		{
			item.damage = 12;
			item.maxStack = 1;
            item.width = 28;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = 1;
			item.defense = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.PushBack = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MarbleBlock, 50); //smooth marble
			recipe.AddIngredient(null, "FragmentOfEarth", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}