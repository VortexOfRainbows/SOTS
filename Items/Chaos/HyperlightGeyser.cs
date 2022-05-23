using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Laser;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class HyperlightGeyser : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/HyperlightGeyserGlow").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Rectangle frame = new Rectangle(0, 0, 78, 36);
			position += new Vector2(39 * scale, 18 * scale);
			for (int i = 0; i < 6; i++)
			{
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 60 - SOTSWorld.GlobalCounter)) * 0.6f;
				color.A = 0;
				Vector2 rotationAround = new Vector2(4 * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + SOTSWorld.GlobalCounter));
				Main.spriteBatch.Draw(texture, position + rotationAround, frame, color, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			texture = Mod.Assets.Request<Texture2D>("Items/Chaos/HyperlightGeyserGlow").Value;
			Main.spriteBatch.Draw(texture, position, frame, Color.White, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
			return true;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Rectangle frame = new Rectangle(0, 0, 78, 36);
			for (int i = 0; i < 6; i++)
			{
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 60 - SOTSWorld.GlobalCounter)) * 0.6f;
				color.A = 0;
				Vector2 rotationAround = new Vector2(4 * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + SOTSWorld.GlobalCounter));
				Main.spriteBatch.Draw(texture, Item.Center + rotationAround - Main.screenPosition + new Vector2(0, 2), frame, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			texture = Mod.Assets.Request<Texture2D>("Items/Chaos/HyperlightGeyserGlow").Value;
			Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + new Vector2(0, 2), frame, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyperlight Geyser");
			Tooltip.SetDefault("Launches 4 beams of light which each home at up to 10 enemies");
		}
        public override void SetDefaults()
		{
			Item.damage = 54;
			Item.DamageType = DamageClass.Magic;
			Item.width = 78;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			//Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Chaos.HyperlightGeyser>();
			Item.shootSpeed = 30f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.mana = 40;
		}
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HyperlightOrb>(), damage, knockback, player.whoAmI);
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Secrets.PhotonGeyser>(), 1).AddIngredient(ModContent.ItemType<PhaseBar>(), 24).AddIngredient(ModContent.ItemType<DissolvingBrilliance>(), 1).AddTile(TileID.MythrilAnvil).Register();
			CreateRecipe(1).AddIngredient(ItemID.RainbowRod, 1).AddIngredient(ModContent.ItemType<PhaseBar>(), 48).AddIngredient(ModContent.ItemType<TerminalCluster>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}