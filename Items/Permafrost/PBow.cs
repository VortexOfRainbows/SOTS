using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Earth;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class PBow : ModItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Permafrost/PBowGlow").Value;
            Color color = Color.White;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 44;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 36;
			Item.height = 114;
			Item.useTime = 41;
			Item.useAnimation = 41;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = null;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.PBow>();
			Item.shootSpeed = 15f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAmmo = AmmoID.Arrow;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Permafrost.PBow>(), damage, knockback, player.whoAmI, -2f, type);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<PitatiLongbow>().AddIngredient<AncientSteelLongbow>().AddIngredient<SporeSprayer>().AddIngredient<AbsoluteBar>(12).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}