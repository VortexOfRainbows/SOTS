using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Laser;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class HyperlightGeyser : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/HyperlightGeyserGlow");
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
			texture = mod.GetTexture("Items/Chaos/HyperlightGeyserGlow");
			Main.spriteBatch.Draw(texture, position, frame, Color.White, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
			return true;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Rectangle frame = new Rectangle(0, 0, 78, 36);
			for (int i = 0; i < 6; i++)
			{
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 60 - SOTSWorld.GlobalCounter)) * 0.6f;
				color.A = 0;
				Vector2 rotationAround = new Vector2(4 * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + SOTSWorld.GlobalCounter));
				Main.spriteBatch.Draw(texture, item.Center + rotationAround - Main.screenPosition + new Vector2(0, 2), frame, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			texture = mod.GetTexture("Items/Chaos/HyperlightGeyserGlow");
			Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition + new Vector2(0, 2), frame, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyperlight Geyser");
			Tooltip.SetDefault("Launches 4 beams of light which each home at up to 10 enemies");
		}
        public override void SetDefaults()
		{
			item.damage = 48;
			item.magic = true;
			item.width = 78;
			item.height = 36;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5f;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Cyan;
			//item.UseSound = SoundID.Item5;
			item.autoReuse = false;
			item.channel = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Chaos.HyperlightGeyser>();
			item.shootSpeed = 34f;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.mana = 40;
		}
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<HyperlightOrb>(), damage, knockBack, player.whoAmI);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Secrets.PhotonGeyser>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 24);
			recipe.AddIngredient(ModContent.ItemType<DissolvingBrilliance>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RainbowRod, 1);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 48);
			recipe.AddIngredient(ModContent.ItemType<TerminalCluster>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}