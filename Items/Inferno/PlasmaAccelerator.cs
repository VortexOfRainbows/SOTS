using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Projectiles.Inferno;
using SOTS.Items.Fragments;

namespace SOTS.Items.Inferno
{
	public class PlasmaAccelerator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Accelerator");
			Tooltip.SetDefault("Fires supercharged plasma arrows");
		}
		public override void SetDefaults()
		{
            item.damage = 44; 
            item.ranged = true;  
            item.width = 28;   
            item.height = 64; 
            item.useTime = 10; 
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 7, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item91;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<PlasmaphobiaBolt>(); 
            item.shootSpeed = 12.5f;
			item.useAmmo = ItemID.WoodenArrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position += new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero) * 24;
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<PlasmaphobiaBolt>(), damage, knockBack, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Sharanga>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DissolvingAether>(), 1);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
