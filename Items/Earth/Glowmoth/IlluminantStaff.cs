using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Earth.Glowmoth;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class IlluminantStaff : ModItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/IlluminantStaffGlow").Value;
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
			Item.damage = 14;
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 72;
			Item.useTime = 23;
			Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<IlluminantStaffOrb>(); 
            Item.shootSpeed = 9f;
			Item.noMelee = true;
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff
			Item.mana = 10;
			Item.scale = 0.9f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = glowTexture;
			}
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int i = 0; i < 3; i++)
            {
				Projectile.NewProjectile(source, position + new Vector2(0, -12), velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-13, 13))) + Main.rand.NextVector2Circular(5, 4.5f) - new Vector2(0, 8.5f), type, damage, knockback, player.whoAmI, 0, Main.rand.Next(-10, 11));
            }
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			position += velocity.SafeNormalize(Vector2.Zero) * 24;
        }
    }
}