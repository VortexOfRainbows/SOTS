using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class BaguetteCrumb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baguette Crumb");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 12;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 1;
		}
        public override bool ItemSpace(Player player)
        {
            return true;
		}
        public override void NetSend(BinaryWriter writer)
        {
			writer.Write(runOnce);
			writer.Write(frame);
		}
        public override void NetRecieve(BinaryReader reader)
        {
			runOnce = reader.ReadBoolean();
			frame = reader.ReadInt32();
		}
		int frame = 0;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			rotation = this.rotation;
			spriteBatch.Draw(Main.itemTexture[Item.type], Item.Center - Main.screenPosition, new Rectangle(0, frame * Item.height, Item.width, Item.height), lightColor, rotation, new Vector2(Item.width / 2, Item.height / 2), 1f, SpriteEffects.None, 0.0f);
            return false;
        }
        float rotation = 0;
		bool runOnce = true;
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			gravity = 0.09f;
			if (runOnce)
			{
				frame = Main.rand.Next(3);
				runOnce = false;
			}
			rotation += Item.velocity.X * 0.1f;
		}
		public override bool OnPickup(Player player)
		{
			Main.PlaySound(SoundID.Grab, player.Center);
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.baguetteLength++;
			player.statLife += 3;
			player.HealEffect(3);
			return false;
		}
	}
}