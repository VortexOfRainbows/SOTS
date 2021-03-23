using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class HealPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heal Pack");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 14;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 1;
		}
        public override bool ItemSpace(Player player)
        {
            return true;
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange = 240;
			base.GrabRange(player, ref grabRange);
		}
        public override void NetSend(BinaryWriter writer)
        {
			writer.Write(runOnce);
			writer.Write(ended);
			writer.Write(item.velocity.X);
			writer.Write(item.velocity.Y);
		}
        public override void NetRecieve(BinaryReader reader)
        {
			runOnce = reader.ReadBoolean();
			ended = reader.ReadBoolean();
			item.velocity.X = reader.ReadSingle();
			item.velocity.Y = reader.ReadSingle();
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			rotation = this.rotation;
			spriteBatch.Draw(Main.itemTexture[item.type], item.Center - Main.screenPosition, null, Color.White, rotation, new Vector2(item.width / 2, item.height / 2), 1, SpriteEffects.None, 0.0f);
            return false;
        }
        float rotation = 0;
		bool runOnce = true;
		bool ended = false;
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			gravity = 0.09f;
			if (runOnce)
			{
				item.velocity *= 0f;
				item.velocity = new Vector2(Main.rand.NextFloat(4, 6)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(180)));
				item.velocity.Y = -item.velocity.Y * 0.6f;
				if (Main.netMode == NetmodeID.Server)
				{
					var num = 0;
					NetMessage.SendData(MessageID.SyncItem, -1, -1, (NetworkText)null, item.whoAmI, (float)num, 0.0f, 0.0f, 0, 0, 0);
					Main.item[item.whoAmI].FindOwner(item.whoAmI);
				}
				runOnce = false;
			}
			item.velocity.X *= 1.02f;
			if (item.velocity.Length() > 0.3f)
			{
				Dust dust = Dust.NewDustDirect(item.Center + new Vector2(8 * item.direction, 0).RotatedBy(rotation) - new Vector2(5), 0, 0, 61);
				dust.velocity *= 0.1f;
				dust.scale *= 1.33f;
				dust.noGravity = true;
			}
			else
				ended = true;
			rotation += item.velocity.X * 0.07f;
		}
		public override bool OnPickup(Player player)
		{
			Main.PlaySound(7, player.Center);
			player.statLife += 5;
			player.HealEffect(5);
			return false;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
		}
		public override bool CanPickup(Player player)
		{
			return ended;
		}
	}
	public class ManaPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Pack");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 18;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 1;
		}
		public override bool ItemSpace(Player player)
		{
			return true;
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange = 240;
			base.GrabRange(player, ref grabRange);
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(runOnce);
			writer.Write(ended);
			writer.Write(item.velocity.X);
			writer.Write(item.velocity.Y);
		}
		public override void NetRecieve(BinaryReader reader)
		{
			runOnce = reader.ReadBoolean();
			ended = reader.ReadBoolean();
			item.velocity.X = reader.ReadSingle();
			item.velocity.Y = reader.ReadSingle();
		}
		bool runOnce = true;
		float rotation = 0;
		bool ended = false;
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			rotation = this.rotation;
			spriteBatch.Draw(Main.itemTexture[item.type], item.Center - Main.screenPosition, null, Color.White, rotation, new Vector2(item.width / 2, item.height / 2), 1, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			gravity = 0.09f;
			if (runOnce)
			{
				item.velocity *= 0f;
				item.velocity = new Vector2(Main.rand.NextFloat(4, 6)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(180)));
				item.velocity.Y = -item.velocity.Y * 0.6f;
				if (Main.netMode == NetmodeID.Server)
				{
					var num = 0;
					NetMessage.SendData(MessageID.SyncItem, -1, -1, (NetworkText)null, item.whoAmI, (float)num, 0.0f, 0.0f, 0, 0, 0);
					Main.item[item.whoAmI].FindOwner(item.whoAmI);
				}
				runOnce = false;
			}
			item.velocity.X *= 1.02f;
			if (item.velocity.Length() > 0.3f)
			{
				Dust dust = Dust.NewDustDirect(item.Center + new Vector2(0, -8 * item.direction).RotatedBy(rotation) - new Vector2(5), 0, 0, 15);
				dust.velocity *= 0.1f;
				dust.scale *= 1.33f;
				dust.noGravity = true;
			}
			else
				ended = true;
			rotation += item.velocity.X * 0.07f;
		}
		public override bool OnPickup(Player player)
		{
			Main.PlaySound(7, player.Center);
			player.statMana += 20;
			player.ManaEffect(20);
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override bool CanPickup(Player player)
		{
			return ended;
		}
	}
}