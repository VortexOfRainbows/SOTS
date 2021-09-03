using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Pyramid
{  
    public class CursedBlade : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Blade");
			Tooltip.SetDefault("Summons a Cursed Blade to assist in combat");
		}
        public override void SafeSetDefaults()
        {
            item.damage = 28;
            item.summon = true;
            item.width = 34;
            item.height = 34;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("CursedBlade");
			item.buffType = mod.BuffType("CursedBlade");
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 8);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
	}
}