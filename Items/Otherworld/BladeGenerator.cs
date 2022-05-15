using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using SOTS.Projectiles.Otherworld;
using System.Runtime.Remoting.Messaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SOTS.Items.Otherworld
{
	public class BladeGenerator : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blade Generator");
			Tooltip.SetDefault("Periodically accumulate up to 9 swords that rotate around you\nEvery 10th melee attack will launch forth the swords");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeGeneratorBase").Value;
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeGeneratorBase").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeGeneratorOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeGeneratorFill").Value;
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeGeneratorOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeGeneratorFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y + 2), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.damage = 27;
			Item.maxStack = 1;
            Item.width = 30;     
            Item.height = 26;
			Item.knockBack = 1f;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			BladePlayer modPlayer = player.GetModPlayer<BladePlayer>();
			modPlayer.maxBlades += 9;
			modPlayer.bladeDamage += (int)(Item.damage * (1f + (player.GetDamage(DamageClass.Melee) - 1f) + (player.allDamage - 1f)));
			modPlayer.bladeGeneration++;
		}
	}
	public class BladeItem : GlobalItem
	{
		public override bool CanUseItem(Item item, Player player)
		{
			BladePlayer modPlayer = player.GetModPlayer<BladePlayer>();
			if (Item.melee == true)
				modPlayer.attackNum++;
			return base.CanUseItem(item, player);
		}
	}
	public class BladePlayer : ModPlayer
	{
		public int attackNum = 0;

		public int bladeDamage = 0;
		public int maxBlades = 0;
		public int bladeGeneration = 0;

		public static readonly int bladeGenSpeed = 90;
		public override void ResetEffects()
		{
			int currentBlades = 0;

			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (ModContent.ProjectileType<TwilightBlade>() == proj.type && proj.active && proj.owner == Player.whoAmI && proj.timeLeft > 748)
				{
					currentBlades++;
				}
			}
			if (bladeGenSpeed < bladeGeneration)
			{
				bladeGeneration -= bladeGenSpeed;
				if(maxBlades > 0 && currentBlades < maxBlades && attackNum < 10 && Player.whoAmI == Main.myPlayer)
					Projectile.NewProjectile(Player.Center.X, Player.Center.Y, 0, 0, ModContent.ProjectileType<TwilightBlade>(), bladeDamage, 1f, Player.whoAmI);
			}
			if(attackNum >= 10)
			{
				attackNum++;
				if(attackNum > 12)
				{
					attackNum = 0;
				}
			}
			bladeDamage = 0;
			maxBlades = 0;
		}
	}
}