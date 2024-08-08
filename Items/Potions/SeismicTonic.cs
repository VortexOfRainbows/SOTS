using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace SOTS.Items.Potions
{
	public class SeismicTonic : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Potions/SeismicTonicEffect").Value;
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 2; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Potions/SeismicTonicEffect").Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 2; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 32;
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;     
			Item.buffType = BuffID.Stoned;
            Item.buffTime = 60;
		}
		public override bool ConsumeItem(Player player) 
		{
			return true;
		}
		public override bool? UseItem(Player player)
		{
			int minute = 3600;
			int type1 = -1;
			int type2 = -1;
			int buff1 = BuffID.Ironskin;
			int buff2 = Mod.Find<ModBuff>("SoulAccess").Type;
			int buff3 = BuffID.Endurance;
			int buff4 = Mod.Find<ModBuff>("Roughskin").Type;
			List<int> capableEffects = new List<int>();

			if (!player.HasBuff(buff1))
				capableEffects.Add(buff1);
			if (!player.HasBuff(buff2))
				capableEffects.Add(buff2);
			if (!player.HasBuff(buff3))
				capableEffects.Add(buff3);
			if (!player.HasBuff(buff4))
				capableEffects.Add(buff4);

			if (capableEffects.Count() >= 1)
			{
				type1 = capableEffects[Main.rand.Next(capableEffects.Count())];
				capableEffects.Remove(type1);
			}

			if (capableEffects.Count() >= 1)
				type2 = capableEffects[Main.rand.Next(capableEffects.Count())];

			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				if (type1 == -1 && type2 == -1)
				{
					player.AddBuff(buff1, minute * 15, true);
					player.AddBuff(buff2, minute * 13, true);
					player.AddBuff(buff3, minute * 11, true);
					player.AddBuff(buff4, minute * 9, true);
				}
				if (type1 == buff1 || type2 == buff1)
					player.AddBuff(buff1, minute * 15, true);
				if (type1 == buff2 || type2 == buff2)
					player.AddBuff(buff2, minute * 13, true);
				if (type1 == buff3 || type2 == buff3)
					player.AddBuff(buff3, minute * 11, true);
				if (type1 == buff4 || type2 == buff4)
					player.AddBuff(buff4, minute * 9, true);
			}
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "DissolvingEarth", 1).Register();
		}
	}
}