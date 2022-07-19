using NLua;
using System.IO;
using Terraria.ModLoader;

namespace WhyLua.Content {
	[Autoload(false)]
	public class LuaItem : ModItem {
		private readonly Lua _interpreter;
		private readonly string _lua;
		private readonly string _path;

		public LuaItem(Lua interpreter, string lua, string path) {
			_interpreter = interpreter;
			_lua = lua;
			_path = path;
		}

		public override string Texture => FindTexture();

		private string FindTexture() {
			LuaFunction texture = _interpreter["texture"] as LuaFunction;
			if (texture == null)
				return Path.GetFileNameWithoutExtension(_path) + ".png";
			else
				return (string)texture.Call()[0];
		}

		public override void AddRecipes() {
			LuaFunction addRecipes = _interpreter["addRecipes"] as LuaFunction;
			addRecipes.Call();
		}

		public override void SetDefaults() {
			LuaFunction setDefaults = _interpreter["setDefaults"] as LuaFunction;
			setDefaults.Call();
		}

		public override void SetStaticDefaults() {
			LuaFunction setStaticDefaults = _interpreter["setStaticDefaults"] as LuaFunction;
			setStaticDefaults.Call();
		}
	}
}
