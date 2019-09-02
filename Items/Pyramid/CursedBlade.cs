using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Pyramid
{  
    public class CursedBlade : ModItem
    {
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Blade");
			Tooltip.SetDefault("Summons a Cursed Blade to assist in combat\nOnly two blades may be active at once, though they don't require any summon slots");
		}
        public override void SetDefaults()
        {
           
            item.damage = 28;
            item.summon = true;
            item.mana = 10;
            item.width = 28;
            item.height = 28;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("CursedBlade");
            item.shootSpeed = 7f;
			item.buffType = mod.BuffType("CursedBlade");
            item.buffTime = 432000;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 5);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(180)); 
            Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return player.altFunctionUse != 2;
		}
    }
}