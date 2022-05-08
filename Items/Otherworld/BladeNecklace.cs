using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SOTS.Items.Otherworld
{
	public class BladeNecklace : VoidItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blade Necklace");
			Tooltip.SetDefault("Increases void gain by 1\nPeriodically accumulate up to 9 swords that rotate around you\nEvery 10th melee attack will launch forth the swords\nGetting hit summons 5 Souls of Retaliation into the air\nEvery 10th void attack will release the souls in the form of a powerful laser");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeNecklaceBase").Value;
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeNecklaceBase").Value;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeNecklaceOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeNecklaceFill").Value;
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeNecklaceOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/BladeNecklaceFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y + 2), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SafeSetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.damage = 40;
			Item.maxStack = 1;
            Item.width = 30;     
            Item.height = 58;
			Item.knockBack = 1f;
            Item.value = Item.sellPrice(0, 3, 50, 0);
            Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BladeGenerator>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TwilightBeads>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			BeadPlayer beadPlayer = player.GetModPlayer<BeadPlayer>();
			BladePlayer bladePlayer = player.GetModPlayer<BladePlayer>();
			int damage = (int)(Item.damage * (1f + (player.meleeDamage - 1f) + (player.allDamage - 1f) + (voidPlayer.voidDamage - 1f)));

			voidPlayer.bonusVoidGain += 1f;
			bladePlayer.maxBlades += 9;
			bladePlayer.bladeDamage += damage;
			bladePlayer.bladeGeneration++;
			beadPlayer.soulDamage += damage;
			beadPlayer.RetaliationSouls = true;
		}
	}
}