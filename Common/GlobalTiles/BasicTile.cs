using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using Microsoft.Xna.Framework;

namespace SOTS.Common.GlobalTiles
{
    public abstract class BasicBlock<ITile> : ModItem where ITile : ModTile
    {
        public sealed override void SetStaticDefaults()
        {
            this.SetResearchCost(100);
            SafeSetStaticDefaults();
        }
        public sealed override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.createTile = ModContent.TileType<ITile>();
            SafeSetDefaults();
        }
        public virtual void SafeSetStaticDefaults()
        {

        }
        public virtual void SafeSetDefaults()
        {

        }
        public abstract class BasicTile : ModTile
        {
            public override void SetStaticDefaults()
            {
                Main.tileBrick[Type] = true;
                Main.tileSolid[Type] = true;
                //Main.tileMergeDirt[Type] = true;
                Main.tileBlockLight[Type] = true;
                Main.tileLighted[Type] = false;
                SafeSetStaticDefaults();
            }
            public virtual void SafeSetStaticDefaults()
            {
                DustType = ModContent.DustType<SootDust>();
                HitSound = SoundID.Tink;
                AddMapEntry(new Color(40, 31, 24));
            }
        }
    }
}