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
			Tooltip.SetDefault("Summons a Cursed Blade to assist in combat\nHit enemies receive a storm of sword strikes, dealing 50% damage each, and ending in an explosion\nThe explosion deals 100% damage and always critical strikes");
		}
        public override void SafeSetDefaults()
        {
            Item.damage = 24;
            Item.summon = true;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.CursedBlade>();
			Item.buffType = ModContent.BuffType<Buffs.MinionBuffs.CursedBlade>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 8);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(Item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
	}
}