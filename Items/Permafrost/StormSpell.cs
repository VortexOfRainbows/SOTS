using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Permafrost
{  
    public class StormSpell : ModItem
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Spell");
			Tooltip.SetDefault("Create an arctic storm targeted on your cursor");
		}
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 38;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.35f;
			Item.shootSpeed = 9;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item92;
			Item.mana = 15;
			Item.crit = 2;
			Item.shoot = ModContent.ProjectileType<IceStorm>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Diamond, 1);
			recipe.AddIngredient(ModContent.ItemType<FrigidBar>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 toPos = Main.MouseWorld;
			Projectile.NewProjectile(toPos.X, toPos.Y, 0, 0, type, damage, knockBack, player.whoAmI);
			return false;
		}
    }
}