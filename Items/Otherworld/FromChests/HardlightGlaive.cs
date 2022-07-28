using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class HardlightGlaive : VoidItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/HardlightGlaiveGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Glaive");
			Tooltip.SetDefault("Unleash a burst of lightning that deals 160% damage");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 54;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4.5f;
            Item.value = Item.sellPrice(0, 5, 75, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
			Item.autoReuse = true;            
			Item.shoot = Mod.Find<ModProjectile>("HardlightGlaive").Type; 
            Item.shootSpeed = 6.2f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override bool BeforeUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<CursedImpale>(1).AddIngredient<ImperialPike>(1).AddIngredient<OreItems.GoldGlaive>(1).AddIngredient<HardlightAlloy>(12).AddTile(ModContent.TileType<Furniture.HardlightFabricatorTile>()).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(0));
                  Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
              }
              return false; 
		}
        public override int GetVoid(Player player)
        {
			return  7;
        }
    }
}
	
