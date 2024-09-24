using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Earth;
using Terraria.DataStructures;

namespace SOTS.Items.AbandonedVillage
{
	public class MagmaBeam : VoidItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/AbandonedVillage/MagmaBeamGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Color color = new Color(100, 100, 100, 0);
            for (int i = 0; i < 6; i++)
            {
                Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
                Vector2 c = new Vector2(1.0f, 0).RotatedBy(MathHelper.TwoPi / 6f * i + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
                Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + c, null, color * 0.7f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
        }
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 12;
            Item.DamageType = DamageClass.Magic;
            Item.width = 44;
            Item.height = 32;
            Item.useTime = 40; 
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 0.05f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = null;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.AbandonedVillage.MagmaBeam>(); 
            Item.shootSpeed = 24f;
			Item.channel = true;
			Item.noUseGraphic = true;
		}
        public override bool BeforeDrainVoid(Player player)
        {
            return false;
        }
        public override int GetVoid(Player player)
        {
            return 4;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			return player.ownedProjectileCounts[type] <= 0; 
		}
	}
}
