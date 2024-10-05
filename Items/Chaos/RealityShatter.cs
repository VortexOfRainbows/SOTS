using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Common.GlobalNPCs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using SOTS.Helpers;

namespace SOTS.Items.Chaos
{
	public class RealityShatter : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/RealityShatterEffect").Value;
			Texture2D textureBlack = Mod.Assets.Request<Texture2D>("Items/Chaos/RealityShatterBlack").Value;
			Color color;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(2 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 6));
				color = ColorHelper.Pastel(MathHelper.ToRadians(k * 60));
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + circular, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(textureBlack, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.Black, rotation, drawOrigin, scale, SpriteEffects.None, 0f); 
			color = Color.Black * 0.7f;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(1 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60));
				Main.spriteBatch.Draw(textureBlack, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + circular, null,color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/RealityShatterEffect").Value;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(1 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 6));
				Color color = ColorHelper.Pastel(MathHelper.ToRadians(k * 60));
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y) + circular, null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 75;
			Item.DamageType = DamageClass.Melee;
			Item.width = 90;
			Item.height = 98;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5.5f;
			Item.value = Item.sellPrice(0, 12, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;            
			Item.noMelee = false;
			Item.crit = 10;
			Item.shoot = ModContent.ProjectileType<Projectiles.Chaos.RealityShatter>();
			Item.shootSpeed = 10;
			Item.scale = 1.1f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Chaos/RealityShatterEffect").Value;
			}
		}
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			int amt = 3;
			Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), Item.shoot, (int)(hit.SourceDamage * 0.5f), hit.Knockback, player.whoAmI, amt, target.whoAmI);
		}
        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			player.itemLocation = player.Center + player.direction * Item.scale * (new Vector2(8, 0) * 0.5f).RotatedBy(player.itemRotation);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			return false; 
		}
		public override void AddRecipes()	
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PhaseBar>(), 21).AddIngredient(ModContent.ItemType<SandstoneEdge>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}