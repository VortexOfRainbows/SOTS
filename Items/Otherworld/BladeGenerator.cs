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
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/BladeGeneratorBase");
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/BladeGeneratorBase");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, lightColor * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/BladeGeneratorOutline");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/BladeGeneratorFill");
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/BladeGeneratorOutline");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/BladeGeneratorFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2), null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			item.melee = true;
			item.damage = 27;
			item.maxStack = 1;
            item.width = 30;     
            item.height = 26;
			item.knockBack = 1f;
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			BladePlayer modPlayer = player.GetModPlayer<BladePlayer>();
			modPlayer.maxBlades += 9;
			modPlayer.bladeDamage += (int)(item.damage * (1f + (player.meleeDamage - 1f) + (player.allDamage - 1f)));
			modPlayer.bladeGeneration++;
		}
	}
	public class BladeItem : GlobalItem
	{
		public override bool CanUseItem(Item item, Player player)
		{
			BladePlayer modPlayer = player.GetModPlayer<BladePlayer>();
			if (item.melee == true)
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
				if (ModContent.ProjectileType<TwilightBlade>() == proj.type && proj.active && proj.owner == player.whoAmI && proj.timeLeft > 748)
				{
					currentBlades++;
				}
			}
			if (bladeGenSpeed < bladeGeneration)
			{
				bladeGeneration -= bladeGenSpeed;
				if(maxBlades > 0 && currentBlades < maxBlades && attackNum < 10 && player.whoAmI == Main.myPlayer)
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<TwilightBlade>(), bladeDamage, 1f, player.whoAmI);
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