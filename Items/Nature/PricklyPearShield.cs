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
			this.SetResearchCost(1);
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
			int damage = SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, Item.damage);
			modPlayer.CactusSpineDamage += damage;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Cactus, 20).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4).AddIngredient(ItemID.PinkPricklyPear, 1).AddTile(TileID.WorkBenches).Register();
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
}
