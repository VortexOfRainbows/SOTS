using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace SOTS.Items.Pyramid
{
	public class RubyKeystone : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			//Item.autoReuse = false;
			//Item.useAnimation = 15;
			//Item.useTime = 10;
			//Item.useStyle = ItemUseStyleID.Swing;
			//Item.consumable = true;
			//Item.createTile = mod.TileType("RubyKeystoneTile");
		}
		float positionMod = 0;
		float counter = 0;
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, 90 / 255f, 10 / 255f, 30 / 255f);
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			counter += 4;
			positionMod = 2 * (float)Math.Sin(MathHelper.ToRadians(counter));
            base.Update(ref gravity, ref maxFallSpeed);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/TaintedKeystone").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 50, 0, 0);
						break;
					case 2:
						color = new Color(255, 100, 0, 0);
						break;
					case 3:
						color = new Color(255, 150, 0, 0);
						break;
					case 4:
						color = new Color(255, 200, 0, 0);
						break;
					case 5:
						color = new Color(255, 250, 0, 0);
						break;
				}
				Vector2 rotationAround = new Vector2((4 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y + positionMod) + rotationAround, null, color, 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/RubyKeystone").Value;
			Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y + positionMod), null, drawColor, 0f, drawOrigin, scale * 1.0f, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Pyramid/TaintedKeystone").Value;
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			float mult = new Vector2(-4f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 40, 0, 0);
						break;
					case 2:
						color = new Color(255, 80, 0, 0);
						break;
					case 3:
						color = new Color(255, 120, 0, 0);
						break;
					case 4:
						color = new Color(255, 160, 0, 0);
						break;
					case 5:
						color = new Color(255, 200, 0, 0);
						break;
				}
				Vector2 rotationAround2 = 0.5f * new Vector2((8 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture2, rotationAround2 + Item.Center - Main.screenPosition + new Vector2(0, 2 + positionMod), null, color, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			texture2 = Mod.Assets.Request<Texture2D>("Items/Pyramid/RubyKeystone").Value;
			Main.spriteBatch.Draw(texture2, Item.Center - Main.screenPosition + new Vector2(0, 2 + positionMod), null, lightColor, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}