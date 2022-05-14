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
			Item.DamageType = DamageClass.Magic;
            Item.width = 26;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<MarbleDefender>(1).AddIngredient<ArcaneAqueduct>(1).AddIngredient<TinyPlanet>(1).AddTile(TileID.TinkerersWorkbench).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			modPlayer.PushBack = true;
			if(Main.myPlayer == player.whoAmI && !hideVisual)
			{
				int damage = SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Magic, Item.damage);
				modPlayer.tPlanetDamage += damage;
				modPlayer.tPlanetNum += 2;
				modPlayer.aqueductDamage += damage;
				modPlayer.aqueductNum += 2;
			}
		}
	}
}