using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Fragments;

namespace SOTS.Items.Nature
{
	public class PricklyPearShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prickly Pear Shield");
			Tooltip.SetDefault("Getting hit releases Cactus Spines that poison nearby enemies");
		}
		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 1.5f; //this is a constant consistent with the projectile value
			Item.damage = 10;
            Item.width = 34;     
            Item.height = 26;  
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int damage = (int)(Item.damage * (1f + (player.GetDamage(DamageClass.Melee) - 1f) + (player.allDamage - 1f)));
			modPlayer.CactusSpineDamage += damage;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Cactus, 20).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4).AddIngredient(ItemID.PinkPricklyPear, 1).AddTile(TileID.WorkBenches).Register();
		}
	}
}
