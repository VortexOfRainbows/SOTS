using SOTS.Items.ChestItems;
using SOTS.Items.Earth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{	[AutoloadEquip(EquipType.Shield)]
	public class ChiseledBarrier : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chiseled Barrier");
			Tooltip.SetDefault("Surrounds you with 4 orbital projectiles\nLaunches attackers away from you with javelins\nProjectiles disabled when hidden");
		}
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.magic = true;
            Item.width = 26;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MarbleDefender>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ArcaneAqueduct>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TinyPlanet>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			modPlayer.PushBack = true;
			if(Main.myPlayer == player.whoAmI && !hideVisual)
			{
				int damage = (int)(Item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f)));
				modPlayer.tPlanetDamage += damage;
				modPlayer.tPlanetNum += 2;
				modPlayer.aqueductDamage += damage;
				modPlayer.aqueductNum += 2;
			}
		}
	}
}