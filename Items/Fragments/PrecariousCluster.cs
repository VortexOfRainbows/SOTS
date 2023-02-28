using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace SOTS.Items.Fragments
{
	public class PrecariousCluster : ModItem
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip1")
				{
					if (!DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNature)
						line.Text = Language.GetTextValue("Mods.SOTS.PrecariousClusterTextList.0");
					else
						line.Text = Language.GetTextValue("Mods.SOTS.PrecariousClusterTextList.1");
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip2")
				{
					if (!DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeEarth)
						line.Text = Language.GetTextValue("Mods.SOTS.PrecariousClusterTextList.2");
					else
						line.Text = Language.GetTextValue("Mods.SOTS.PrecariousClusterTextList.3");
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip3")
				{
					if (!DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAurora)
						line.Text = Language.GetTextValue("Mods.SOTS.PrecariousClusterTextList.4");
					else
						line.Text = Language.GetTextValue("Mods.SOTS.PrecariousClusterTextList.5");
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip4")
				{
					if (!DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAether)
						line.Text = Language.GetTextValue("Mods.SOTS.PrecariousClusterTextList.6");
					else
						line.Text = Language.GetTextValue("Mods.SOTS.PrecariousClusterTextList.7");
				}
			}
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 66;
			Item.height = 66;
            Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingAether>(1).AddIngredient<DissolvingEarth>(1).AddIngredient<DissolvingAurora>(1).AddIngredient<DissolvingNature>(1).AddTile(TileID.DemonAltar).Register();
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Fragments/PrecariousClusterSymbols").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f * 0.5f);
			position += new Vector2(33 * scale, 33 * scale);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			int bonus = (int)(counter / 360f);
			float mult = new Vector2(-11f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 3; i++)
			{
				int frameNum = (i + bonus) % 4;
				Rectangle frame = new Rectangle(0, 22 * frameNum, 22, 22);
				Vector2 rotationAround = new Vector2((11 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(120 * i + counter));
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (frameNum == 0 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeEarth)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 1 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAurora)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 2 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNature)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 3 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAether)
					spriteEffects = SpriteEffects.FlipVertically;
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(position.X + x), (float)(position.Y + y)) + rotationAround, frame, new Color(100, 100, 100, 0) * (1f - (Item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, spriteEffects, 0f);
				}
			}
			for (int k = 0; k < 7; k++)
			{
				int frameNum = (3 + bonus) % 4;
				Rectangle frame = new Rectangle(0, 22 * frameNum, 22, 22);
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (frameNum == 0 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeEarth)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 1 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAurora)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 2 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNature)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 3 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAether)
					spriteEffects = SpriteEffects.FlipVertically;
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(position.X + x), (float)(position.Y + y)), frame, new Color(100, 100, 100, 0) * (1f - (Item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, spriteEffects, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Fragments/PrecariousClusterSymbols").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f * 0.5f);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			int bonus = (int)(counter / 360f);
			float mult = new Vector2(-11f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			SpriteEffects spriteEffects = SpriteEffects.None;
			int frameNum;
			for (int i = 0; i < 3; i++)
			{
				frameNum = (i + bonus) % 4;
				Rectangle frame = new Rectangle(0, 22 * frameNum, 22, 22);
				spriteEffects = SpriteEffects.None;
				if (frameNum == 0 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeEarth)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 1 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAurora)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 2 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNature)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 3 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAether)
					spriteEffects = SpriteEffects.FlipVertically;
				Vector2 rotationAround = new Vector2((11 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(120 * i + counter));
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y) + rotationAround, frame, new Color(100, 100, 100, 0) * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale * 1.1f, spriteEffects, 0f);
				}
			}
			frameNum = (3 + bonus) % 4;
			Rectangle frame2 = new Rectangle(0, 22 * frameNum, 22, 22);
			spriteEffects = SpriteEffects.None;
			if (frameNum == 0 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeEarth)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 1 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAurora)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 2 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNature)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 3 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAether)
				spriteEffects = SpriteEffects.FlipVertically;
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), frame2, new Color(100, 100, 100, 0) * (1f - (Item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, spriteEffects, 0f);
			}
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			DissolvingElementsPlayer DEP = DissolvingElementsPlayer.ModPlayer(player);
			DEP.DissolvingAether += Item.stack;
			DEP.DissolvingEarth += Item.stack;
			DEP.DissolvingAurora += Item.stack;
			DEP.DissolvingNature += Item.stack;
		}
	}
	public class TerminalCluster : ModItem
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip1")
				{
					if (!DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeDeluge)
						line.Text = Language.GetTextValue("Mods.SOTS.TerminalClusterTextList.0");
					else
						line.Text = Language.GetTextValue("Mods.SOTS.TerminalClusterTextList.1");
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip2")
				{
					if (!DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeUmbra)
						line.Text = Language.GetTextValue("Mods.SOTS.TerminalClusterTextList.2");
					else
						line.Text = Language.GetTextValue("Mods.SOTS.TerminalClusterTextList.3");
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip3")
				{
					if (!DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNether)
						line.Text = Language.GetTextValue("Mods.SOTS.TerminalClusterTextList.4");
					else
						line.Text = Language.GetTextValue("Mods.SOTS.TerminalClusterTextList.5");
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip4")
				{
					if (!DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeBrilliance)
						line.Text = Language.GetTextValue("Mods.SOTS.TerminalClusterTextList.6");
					else
						line.Text = Language.GetTextValue("Mods.SOTS.TerminalClusterTextList.7");
				}
			}
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 66;
			Item.height = 66;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingBrilliance>(), 1).AddIngredient(ModContent.ItemType<DissolvingNether>(), 1).AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1).AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1).AddTile(TileID.DemonAltar).Register();
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Fragments/TerminalClusterSymbols").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f * 0.5f);
			position += new Vector2(33 * scale, 33 * scale);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			int bonus = (int)(counter / 360f);
			float mult = -12f * (float)Math.Cos(MathHelper.ToRadians(counter));
			int frameNum = 0;
			SpriteEffects spriteEffects = SpriteEffects.None;
			for (int i = 0; i < 3; i++)
			{
				frameNum = (i + bonus) % 4;
				spriteEffects = SpriteEffects.None;
				if (frameNum == 0 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeBrilliance)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 1 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNether)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 2 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeDeluge)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 3 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeUmbra)
					spriteEffects = SpriteEffects.FlipVertically;
				Rectangle frame2 = new Rectangle(0, 28 * frameNum, 28, 28);
				Vector2 rotationAround = new Vector2((12 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(120 * i + counter));
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(position.X + x), (float)(position.Y + y)) + rotationAround, frame2, new Color(100, 100, 100, 0) * (1f - (Item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, spriteEffects, 0f);
				}
			}
			frameNum = (3 + bonus) % 4;
			Rectangle frame = new Rectangle(0, 28 * frameNum, 28, 28);
			spriteEffects = SpriteEffects.None;
			if (frameNum == 0 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeBrilliance)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 1 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNether)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 2 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeDeluge)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 3 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeUmbra)
				spriteEffects = SpriteEffects.FlipVertically;
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(position.X + x), (float)(position.Y + y)), frame, new Color(100, 100, 100, 0) * (1f - (Item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, spriteEffects, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Fragments/TerminalClusterSymbols").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f * 0.5f);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			int bonus = (int)(counter / 360f);
			float mult = -18f * (float)Math.Cos(MathHelper.ToRadians(counter));
			int frameNum;
			SpriteEffects spriteEffects = SpriteEffects.None;
			for (int i = 0; i < 3; i++)
			{
				frameNum = (i + bonus) % 4;
				Rectangle frame2 = new Rectangle(0, 28 * frameNum, 28, 28);
				spriteEffects = SpriteEffects.None;
				if (frameNum == 0 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeBrilliance)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 1 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNether)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 2 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeDeluge)
					spriteEffects = SpriteEffects.FlipVertically;
				if (frameNum == 3 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeUmbra)
					spriteEffects = SpriteEffects.FlipVertically;
				Vector2 rotationAround = new Vector2((18 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(120 * i + counter));
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y) + rotationAround, frame2, new Color(100, 100, 100, 0) * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale * 1.1f, spriteEffects, 0f);
				}
			}
			frameNum = (3 + bonus) % 4;
			Rectangle frame = new Rectangle(0, 28 * frameNum, 28, 28);
			spriteEffects = SpriteEffects.None;
			if (frameNum == 0 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeBrilliance)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 1 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNether)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 2 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeDeluge)
				spriteEffects = SpriteEffects.FlipVertically;
			if (frameNum == 3 && DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeUmbra)
				spriteEffects = SpriteEffects.FlipVertically;
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), frame, new Color(100, 100, 100, 0) * (1f - (Item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, spriteEffects, 0f);
			}
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			DissolvingElementsPlayer DEP = DissolvingElementsPlayer.ModPlayer(player);
			DEP.DissolvingNether += Item.stack;
			DEP.DissolvingUmbra += Item.stack;
			DEP.DissolvingDeluge += Item.stack;
			DEP.DissolvingBrilliance += Item.stack;
		}
	}
}