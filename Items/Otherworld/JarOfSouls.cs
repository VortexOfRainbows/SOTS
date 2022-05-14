using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class JarOfSouls : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jar of Souls");
			Tooltip.SetDefault("Right click to open");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/JarOfSoulsEffect").Value;
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				if (k == 0)
				{
					x = 0;
					y = 0;
				}
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/JarOfSoulsEffect").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				if(k == 0)
				{
					x = 0;
					y = 0;
				}
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y + 2),
				null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 58;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.consumable = true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			int[] typeAmt = new int[9];
			int randomItem = Main.rand.Next(30);
			int drops = 0;
			int iterations = 0;
			while(drops < 25)
			{
				if (NPC.downedBoss2 && Main.rand.Next(3) != 0 && typeAmt[0] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(mod.ItemType("SoulResidue"), rand);
					drops += rand;
					typeAmt[0] += rand;
				}
				if(SOTSWorld.downedCurse && Main.rand.Next(3) != 0 && typeAmt[1] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(ModContent.ItemType<CursedMatter>(), rand);
					drops += rand;
					typeAmt[1] += rand;
				}
				if (Main.hardMode && Main.rand.Next(3) != 0 && typeAmt[2] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(ItemID.SoulofFlight, rand);
					drops += rand;
					typeAmt[2] += rand;
				}
				if (Main.hardMode && Main.rand.Next(3) != 0 && typeAmt[3] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(ItemID.SoulofLight, rand);
					drops += rand;
					typeAmt[3] += rand;
				}
				if (Main.hardMode && Main.rand.Next(3) != 0 && typeAmt[4] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(ItemID.SoulofNight, rand);
					drops += rand;
					typeAmt[4] += rand;
				}
				if (NPC.downedMechBoss1 && Main.rand.Next(3) != 0 && typeAmt[5] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(ItemID.SoulofMight, rand);
					drops += rand;
					typeAmt[5] += rand;
				}
				if (NPC.downedMechBoss2 && Main.rand.Next(3) != 0 && typeAmt[6] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(ItemID.SoulofSight, rand);
					drops += rand;
					typeAmt[6] += rand;
				}
				if (NPC.downedMechBoss3 && Main.rand.Next(3) != 0 && typeAmt[7] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(ItemID.SoulofFright, rand);
					drops += rand;
					typeAmt[7] += rand;
				}
				if (NPC.downedPlantBoss && Main.rand.Next(3) != 0 && typeAmt[8] < Main.rand.Next(5, 15))
				{
					int rand = Main.rand.Next(5) + 3;
					player.QuickSpawnItem(ItemID.Ectoplasm, rand);
					drops += rand;
					typeAmt[8] += rand;
				}
				if(iterations >= 5)
				{
					break;
				}
				iterations++;
			}
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(2) + 1);
			player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99) + 1);
			player.QuickSpawnItem(ItemID.CopperCoin, Main.rand.Next(99) + 1);
		}
	}
}