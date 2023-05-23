using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SOTS.Projectiles.Earth.Glowmoth;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Earth.Glowmoth
{
	public class IlluminantBow : VoidItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/IlluminantBowGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 60;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;            
			Item.shoot = ProjectileID.WoodenArrowFriendly; 
            Item.shootSpeed = 14.5f;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = glowTexture;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -2;
			}
		}
		public override int GetVoid(Player player)
		{
			return 4;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ArrowMoth>(), damage, knockback, player.whoAmI, Main.rand.Next(36) * 10, type);
			return false; 
		}
	}
}