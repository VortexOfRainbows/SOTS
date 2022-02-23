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
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/HardlightGlaiveGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
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
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 2f;
            item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = ItemRarityID.LightPurple;
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
			return true; 
		}
    }
}
	
