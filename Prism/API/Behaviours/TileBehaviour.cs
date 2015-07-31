using Prism.Mods.Hooks;
using Terraria;

namespace Prism.API.Behaviours {
    public abstract class TileBehaviour : EntityBehaviour<Tile> {
        [Hook]
        public virtual void OnUpdate() { }
    }
}