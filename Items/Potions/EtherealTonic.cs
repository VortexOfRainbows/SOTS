using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Items.Fragments;

namespace SOTS.Items.Potions
{
	public class EtherealTonic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Tonic");
			Tooltip.SetDefault("Receive the following:\nHarmony for 15 minutes\nLifeforce for 13 minutes\nRegeneration for 11 minutes\nMagic Power for 9 minutes");
			this.SetResearchCost(20);
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Potions/EtherealTonicEffect").Value;
			Color color = new Color(100, 100, 100, 0);
			float x = Main.rand.Next(-10, 11) * 0.15f;
			float y = Main.rand.Next(-10, 11) * 0.15f;
			Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Potions/EtherealTonicEffect").Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			float x = Main.rand.Next(-10, 11) * 0.15f;
			float y = Main.rand.Next(-10, 11) * 0.15f;
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y + 2), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 32;
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 30;
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = ItemUseStyleID.EatFood;        
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;     
			Item.buffType = BuffID.Silenced;
            Item.buffTime = 60;
		}
		public override bool ConsumeItem(Player player) 
		{
			return true;
		}
		public override bool? UseItem(Player player)
		{
			int minute = 3600;
			int buff1 = ModContent.BuffType<Harmony>();
			int buff2 = BuffID.Lifeforce;
			int buff3 = BuffID.Regeneration;
			int buff4 = BuffID.MagicPower;

			player.AddBuff(buff1, minute * 15, true);
			player.AddBuff(buff2, minute * 13, true);
			player.AddBuff(buff3, minute * 11, true);
			player.AddBuff(buff4, minute * 9, true);
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingBrilliance>(), 1).Register();
		}
	}
}