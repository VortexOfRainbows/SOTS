using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.NPCs.ArtificialDebuffs;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Chaos
{
	public class RealityShatter : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/RealityShatterEffect");
			Texture2D textureBlack = mod.GetTexture("Items/Chaos/RealityShatterBlack");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(2 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 6));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2) + circular, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(textureBlack, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, Color.Black, rotation, drawOrigin, scale, SpriteEffects.None, 0f); 
			color = Color.Black * 0.7f;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(1 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60));
				Main.spriteBatch.Draw(textureBlack, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2) + circular, null,color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/RealityShatterEffect");
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(1 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 6));
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y) + circular, null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality Shatter");
			Tooltip.SetDefault("Enemies recieve an additional flurry of slashes that do 4x50% damage");
		}
		public override void SetDefaults()
		{
			item.damage = 100;
			item.melee = true;
			item.width = 90;
			item.height = 98;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.knockBack = 5.5f;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.noMelee = false;
			item.crit = 10;
			item.shoot = ModContent.ProjectileType<Projectiles.Chaos.RealityShatter>();
			item.shootSpeed = 10;
			item.scale = 1.1f;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Chaos/RealityShatterEffect");
			}
		}
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			int amt = 3;
			Projectile.NewProjectile(target.Center, new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), item.shoot, (int)(damage * 0.5f), knockBack, player.whoAmI, amt, target.whoAmI);
		}
        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			player.itemLocation = player.Center + player.direction * item.scale * (new Vector2(8, 0) * 0.5f).RotatedBy(player.itemRotation);
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return false; 
		}
		public override void AddRecipes()	
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 21);
			recipe.AddIngredient(ModContent.ItemType<SandstoneEdge>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}