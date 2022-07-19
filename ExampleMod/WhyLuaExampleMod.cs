using Terraria.ModLoader;
using WhyLua;

namespace WhyLuaExampleMod {
	public class WhyLuaExampleMod : Mod {
		public override void Load() {
			WhyLua.LoadLua(this);
		}
	}
}
