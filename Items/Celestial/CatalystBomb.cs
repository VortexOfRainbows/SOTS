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
			item.damage = 0;
			item.width = 34;
			item.height = 38;
			item.useStyle = 1;
			item.value = 0;
			item.rare = ItemRarityID.Yellow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("CatalystBomb"); 
            item.shootSpeed = 12f;
			item.consumable = true;
			item.maxStack = 30;
			item.noUseGraphic = true;
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