using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class TorchBomb : ModItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/TorchBombGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Bomb);
			Item.value += Item.sellPrice(0, 0, 1, 0);
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Blue;
			Item.width = 22;
			Item.height = 32;
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.Glowmoth.TorchBomb>();
			Item.shootSpeed += 1.5f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bomb).AddIngredient(ItemID.Torch, 1).AddIngredient<GlowNylon>(1).AddTile(TileID.Anvils).Register();
		}
	}
}
