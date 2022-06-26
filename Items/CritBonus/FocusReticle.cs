using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Celestial;
using SOTS.Items.Chaos;
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
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 16));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 30;     
            Item.height = 30;  
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
			Item.defense = 1;
		}
		int frame;
		int frameCounter;
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			frameCounter++;
			if (frameCounter >= 8)
			{
				frameCounter = 0;
				this.frame++;
			}
			if (this.frame >= 16)
			{
				this.frame = 0;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/CritBonus/FocusReticle_Glow").Value;
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
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/CritBonus/FocusReticle_Glow").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 30 * frame, 30, 30), lightColor, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture2, Item.Center - Main.screenPosition, new Rectangle(0, 30 * frame, 30, 30), Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			player.GetCritChance(DamageClass.Melee) += 20;
			player.GetCritChance(DamageClass.Ranged) += 20;
			player.GetCritChance(DamageClass.Magic) += 20;
			player.GetCritChance(DamageClass.Throwing) += 20;
			modPlayer.CritBonusDamage += 25;
            player.buffImmune[BuffID.Bleeding] = true; 
            player.buffImmune[BuffID.Poisoned] = true; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<FocusCrystal>(), 1).AddIngredient(ModContent.ItemType<EyeOfChaos>(), 1).AddIngredient(ModContent.ItemType<SanguiteBar>(), 10).AddTile(TileID.TinkerersWorkbench).Register();
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
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.wornArmor = false;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetCritChance(DamageClass.Melee) += 5;
			player.GetCritChance(DamageClass.Ranged) += 5;
			player.GetCritChance(DamageClass.Magic) += 5;
			player.GetCritChance(DamageClass.Throwing) += 5;
			modPlayer.CritLifesteal += 1 + (Main.rand.NextBool(3)? 1 : 0);
			modPlayer.CritVoidsteal += 1.25f;
			modPlayer.CritManasteal += 5 + Main.rand.Next(4);
			modPlayer.CritCurseFire = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CursedIcosahedron>(), 1).AddIngredient(ModContent.ItemType<SoulCharm>(), 1).AddIngredient(ModContent.ItemType<PhaseBar>(), 10).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
