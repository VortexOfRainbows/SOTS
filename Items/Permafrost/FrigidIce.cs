using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class FrigidIceTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileSpelunker[Type] = true;
			minPick = 45; //requires silver to mine
			mineResist = 0.5f;
			dustType = ModContent.DustType<ModIceDust>();
			drop = ModContent.ItemType<FrigidIce>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frigid Ore");
			AddMapEntry(new Color(96, 111, 215), name);
			soundType = SoundID.Tink;
			soundStyle = 2;
		}
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			if(!fail && !effectOnly && !noItem && Main.rand.NextBool(3))
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int amt = 2 + Main.rand.Next(3);
					for (int a = 0; a < amt; a++)
					{
						Projectile.NewProjectile(new Vector2(i * 16 + 8, j * 16 + 8), new Vector2(0, -0.25f) + Main.rand.NextVector2Circular(1.5f, 1.5f), ModContent.ProjectileType<FrigidIceShard>(), 20, 3, Main.myPlayer); //40 damage normal mode, 80 expert
						noItem = true;
					}
				}
			}
        }
    }
	public class FrigidIceTileSafe : ModTile
	{
        public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SOTS/Items/Permafrost/FrigidIceTile";
			return base.Autoload(ref name, ref texture);
        }
        public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			minPick = 45; //requires silver to mine
			mineResist = 0.5f;
			dustType = ModContent.DustType<ModIceDust>();
			drop = ModContent.ItemType<FrigidIce>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frigid Ore");
			AddMapEntry(new Color(96, 111, 215), name);
			soundType = SoundID.Tink;
			soundStyle = 2;
		}
	}
	public class FrigidIce : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Ore");
		}
        public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 3, 0);
			item.createTile = ModContent.TileType<FrigidIceTileSafe>();
		}
	}
}