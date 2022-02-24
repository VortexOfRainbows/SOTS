using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GhostTown
{
	public class AncientSteelHalberd : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Steel Halberd");
			Tooltip.SetDefault("Shatters enemies, making the next melee attack ignore defense and a guaranteed critical strike\nThough the Halberd cannot make use of the shattered effect");
		}
		public override void SetDefaults()
		{
			item.damage = 6;
			item.melee = true;
			item.width = 66;
			item.height = 66;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 1.6f;
            item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<Projectiles.Evil.AncientSteelHalberd>(); 
            item.shootSpeed = 6.2f;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
        public override void GetWeaponCrit(Player player, ref int crit)
        {
			crit = 0;
            base.GetWeaponCrit(player, ref crit);
        }
        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientSteelBar>(), 16);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, (int)(item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod));
			return false; 
		}
    }
}
	
