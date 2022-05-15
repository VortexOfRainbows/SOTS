using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace SOTS.Items.Otherworld.FromChests
{
	public class CrescentStaff : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crescent Staff");
			Tooltip.SetDefault("Cast a wave of Crescents that each lock onto an enemy, and then steal life and mana from them");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 36;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 10; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 2f;
			Item.value = Item.sellPrice(0, 3, 80, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("MacaroniMoon").Type;
            Item.shootSpeed = 4.25f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/CrescentStaffGlow").Value;
			}
			Item.staff[Item.type] = true;
		}
		int projectileNum = 0;
		int highestProjectileNum = 0;
		public override bool BeforeUseItem(Player player)
		{
			projectileNum = 0;
			return base.BeforeUseItem(player);
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/CrescentStaffGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }
        public override int GetVoid(Player player)
		{
			return  8;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians((speedX < 0 ? -1 : 1) * (-20f + 20f * projectileNum)));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			position += new Vector2(speedX, speedY) * 6;

			projectileNum++;
			if(highestProjectileNum < projectileNum)
				highestProjectileNum = projectileNum;
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "PlatinumSoulStaff", 1).AddIngredient(null, "StarlightAlloy", 8).AddTile(mod.TileType("HardlightFabricatorTile")).Register();
		}
	}
}
