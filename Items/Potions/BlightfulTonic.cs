using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Potions
{
	public class BlightfulTonic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blightful Tonic");
			
			Tooltip.SetDefault("Randomly recieve 2 of the following:\nIronskin for 15 minutes\nGood Vibes for 13 minutes\nSwiftness for 11 minutes\nRegeneration for 9 minutes");
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Potions/BlightfulTonicEffect");
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Potions/BlightfulTonicEffect");
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 30;
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.maxStack = 30;
            item.UseSound = SoundID.Item3;            
            item.useStyle = 2;        
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.consumable = true;     
			item.buffType = BuffID.Obstructed;
            item.buffTime = 60;
		}
		public override bool ConsumeItem(Player player) 
		{
			return true;
		}
		public override bool UseItem(Player player) 
		{
			int type1 = Main.rand.Next(4);
			int type2 = Main.rand.Next(4);
			while(type1 == (player.HasBuff(BuffID.Ironskin) ? 0 : -1) || type1 == (player.HasBuff(mod.BuffType("GoodVibes")) ? 1 : -1) || type1 == (player.HasBuff(BuffID.Swiftness) ? 2 : -1) || type1 == (player.HasBuff(BuffID.Regeneration) ? 3 : -1))
			{
				type1 = Main.rand.Next(4);
				if(player.HasBuff(BuffID.Ironskin) && player.HasBuff(mod.BuffType("GoodVibes")) && player.HasBuff(BuffID.Swiftness) && player.HasBuff(BuffID.Regeneration))
				{
					type1 = -1;
					break;
				}
			}
			while(type1 == type2 || type2 == (player.HasBuff(BuffID.Ironskin) ? 0 : -1) || type2 == (player.HasBuff(mod.BuffType("GoodVibes")) ? 1 : -1) || type2 == (player.HasBuff(BuffID.Swiftness) ? 2 : -1) || type2 == (player.HasBuff(BuffID.Regeneration) ? 3 : -1))
			{
				type2 = Main.rand.Next(4);
				if(player.HasBuff(BuffID.Ironskin) && player.HasBuff(mod.BuffType("GoodVibes")) && player.HasBuff(BuffID.Swiftness) && player.HasBuff(BuffID.Regeneration))
				{
					type2 = -1;
					break;
				}
			}
			int minute = 3600;
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
				if(type1 == -1 && type2 == -1)
				{
					player.AddBuff(BuffID.Ironskin, minute * 15, true);
					player.AddBuff(mod.BuffType("GoodVibes"), minute * 13, true);
					player.AddBuff(BuffID.Swiftness, minute * 11, true);
					player.AddBuff(BuffID.Regeneration, minute * 9, true);
				}
				if(type1 == 0 || type2 == 0)
				{
					player.AddBuff(BuffID.Ironskin, minute * 15, true);
				}
				if(type1 == 1 || type2 == 1)
				{
					player.AddBuff(mod.BuffType("GoodVibes"), minute * 13, true);
				}
				if(type1 == 2 || type2 == 2)
				{
					player.AddBuff(BuffID.Swiftness, minute * 11, true);
				}
				if(type1 == 3 || type2 == 3)
				{
					player.AddBuff(BuffID.Regeneration, minute * 9, true);
				}
            }
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingNature", 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}