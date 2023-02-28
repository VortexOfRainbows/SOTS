using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Earth;
using Terraria.DataStructures;

namespace SOTS.Items.Earth
{
	public class VibrantPistol : VoidItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/VibrantPistolGlow").Value;
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
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 24;
            Item.useTime = 5; 
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 2f;  
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<VibrantBolt>(); 
            Item.shootSpeed = 24f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = glowTexture;
			}
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.player[Main.myPlayer];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.VibrantArmor)
			{
				Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Earth/VibrantRifle").Value;
				Main.spriteBatch.Draw(texture, position - new Vector2((texture.Width - Item.width)/ 2 - 9f, -3f), null, drawColor, 0f, origin, scale * 0.72f, SpriteEffects.None, 0f); //I had to position and draw this by testing values manually ughh
				return false;
			}
			return true;
		}
		public void triggerItemUpdates(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.VibrantArmor)
			{
				Item.autoReuse = true;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.autoReuse = false;
				Item.noUseGraphic = false;
			}
		}
		public override bool BeforeUseItem(Player player)
		{
			triggerItemUpdates(player);
			return base.BeforeUseItem(player);
		}
		public override int GetVoid(Player player)
		{
			return 1;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			triggerItemUpdates(player);
			if (modPlayer.VibrantArmor)
			{
				float mult = 1.33f;
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(4.5f));
				Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<VibrantRifle>(), 0, 0, player.whoAmI, perturbedSpeed.ToRotation() - velocity.ToRotation());
				velocity.X = perturbedSpeed.X * mult;
				velocity.Y = perturbedSpeed.Y * mult;
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				//Terraria.Audio.SoundEngine.PlaySound(SoundID.Item11, position);
				return false;
			}
			return true; 
		}
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VibrantBar>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
}
