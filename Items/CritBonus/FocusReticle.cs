using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Celestial;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	[AutoloadEquip(EquipType.Face)]
	public class FocusReticle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Reticle");
			Tooltip.SetDefault("20% increased crit chance\nCritical strikes deal 50 more damage\nImmunity to bleeding and poisoned debuffs");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(16, 16));
		}
		public override void SetDefaults()
		{
            item.width = 30;     
            item.height = 30;  
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
			item.defense = 1;
		}
		int frame;
		int frameCounter;
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Texture2D texture2 = mod.GetTexture("Items/CritBonus/FocusReticle_Glow");
			Main.spriteBatch.Draw(texture, position, new Rectangle(0, 30 * this.frame, 30, 30), drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture2, position, new Rectangle(0, 30 * this.frame, 30, 30), Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 8)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 16)
			{
				frame = 0;
			}
			Texture2D texture = Main.itemTexture[item.type];
			Texture2D texture2 = mod.GetTexture("Items/CritBonus/FocusReticle_Glow");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition, new Rectangle(0, 30 * frame, 30, 30), lightColor, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture2, item.Center - Main.screenPosition, new Rectangle(0, 30 * frame, 30, 30), Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 8)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 16)
			{
				frame = 0;
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			player.meleeCrit += 20;
			player.rangedCrit += 20;
			player.magicCrit += 20;
			player.thrownCrit += 20;
			modPlayer.CritBonusDamage += 25;
            player.buffImmune[BuffID.Bleeding] = true; 
            player.buffImmune[BuffID.Poisoned] = true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FocusCrystal>(), 1);
			recipe.AddIngredient(ModContent.ItemType<EyeOfChaos>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SanguiteBar>(), 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class BagOfCharms : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bag of Charms");
			Tooltip.SetDefault("5% increased critical strike chance\nCritical strikes may detonate enemies for 50% critical damage\nCritical strikes steal life, regenerate void, and recover mana");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.wornArmor = false;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.magicCrit += 5;
			player.thrownCrit += 5;
			modPlayer.CritLifesteal += 1 + (Main.rand.Next(3) == 0 ? 1 : 0);
			modPlayer.CritVoidsteal += 1.25f;
			modPlayer.CritManasteal += 5 + Main.rand.Next(4);
			modPlayer.CritCurseFire = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedIcosahedron>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SoulCharm>(), 1);
			recipe.AddIngredient(ModContent.ItemType<StarShard>(), 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
