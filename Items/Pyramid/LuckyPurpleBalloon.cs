using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Pyramid
{
    public class LuckyPurpleBalloon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lucky Purple Balloon");
			Tooltip.SetDefault("Grants an additional fishing line\nCounts as a light pet");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Carrot);
            Item.shoot = ModContent.ProjectileType<Projectiles.LuckyPurpleBalloon>();
            Item.buffType = ModContent.BuffType<Buffs.PurpleBalloon>();
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Orange;
			Item.width = 18;
			Item.height = 42;
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<CursedMatter>(4).AddIngredient(ItemID.Sapphire, 1).AddTile(TileID.Anvils).Register();
		}
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}