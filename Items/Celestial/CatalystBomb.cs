using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class CatalystBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.width = 34;
			Item.height = 38;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<Projectiles.Celestial.CatalystBomb>(); 
            Item.shootSpeed = 12f;
			Item.consumable = true;
			Item.maxStack = 30;
			Item.noUseGraphic = true;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			damage = 0;
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bomb, 5).AddIngredient(ItemID.Ectoplasm, 1).AddIngredient(ItemID.SoulofNight, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}