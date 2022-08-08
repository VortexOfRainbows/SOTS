using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Earth;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Earth
{
	public class VibrantCannon : VoidItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/VibrantCannonGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Cannon");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 66;
            Item.height = 30;
            Item.useTime = 50; 
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 3f;  
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VibrantBall>(); 
            Item.shootSpeed = 8;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = glowTexture;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -21;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -2;
			}
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			position += velocity.SafeNormalize(Vector2.Zero) * 24;
        }
        public override int GetVoid(Player player)
		{
			return 22;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-21f, -2f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VibrantBar>(), 10).AddTile(TileID.Anvils).Register();
		}
	}
}
