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
			DisplayName.SetDefault("Catalyst Bomb");
			Tooltip.SetDefault("'It's almost strong enough to tear a hole between dimensions, if only it were used in the right place'");
		}
		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.width = 34;
			Item.height = 38;
			Item.useStyle = 1;
			Item.value = 0;
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;            
			Item.shoot = mod.ProjectileType("CatalystBomb"); 
            Item.shootSpeed = 12f;
			Item.consumable = true;
			Item.maxStack = 30;
			Item.noUseGraphic = true;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			damage = 0;
            return true;
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bomb, 5);
			recipe.AddIngredient(ItemID.Ectoplasm, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}