using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{
	public class EchoDisk : ModItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/EchoDiskGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Echo Disc");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.width = 26;
			Item.height = 26;
			Item.maxStack = 1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.EchoDisk>(); 
            Item.shootSpeed = 9.5f;
			Item.knockBack = 3f;
			Item.consumable = false;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VibrantBar>(), 6).AddTile(TileID.Anvils).Register();
		}
	}
}