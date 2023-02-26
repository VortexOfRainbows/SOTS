using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Tools
{
	public class ManicMiner : VoidItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Tools/ManicMinerGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 48;
			Item.height = 18;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = null;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.ManicMiner>(); 
            Item.shootSpeed = 4f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Tools/ManicMinerGlow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -4;
			}
		}
        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(-4, 0);
        }
        public override int GetVoid(Player player)
		{
			return 5;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position += velocity.SafeNormalize(Vector2.Zero) * 28;
        }
	}
}