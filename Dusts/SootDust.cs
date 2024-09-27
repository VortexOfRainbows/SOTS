using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Dusts
{
    public abstract class BasicDust : ModDust
    {
        public sealed override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(1, 1 + Main.rand.Next(3) * 10, 8, 8);
            dust.velocity.Y = Main.rand.NextFloat(-2, 2);
            dust.velocity.X = Main.rand.NextFloat(-2, 2);
            dust.scale *= Main.rand.NextFloat(0.8f, 1.1f);
        }
    }
    public class SootDust : BasicDust { }
    public class CrimsonSootDust : BasicDust { }
    public class CorruptionSootDust : BasicDust { }
    public class CharredWoodDust : BasicDust { }
    public class FamishedDustCorruption : BasicDust { }
    public class FamishedDustCrimson : BasicDust { }
}